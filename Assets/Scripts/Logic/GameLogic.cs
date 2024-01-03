using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameLogic : MonoBehaviour
{
    [Header("UI")]
    public GameObject titleUi;
    public GameObject gameUi;
    public Text scoreText;

    [Header("Logic Part")]
    public PlatformGenerator platformGenerator;

    private void Awake()
    {

    }

    private void FixedUpdate()
    {
        
    }

    public void GameStart()
    {
        titleUi.SetActive(false);
        gameUi.SetActive(true);

        platformGenerator.Initialize();
    }
}
