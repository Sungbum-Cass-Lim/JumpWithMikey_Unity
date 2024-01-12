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

    public Stack<PlatformObj> curObjStack = new Stack<PlatformObj>();

    private bool isRelease = false;
    private PlatformGenerator parentPlatform;
    public int platformLevel;
    public int platformIdx;

    private void LateUpdate()
    {
        if (GameMgr.Instance.player.curFloor - 2 > platformLevel && isRelease == false)
        {
            isRelease = true;

            while(curObjStack.TryPop(out var obj))
            {
                ObjectPoolMgr.Instance.ReleasePool(obj.gameObject);
            }

            ObjectPoolMgr.Instance.ReleasePool(gameObject);
        }
    }

    public void Initialize(PlatformGenerator parentPlatform)
    {
        this.parentPlatform = parentPlatform;
        this.isRelease = false;
    }

    public float Top()
    {
        return boxCollider2D.bounds.max.y + 0.39f;
    }
}
