using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : PlatformObj
{
    public override void Initialize()
    {
        transform.localPosition = SpawnPos;
    }

    protected override void PlayerTouch(PlayerController player)
    {
        if (player.isDie == false)
        {
            player.playerCharacter.transform.eulerAngles = Vector3.forward * -90 * Mathf.PI * player.dir.x;
            player.playerVelocityY = -15;

            //TODO: Log

            //TODO: Send GameEndReq
            Debug.Log("player Die");
            player.isDie = true;
        }
    }
}
