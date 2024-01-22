using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Debug = UnityEngine.Debug;

namespace TournamentSDKUnity
{
    public class SetLoading : ICommand
    {
        private string message;
        private Dictionary<string, bool> data = new Dictionary<string, bool>();

        public SetLoading(bool isLoading)
        {
            this.message = "loading";
            this.data.Add("show", isLoading);
        }
        
        public void Excute()
        {
    
        }

        public async UniTask<TResult> ExcuteAsync <TResult>(string endPoint, string secretKey)
        {
            var json = JObject.FromObject(new { message = this.message, data = this.data });
            var aa = await PostMessage.SendPostMessage(json.ToString(Formatting.None));
            Debug.Log("[Excute] :: " + aa);
            return JsonConvert.DeserializeObject<TResult>(aa);
        }
    }
}