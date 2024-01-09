using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : PlatformObj
{
    protected override void PlayerTouch(PlayerController player)
    {
        player.playerCharacter.transform.eulerAngles = Vector3.forward * -90 * Mathf.PI * player.dir.x;
        player.playerVelocityY = -15;

        //TODO: Log

        //TODO: Send GameEndReq
    }
}
