using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Title = 0,
    Game,
    GameEnd,
    Retry,
}

public class GameMgr : SingletonComponentBase<GameMgr>
{
    [Header("Current Game Info")]
    public GameState gameState = GameState.Title;
    public PlayerController player { private set; get; }
    public int gameScore { set; get; }

    [Header("Importent Game Component")]
    public GameLogic GameLogic = null;
    public TitleLogic TitleLogic = null;

    public int height { set; get; }
    public Queue<int[]> platforms = new Queue<int[]>();
    public int[] enemyMap { private set; get; }
    public bool result { private set; get; }
    public int renderCondition { private set; get; }
    public bool isReceiveStart { private set; get; }

    protected override void InitializeSingleton()
    {
        gameState = GameState.Title;

        GameLogic = FindObjectOfType<GameLogic>();
    }

    public override void ResetSingleton()
    {
        gameState = GameState.Retry;
        player = null;
        gameScore = 0;

        GameLogic = null;
        TitleLogic = null;

        height = 0;
        platforms = new();
        enemyMap = null;
        result = false;
        renderCondition = 0;
        isReceiveStart = false;
    }

    //GameStartData Init
    public void Initialize(int height, int[][] platforms, int[] enemyMap, bool result, int renderCondition)
    {
        this.height = height;

        foreach (var arr in platforms)
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
        SoundMgr.Instance.PlayBGM(BGMType.bgm_jumpmikey_ingame);
        player = ObjectPoolMgr.Instance.Load<PlayerController>(PoolObjectType.player, "player");

        GameLogic.Initialize(player);
    }

    public void GameOver()
    {
        gameState = GameState.GameEnd;

        SoundMgr.muteBackup = SoundMgr.Instance.audioMute;

        SoundMgr.Instance.SetAllMute(true);
    }
    public void SetScore(int score)
    {
        gameScore += score;
        GameLogic.scoreText.text = $"{GameMgr.Instance.gameScore}";
    }
}
