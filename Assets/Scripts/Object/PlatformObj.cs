using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObj : MonoBehaviour
{
    public Platform parentPlatform { private set; get; }
    public Vector2 spawnPos;

#nullable enable
    public virtual void Initialize(Platform? platform)
    {
        if (platform != null)
        {
            parentPlatform = platform;
        }
    }

    protected virtual void PlayerTouch(PlayerController player)
    {

    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            if (player.isDie == true)
                return;

            PlayerTouch(player);
        }
    }
}
