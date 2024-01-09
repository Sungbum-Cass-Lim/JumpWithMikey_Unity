using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pla : PlatformObj
{
    public int addScore;

    //TODO: Magnet

    protected override void PlayerTouch()
    {
        //TODO: Change ObjPool

        GameMgr.Instance.GameScore += 100;
        gameObject.SetActive(false);
    }

    //TODO: Log
}
