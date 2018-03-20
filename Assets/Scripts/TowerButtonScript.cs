using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerButtonScript : MonoBehaviour {

	// Use this for initialization
	[SerializeField]
	private GameObject towerPrefab;
	
	[SerializeField]
	private Sprite sprite;
    
	[SerializeField]
	private int price;

	[SerializeField]
	private Text priceTxt;
	
	public GameObject TowerPrefab
	{ 
		get 
		{
			return towerPrefab;
		}
	}

    public Sprite Sprite 
	{ 
		get
		{
			return sprite;
		}
	}

    public int Price 
	{ 
		get
		{
			return price; 
		}
	}

    private void Start()
	{
		priceTxt.text = price + "$";
        GameManagerScript.Instance.Changed += new CurrencyChanged(PriceCheck);
	}

    private void Update()
    {
        PriceCheck();
    }

    private void PriceCheck()
    {
       if (GameManagerScript.Instance.WaveActive)
        {
                GetComponent<Image>().color = Color.grey;
                priceTxt.color = Color.grey;
       }
       else
       {
            if (price <= GameManagerScript.Instance.Currency)
            {
                GetComponent<Image>().color = Color.white;
                priceTxt.color = Color.white;
            }
            else
            {
                GetComponent<Image>().color = Color.grey;
                priceTxt.color = Color.grey;
            }
        }
    }

    public void ShowInfo(string type)
    {
        string tooltip = string.Empty;

        switch (type)
        {
            case "Jamur":
                JamurTower jamur = towerPrefab.GetComponentInChildren<JamurTower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Jamur</b></size></color>\nDamage: {0} \nProc: {1}% \nDebufff duration: {2}sec \n Poison factor: {3}% \n Has a chance to slow down the target", jamur.Damage, jamur.Proc, jamur.DebuffDuration, jamur.SplashDamage);
                break;
            case "Brokoli":
                BrokoliTower brokoli = towerPrefab.GetComponentInChildren<BrokoliTower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Brokoli</b></size></color>\nDamage: {0} \nProc: {1}% \nDebufff duration: {2}sec \n Doesn't have a chance to slow down the target", brokoli.Damage, brokoli.Proc, brokoli.DebuffDuration, brokoli.SlowingFactor);
                break;
            case "BungaKol":
                BrokoliTower bungakol = towerPrefab.GetComponentInChildren<BrokoliTower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Bunga Kol</b></size></color>\nDamage: {0} \nProc: {1}% \nDebufff duration: {2}sec \n  Doesn't have a chance to slow down the target", bungakol.Damage, bungakol.Proc, bungakol.DebuffDuration, bungakol.SlowingFactor);
                //tooltip = string.Format("<color=#ffa500ff><size=20><b>BungaKol</b></size></color>\nDamage: {0} \nProc: {1}% \nDebufff duration: {2}sec \n Slowing factor: {3}% \n Has a chance to slow down the target", bungakol.Damage, bungakol.Proc, bungakol.DebuffDuration, bungakol.SlowingFactor);
                break;
            case "Kiwi":
                JamurTower kiwi = towerPrefab.GetComponentInChildren<JamurTower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Kiwi</b></size></color>\nDamage: {0} \nProc: {1}% \nDebufff duration: {2}sec \n Poison factor: {3}% \n Has a chance to slow down the target", kiwi.Damage, kiwi.Proc, kiwi.DebuffDuration, kiwi.SplashDamage);
                //tooltip = string.Format("<color=#ffa500ff><size=20><b>Kiwi</b></size></color>\nDamage: {0} \nProc: {1}% \nDebufff duration: {2}sec \n Slowing factor: {3}% \n Has a chance to slow down the target", kiwi.Damage, kiwi.Proc, kiwi.DebuffDuration, kiwi.SlowingFactor);
                break;
        }
        /*
        switch (type)
        {
            case "Jamur":
                JamurTower jamur = towerPrefab.GetComponentInChildren<JamurTower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Jamur</b></size></color>\nDamage: {0} \nProc: {1}% \nDebufff duration: {2}sec \n Poison factor: {3}% \n Has a chance to slow down the target", jamur.Damage,jamur.Proc,jamur.DebuffDuration,jamur.SplashDamage);
                break;
            case "Brokoli":
                BrokoliTower brokoli = towerPrefab.GetComponentInChildren<BrokoliTower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Brokoli</b></size></color>\nDamage: {0} \nProc: {1}% \nDebufff duration: {2}sec \n Slowing factor: {3}% \n Dosen't have a chance to slow down the target", brokoli.Damage, brokoli.Proc, brokoli.DebuffDuration, brokoli.SlowingFactor);
                break;
            case "BungaKol":
                KolTower bungakol = towerPrefab.GetComponentInChildren<KolTower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>BungaKol</b></size></color>\nDamage: {0} \nProc: {1}% \nDebufff duration: {2}sec \n Slowing factor: {3}% \n Has a chance to slow down the target", bungakol.Damage, bungakol.Proc, bungakol.DebuffDuration, bungakol.SlowingFactor);
                break;
            case "Kiwi":
                KiwiTower kiwi = towerPrefab.GetComponentInChildren<KiwiTower>();
                tooltip = string.Format("<color=#ffa500ff><size=20><b>Kiwi</b></size></color>\nDamage: {0} \nProc: {1}% \nDebufff duration: {2}sec \n Slowing factor: {3}% \n Has a chance to slow down the target", kiwi.Damage, kiwi.Proc, kiwi.DebuffDuration, kiwi.SlowingFactor);
                break;
        }*/
        GameManagerScript.Instance.SetTooltipText(tooltip);
        GameManagerScript.Instance.ShowStats();
    }
}
