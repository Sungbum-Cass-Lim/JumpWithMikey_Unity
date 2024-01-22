using Cysharp.Threading.Tasks;

namespace TournamentSDKUnity
{
    public class RestInvoker : IInvoker
    {
        private ICommand postCommand;
        private string endPoint = "";
        private string secretKey = "";
        
        public RestInvoker(string zone, string secretKey)
        {
            if (zone ==  "dev")
            {
                this.endPoint = "https://dev-tournament.playdapp.com/api/v3/";
            } else if (zone == "qa")
            {
                
            } else if (zone == "prod")
            {
                
            }
            this.secretKey = secretKey;
        }

        public UniTask<TResponse> ExcuteAsync<TResponse>(ICommand Message)
        {
            this.postCommand = Message;
            return this.postCommand.ExcuteAsync<TResponse>(endPoint, secretKey);
        }
    }
}