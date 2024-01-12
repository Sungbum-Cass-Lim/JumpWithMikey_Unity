using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Can : PlatformObj
{
    public override void Initialize()
    {
        transform.localPosition = SpawnPos;
    }
}
