using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : PlatformObj
{
    public SpriteRenderer enemySpriter;
    public GameObject bombEffect;

    public int[] moveRadius;
    public int curPlatformIdx = 0;

    public Vector2 dir;
    public float enemyVelocityX;
    public float enemyVelocityY;
    public float moveX { set; get; }
    public float moveY { set; get; }
    private bool isRotate = false;
    private Vector2 moveTo;

    public int maxDistance = 0;
    public int playerDistance = 0;
    public bool isJump = false;

    private float gravity = 1.2f;

    [Header("Floor")]
    public int curFloor = 0;

    private void Update()
    {
        if (GameMgr.Instance.gameState != GameState.Retry)
        {
            Move();

            //실제 위치 이동 부분
            transform.position = new Vector2(moveX, moveY);

            if (isJump)
            {
                //다음 올라갈 위치 선정
                moveY -= enemyVelocityY * Time.deltaTime;
                enemyVelocityY += this.gravity * Time.deltaTime * 50;
            }
            else
            {
                FollowerJump();
            }

            if (curFloor + 5 < GameMgr.Instance.player.passFloor)
            {
                isJump = true;
                enemyVelocityY = 0;

                moveX = UnityEngine.Random.Range(-3.15f, 3.15f);
                moveY = GameConfig.INTERVAL_Y * (GameMgr.Instance.player.passFloor - 3) + GameConfig.MIN_Y + 0.8f;

                curFloor = GameMgr.Instance.player.passFloor - 3;
            }
        }
    }

    public override void Initialize(Platform platform)
    {
        base.Initialize(platform);

        moveX = transform.position.x;
        moveY = transform.position.y;
    }

    private void Move()
    {
        if (dir.x < 0) //TODO: 공식화 필요
        {
            curPlatformIdx = (int)Mathf.Ceil((transform.position.x + 3.15f) / 0.7f);
            enemySpriter.flipX = false;
        }
        else
        {
            curPlatformIdx = (int)Mathf.Floor((transform.position.x + 3.15f) / 0.7f);
            enemySpriter.flipX = true;
        }

        var nextPlatformIdx = curPlatformIdx + (int)dir.x;

        //curPlatformIdx: -1, 10
        if ((curPlatformIdx >= moveRadius.Length || curPlatformIdx < 0) && isRotate == false)
        {
            isRotate = true;

            if (moveRadius[dir.x > 0 ? 0 : moveRadius.Length - 1] != 0)
                moveX = 3.5f * -dir.x;
            else
                dir.x *= -1;
        }

        //curPlatformIdx: 0 ~ 9
        else if (nextPlatformIdx >= 0 && nextPlatformIdx < moveRadius.Length && moveRadius[nextPlatformIdx] == 0)
            dir.x *= -1;

        if (curPlatformIdx >= 0 && curPlatformIdx < moveRadius.Length)
            isRotate = false;

        moveX += enemyVelocityX * dir.x * Time.deltaTime * 0.7f;

    }

    private void FollowerJump()
    {
        if (GameMgr.Instance.gameScore < 1000)
            playerDistance = 2;
        else
            playerDistance = 1;

        var Distance = GameMgr.Instance.player.passFloor - playerDistance - curFloor;

        //중간에 플랫폼이 원하는 갯수 이하일 때 리턴
        if (Distance < maxDistance)
            return;

        isJump = true;

        if (Distance < 1)
            Distance = 1;

        switch (Distance)
        {
            case 1:
                enemyVelocityY = -20;
                break;
            case 2:
                enemyVelocityY = -27;
                break;
            default:
                enemyVelocityY = -27;
                break;
        }

        dir.x = UnityEngine.Random.Range(0, 2) == 0 ? -1 : 1;
    }

    protected override void PlayerTouch(PlayerController player)
    {
#if !UNITY_EDITOR
        SoundMgr.Instance.PlaySFX(SFXType.gameover);

        if (player.dir.x == 0)
            player.dir.x = 1;

        bombEffect.SetActive(true);

        player.isDie = true;
        player.playerCharacter.transform.eulerAngles = Vector3.forward * -90 * Mathf.PI * player.dir.x;
        player.playerVelocityY = -15;

        //FollowerEnemy Touch To Player Log
        var gameLog = new GameLog();
        gameLog.a = "d";
        gameLog.s = GameMgr.Instance.gameScore;
        gameLog.uf = player.curFloor;
        gameLog.of = curFloor;
        gameLog.oi = curPlatformIdx;
        gameLog.n = "JM002";
        gameLog.unt = Extension.GetUnixTimeStamp(DateTime.UtcNow);
        GameLogic.LogPush(gameLog);

        GameLogic.PlayerDie();
#endif
    }

    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Platform>(out var platform))
        {
            if (moveY + 0.4f > platform.Top() && enemyVelocityY > 0)
            {
                moveY = platform.Top() - 0.1f;

                curFloor = platform.platformLevel;
                moveRadius = GameMgr.Instance.GameLogic.platformDataList[curFloor];
                enemyVelocityY = 0;
                isJump = false;
            }
        }

        base.OnTriggerEnter2D(other);
    }
}
