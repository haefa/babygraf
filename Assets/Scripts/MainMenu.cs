using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
    [SerializeField]
    private GameObject optionsMenu;
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject infoMenu;
    [SerializeField]
    private GameObject helpMenu;
    [SerializeField]
    private GameObject intro2;
    [SerializeField]
    private GameObject intro3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Options()
    {
        if (optionsMenu.activeSelf)
        {
            mainMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
            optionsMenu.SetActive(true);
        }
    }

    public void Help()
    {
        if (helpMenu.activeSelf)
        {
            mainMenu.SetActive(true);
            helpMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
            helpMenu.SetActive(true);
        }
    }

    public void Info()
    {
        if (infoMenu.activeSelf)
        {
            mainMenu.SetActive(true);
            infoMenu.SetActive(false);
        }
        else
        {
            mainMenu.SetActive(false);
            infoMenu.SetActive(true);
        }
    }

    public void Intro2()
    {
        if(intro2.activeSelf)
        {
            helpMenu.SetActive(true);
            intro2.SetActive(false);
        }
        else
        {
            helpMenu.SetActive(false);
            intro2.SetActive(true);
        }
    }

    public void Intro3()
    {
        if (intro3.activeSelf)
        {
            intro2.SetActive(true);
            intro3.SetActive(false);
        }
        else
        {
            intro2.SetActive(false);
            intro3.SetActive(true);
        }
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }
}
