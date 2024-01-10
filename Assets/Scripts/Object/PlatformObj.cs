using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObj : MonoBehaviour
{
    private void OnEnable()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {

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
