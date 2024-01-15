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
            Debug.Log(parentPlatform);
        }
    }

    protected virtual void PlayerTouch(PlayerController player)
    {

    }

    protected virtual void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            PlayerTouch(player);
        }
    }
}
