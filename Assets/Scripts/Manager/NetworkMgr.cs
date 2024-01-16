using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BestHTTP.SocketIO3;
using BestHTTP.SocketIO3.Transports;
using Newtonsoft.Json;

public class NetworkMgr : SingletonComponentBase<NetworkMgr>
{
    private string address = "";

    private SocketManager serverManager;
    private Socket serverSocket;

    private Action connetCallBack;

    protected override void InitializeSingleton()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
#if USE_WEBGL_LOCAL_CASS
        address = "ws://127.0.0.1:3001/jumpwithmikey/socket.io"; //본점
#elif USE_WEBGL_DEV
        address = "https://dev-tournament.playdapp.com/jumpwithmikey/socket.io";
#elif USE_WEBGL_QA
        address = "https://qa-tournament.playdapp.com/jumpwithmikey/socket.io";
#elif USE_WEBGL_PROD
        address = "https://prod-tournament.playdapp.com/jumpwithmikey/socket.io";
#endif
#elif UNITY_EDITOR
        address = "ws://127.0.0.1:3001/jumpwithmikey/socket.io"; //본점
#endif
    }

    public void OnConnect(Action _connectCallBack)
    {
        Debug.Log("Connecting...");
        SocketOptions options = new SocketOptions();

        options.ConnectWith = TransportTypes.WebSocket;
        options.AutoConnect = false;

        options.Auth = (serverManager, serverSocket) => new { token = UserManager.Instance.userInfo.token, uid = UserManager.Instance.userInfo.uid, appVersion = Application.version, appName = Application.productName };

        serverManager = new SocketManager(new Uri(address), options);
        serverSocket = serverManager.GetSocket("/");

        InitCallBack();

        connetCallBack = _connectCallBack;

        serverManager.Open();
    }

    private void InitCallBack()
    {
        serverManager.Socket.On(SocketIOEventTypes.Connect, () =>
        {
            Debug.Log("Connected!");
            connetCallBack.Invoke();
        });

        serverManager.Socket.On(SocketIOEventTypes.Disconnect, () =>
        {
            Debug.Log("Disconnected!");
        });

        // The argument will be an Error object.
        serverManager.Socket.On<Error>(SocketIOEventTypes.Error, (resp) =>
        {
            Debug.Log($"Error msg: {resp.message}");
        });

        serverManager.Socket.On<string>("collectItem", (res) =>
        {
            OnCollectItem(res);
        });

        serverManager.Socket.On<string>("expiredDuration", (res) =>
        {
            OnExpiredDuration(res);
        });

        serverManager.Socket.On<string>("dead", (res) =>
        {
            Debug.Log(res);
        });

        serverManager.Socket.On("onWhiteList", () =>
        {

        });
    }

    #region Start Communication
    public void RequestStartGame(GameStartReqDto data)
    {
        Send<string>("start", data, ResponseStartGame);
    }
    private void ResponseStartGame(string res)
    {
        //Debug.Log($"[Recv : StartGame] => {res}");
        var gameStartRes = JsonConvert.DeserializeObject<GameStartResDto>(res);

        GameMgr.Instance.Initialize(gameStartRes.height, gameStartRes.platforms, gameStartRes.map, gameStartRes.result, gameStartRes.renderCondition);
    }
    #endregion

    #region Climb Communication
    public void RequestClimb(ClimbReqDto data)
    {
        Send<string>("climb", data, ResponseClimb);
    }
    public void ResponseClimb(string res)
    {
        //Debug.Log($"[Recv : Climb] => {res}");
        var climbResDto = JsonConvert.DeserializeObject<ClimbResDto>(res);

        if (climbResDto.platforms != null)
        {
            foreach (var arr in climbResDto.platforms)
                GameMgr.Instance.platforms.Enqueue(arr);

            if(GameMgr.Instance.platforms.Count < 10)
            {
                //TODO: Get More Platform
            }

            GameMgr.Instance.GameLogic.platformGererate();
        }
    }
    #endregion

    #region Item Communication
    public void RequestItem(GameGetItemReqDto data)
    {
        Send("collectItem", data);
    }

    private void OnCollectItem(string res)
    {
        //Debug.Log($"[Recv : Climb] => {res}");
        var gameGetItemResDto = JsonConvert.DeserializeObject<GameGetItemResDto>(res);

        CharacterMgr.Instance.SetCharacter(gameGetItemResDto, gameGetItemResDto.duration);

        //TODO: Change Time
    }

    private void OnExpiredDuration(string res)
    {
        //Debug.Log($"[Recv : Climb] => {res}");
        var gameExpiredResDto = JsonConvert.DeserializeObject<GameExpiredResDto>(res);

        CharacterMgr.Instance.Reset();
    }
    #endregion

    #region renderCat Communication
    public void RequestRenderCat(GameRenderCatReqDto data)
    {
        Send("renderCat", data);
    }
    #endregion

    #region BumpFloor Communication
    public void RequestBumpFloor(BumpUpReqDto data)
    {
        Send("bumpFloor", data);
    }
    #endregion

    #region End Communication
    private void OnDead(string res)
    {

    }

    public void RequestEndGame(GameEndReqDto data)
    {
        UserInfo user = UserManager.Instance.userInfo;

        data.uid = user.uid;
        data.tid = user.tid;
        data.gameId = user.gameId;
        data.pid = user.pid;
        data.token = user.token;

        var jsonData = JsonConvert.SerializeObject(data);

        Debug.Log($"[Send : EndGame] => " + jsonData);

        Send<string>("end", data, ResponseEndGame);
    }
    private void ResponseEndGame(string res)
    {
        Debug.Log($"[Recv : EndGame] => {res}");
        var endRes = JsonUtility.FromJson<GameEndResDto>(res);

#if !UNITY_EDITOR && UNITY_WEBGL
        WebNetworkMgr.SendEndGame(endRes.score);
#endif

        if (endRes.result)
        {
            GameMgr.Instance.GameOver();

            serverSocket.Disconnect();
            serverManager.Close();

            serverSocket = null;
            serverManager = null;
        }
    }
    #endregion

    private void Send(string message, BaseReqDto data)
    {
        if (serverSocket == null)
            return;

        UserInfo user = UserManager.Instance.userInfo;

        data.uid = user.uid;
        data.tid = user.tid;
        data.gameId = user.gameId;
        data.pid = user.pid;
        data.token = user.token;

        var jsonData = JsonConvert.SerializeObject(data);

        //Debug.Log($"[Send : {message}] => " + jsonData);
        serverSocket.Emit(message, jsonData);
    }

    private void Send<T>(string message, BaseReqDto data, Action<T> callBack)
    {
        if (serverSocket == null)
            return;

        UserInfo user = UserManager.Instance.userInfo;

        data.uid = user.uid;
        data.tid = user.tid;
        data.gameId = user.gameId;
        data.pid = user.pid;
        data.token = user.token;

        var jsonData = JsonConvert.SerializeObject(data);

        //Debug.Log($"[Send : {message}] => " + jsonData);
        serverSocket.EmitCallBack(callBack, message, jsonData);
    }
}
