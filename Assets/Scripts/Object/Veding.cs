using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Veding : PlatformObj
{
    public override void Initialize(Platform platform)
    {
        base.Initialize(platform);

        transform.localPosition = spawnPos;
    }
}
