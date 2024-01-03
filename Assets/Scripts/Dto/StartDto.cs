using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStartReqDto : BaseReqDto { }

public class GameStartResDto : BaseResDto
{
    public int bestScore;
    public int renderCondition;
    public string pid;
    public int height;
    public int[][] platforms;
    public int[] map;
}
