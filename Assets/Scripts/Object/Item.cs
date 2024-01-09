using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : PlatformObj
{
    protected override void PlayerTouch()
    {
        //TODO: Log

        GameGetItemReqDto gameGetItemReqDto = new();
        gameGetItemReqDto.score = GameMgr.Instance.GameScore;

        NetworkMgr.Instance.RequestItem(gameGetItemReqDto);

        gameObject.SetActive(false);
    }
}
