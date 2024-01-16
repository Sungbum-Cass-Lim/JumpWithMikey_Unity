using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMgr : SingletonBase<CharacterMgr>
{
    public string name { private set; get; }
    public float velocityX { private set; get; }
    public float velocityY { private set; get; }
    public int jump { private set; get; }
    public bool isTank { private set; get; }
    public int plaMagnet { private set; get; }

    public float changeTime { set; get; }

    public bool isChange { private set; get; }

    public void Initialize()
    {
        this.velocityX = 1;
        this.velocityY = -28;
        this.jump = 2;
        this.isTank = false;
        this.plaMagnet = 0;
        this.isChange = false;
        this.changeTime = 0;
        this.name = "standard";

        GameMgr.Instance.player.CharacterAnimChange(0);
    }

    public void Reset()
    {
        Initialize();

        var gameLog = new GameLog();
        gameLog.a = "exbj";
        gameLog.s = 0;
        gameLog.uf = GameMgr.Instance.player.curFloor;
        gameLog.of = 0;
        gameLog.oi = GameMgr.Instance.player.curPlatformIdx;
        gameLog.n = name;
        gameLog.unt = Extension.GetUnixTimeStamp(DateTime.UtcNow);
        GameLogic.LogPush(gameLog);
    }

    public void SetCharacter(GameGetItemResDto gameGetItemResDto, float duration)
    {
        name = gameGetItemResDto.effect.name;
        velocityX = gameGetItemResDto.effect.velocityX;
        velocityY = gameGetItemResDto.effect.velocityY;
        jump = gameGetItemResDto.effect.jump;
        isTank = gameGetItemResDto.effect.isTank;
        plaMagnet = 0;

        if (gameGetItemResDto.effect.PLAMagnet > 0)
        {
            plaMagnet = 400;
        }

        changeTime = duration;
        isChange = true;

        int type = 0;
        switch(name)
        {
            case "magnet":
                type = 1;
                break;
            case "tripleJump":
                type = 2;
                break;
            case "tank":
                type = 3;
                break;
            case "highJump":
                type = 4;
                break;
        }
        GameMgr.Instance.player.CharacterAnimChange(type);

        var gameLog = new GameLog();
        gameLog.a = "bj";
        gameLog.s = 0;
        gameLog.uf = GameMgr.Instance.player.curFloor;
        gameLog.of = 0;
        gameLog.oi = GameMgr.Instance.player.curPlatformIdx;
        gameLog.n = name;
        gameLog.unt = Extension.GetUnixTimeStamp(DateTime.UtcNow);
        GameLogic.LogPush(gameLog);
    }

}
