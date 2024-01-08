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
    public GameObject tutorial;
    public Text scoreText;

    private float cameraSpeed = 14;
    private float cameraY = 0;

    [Header("Logic Part")]
    public PlatformGenerator platformGenerator;
    private PlayerController player;

    private void Update()
    {
        scoreText.text = $"{GameMgr.Instance.GameScore}";

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(0, cameraY, -10), cameraSpeed * Time.deltaTime);
        if (player != null && player.transform.position.y > Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height / 2).y)
        {
            cameraY = player.transform.position.y + 0.3f;
        }
    }

    public void Initialize(PlayerController player)
    {
        this.player = player;
        player.transform.SetParent(transform);

        titleUi.SetActive(false);
        gameUi.SetActive(true);

        platformGererate();
    }

    public void platformGererate()
    {
        platformGenerator.Initialize();
    }

    public void TutorialOff()
    {
        tutorial.gameObject.SetActive(false);
    }
}
