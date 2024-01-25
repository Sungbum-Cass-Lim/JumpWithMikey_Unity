using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour
{
    public GameLogic gameLogic;
    private int platformLevel = 0;

    private int platformBin = 7;
    private int itemsBin = 0b11000;
    private int enemyBin = 0b100000;
    private int enemyDirectionBin = 64;

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
        int[] currentPlatforms = null;

        if (GameMgr.Instance.platforms.TryDequeue(out var firstIdx))
        {
            currentPlatforms = firstIdx;
        }

        for (int i = 0; i < currentPlatforms.Length; i++)
        {
            Platform createdPlatform = null;

            int platformIdx = platformBin & currentPlatforms[i];
            int itemsIdx = itemsBin & currentPlatforms[i];
            int enemyIdx = enemyBin & currentPlatforms[i];
            int enemyDirIdx = 0;

            if (enemyIdx == 32)
                enemyDirIdx = enemyDirectionBin & currentPlatforms[i];

            if (platformIdx == 0)
                continue;

            else
            {
                //ÇÃ·§Æû »ý¼º
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

                //ÇÃ·§Æû Ãþ ÀÔ·Â
                createdPlatform.platformLevel = this.platformLevel;
                createdPlatform.platformIdx = i;
                createdPlatform.transform.SetParent(transform);

                // ÇÃ·§Æû À§Ä¡ ÁöÁ¤
                if (makeCount != 0)
                {
                    createdPlatform.transform.position = new Vector2(GameConfig.INTERVAL_X * i + GameConfig.MIN_X, GameConfig.INTERVAL_Y * stackPosY + GameConfig.MIN_Y);
                }
                else
                {
                    createdPlatform.transform.position = new Vector2(GameConfig.INTERVAL_X * i + GameConfig.MIN_X, GameConfig.MIN_Y);   
                }

                //Àû »ý¼º
                if(enemyIdx == 32)
                {
                    //Enemy Spawn
                    var enemy = ObjectPoolMgr.Instance.Load<Enemy>(PoolObjectType.Object, "Enemy");
                    enemy.Initialize(createdPlatform);

                    enemy.enemyVelocityX = 1.0f;
                    enemy.enemyVelocityY = 14;
                    enemy.isDie = false;
                    enemy.dir.x = enemyDirIdx == 64 ? 1 : -1;
                    enemy.moveRadius = currentPlatforms;
                    enemy.curPlatformIdx = i;

                    enemy.moveX = createdPlatform.transform.position.x;
                    enemy.moveY = createdPlatform.transform.position.y + 0.6f;

                    createdPlatform.curObjStack.Push(enemy);

                    //RenderCat Communication
                    var renderCatDto = new GameRenderCatReqDto();
                    renderCatDto.floor = platformIdx;
                    NetworkMgr.Instance.RequestRenderCat(renderCatDto);
                }

                //Vending
                if (Random.Range(0, 10) >= 9)
                {
                    var vending = ObjectPoolMgr.Instance.Load<PlatformObj>(PoolObjectType.Object, "Vending");

                    createdPlatform.curObjStack.Push(vending);
                    vending.transform.SetParent(createdPlatform.transform);
                    vending.Initialize(null);
                }

                //Can
                if (Random.Range(0, 10) >= 9)
                {
                    PlatformObj can = null;

                    switch(Random.Range(0, 2))
                    {
                        case 0:
                            can = ObjectPoolMgr.Instance.Load<PlatformObj>(PoolObjectType.Object, "GreenCan");
                            break;
                        case 1:
                            can = ObjectPoolMgr.Instance.Load<PlatformObj>(PoolObjectType.Object, "BlueCan");
                            break;
                        //case 2:
                        //    can = ObjectPoolMgr.Instance.Load<Transform>(PoolObjectType.Object, "HorizontalCan");
                        //    break;
                    }

                    createdPlatform.curObjStack.Push(can);
                    can.transform.SetParent(createdPlatform.transform);
                    can.Initialize(null);
                }

                //ÇÃ·§Æû Init
                switch (itemsIdx)
                {
                    case 8:
                        Pla pla = ObjectPoolMgr.Instance.Load<Pla>(PoolObjectType.Object, "Pla");
                        createdPlatform.curObjStack.Push(pla);
                        pla.transform.SetParent(createdPlatform.transform);
                        pla.Initialize(createdPlatform);
                        break;

                    case 16:
                        Item item = ObjectPoolMgr.Instance.Load<Item>(PoolObjectType.Object, "Item");
                        createdPlatform.curObjStack.Push(item);
                        item.transform.SetParent(createdPlatform.transform);
                        item.Initialize(createdPlatform);
                        break;

                    case 24:
                        Fire fire = ObjectPoolMgr.Instance.Load<Fire>(PoolObjectType.Object, "Fire");
                        createdPlatform.curObjStack.Push(fire);
                        fire.transform.SetParent(createdPlatform.transform);
                        fire.Initialize(createdPlatform);
                        break;
                }

                createdPlatform.Initialize(this);

                currentPlatforms[i] = platformIdx;
            }
        }

        GameMgr.Instance.GameLogic.platformDataList.Add(currentPlatforms);
        stackPosY++;
    }
}
