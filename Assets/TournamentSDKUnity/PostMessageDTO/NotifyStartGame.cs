using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TournamentSDKUnity
{
    public class NotifyStartGame : ICommand
    {
        private string message;
        private Dictionary<string, string> data = new Dictionary<string, string>();
        public NotifyStartGame()
        {
            this.message = "sendStartGame";
        }
        
        public NotifyStartGame(string pid)
        {
            this.message = "sendStartGame";
            data.Add("pid", pid);
        }

        public void Excute()
        {
    
        }

        public async UniTask<TResult> ExcuteAsync<TResult>(string endPoint, string secretKey)
        {
            var json = new JObject();
            
            if (this.data.Count > 0)
            {
                json = JObject.FromObject(new { message = this.message, data = this.data });
            }
            else
            {
                json = JObject.FromObject(new { message = this.message });
            }
           
            var result = await PostMessage.SendPostMessage(json.ToString(Formatting.None));
            return JsonConvert.DeserializeObject<TResult>(result);
        }
    }
}