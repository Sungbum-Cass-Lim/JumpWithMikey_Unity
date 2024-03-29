using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : PlatformObj
{
    public bool isDie = false;

    public SpriteRenderer enemySpriter;

    public int[] moveRadius;
    public int curPlatformIdx = 0;

    public Vector2 dir;
    public float enemyVelocityX;
    public float enemyVelocityY;
    public float moveX { set; get; }
    public float moveY { set; get; }
    private bool isRotate = false;
    private bool isStay = false;

    private float gravity = 1.8f;

    private void Update()
    {
        //실제 위치 이동 부분
        Move();

        transform.position = new Vector2(moveX, moveY);

        if (isDie == true)
        {
            //다음 올라갈 위치 선정
            enemyVelocityY += this.gravity * Time.deltaTime * 25f;
            moveY -= enemyVelocityY * Time.deltaTime;
        }
    }

    public override void Initialize(Platform platform)
    {
        base.Initialize(platform);

        isDie = false;
        transform.eulerAngles = Vector3.zero;
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

        if (curPlatformIdx > 1 && curPlatformIdx < 9 && moveRadius[curPlatformIdx + 1] == 0 && moveRadius[curPlatformIdx - 1] == 0)
            return;

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
        {
            isRotate = false;
            dir.x *= -1;
        }

        moveX += enemyVelocityX * dir.x * Time.deltaTime * 0.7f;
    }

    protected override void PlayerTouch(PlayerController player)
    {
        if ((player.transform.position.y > transform.position.y && player.playerVelocityY > 0 && isDie == false && player.jumpCount != 0) || (CharacterMgr.Instance.isTank == true && isDie == false))
        {
            SoundMgr.Instance.PlaySFX(SFXType.enemy_die);

            player.playerVelocityY = CharacterMgr.Instance.velocityY * 0.73f;
            player.jumpCount = 1;

            player.curRotation = -361;

            isDie = true;
            enemyVelocityX *= 1.5f;
            moveRadius = new int[] { 2, 2, 2, 2, 2, 2, 2, 2, 2, 2 };

            GameMgr.Instance.SetScore(100);

            //Player Kill To Enemy
            var gameLog = new GameLog();
            gameLog.a = "k";
            gameLog.s = 100;
            gameLog.uf = player.curFloor;
            gameLog.of = parentPlatform.platformLevel;
            gameLog.oi = curPlatformIdx;
            if (CharacterMgr.Instance.isTank && player.jumpCount < 1)
            {
                gameLog.n = "mikeyStatus = 0";
            }
            else
            {
                gameLog.n = "mikeyStatus = 1";
            }
            gameLog.unt = Extension.GetUnixTimeStamp(DateTime.UtcNow);
            GameLogic.LogPush(gameLog);
        }

#if !UNITY_EDITOR
        else if (player.transform.position.y < transform.position.y && player.playerVelocityY < 0 && player.jumpCount != 0)
        {
            SoundMgr.Instance.PlaySFX(SFXType.gameover);

            Debug.Log("player Die");
            player.isDie = true;
            player.playerCharacter.transform.eulerAngles = Vector3.forward * -90 * Mathf.PI * player.dir.x;
            player.playerVelocityY = -15;

            //Enemy Touch To Player
            var gameLog = new GameLog();
            gameLog.a = "d";
            gameLog.s = GameMgr.Instance.gameScore;
            gameLog.uf = player.curFloor;
            gameLog.of = parentPlatform.platformLevel;
            gameLog.oi = curPlatformIdx;
            gameLog.n = "JM001";
            gameLog.unt = Extension.GetUnixTimeStamp(DateTime.UtcNow);
            GameLogic.LogPush(gameLog);

            GameLogic.PlayerDie();
        }

        else
        {
            SoundMgr.Instance.PlaySFX(SFXType.gameover);

            Debug.Log("player Die");
            player.isDie = true;
            player.playerCharacter.transform.eulerAngles = Vector3.forward * -90 * Mathf.PI * player.dir.x;
            player.playerVelocityY = -15;

            //Enemy Touch To Player
            var gameLog = new GameLog();
            gameLog.a = "d";
            gameLog.s = GameMgr.Instance.gameScore;
            gameLog.uf = player.curFloor;
            gameLog.of = parentPlatform.platformLevel;
            gameLog.oi = curPlatformIdx;
            gameLog.n = "JM001";
            gameLog.unt = Extension.GetUnixTimeStamp(DateTime.UtcNow);
            GameLogic.LogPush(gameLog);

            GameLogic.PlayerDie();
        }
#endif
    }
}
