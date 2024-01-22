using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;

namespace TournamentSDKUnity
{
    public class StartPVPGame : ICommand
    {
         private readonly string pathName = "game/pvp/start";
        private readonly Func<UniTask> _command;

        private string token;
        private string playerType;
        
        public StartPVPGame(string token, string playerType)
        {
            this.token = token;
            this.playerType = playerType;
        }

        public async UniTask<TResult> ExcuteAsync<TResult>(string endPoint, string secretKey)
        {
            var parameters = new Dictionary<string, string>
            {
                { "token", token },
                { "ip", "127.0.0.1" },
                {"playerType", playerType}
            };

            var json = JsonConvert.SerializeObject(parameters);
            Console.WriteLine("unitySDK endPoint :: " + endPoint + pathName);
            Console.WriteLine("unitySDK Token :: " + json);

            byte[] jsonByte = Encoding.UTF8.GetBytes(json);

            using (UnityWebRequest request = new UnityWebRequest(endPoint + pathName, "POST"))
            {
                request.downloadHandler = new DownloadHandlerBuffer();
                request.uploadHandler = new UploadHandlerRaw(jsonByte);

                request.SetRequestHeader("accept", "application/json");
                request.SetRequestHeader("Authorization", "Basic " + secretKey);
                request.SetRequestHeader("Content-Type", "application/json");

                try
                {
                    var responseBody = await request.SendWebRequest();
                    Debug.Log("unitySDK response :: " + responseBody.downloadHandler.text);
                    return JsonConvert.DeserializeObject<TResult>(responseBody.downloadHandler.text);
                }
               
                catch (UnityWebRequestException ex)
                {
                    string[] split = ex.Message.Split( Environment.NewLine.ToCharArray(),
                        StringSplitOptions.RemoveEmptyEntries); ;

                    var errorResponse =
                        JsonConvert.DeserializeObject<RestResponseDTO>(split[split.Length - 1]);
                    
                    throw new TournamentErrorException()
                    {
                        status = int.Parse(errorResponse.status.ToString()),
                        code = errorResponse.code,
                        message = errorResponse.message
                    };
                }
            }
        }
    }
}