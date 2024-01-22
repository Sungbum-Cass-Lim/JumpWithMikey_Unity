using System;
using Cysharp.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;


namespace TournamentSDKUnity
{
    public partial class TournamentUnitySDK : MonoBehaviour
    {
        private IInvoker postInvoker;
        private IInvoker restInvoker;
        private bool isLoading = false;

        private ResponseToken thisGameToken = new ResponseToken();
        private string thisGamePID = "";

        private Action<string> notifyChannel;

        public Action<string> getNotiChannel()
        {
            return notifyChannel;
        }

        private static TournamentUnitySDK _instance;

        public static TournamentUnitySDK Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = (TournamentUnitySDK)FindObjectOfType(typeof(TournamentUnitySDK));

                    if (FindObjectsOfType(typeof(TournamentUnitySDK)).Length > 1)
                    {
                        Debug.LogError("Already has more than 1 Tournament Singleton object");
                        return _instance;
                    }

                    if (_instance == null)
                    {
                        GameObject singleton = new GameObject();
                        _instance = singleton.AddComponent<TournamentUnitySDK>();
                        singleton.name = "(singleton) " + typeof(TournamentUnitySDK).ToString();

                        DontDestroyOnLoad(singleton);
                    }
                    else
                    {
                        Debug.Log("instance already created: " + _instance.gameObject.name);
                    }
                }

                return _instance;
            }
        }

        public TournamentUnitySDK()
        {
        }

        public async UniTask<bool> Init(string zone, string secretKey, Action<string> notifyChannelCallBack)
        {

            try
            {

                postInvoker = new PostInvoker(zone);
                restInvoker = new RestInvoker(zone, secretKey);
                this.notifyChannel = notifyChannelCallBack;
#if !UNITY_EDITOR
                var aa = await postInvoker.ExcuteAsync<PostResponse>(new SetLoading(false));
#endif
                return true;
            }
            catch (TournamentErrorException ex)
            {
                Debug.Log("[Error] :: " + ex.Message);
                return false;

            }
        }

        public async UniTask<ResponseToken> RequestToken()
        {
            Debug.Log("onClickToken");
            try
            {
                var tokenResult = await postInvoker.ExcuteAsync<ResponseToken>(new RequestToken());
                Debug.Log(
                    $"response token :: \nuid :: {tokenResult.uid}\ntid :: {tokenResult.tid}\ntoken :: {tokenResult.token}");
                this.thisGameToken = tokenResult;

                return this.thisGameToken;
            }
            catch (Exception ex)
            {
                Debug.Log("[RequestToken Error] :: " + ex);
                return null;
            }
        }
    }
}