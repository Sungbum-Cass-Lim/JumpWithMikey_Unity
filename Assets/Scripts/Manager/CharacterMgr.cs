using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMgr : SingletonBase<CharacterMgr>
{
    public string name { private set; get; }
    public int velocityX { private set; get; }
    public int velocityY { private set; get; }
    public int jump { private set; get; }
    public bool isTank { private set; get; }
    public int plaMagnet { private set; get; }

    public int changeTime { private set; get; }

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
    }

    private void SetCharacter(GameGetItemResDto gameGetItemResDto, int duration)
    {
        name = gameGetItemResDto.effect.name;
        velocityX = gameGetItemResDto.effect.velocityX;
        velocityY = gameGetItemResDto.effect.velocityY;
        jump = gameGetItemResDto.effect.jump;
        isTank = gameGetItemResDto.effect.isTank;

        if (gameGetItemResDto.effect.PLAMagnet > 0)
        {
            plaMagnet = 400;
        }

        changeTime = duration;
        isChange = true;
    }

}
