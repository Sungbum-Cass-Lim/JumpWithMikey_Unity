using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public bool isDie = false;

    [Header("player Parts")]
    public GameObject playerCharacter;
    public GameObject ChangeEffect;
    public Text ChangeTimeText;
    private Coroutine ChangeEffectCorutine;

    [Header("Componenet")]
    public SpriteRenderer playerSpriter;
    public Animator playerAnimator;
    public List<AnimatorOverrideController> playerCharacterList;
    public Rigidbody2D rigidbody2D;
    public BoxCollider2D boxCollider2D;

    [Header("Move")]
    public float playerVelocityX = 0;
    public float playerVelocityY = 0;
    public Vector2 dir;
    private float moveX = 0;
    private float moveY = -3.45f;

    [Header("Jump")]
    public float rotationForce = 0;
    public float curRotation = 0;
    public bool isJump = false;
    public int jumpCount = 0;
    private float gravity = 1.2f;

    [Header("Floor")]
    public int curFloor = 0;
    public int passFloor = 0;
    public int curPlatformIdx = 0;

    private void Start()
    {
        CharacterMgr.Instance.Initialize();
    }

    private void Update()
    {
        #region JumpKey
        if (isJump == false)
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow) || (Input.GetMouseButtonDown(0) && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 0))
            {
                dir = Vector2.left;
                playerSpriter.flipX = true;

                Jump();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) || (Input.GetMouseButtonDown(0) && Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= 0))
            {
                dir = Vector2.right;
                playerSpriter.flipX = false;

                Jump();
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow) || (Input.GetMouseButtonUp(0) && Camera.main.ScreenToWorldPoint(Input.mousePosition).x < 0))
            isJump = false;
        else if (Input.GetKeyUp(KeyCode.RightArrow) || (Input.GetMouseButtonUp(0) && Camera.main.ScreenToWorldPoint(Input.mousePosition).x >= 0))
            isJump = false;
        #endregion

        #region PlayerMissingFeet
        if (rigidbody2D.position.y <= Camera.main.transform.position.y - Camera.main.orthographicSize && isDie == false)
        {
            isDie = true;

            var gameLog = new GameLog();
            gameLog.a = "d";
            gameLog.s = GameMgr.Instance.gameScore;
            gameLog.uf = curFloor;
            gameLog.of = 0;
            gameLog.oi = curPlatformIdx;
            gameLog.n = "JM003";
            gameLog.unt = Extension.GetUnixTimeStamp(DateTime.UtcNow);
            GameLogic.LogPush(gameLog);

            GameLogic.PlayerDie();
        }
        #endregion

        #region PlayerMove
        if (dir.x != 0)
        {
            Move();

            //실제 위치 이동 부분
            transform.position = new Vector2(moveX, moveY);

            if (curRotation < 0 && isDie == false)
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

            if (playerVelocityY > 20)
                playerVelocityY = 20;

            moveY -= playerVelocityY * Time.deltaTime;
        }

        //Debug.Log(transform.position);
        #endregion

        if (dir.x != 0)
        {
            playerAnimator.SetBool("MoveStart", true);
            GameMgr.Instance.GameLogic.TutorialOff();
        }

        if (CharacterMgr.Instance.changeTime > 0)
        {
            CharacterMgr.Instance.changeTime -= Time.deltaTime;
            ChangeTimeText.text = Mathf.Ceil(CharacterMgr.Instance.changeTime).ToString();
        }
    }

    public void CharacterAnimChange(int type)
    {
        if (type != 0)
        {
            if (ChangeEffectCorutine != null)
                StopCoroutine(ChangeEffectCorutine);

            ChangeEffectCorutine = StartCoroutine(ChangeEffectActive());

            ChangeTimeText.gameObject.SetActive(true);
            ChangeTimeText.text = CharacterMgr.Instance.changeTime.ToString();
        }
        else
        {
            ChangeTimeText.gameObject.SetActive(false);
        }

        playerAnimator.runtimeAnimatorController = playerCharacterList[type];
    }

    private void Jump()
    {
        if (jumpCount < CharacterMgr.Instance.jump && isDie == false)
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
        if (rigidbody2D.position.x <= -3.5f)
        {
            moveX = 3.5f;
        }
        else if (rigidbody2D.position.x >= 3.5f)
        {
            moveX = -3.5f;
        }

        playerVelocityX = (640 / (100 * CharacterMgr.Instance.velocityX)) * dir.x;
        moveX += playerVelocityX * Time.deltaTime * 0.65f;
    }

    private IEnumerator ChangeEffectActive()
    {
        yield return null;
        ChangeEffect.SetActive(false);
        ChangeEffect.SetActive(true);

        yield return YieldCacheMgr.WaitForSeconds(1.0f);

        ChangeEffect.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Platform>(out var platform))
        {
            if (dir.x != 0 && curPlatformIdx != platform.platformIdx && isDie == false)
            {
                var bumpReqDto = new BumpUpReqDto();

                bumpReqDto.floor = platform.platformLevel;
                bumpReqDto.platformTop = platform.Top();
                bumpReqDto.posX = rigidbody2D.position.x;
                bumpReqDto.posY = rigidbody2D.position.y;
                bumpReqDto.score = GameMgr.Instance.gameScore;

                NetworkMgr.Instance.RequestBumpFloor(bumpReqDto);
            }

            if (transform.position.y > platform.Top() && passFloor < platform.platformLevel)
                passFloor = platform.platformLevel;

            if (moveY + 0.35f > platform.Top() && playerVelocityY > 0 && isDie == false)
            {
                moveY = platform.Top() + 0.01f;

                playerVelocityY = 0;
                jumpCount = 0;

                if (curFloor < platform.platformLevel)
                {
                    var score = (platform.platformLevel - curFloor) * 10;
                    GameMgr.Instance.gameScore += score;

                    ClimbReqDto climbReqDto = new();

                    climbReqDto.floorsUp = platform.platformLevel - curFloor;
                    climbReqDto.floor = platform.platformLevel;
                    climbReqDto.score = GameMgr.Instance.gameScore;

                    curPlatformIdx = platform.platformIdx;
                    curFloor = platform.platformLevel;

                    NetworkMgr.Instance.RequestClimb(climbReqDto);

                    var gameLog = new GameLog();
                    gameLog.a = "c";
                    gameLog.s = score;
                    gameLog.uf = platform.platformLevel;
                    gameLog.of = 0;
                    gameLog.oi = platform.platformIdx;
                    gameLog.n = "";
                    gameLog.unt = Extension.GetUnixTimeStamp(DateTime.UtcNow);
                    GameLogic.LogPush(gameLog);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.TryGetComponent<Platform>(out var platform))
        {
            if (dir.x != 0 && curPlatformIdx != platform.platformIdx && isDie == false)
            {
                var bumpReqDto = new BumpUpReqDto();

                bumpReqDto.floor = platform.platformLevel;
                bumpReqDto.platformTop = platform.Top();
                bumpReqDto.posX = transform.position.x;
                bumpReqDto.posY = transform.position.y;
                bumpReqDto.score = GameMgr.Instance.gameScore;

                NetworkMgr.Instance.RequestBumpFloor(bumpReqDto);
            }

            if (transform.position.y > platform.Top() && passFloor < platform.platformLevel)
                passFloor = platform.platformLevel;

            if (moveY + 0.35f > platform.Top() && playerVelocityY > 0 && isDie == false)
            {
                moveY = platform.Top() + 0.01f;

                playerVelocityY = 0;
                jumpCount = 0;
            }
        }
    }
}
