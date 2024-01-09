using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGetItemReqDto : BaseReqDto
{
    public int score;
}

public class GameGetItemResDto : BaseResDto
{
    public int score;
    public CharacterEffect effect;
    public float duration;
}

public class GameExpiredResDto : BaseResDto { }

public class CharacterEffect
{
    public string name;
    public float velocityX;
    public float velocityY;
    public int jump;
    public bool isTank;
    public int PLAMagnet; // server give 1 is client value 400
}