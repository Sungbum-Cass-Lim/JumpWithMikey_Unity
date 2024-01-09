using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformObj : MonoBehaviour
{
    protected virtual void PlayerTouch()
    {

    }

    private void OnCollisionEnter2D(Collision2D Other)
    {
        if(Other.gameObject.CompareTag("Player"))
        {
            PlayerTouch();
        }
    }
}
