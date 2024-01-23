using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace TournamentSDKUnity
{
    public class PVPGameEndData
    {
        public bool result;
        public double score;
        public Host host = new Host();
        public Challenger challenger = new Challenger();
    }
    
    public class Prize
    {
        public Host host;
        public Challenger challenger;
    }

    public class Host
    {
        public double remainTime = 0;
    }

    public class Challenger
    {
        public double oppentScore = 0;
        public string rewardType = "";
        public double reward = 0;
    }
    
    public class NotifyPVPEndGame : ICommand
    {
        private string message;
        private PVPGameEndData data = new();
        
        public NotifyPVPEndGame(PVPGameEndData endData)
        {
            this.message = "sendEndGame";
            data = endData;
        }
        
        public void Excute()
        {
    
        }

        public async UniTask<TResult> ExcuteAsync<TResult>(string endPoint, string secretKey)
        {
            var json = JObject.FromObject(new { message = this.message, data = this.data });
            Debug.Log("aaa :: " + json.ToString(Formatting.None));
            var result = await PostMessage.SendPostMessage(json.ToString(Formatting.None));
            
            return JsonConvert.DeserializeObject<TResult>(result);
        }
    }
}