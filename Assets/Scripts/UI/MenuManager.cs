﻿using System;
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
    private TMP_InputField inputSeed;
    [SerializeField]
    private TMP_InputField inputWidth;
    [SerializeField]
    private TMP_InputField inputHeight;

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
    private int width;
    private int height;
    private int minimumDimension;

    private float initialProbability = .6f;

    private MenuManager()
    {
        width = 60;
        height = 60;

        initialProbability = 0.6f;
    }

    void Start()
    {
        inputWidth.text = "60";
        inputHeight.text = "60";

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
        Random.InitState(seed);
        gameController.StartGame(width, height);
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
        Random.InitState(seed);
    }

    public void OnWidthChange()
    {
        if (!String.IsNullOrEmpty(inputWidth.text))
        {
            width = int.Parse(inputWidth.text);
            if (width < minimumDimension)
            {
                width = minimumDimension;
            }
        }
    }

    public void OnHeightChange()
    {
        if (!String.IsNullOrEmpty(inputHeight.text))
        {
            height = int.Parse(inputHeight.text);
            if (height < minimumDimension)
            {
                height = minimumDimension;
            }
        }
    }
}