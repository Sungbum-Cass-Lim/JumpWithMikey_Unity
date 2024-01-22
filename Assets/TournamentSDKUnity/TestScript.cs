using System;
using TournamentSDKUnity;
using UnityEngine;

using UnityEngine.UI;

public class TestScript : MonoBehaviour
{
    
    public Text taskResultTxt;
    public Text cheatValue;

    public int cheatInt = 5;
    
    private ResponseToken thisGameToken = new ResponseToken();
    private string thisGamePID = "";

    private string playerType;
    
    // Start is called before the first frame update
    public async void Start()
    {
        cheatValue.text = cheatInt.ToString();

        var init = await TournamentUnitySDK.Instance.Init("dev", "883b613684351d7aa8b7eef44d48748c", SetNotiChannel);
        taskResultTxt.text = String.Format($"response loading :: {init}");
    }
    
    
    public async void OnClickRequestToken()
    {
        
        var token = await TournamentUnitySDK.Instance.RequestToken();
        this.thisGameToken = token;
        taskResultTxt.text = $"get token Success :: {token.pid}, {token.uid}";
      
    }

    public async void OnClickGameStart()
    {
        var gameStart = await TournamentUnitySDK.Instance.SingleGameStart();
        if (gameStart.status == 200)
        {
            thisGamePID = gameStart.pid;
            taskResultTxt.text = "game start :: " + gameStart.pid + ", " + gameStart.bestScore;
            
        }
        else
        {
            taskResultTxt.text = "game start fail :: " + gameStart.code + ", " + gameStart.message;
        }
    }

    public async void OnNotifyGameStart()
    {
        var notify = await TournamentUnitySDK.Instance.NotifyGameStart();
        taskResultTxt.text = "notify game start :: " + notify.isSuccess;
    }
    
    
    public async void OnClickEndSingleGame()
    {
        try
        {
            var aa = await TournamentUnitySDK.Instance.SingleGameEnd(new GameEndBuilder().SetToken(thisGameToken.token)
                .SetScore(100).SetPlayTime(600).SetCheating(0).SetRecord("").SetPid(thisGamePID));
            Debug.Log("end aa");
            taskResultTxt.text = String.Format($"single game Start :: {aa.status} , {aa.message}");
            Debug.Log(String.Format($"single game Start :: {aa.status} , {aa.message}"));
            Debug.Log(("end bb"));
        }
        catch (TournamentErrorException ex)
        {
            SetExceptionText(ex);
        }
    }
    
    public async void OnNotifyGameEnd()
    {
        var notify = await TournamentUnitySDK.Instance.NotifyGameEnd(1000);
        taskResultTxt.text = "notify game end :: " + notify.isSuccess;
    }


    public async void OnRequetMatch()
    {
        var match = await TournamentUnitySDK.Instance.RequestMatch();
        this.playerType = match.playerType;
        taskResultTxt.text = "notify game end :: " + match.playerType + ", " + match?.hostInfo?.hostId;
    }

    public async void OnClickStartPVPGame()
    {
        var gameStart = await TournamentUnitySDK.Instance.PVPGameStart(playerType);
        Debug.Log(String.Format($"single game Start :: {gameStart.status} , {gameStart.message}"));
        if (gameStart.status == 200)
        {
            thisGamePID = gameStart.pid;
            taskResultTxt.text = "game start :: " + gameStart.pid;
            
        }
        else
        {
            taskResultTxt.text = "game start fail :: " + gameStart.code + ", " + gameStart.message;
        }
    }
    
    public async void OnClickEndPvpGame()
    {
        try
        {
            var aa = await TournamentUnitySDK.Instance.PVPGameEnd(new GameEndBuilder().SetToken(thisGameToken.token)
                .SetScore(100).SetPlayTime(600).SetCheating(0).SetRecord("").SetReplay("{\"replay\":[{\"wave\":1,\"hit\":0.9,\"ticks\":12345},{\"wave\":2,\"hit\":0.5,\"ticks\":13345}]}").SetPid(thisGamePID).SetPlayerType(this.playerType));
            Debug.Log("end aa");
            taskResultTxt.text = String.Format($"single game Start :: {aa.status} , {aa.message}");
            Debug.Log(String.Format($"single game Start :: {aa.status} , {aa.message}"));
            Debug.Log(("end bb"));
        }
        catch (TournamentErrorException ex)
        {
            SetExceptionText(ex);
        }
    }

    
    
    public void OnClickMinusValue()
    {
        cheatInt -= 1;
        cheatValue.text = cheatInt.ToString();
    }

    public void OnClickPlusValue()
    {
        cheatInt += 1;
        cheatValue.text = cheatInt.ToString();
    }
    
    void SetExceptionText(TournamentErrorException ex) {
        taskResultTxt.text = string.Format($"errormessage :: {ex.code}, {ex.message}");
    }

    void SetNotiChannel(string notify)
    {
        taskResultTxt.text = notify;
    }
    
}
