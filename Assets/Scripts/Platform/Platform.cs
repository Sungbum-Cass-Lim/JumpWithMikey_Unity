using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    None = 0,
    Pla,
    Item,
    Fire
}

public class Platform : MonoBehaviour
{
    public BoxCollider2D boxCollider2D;

    public GameObject vending;
    public GameObject can;
    public GameObject pla;
    public GameObject item;
    public GameObject fire;

    private PlatformGenerator parentPlatform;
    public int platformLevel;
    public int platformIdx;

    private void LateUpdate()
    {
        if (GameMgr.Instance.Player.curFloor - 2 > platformLevel)
            ObjectPoolMgr.Instance.ReleasePool(gameObject);
    }

    public void Initialize(PlatformGenerator parentPlatform, ObjectType objectType)
    {
        vending.SetActive(false);
        can.SetActive(false);
        pla.SetActive(false);
        item.SetActive(false);
        fire.SetActive(false);

        if (Random.Range(0, 10) >= 9)
            vending.SetActive(true);
        if (Random.Range(0, 10) >= 9)
            can.SetActive(true);

        switch (objectType)
        {
            case ObjectType.Pla:
                pla.SetActive(true);
                break;
            case ObjectType.Item:
                item.SetActive(true);
                break;
            case ObjectType.Fire:
                fire.SetActive(true);
                break;
        }

        this.parentPlatform = parentPlatform;
    }

    public float Top()
    {
        return boxCollider2D.bounds.max.y + 0.39f;
    }
}
