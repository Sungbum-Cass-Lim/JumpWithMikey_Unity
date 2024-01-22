using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using AOT;
using Cysharp.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;

namespace TournamentSDKUnity
{
    public class PostMessage : MonoBehaviour
     {
         [DllImport("__Internal")]
        private static extern void initialize(string zone);
        
        [DllImport("__Internal")]
        private static extern void SendFrontPostMessage(string postMessage);
        
        [DllImport("__Internal")]
        private static extern void PostOnMessage(Action<string> callBack);
        
        private static UniTaskCompletionSource<string> utcs;

        public static void Initialize(string zone)
        {
            #if !UNITY_EDITOR
            
            initialize(zone);
            PostOnMessage(DelegateOnMessageEvent);
#endif
        }
        
        public static UniTask<string> SendPostMessage (string postMessage)
        {
            utcs = new UniTaskCompletionSource<string>();
            SendFrontPostMessage(postMessage);
            return utcs.Task;
        }
        
        [MonoPInvokeCallback(typeof(Action<string>))]
        public static void DelegateOnMessageEvent(string msg)
        {
            Debug.Log($"DelegateOnMessageEvent : {msg}");

            if (msg.Contains("mute"))
            {
                Debug.Log("Call Mute :: " + msg);
                TournamentUnitySDK.Instance.getNotiChannel()?.Invoke(msg);
            }
            else if (msg.Contains("onRestart"))
            {
                Debug.Log("Call onRestart :: " + msg);
                TournamentUnitySDK.Instance.getNotiChannel()?.Invoke(msg);
            }
            utcs.TrySetResult(msg);
        }
    }
}