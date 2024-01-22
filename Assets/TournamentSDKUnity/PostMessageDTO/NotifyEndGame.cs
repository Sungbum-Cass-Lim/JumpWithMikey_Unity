using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TournamentSDKUnity
{
    public class NotifyEndGame : ICommand
    {
        private string message;
        private Dictionary<string, int> data = new Dictionary<string, int>();
        
        public NotifyEndGame(int score)
        {
            this.message = "sendEndGame";
            data.Add("score", score);
        }

        public void Excute()
        {
    
        }

        public async UniTask<TResult> ExcuteAsync<TResult>(string endPoint, string secretKey)
        {
            var json = JObject.FromObject(new { message = this.message, data = this.data });
            var result = await PostMessage.SendPostMessage(json.ToString(Formatting.None));
            return JsonConvert.DeserializeObject<TResult>(result);
        }
    }
}