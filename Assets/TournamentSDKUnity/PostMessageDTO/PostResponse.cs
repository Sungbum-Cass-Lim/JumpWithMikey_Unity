using System.Collections.Generic;

namespace TournamentSDKUnity
{
    public class PostResponse
    {
        public bool isSuccess;
    }

    public class PostResponse<T> 
    {
        public string message;
        public Dictionary<string, T> data;
    }
}