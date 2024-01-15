using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpUpReqDto : BaseReqDto
{
    public int floor;
    public float posX;
    public float posY;
    public float platformTop;
    public int score;
}

public class GameDeadResDto : BaseReqDto
{
    public bool result;
    public string reason;
}
