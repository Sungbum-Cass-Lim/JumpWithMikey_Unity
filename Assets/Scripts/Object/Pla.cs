using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pla : PlatformObj
{
    public int addScore;
    public float MagenetForce;

    private float maxDistance = 0.007f;
    private bool isEat = false;
    private float distance = 0;

    private void Update()
    {
        if (isEat == false && CharacterMgr.Instance.plaMagnet > 0)
        {
            distance = Vector3.Distance(transform.position, GameMgr.Instance.player.transform.position);

            if (distance < CharacterMgr.Instance.plaMagnet * maxDistance)
            {
                transform.position = Vector2.Lerp(transform.position, GameMgr.Instance.player.transform.position, MagenetForce * Time.deltaTime);
            }
        }
    }

    public override void Initialize()
    {
        transform.localPosition = SpawnPos;
        isEat = false;
        distance = 0;
    }

    protected override void PlayerTouch(PlayerController player)
    {
        //TODO: Change ObjPool

        isEat = true;

        GameMgr.Instance.gameScore += 100;
        gameObject.SetActive(false);
    }

    //TODO: Log
}
