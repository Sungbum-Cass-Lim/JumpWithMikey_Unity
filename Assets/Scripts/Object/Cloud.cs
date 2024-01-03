using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float moveSpeed;

    public void Initilize()
    {
        transform.position = new Vector2(Random.Range(4.5f, 6f), Random.Range(3f, 4.5f));
        moveSpeed = Random.Range(1f, 2f);
    }

    public void Update()
    {
        transform.Translate(Vector2.left * Time.deltaTime * moveSpeed);

        //TODO: �ϵ��ڵ� �������� �ٲ� �ʿ� ����
        if(transform.position.x <= -5.0f)
        {
            Initilize();
        }
    }
}
