using System.Collections.Generic;
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

    public bool isFollowerSpawn = false;
    public float waitingTime = 0.0f;

    public List<int[]> createdPlatformSaveList = new List<int[]>();
    public List<FollowEnemy> followerEnemyList = new List<FollowEnemy>();

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            MakeFollower(-3.6f);
        }

        followerEnemyList[2].maxDistance = 3;
    }

    private void Update()
    {
        scoreText.text = $"{GameMgr.Instance.gameScore}";

        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(0, cameraY, -10), cameraSpeed * Time.deltaTime);
        if (player != null && player.transform.position.y > Camera.main.ScreenToWorldPoint(Vector3.up * Screen.height / 2).y + 0.45f)
        {
            cameraY = player.transform.position.y + 0.1f;
        }

        if(isFollowerSpawn == false && GameMgr.Instance.gameState == GameState.Game)
        {
            waitingTime += Time.deltaTime;

            if(waitingTime > 4)
            {
                isFollowerSpawn = true;

                foreach (var follower in followerEnemyList)
                {
                    follower.gameObject.SetActive(true);
                }
            }
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

    public void MakeFollower(float posY)
    {
        FollowEnemy follower = ObjectPoolMgr.Instance.Load<FollowEnemy>(PoolObjectType.Object, "Follower");

        follower.transform.position = new Vector2(Random.Range(-3.15f, 3.15f), posY);
        follower.dir.x = 1;
        follower.enemyVelocityX = 5.5f;
        follower.moveY = posY;
        follower.moveRadius = new int[] { 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 };
        follower.curFloor = 0;
        follower.maxDistance = 1;

        follower.Initialize();
        follower.gameObject.SetActive(false);
        followerEnemyList.Add(follower);
    }

    public void TutorialOff()
    {
        tutorial.gameObject.SetActive(false);
    }
}
