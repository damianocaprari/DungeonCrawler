using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class MenuManagerScript : MonoBehaviour
{
    public Canvas canvasPanelSettings;
    public bool canvasPanelSettingsOpen = false;
  

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void StartGame()
    {
         SceneManager.LoadScene("gameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PanelSettings() // menù delle opzioni a scomparsa
    {
        if (canvasPanelSettingsOpen==false)
        {
            canvasPanelSettingsOpen = true;
            canvasPanelSettings.enabled=true;
        }
        else if (canvasPanelSettingsOpen==true)
        {
            canvasPanelSettingsOpen = false;
            canvasPanelSettings.enabled = false;
        }
    }
}