using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TournamentSDKUnity
{
    public partial class TournamentUnitySDK : MonoBehaviour
    {
        public async UniTask<MatchResult> RequestMatch()
        {
            try
            {
                var matchResult = await postInvoker.ExcuteAsync<MatchResult>(new RequestMatch());
                Debug.Log(String.Format($"single game Start :: {matchResult.playerType}, {matchResult.hostInfo}"));
                return matchResult;
            }
            catch (TournamentErrorException ex)
            {
                return new MatchResult()
                {
                    playerType = "host",
                    hostInfo = null
                };
            }
        }

        public async UniTask<StartPVPGameResult> PVPGameStart(string playerType)
        {
            try
            {
                var startResult = await restInvoker.ExcuteAsync<StartPVPGameResult>(
                    new StartPVPGame(this.thisGameToken.token, playerType));
                Debug.Log(String.Format($"pvp game Start :: {startResult.pid}, {startResult.code}, {startResult.status}"));
                thisGamePID = startResult.pid;
                return startResult;
            }
            catch (TournamentErrorException ex)
            {
                return new StartPVPGameResult(ex);
            }
        }
        
        public async UniTask<GameEndResult> PVPGameEnd(GameEndBuilder endData)
        {
            try
            {
                var endReulst = await restInvoker.ExcuteAsync<GameEndResult>(new EndPVPGame(endData));
       
                Debug.Log(String.Format($"pvp game End :: {endReulst.status} , {endReulst.message}"));
                Debug.Log(("end bb"));
                return endReulst;
            }
            catch (TournamentErrorException ex)
            {
                return new GameEndResult(ex);
            }
        }
    }
}