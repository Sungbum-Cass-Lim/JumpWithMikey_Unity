using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameLogic gameLogic;
    private int platformLevel = 0;

    private int platformCount = 10;
    private int enemyCount = 0;

    private int platformBin = 7;
    private int itemsBin = 0b11000;
    private int enemyBin = 0b100000;
    private int enemyDirectionBin = 64;

    private int deletePlatform = 0;

    private float MinX = -3.15f;
    private float MinY = -4.25f;
    private float intervalX = 0.7f;
    private float intervalY = 2.8f;
    private int stackPosY = 0;

    public void Initialize()
    {
        int Count = GameMgr.Instance.platforms.Count;
        for (int i = 0; i < Count; i++)
        {
            MakePlatform(platformLevel);
            platformLevel++;
        }
    }

    public void MakePlatform(int makeCount)
    {
        int[] currentPlatform = null;

        if (GameMgr.Instance.platforms.TryDequeue(out var firstIdx))
        {
            currentPlatform = firstIdx;
        }

        for (int i = 0; i < currentPlatform.Length; i++)
        {
            Platform createdPlatform = null;

            int platformIdx = platformBin & currentPlatform[i];
            int itemsIdx = itemsBin & currentPlatform[i];
            int enemyIdx = enemyBin & currentPlatform[i];
            int enemyDirIdx = 0;

            if (enemyIdx == 32)
                enemyDirIdx = enemyDirectionBin & currentPlatform[i];

            if (platformIdx == 0)
                continue;

            else
            {
                //�÷��� ����
                switch(platformIdx)
                {
                    case 1:
                        createdPlatform = ObjectPoolMgr.Instance.Load<Platform>(PoolObjectType.Platform, "Platform1");
                        break;
                    case 2:
                        createdPlatform = ObjectPoolMgr.Instance.Load<Platform>(PoolObjectType.Platform, "Platform2");
                        break;
                    case 3:
                        createdPlatform = ObjectPoolMgr.Instance.Load<Platform>(PoolObjectType.Platform, "Platform3");
                        break;
                    case 4:
                        createdPlatform = ObjectPoolMgr.Instance.Load<Platform>(PoolObjectType.Platform, "Platform4");
                        break;
                }

                //�÷��� �� �Է�
                createdPlatform.platformLevel = this.platformLevel;

                // �÷��� ��ġ ����
                if (makeCount != 0)
                {
                    createdPlatform.transform.position = new Vector2(intervalX * i + MinX, intervalY * stackPosY + MinY);
                }
                else
                {
                    createdPlatform.transform.position = new Vector2(intervalX * i + MinX, MinY);   
                }

                //TODO: �� ����
                if(enemyIdx == 32)
                {

                }

                // �÷��� Init
                switch(itemsIdx)
                {
                    case 8:
                        createdPlatform.Initialize(this, ObjectType.Pla);
                        break;
                    case 16:
                        createdPlatform.Initialize(this, ObjectType.Item);
                        break;
                    case 24:
                        createdPlatform.Initialize(this, ObjectType.Fire);
                        break;
                    default:
                        createdPlatform.Initialize(this, ObjectType.None);
                        break;
                }
            }
        }

        stackPosY++;
    }
}
