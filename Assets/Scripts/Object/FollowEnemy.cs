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

    public int maxDistance = 0;
    public int playerDistance = 0;
    public bool isJump = false;

    private float gravity = 1.2f;

    [Header("Floor")]
    public int curFloor = 0;

    private void FixedUpdate()
    {
        
        //실제 위치 이동 부분
        Move();

        transform.position = new Vector2(moveX, moveY);

        if(isJump)
        {
            //다음 올라갈 위치 선정
            moveY -= enemyVelocityY * Time.deltaTime;
            enemyVelocityY += this.gravity * Time.deltaTime * 50;
        }
        else
        {
            FollowerJump();
        }
    }

    public override void Initialize()
    {
        moveX = transform.position.x;
        moveY = transform.position.y;
    }

    private void Move()
    {
        if (dir.x < 0) //TODO: 공식화 필요
        {
            curPlatformIdx = (int)Mathf.Ceil((transform.position.x + 3.15f) / 0.7f);
            enemySpriter.flipX = true;
        }
        else
        {
            curPlatformIdx = (int)Mathf.Floor((transform.position.x + 3.15f) / 0.7f);
            enemySpriter.flipX = false;
        }

        var nextPlatformIdx = curPlatformIdx + (int)dir.x;

        //curPlatformIdx: -1, 10
        if ((curPlatformIdx >= moveRadius.Length || curPlatformIdx < 0) && isRotate == false)
        {
            isRotate = true;

            if (moveRadius[dir.x > 0 ? 0 : moveRadius.Length - 1] != 0)
                moveX = 3.5f * dir.x;
            else
                dir.x *= -1;

            isRotate = false;
        }

        //curPlatformIdx: 0 ~ 9
        else if (nextPlatformIdx >= 0 && nextPlatformIdx < moveRadius.Length && moveRadius[nextPlatformIdx] == 0)
        {
            isRotate = false;
            dir.x *= -1;
        }

        moveX += enemyVelocityX * dir.x * Time.deltaTime * 0.7f;
    }

    private void FollowerJump()
    {
        if (GameMgr.Instance.gameScore < 1000)
            playerDistance = 1;
        else
            playerDistance = 1;

        var Distance = GameMgr.Instance.player.passFloor - playerDistance - curFloor;

        //중간에 플랫폼이 원하는 갯수 이하일 때 리턴
        if (Distance < maxDistance)
            return;

        //TODO: 경공술 중일떄 예외 처리
        isJump = true;

        if (Distance < 1)
            Distance = 1;

        switch(Distance)
        {
            case 1:
                enemyVelocityY = -20;
                curPlatformIdx = curPlatformIdx + 1;
                break;
            case 2:
                enemyVelocityY = -27;
                curPlatformIdx = curPlatformIdx + 2;
                break;
            case 3:
                enemyVelocityY = -34;
                curPlatformIdx = curPlatformIdx + 3;
                break;
            default:
                enemyVelocityY = -34;
                curPlatformIdx = curPlatformIdx + 3;
                break;
        }
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
