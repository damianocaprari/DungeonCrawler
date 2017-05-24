using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using LitJson;

public class SettingsManager : MonoBehaviour {
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Slider musicSlider;
    public Slider soundSlider;
    public Button saveSettingsButton;

    public Resolution[] resolutions;
    public GameSettings gameSettings;
    public AudioSource musicSource;


    public void OnEnable()
    {
        gameSettings = new GameSettings();
        resolutions = Screen.resolutions;
        

        musicSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        soundSlider.onValueChanged.AddListener(delegate { OnSoundVolumeChange(); });
        fullscreenToggle.onValueChanged.AddListener(delegate { OnFullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        saveSettingsButton.onClick.AddListener(delegate { SaveSettings(); });

        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        LoadSettings();
    }


    public void OnMusicVolumeChange()
    {
        musicSource.volume = musicSlider.value;
        gameSettings.musicVolume = musicSlider.value ;
    }
    
    public void OnSoundVolumeChange()
    {
             
    }

    public void OnFullscreenToggle()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        gameSettings.fullscreen = fullscreenToggle.isOn;

    }
    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gameSettings.resolutionIndex = resolutionDropdown.value;
    }


    public void SaveSettings()
    {
        string jsonGameSettings = JsonUtility.ToJson(gameSettings,true);
        File.WriteAllText(Application.dataPath + "/Resources/gamesettings.json",jsonGameSettings);

    }

    public void LoadSettings()
    {
        gameSettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.dataPath + "/Resources/gamesettings.json"));
        musicSlider.value = gameSettings.musicVolume;
        fullscreenToggle.isOn = gameSettings.fullscreen;
        Screen.fullScreen = gameSettings.fullscreen;
        resolutionDropdown.value = gameSettings.resolutionIndex;

        resolutionDropdown.RefreshShownValue();
    }
}

