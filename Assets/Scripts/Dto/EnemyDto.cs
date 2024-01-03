using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameKillEnemyReqDto : BaseReqDto
{
    public int floor;
    public int occurrenceIdx; // enemy index(only for log)
    public int mikeyStatus; // if mikey is jumping, this value is 1
}

public class GameKillEnemyResDto : BaseResDto
{
    public int score;
}