using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TournamentSDKUnity;

[System.Serializable]
public class UserInfo
{
    public int uid = 0;
    public int tid = 0;
    public string gameId = "";
    public string token = "";
    public string pid = "";
}

public class UserManager : SingletonComponentBase<UserManager>
{
    public UserInfo userInfo;

    protected override void InitializeSingleton(){}

    public override void ResetSingleton()
    {
        userInfo = new();
    }

    public void OnUser(ResponseToken token)
    {
        userInfo = new UserInfo();
        userInfo.uid = token.uid;
        userInfo.pid = token.pid;
        userInfo.tid = token.tid;
        userInfo.gameId = token.gameId;
        userInfo.token = token.token;
    }


}
