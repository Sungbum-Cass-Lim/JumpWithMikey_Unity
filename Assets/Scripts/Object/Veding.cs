using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veding : PlatformObj
{
    public override void Initialize()
    {
        transform.localPosition = SpawnPos;
    }
}
