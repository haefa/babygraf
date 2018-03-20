using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public delegate void CurrencyChanged();

public class GameManagerScript : Singleton<GameManagerScript> 
{
    public event CurrencyChanged Changed;
	public TowerButtonScript ClickedBtn { get; set; }
    public int Currency { 
		get 
		{
			return currency;
		}
	 	set
		{
			this.currency = value; 
			this.currencyTxt.text = value.ToString() + " <color=lime>$</color>";

            OnCurrencyChanged();
		}
	}

    public int Lives
    {
        get { return lives; }
        set
        {

            this.lives = value;

            if (lives <= 0)
            {
                this.lives = 0;
                GameOver();
            }

            livesTxt.text = lives.ToString();
        }
    }

    private int currency;
    private int wave = 0;
    private int lives;

    private bool gameOver = false;

    private int health = 100;

    [SerializeField]
    private Text livesTxt;

    [SerializeField]
    private Text waveTxt;

	[SerializeField]
	private Text currencyTxt;

    [SerializeField]
    private GameObject waveBtn;

    [SerializeField]
    private GameObject gameOverMenu;

    [SerializeField]
    private GameObject upgradePanel;

    [SerializeField]
    private GameObject statsPanel;

    [SerializeField]
    private GameObject nextLevel;

    [SerializeField]
    private GameObject win;

    [SerializeField]
    private Text sellText;

    [SerializeField]
    private Text statTxt;

    [SerializeField]
    private Text upgradePrice;

    [SerializeField]
    private GameObject inGameMenu;

    [SerializeField]
    private GameObject optionsMenu;

    private Tower selectedTower;

    private List<Virus> activeVirus = new List<Virus>();

	public ObjectPool Pool { get; set; }
	
    public bool WaveActive
    {
        get { return activeVirus.Count > 0; }
    }

	private void Awake()
	{
//        LevelController.Level = 1;
		Pool = GetComponent<ObjectPool>();
	}

	// Use this for initialization
	void Start () {
        if (LevelController.Level == 1)
        {
            Lives = 15;
            Currency = 30;
        }
        else
        {
            Lives = (int)PlayerPrefs.GetFloat("Lives");
            Currency = (int)PlayerPrefs.GetFloat("Currency");
        }
    }
	
	// Update is called once per frame
	void Update () {
		HandleEscape();
	}
	public void PickTower(TowerButtonScript towerBtn)
	{
		if(Currency >= towerBtn.Price && !WaveActive)
		{
			this.ClickedBtn = towerBtn;
			HoverScript.Instance.Activate(towerBtn.Sprite);
		}
	}
	public void BuyTower()
	{
		if(Currency >= ClickedBtn.Price)
		{
			Currency -= ClickedBtn.Price;
			HoverScript.Instance.Deactive();
		}
	}

    public void OnCurrencyChanged()
    {
        if (Changed != null)
        {
            Changed();
        }
    }

