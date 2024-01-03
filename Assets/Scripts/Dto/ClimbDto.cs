using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbReqDto : BaseReqDto
{
    public int floorsUp;
    public int floor;
    public int score;
}

public class ClimbResDto : BaseResDto
{
    public string pid;
    public bool isPaidPlatform;
    public int[][] platform;
    public int[] enemyMap;
    public int height;
}
