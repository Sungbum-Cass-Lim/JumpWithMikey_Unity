using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : PlatformObj
{
    public override void Initialize()
    {
        transform.localPosition = SpawnPos;
    }

    protected override void PlayerTouch(PlayerController player)
    {
        //TODO: Log

        GameGetItemReqDto gameGetItemReqDto = new();
        gameGetItemReqDto.score = GameMgr.Instance.gameScore;

        NetworkMgr.Instance.RequestItem(gameGetItemReqDto);

        gameObject.SetActive(false);
    }
}