    public void SelectTower(Tower tower)
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }

        selectedTower = tower;
        selectedTower.Select();
        sellText.text = "+ " + (selectedTower.Price / 2).ToString() + " $";
        upgradePanel.SetActive(true);
    }

    public void DeselectTower()
    {
        if(selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = null;
        upgradePanel.SetActive(false);
    }

	private void HandleEscape()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{

            if (selectedTower == null && !HoverScript.Instance.IsVisible)
            {
                ShowIngameMenu();
            }

            else if(HoverScript.Instance.IsVisible)
            {
                DropTower();
            }
            else if (selectedTower != null)
            {
                DeselectTower();
            }
		}
	}

	public void StartWave()
	{
            wave++;
            waveTxt.text = string.Format("Wave: <color=lime>{0}</color>", wave);
            StartCoroutine(SpawnWave());
            waveBtn.SetActive(false);
	}

	private IEnumerator SpawnWave()
	{
        LevelManager.Instance.GeneratePath();

        if (LevelController.Level == 1)
        {
            for (int i = 0; i < wave * 10; i++)
            {

                int monterIndex = Random.Range(0, 8);

                string type = string.Empty;

                switch (monterIndex)
                {
                    case 0:
                        type = "VirusBiru";
                        break;
                    case 1:
                        type = "VirusUngu";
                        break;
                    case 2:
                        type = "VirusOrange";
                        break;
                    case 3:
                        type = "VirusKuning";
                        break;
                    case 4:
                        type = "VirusHijau";
                        break;
                    case 5:
                        type = "VirusOrange";
                        break;
                    case 6:
                        type = "VirusKuning";
                        break;
                    case 7:
                        type = "VirusOrange";
                        break;
                    case 8:
                        type = "VirusKuning";
                        break;
                }

                Virus virus = Pool.GetObject(type).GetComponent<Virus>();
                virus.Spawn(health);

                if (wave % 2 == 0)
                {
                    health += 5;
                }

                activeVirus.Add(virus);

                yield return new WaitForSeconds(2.5f);

            }
        }
        else
        {
            for (int i = 0; i < wave * 10; i++)
            {

                int monterIndex = Random.Range(0, 10);

                string type = string.Empty;

                switch (monterIndex)
                {
                    case 0:
                        type = "VirusBiru";
                        break;
                    case 1:
                        type = "VirusUngu";
                        break;
                    case 2:
                        type = "VirusHijau";
                        break;
                    case 3:
                        type = "VirusKuning";
                        break;
                    case 4:
                        type = "VirusOrange";
                        break;
                    case 5:
                        type = "VirusBiru";
                        break;
                    case 6:
                        type = "VirusUngu";
                        break;
                    case 7:
                        type = "VirusHijau";
                        break;
                    case 8:
                        type = "VirusBiru";
                        break;
                    case 9:
                        type = "VirusUngu";
                        break;
                    case 10:
                        type = "VirusHijau";
                        break;


                }

                Virus virus = Pool.GetObject(type).GetComponent<Virus>();
                virus.Spawn(health);

                if (wave % 2 == 0)
                {
                    health += 5;
                }

                activeVirus.Add(virus);

                yield return new WaitForSeconds(2.5f);

            }

        }
    }

    public void RemoveVirus(Virus virus)
    {
        if (wave <= 4)
        {
            activeVirus.Remove(virus);
            if (!WaveActive && !gameOver)
            {
                waveBtn.SetActive(true);
            }
        }
        else
        {
            activeVirus.Remove(virus);
            if (!WaveActive && !gameOver)
            {
                    PlayerPrefs.SetFloat("Currency", currency);
                    PlayerPrefs.SetFloat("Lives", lives);
                    nextLevel.SetActive(true);
            }
        }
    }

    public void GameOver()
    {
        if(!gameOver)
        {
            gameOver = true;
            gameOverMenu.SetActive(true);
        }
    }

    public void Restart()
    {
        Time.timeScale = 1;

        if(LevelController.Level == 1)
        {
            SceneManager.LoadScene(1);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    public void QuitGame()
    {
        Time.timeScale = 1;
        LevelController.Level = 1;
        SceneManager.LoadScene(0);
    }

    public void SellTower()
    {
        if (selectedTower != null)
        {
            Currency += selectedTower.Price / 2;
            selectedTower.GetComponentInParent<TileScript>().IsEmpty = true;
            Destroy(selectedTower.transform.parent.gameObject);
            DeselectTower();
        }
    }

    public void ShowStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
    }

    public void ShowSelectedTowerStats()
    {
        statsPanel.SetActive(!statsPanel.activeSelf);
        UpdateUpgradeTip();
    }

    public void SetTooltipText(string txt)
    {
        statTxt.text = txt;
    }

    public void UpdateUpgradeTip()
    {
        if (selectedTower != null)
        {
            sellText.text = "+ " + (selectedTower.Price / 2).ToString() + " $";
            SetTooltipText(selectedTower.GetStats());

            if(selectedTower.NextUpgrade != null)
            {
                upgradePrice.text = selectedTower.NextUpgrade.Price.ToString() + " $";
            }
            else
            {
                upgradePrice.text = string.Empty;
            }
        }
    }

    public void UpgradeTower()
    {
        if (selectedTower != null)
        {
            if (selectedTower.Level <= selectedTower.Upgrades.Length && currency >= selectedTower.NextUpgrade.Price)
            {
                selectedTower.Upgrade();
            }
        }
    }

    public void ShowIngameMenu()
    {
        if (optionsMenu.activeSelf)
        {
            ShowMain();
        }
        else
        {
            inGameMenu.SetActive(!inGameMenu.activeSelf);
            if (!inGameMenu.activeSelf)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
        }
    }

    private void DropTower()
    {
        ClickedBtn = null;
        HoverScript.Instance.Deactive();
    }

    public void ShowOptions()
    {
        inGameMenu.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ShowMain()
    {
        inGameMenu.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void NextLevel()
    {
        LevelController.Level = 2;
        SceneManager.LoadScene(2);
        nextLevel.SetActive(false);
    }

    public void PlayAgain()
    {
        LevelController.Level = 1;
        SceneManager.LoadScene(1);
        win.SetActive(false);
    }
}
