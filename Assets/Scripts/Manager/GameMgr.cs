using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameState
{
    Title = 0,
    Game,
    GameEnd,
}

public class GameMgr : SingletonComponentBase<GameMgr>
{
    private static bool muteBackup = false;

    [Header("Current Game Info")]
    public GameState gameState = GameState.Title;
    public PlayerController Player { private set; get; }
    public int GameScore { set; get; }

    [Header("Importent Game Component")]
    public GameLogic GameLogic = null;
    public TitleLogic TitleLogic = null;

    public int height { private set; get; }
    public Queue<int[]> platforms = new Queue<int[]>();
    public int[] enemyMap { private set; get; }
    public bool result { private set; get; }
    public int renderCondition { private set; get; }
    public bool isReceiveStart { private set; get; }

    protected override void InitializeSingleton()
    {
        SoundMgr.Instance.SetMute(muteBackup);
        gameState = GameState.Title;

        GameLogic = FindObjectOfType<GameLogic>();
    }

    //GameStartData Init
    public void Initialize(int height, int[][]platforms, int[]enemyMap, bool result, int renderCondition)
    {
        this.height = height;

        foreach(var arr in platforms)
            this.platforms.Enqueue(arr);
        
        this.enemyMap = enemyMap;
        this.result = result;
        this.renderCondition = renderCondition;
        this.isReceiveStart = true;

        GameStart();
    }

    public void GameStart()
    {
        gameState = GameState.Game;
        Player = ObjectPoolMgr.Instance.Load<PlayerController>(PoolObjectType.Player, "Player");

        GameLogic.Initialize(Player);
    }

    public void GameOver()
    {
        gameState = GameState.GameEnd;
        GameScore = 0;
        Player = null;

        muteBackup = SoundMgr.isMute;

        SoundMgr.Instance.SetMute(true);
    }
}
