using Cysharp.Threading.Tasks;

namespace TournamentSDKUnity
{
    public interface IInvoker
    {
        public UniTask<TResponse> ExcuteAsync<TResponse>(ICommand Message);
    }
}