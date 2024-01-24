using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pla : PlatformObj
{
    public int addScore;
    public float MagenetForce;

    private float maxDistance = 0.007f;
    private bool isEat = false;
    private float distance = 0;

    private void Update()
    {
        if (isEat == false && CharacterMgr.Instance.plaMagnet > 0)
        {
            distance = Vector3.Distance(transform.position, GameMgr.Instance.player.transform.position);

            if (distance < CharacterMgr.Instance.plaMagnet * maxDistance)
            {
                transform.position = Vector2.Lerp(transform.position, GameMgr.Instance.player.transform.position, MagenetForce * Time.deltaTime);
            }
        }
    }

    public override void Initialize(Platform? platform)
    {
        base.Initialize(platform);

        transform.localPosition = spawnPos;
        isEat = false;
        distance = 0;
    }

    protected override void PlayerTouch(PlayerController player)
    {
        SoundMgr.Instance.PlaySFX(SFXType.coin);

        isEat = true;

        GameMgr.Instance.SetScore(100);

        //Player Touch To Pla
        var gameLog = new GameLog();
        gameLog.a = "cp";
        gameLog.s = 100;
        gameLog.uf = player.curFloor;
        gameLog.of = parentPlatform.platformLevel;
        gameLog.oi = parentPlatform.platformIdx;
        gameLog.n = "";
        gameLog.unt = Extension.GetUnixTimeStamp(DateTime.UtcNow);
        GameLogic.LogPush(gameLog);

        gameObject.SetActive(false);
    }
}
