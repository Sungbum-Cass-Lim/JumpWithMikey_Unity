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

    private void OnCollisionEnter2D(Collision2D Other)
    {
        if(Other.gameObject.TryGetComponent<PlayerController>(out var player))
        {
            PlayerTouch(player);
        }
    }
}
