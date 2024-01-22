using System;
using System.Text;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine.Networking;

namespace TournamentSDKUnity
{
    public class EndSingleGame : ICommand
    {
        private readonly string pathName = "game/single/finish";
        private readonly Func<UniTask> _command;

        private GameEndBuilder request;
        
        public EndSingleGame(GameEndBuilder endRequest) 
        {
            Console.WriteLine("request :: " + endRequest.ToString());
            request = endRequest;
        }


        public async UniTask<TResult> ExcuteAsync<TResult>(string endPoint, string secretKey)
        {
            var json = request.ToString();;
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
                    
                    if (responseBody.responseCode == 200 || responseBody.responseCode == 204)
                    {
                        RestResponseDTO res = new RestResponseDTO()
                        {
                            code = 200,
                            message = "success",
                            status = 200
                        };
                        
                        string resJson = JsonConvert.SerializeObject(res);
                        
                        return JsonConvert.DeserializeObject<TResult>(resJson);
                    }
                    else
                    {
                        return JsonConvert.DeserializeObject<TResult>(responseBody.downloadHandler.text);
                    }
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