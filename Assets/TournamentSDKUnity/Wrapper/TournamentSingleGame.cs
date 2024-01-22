using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace TournamentSDKUnity
{
     public partial class TournamentUnitySDK : MonoBehaviour
    {
        public async UniTask<PostResponse> NotifyGameStart()
        {
            try
            {
                var startResult = await postInvoker.ExcuteAsync<PostResponse>(new NotifyStartGame());
                Debug.Log(String.Format($"single game Start :: {startResult.isSuccess}"));
                return startResult;
            }
            catch (TournamentErrorException ex)
            {
                return new PostResponse()
                {
                    isSuccess = false
                };
            }
        }
        
        public async UniTask<PostResponse> NotifyGameEnd(int score)
        {
            try
            {
                var endResult = await postInvoker.ExcuteAsync<PostResponse>(new NotifyEndGame(score));
                Debug.Log(String.Format($"single game Start :: {endResult.isSuccess}"));
                return endResult;
            }
            catch (TournamentErrorException ex)
            {
                return new PostResponse()
                {
                    isSuccess = false
                };
            }
        }
        
        
        public async UniTask<StartSingleGameResult> SingleGameStart()
        {
            try
            {
                var startResult = await restInvoker.ExcuteAsync<StartSingleGameResult>(
                    new StartSingleGame(this.thisGameToken.token));
                Debug.Log(String.Format($"single game Start :: {startResult.pid} , {startResult.bestScore}"));
                thisGamePID = startResult.pid;
                return startResult;
            }
            catch (TournamentErrorException ex)
            {
                return new StartSingleGameResult(ex);
            }
        }
        
        public async UniTask<GameEndResult> SingleGameEnd(GameEndBuilder endData)
        {
            try
            {
                var endReulst = await restInvoker.ExcuteAsync<GameEndResult>(new EndSingleGame(endData));
       
                Debug.Log(String.Format($"single game Start :: {endReulst.status} , {endReulst.message}"));
                Debug.Log(("end bb"));
                return endReulst;
            }
            catch (TournamentErrorException ex)
            {
                return new GameEndResult(ex);
            }
        }


        public async UniTask<StartSingleGameResult> ServerlessSingleGameStart()
        {
            var token = await RequestToken();

            if (token.token != null)
            {
                return await SingleGameStart();
            }
            else
            {
                StartSingleGameResult errorResult = new StartSingleGameResult()
                {
                    status = 404,
                    message = "Can not get token"
                };
                return errorResult;
            } 
        }
    }
}