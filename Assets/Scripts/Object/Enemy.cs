using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isDie = false;

    public SpriteRenderer enemySpriter;
    public Rigidbody2D rigidbody2D;

    public float enemyVelocityX;
    public float enemyVelocityY;
    public Vector2 dir;
    public float moveX;
    public float moveY;

    private float gravity = 1.8f;

    private void Update()
    {
        if (dir.x == 0)
            enemySpriter.flipX = false;
        else
            enemySpriter.flipX = true;

        //���� ��ġ �̵� �κ�
        transform.position = new Vector2(moveX, transform.position.y);
    }

    private void FixedUpdate()
    {
        Move();

        //���� �ö� ��ġ ����
        enemyVelocityY += this.gravity * Time.deltaTime * 50;
        moveY -= enemyVelocityY * Time.deltaTime;
    }

    private void Move()
    {
        //�÷��̾� ȭ�� ��� �� �̵�
        if (transform.position.x <= -3.5f)
        {
            moveX = 3.5f;
        }
        else if (transform.position.x >= 3.5f)
        {
            moveX = -3.5f;
        }


        moveX += enemyVelocityX * (dir.x - 64 / 2) * Time.deltaTime * 0.05f;
    }
}
