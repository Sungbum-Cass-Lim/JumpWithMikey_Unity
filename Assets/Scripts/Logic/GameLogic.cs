using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{
    public static int logCount = 0;
    public static GameLog[] gameLogArray = new GameLog[5000];

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

    public bool isFollowerSpawn = false;
    public float waitingTime = 0.0f;

    public bool isEmptyPlatform = false;

    public List<int[]> platformDataList = new List<int[]>();
    public List<FollowEnemy> followerEnemyList = new List<FollowEnemy>();

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            MakeFollower(-3.6f);
        }

        followerEnemyList[followerEnemyList.Count - 1].maxDistance = 3;
    }

    private void Update()
    {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(0, cameraY, -10), cameraSpeed * Time.deltaTime);
        if (player != null && player.transform.position.y > Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height / 2).y + 0.45f)
        {
            cameraY = player.transform.position.y + 0.1f;
        }

        if (isFollowerSpawn == false && GameMgr.Instance.gameState == GameState.Game)
        {
            waitingTime += Time.deltaTime;

            if (waitingTime > 4)
            {
                isFollowerSpawn = true;

                foreach (var follower in followerEnemyList)
                {
                    follower.gameObject.SetActive(true);
                }
            }
        }

        if (GameMgr.Instance.gameState == GameState.Game && player.isDie != true && GameMgr.Instance.height <= 10)
        {
            if (isEmptyPlatform != false)
                return;
            isEmptyPlatform = true;

            var platformReq = new GamePlatformReqDto();

            if (player.curFloor == 0)
                platformReq.floor = 1;
            else
                platformReq.floor = player.curFloor;

            platformReq.score = GameMgr.Instance.gameScore;

            NetworkMgr.Instance.RequestGetPlatform(platformReq, () => { isEmptyPlatform = false; });
        }
    }

    public void Initialize(PlayerController player)
    {
        this.player = player;
        player.transform.SetParent(transform);

        titleUi.SetActive(false);
        gameUi.SetActive(true);

        platformGenerate();
    }

    public void platformGenerate()
    {
        platformGenerator.Initialize();
    }

    public void MakeFollower(float posY)
    {
        FollowEnemy follower = ObjectPoolMgr.Instance.Load<FollowEnemy>(PoolObjectType.Object, "Follower");

        follower.transform.position = new Vector2(UnityEngine.Random.Range(-3.15f, 3.15f), posY);
        follower.dir.x = 1;
        follower.enemyVelocityX = 8.5f;
        follower.moveY = posY;
        follower.moveRadius = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        follower.curFloor = 0;
        follower.maxDistance = 1;

        follower.Initialize(null);
        follower.gameObject.SetActive(false);
        followerEnemyList.Add(follower);
    }

    public void TutorialOff()
    {
        tutorial.gameObject.SetActive(false);
    }

    public static void LogPush(GameLog log)
    {
        if (logCount < gameLogArray.Length)
        {
            gameLogArray[logCount] = log;
            logCount++;
        }

        //TODO:  예외처리
    }
    public static void PlayerDie()
    {
        var gameEndReqDto = new GameEndReqDto();
        gameEndReqDto.score = GameMgr.Instance.gameScore;

        gameEndReqDto.gameLog = new GameLog[logCount];
        Array.Copy(gameLogArray, gameEndReqDto.gameLog, logCount);
        NetworkMgr.Instance.RequestEndGame(gameEndReqDto);
    }
}
