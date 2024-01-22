using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TournamentSDKUnity
{
    public class NotifyStartGame : ICommand
    {
        private string message;
        
        public NotifyStartGame()
        {
            this.message = "sendStartGame";
        }

        public void Excute()
        {

        }

        public async UniTask<TResult> ExcuteAsync<TResult>(string endPoint, string secretKey)
        {

            var json = JObject.FromObject(new { message = this.message });
            var result = await PostMessage.SendPostMessage(json.ToString(Formatting.None));
            return JsonConvert.DeserializeObject<TResult>(result);
        }
    }
}