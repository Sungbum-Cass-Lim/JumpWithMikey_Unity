using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : PlatformObj
{
    public override void Initialize(Platform? platform)
    {
        base.Initialize(platform);

        transform.localPosition = spawnPos;
    }

    protected override void PlayerTouch(PlayerController player)
    {
        GameGetItemReqDto gameGetItemReqDto = new();
        gameGetItemReqDto.score = GameMgr.Instance.gameScore;

        NetworkMgr.Instance.RequestItem(gameGetItemReqDto);

        //Player Touch To Item
        var gameLog = new GameLog();
        gameLog.a = "cbj";
        gameLog.s = 0;
        gameLog.uf = player.curFloor;
        gameLog.of = parentPlatform.platformLevel;
        gameLog.oi = parentPlatform.platformIdx;
        gameLog.n = "";
        gameLog.unt = Extension.GetUnixTimeStamp(DateTime.UtcNow);
        GameLogic.LogPush(gameLog);

        gameObject.SetActive(false);
    }
}
