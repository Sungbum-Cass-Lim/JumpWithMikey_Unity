using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndReqDto : BaseReqDto
{
    public int score;
    public GameLog[] gameLog;
}

public class GameEndResDto : BaseResDto
{
    public int score;
}
