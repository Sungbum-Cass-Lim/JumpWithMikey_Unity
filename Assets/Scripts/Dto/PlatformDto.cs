using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlatformReqDto : BaseReqDto
{
    public int floor;
    public int score;
}

public class GamePlatformResDto : BaseResDto
{
    public int HEIGHT;
    public int[] platforms;
    public int[] enemyMap;
}
