using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace TournamentSDKUnity
{
    public class PostInvoker : IInvoker
    {
        private ICommand postCommand;
        private string zone = null;
        
        public PostInvoker(string endPoint = null)
        {
            this.zone = endPoint;
            PostMessage.Initialize(this.zone);
            
        }

        public UniTask<TResponse> ExcuteAsync<TResponse>(ICommand Message)
        {
            this.postCommand = Message;
            return this.postCommand.ExcuteAsync<TResponse>("");
        }
    }
}