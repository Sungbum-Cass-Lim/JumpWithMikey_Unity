using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject playerCharacter;
    public Animator PlayerAnimator;

    public Vector2 Dir;
    public float MoveSpeed;



    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayerAnimator.SetBool("MoveStart", true);

            this.GetComponent<SpriteRenderer>().flipX = true;
            Dir = Vector2.left;
        }
        else if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayerAnimator.SetBool("MoveStart", true);

            this.GetComponent<SpriteRenderer>().flipX = false;
            Dir =  Vector2.right;
        }

        transform.Translate(Dir * MoveSpeed * Time.deltaTime);
    }
}
