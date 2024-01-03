using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpUpReqDto : BaseReqDto
{
    public int floor;
    public int posX;
    public int posY;
    public int platformTop;
    public int score;
}

public class GameDeadResDto : BaseReqDto
{
    public bool result;
    public string reason;
}
