using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : PlatformObj
{
    public override void Initialize(Platform? platform)
    {
        base.Initialize(platform);

        transform.localPosition = spawnPos;
    }

    protected override void PlayerTouch(PlayerController player)
    {
        if (player.isDie == false)
        {
            Debug.Log("player Die");
            player.isDie = true;
            player.playerCharacter.transform.eulerAngles = Vector3.forward * -90 * Mathf.PI * player.dir.x;
            player.playerVelocityY = -15;

            //Fire Touch To Player Log
            var gameLog = new GameLog();
            gameLog.a = "d";
            gameLog.s = GameMgr.Instance.gameScore;
            gameLog.uf = player.curFloor;
            gameLog.of = parentPlatform.platformLevel;
            gameLog.oi = parentPlatform.platformIdx;
            gameLog.n = "";
            gameLog.unt = Extension.GetUnixTimeStamp(DateTime.UtcNow);
            GameLogic.LogPush(gameLog);

            GameLogic.PlayerDie();
        }
    }
}
