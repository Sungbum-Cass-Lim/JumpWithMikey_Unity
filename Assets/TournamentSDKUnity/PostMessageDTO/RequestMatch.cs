using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;


namespace TournamentSDKUnity
{
    public class HostInfo
    {
        public string hostId;
        public int uid;
        public string pid;
        public string gameId;
        public string nickName;
        public string imageUrl;
    }

    public class MatchResult
    {
        public string playerType;
        public HostInfo hostInfo = new HostInfo();
    }
    
    
    public class RequestMatch : ICommand
    {
        private string message;
        
        public RequestMatch()
        {
            this.message = "requestMatch";
        }

        public void Excute()
        {
    
        }

        public async UniTask<TResult> ExcuteAsync <TResult>(string endPoint, string secretKey)
        {
            #if !UNITY_EDITOR
            var json = JObject.FromObject(new { message = this.message });
            var result = await PostMessage.SendPostMessage(json.ToString(Formatting.None));
            Debug.Log("client match result :: " + result);
            return JsonConvert.DeserializeObject<TResult>(result);
#else
            MatchResult matchResult = new MatchResult()
            {
                playerType = "host",
                hostInfo = null
            };
            var result = JsonConvert.SerializeObject(matchResult);
            return JsonConvert.DeserializeObject<TResult>(result);
#endif
        }
    }
}