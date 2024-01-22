using Cysharp.Threading.Tasks;

namespace TournamentSDKUnity
{
    public interface ICommand
    {
        UniTask<TResponse> ExcuteAsync<TResponse>(string endPoint, string secretKey = null);
       
    }
}