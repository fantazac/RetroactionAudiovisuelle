using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class MenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mainCamera;
    [SerializeField]
    private GameController gameController;

    [SerializeField]
    private Slider sliderMusic;
    [SerializeField]
    private Slider sliderSound;
    [SerializeField]
    private Slider sliderFoley;

    [SerializeField]
    private TMP_InputField inputSeed;

    [SerializeField]
    private GameObject startPanel;
    [SerializeField]
    private GameObject mainPanel;

    [SerializeField]
    private GameObject main;
    [SerializeField]
    private GameObject play;
    [SerializeField]
    private GameObject settings;
    [SerializeField]
    private GameObject advanced;

    [SerializeField]
    private GameObject mapHolder;

    private int seed;

    void Start()
    {
        mainPanel.SetActive(false);
    }

    public void OnClickEnter()
    {
        mainCamera.transform.position += Vector3.left * 600;
        startPanel.SetActive(false);
        mainPanel.SetActive(true);
        play.SetActive(false);
        settings.SetActive(false);
        advanced.SetActive(false);
    }

    public void OnClickPlay()
    {
        main.SetActive(false);
        play.SetActive(true);
    }

    public void OnClickSettings()
    {
        main.SetActive(false);
        settings.SetActive(true);
    }

    public void OnClickExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickStart()
    {
        mainCamera.SetActive(false);
        play.SetActive(false);
        mainPanel.SetActive(false);
        Random.InitState(seed == 0 ? Environment.TickCount : seed);
        StartGame();
    }

    public void StartGame()
    {
        gameController.StartGame();
    }

    public void OnClickAdvanced()
    {
        play.SetActive(false);
        advanced.SetActive(true);
    }

    public void OnClickBackToMain()
    {
        play.SetActive(false);
        settings.SetActive(false);
        main.SetActive(true);
    }

    public void OnClickBackToPlay()
    {
        settings.SetActive(false);
        advanced.SetActive(false);
        play.SetActive(true);
    }

    public void OnClickExitToMain()
    {
        mainCamera.SetActive(true);
        StaticObjects.BGMManager.PlayBGM(0);
        Destroy(StaticObjects.Player);
        mainPanel.SetActive(true);
        main.SetActive(true);
    }

    public void OnMusicVolumeChange()
    {
        StaticObjects.BGMManager.UpdateVolume(sliderMusic.value);
    }

    public void OnSoundVolumeChange()
    {
        StaticObjects.SoundEffectManager.UpdateVolume(sliderSound.value);
    }

    public void OnFoleyVolumeChange()
    {
        StaticObjects.FoleySoundEffectManager.UpdateVolume(sliderFoley.value);
    }

    public void OnSeedChange()
    {
        if (!String.IsNullOrEmpty(inputSeed.text))
        {
            seed = int.Parse(inputSeed.text);
        }
        else
        {
            seed = Environment.TickCount;
        }
    }
}
