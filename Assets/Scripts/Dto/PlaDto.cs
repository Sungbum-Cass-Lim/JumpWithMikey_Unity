using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameGetPlaReqDto : BaseReqDto
{
    public int floor;
    public int occurrenceIdx;
}

public class GameGetPlaResDto : BaseResDto
{
    public int score;
}
