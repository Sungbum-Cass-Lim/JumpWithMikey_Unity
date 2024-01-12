using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowEnemy : PlatformObj
{
    public SpriteRenderer enemySpriter;

    public int[] moveRadius;
    public int curPlatformIdx = 0;

    public Vector2 dir;
    public float enemyVelocityX;
    public float enemyVelocityY;
    public float moveX { set; get; }
    public float moveY { set; get; }
    private bool isRotate = false;

    public int platformDistance = 0;
    public bool isJump = false;

    private float gravity = 1.2f;

    private void FixedUpdate()
    {
        //실제 위치 이동 부분
        Move();

        transform.position = new Vector2(moveX, moveY);
    }

    private void Move()
    {
        if (dir.x < 0) //TODO: 공식화 필요
        {
            curPlatformIdx = (int)Mathf.Ceil((transform.position.x + 3.15f) / 0.7f);
            enemySpriter.flipX = false;
        }
        else
        {
            curPlatformIdx = (int)Mathf.Floor((transform.position.x + 3.15f) / 0.7f);
            enemySpriter.flipX = true;
        }

        var nextPlatformIdx = curPlatformIdx + (int)dir.x;

        //curPlatformIdx: -1, 10
        if ((curPlatformIdx >= moveRadius.Length || curPlatformIdx < 0) && isRotate == false)
        {
            isRotate = true;

            if (moveRadius[dir.x > 0 ? 0 : moveRadius.Length - 1] != 0)
                moveX = 3.5f * -dir.x;
            else
                dir.x *= -1;
        }

        //curPlatformIdx: 0 ~ 9
        else if (nextPlatformIdx >= 0 && nextPlatformIdx < moveRadius.Length && moveRadius[nextPlatformIdx] == 0)
        {
            isRotate = false;
            dir.x *= -1;
        }

        moveX += enemyVelocityX * dir.x * Time.deltaTime * 0.7f;
    }

    private void FollowerJump(float force)
    {
        if (GameMgr.Instance.gameScore < 1000)
            platformDistance = 2;
        else
            platformDistance = 1;

        var Distance = GameMgr.Instance.player.passFloor - platformDistance - curPlatformIdx;
    }

    protected override void PlayerTouch(PlayerController player)
    {

    }

    protected override void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.TryGetComponent<Platform>(out var platform))
        {

        }

        base.OnCollisionEnter2D(other);
    }
}
