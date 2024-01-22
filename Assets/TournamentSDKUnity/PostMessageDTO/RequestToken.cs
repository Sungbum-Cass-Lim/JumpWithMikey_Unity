using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace TournamentSDKUnity
{
    public class ResponseToken
    {
        public int uid;
        public int tid;
        public string gameId;
        public string token;
        public string pid;
    }
    
    public class RequestToken : ICommand
    {
        private string message;
        
        public RequestToken()
        {
            this.message = "requestToken";
        }

        public void Excute()
        {
    
        }

        public async UniTask<TResult> ExcuteAsync <TResult>(string endPoint, string secretKey)
        {
#if !UNITY_EDITOR
            var json = JObject.FromObject(new { message = this.message });
            var result = await PostMessage.SendPostMessage(json.ToString(Formatting.None));
        
           
                return JsonConvert.DeserializeObject<TResult>(result);
           
            
            
            
#else
            Debug.Log("aaaa");

            ResponseToken token = new ResponseToken()
            {
                uid = 2,
                tid = 2,
                pid = "3",
                gameId = "yokaiwatch",
                token =
                    "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1aWQiOjEsInRvdXJuYW1lbnRVaWQiOjEsImdhbWVJZCI6InNwb29reWJvbWIifQ._1h1Q1-tObbwrxmzhyer6QLN_4hi-V62EKRHsWnrU4Q"

            };

            var result = JsonConvert.SerializeObject(token);
            return JsonConvert.DeserializeObject<TResult>(result);
#endif
           
        }
    }
}