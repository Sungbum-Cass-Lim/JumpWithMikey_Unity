using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Componenet")]
    public GameObject playerCharacter;
    public SpriteRenderer playerSpriter;
    public Animator playerAnimator;
    public List<AnimatorOverrideController> playerCharacterList;
    public Rigidbody2D rigidbody2D;

    private bool isRun = false;
    private Vector2 dir;
    private float moveX = 0;
    private float moveY = -3.45f;
    private float playerVelocityX = 0;
    private float playerVelocityY = 0;

    [Header("Jump")]
    public float rotationForce = 0;
    public float curRotation = 0;
    public bool isJump = false;
    private int jumpCount = 0;
    private float gravity = 1.2f;

    [Header("Floor")]
    public int curFloor = 0;

    private void Start()
    {
        CharacterMgr.Instance.Initialize();
    }

    private void Update()
    {
        if (isJump == false)
        {

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                dir = Vector2.left;
                playerSpriter.flipX = true;

                Jump();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                dir = Vector2.right;
                playerSpriter.flipX = false;

                Jump();
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow))
            isJump = false;
        else if (Input.GetKeyUp(KeyCode.RightArrow))
            isJump = false;


        if (dir.x != 0 && isRun == false)
        {
            isRun = true;

            playerAnimator.SetBool("MoveStart", true);
            GameMgr.Instance.GameLogic.TutorialOff();
        }
    }

    private void FixedUpdate()
    {
        if (isRun)
        {
            Move();

            //실제 위치 이동 부분
            rigidbody2D.MovePosition(new Vector2(moveX, moveY));

            if (curRotation < 0)
            {
                curRotation -= rotationForce * Time.deltaTime;
                playerCharacter.transform.eulerAngles = Vector3.forward * dir.x * curRotation;

                if (curRotation < -360)
                {
                    curRotation = 0;
                    playerCharacter.transform.eulerAngles = Vector3.zero;
                }
            }

            //다음 올라갈 위치 선정
            playerVelocityY += this.gravity * Time.deltaTime * 50;
            moveY -= playerVelocityY * Time.deltaTime;
        }
    }

    public void CharacterAnimChange(int type)
    {
        playerAnimator.runtimeAnimatorController = playerCharacterList[type];
    }

    private void Jump()
    {
        if (jumpCount < CharacterMgr.Instance.jump)
        {
            playerVelocityY = CharacterMgr.Instance.velocityY * 0.75f;

            transform.eulerAngles = Vector3.zero;
            curRotation = -rotationForce * Time.deltaTime;

            isJump = true;
            jumpCount++;
        }
    }

    private void Move()
    {
        //플레이어 화면 벗어날 시 이동
        if (transform.position.x <= -3.5f)
        {
            moveX = 3.5f;
        }
        else if (transform.position.x >= 3.5f)
        {
            moveX = -3.5f;
        }

        playerVelocityX = (640 / (100 * CharacterMgr.Instance.velocityX)) * dir.x;
        moveX += playerVelocityX * Time.deltaTime * 0.75f;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.TryGetComponent<Platform>(out var platform))
        {
            if (moveY > platform.Top() && playerVelocityY > 0)
            {
                moveY = platform.Top() + 0.01f;

                playerVelocityY = 0;
                jumpCount = 0;

                if (curFloor < platform.platformLevel)
                {
                    GameMgr.Instance.GameScore += (platform.platformLevel - curFloor) * 10;

                    ClimbReqDto climbReqDto = new();

                    climbReqDto.floorsUp = platform.platformLevel - curFloor;
                    climbReqDto.floor = platform.platformLevel;
                    climbReqDto.score = GameMgr.Instance.GameScore;

                    curFloor = platform.platformLevel;
                    NetworkMgr.Instance.RequestClimb(climbReqDto);

                    //TODO: Log 남기기
                }
            }
        }
    }
}
