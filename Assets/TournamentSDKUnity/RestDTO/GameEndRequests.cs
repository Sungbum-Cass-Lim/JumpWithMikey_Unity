using Newtonsoft.Json;

namespace TournamentSDKUnity
{
    public class GameEndRequest
    {
        public string token;
        public string pid;
        public int playtime;
        public int cheating;
        public string cheatingDetail;
        public int score;
        public string record;
        public string replay;
        public string playerType;
        public GameEndRequest()
        {
        }

        public GameEndRequest(string token, string pid, int playtime, int cheating, string cheatingDetail, int score,
            string record, string replay, string playerType)
        {
            
        }
    }

    public class GameEndBuilder
    {
        private GameEndRequest request;

        public GameEndBuilder()
        {
            request = new GameEndRequest("", "", 0, 0, "", 0, "", "", "");
        }

        public GameEndBuilder SetToken(string token)
        {
            request.token = token;
            return this;
        }
        
        public GameEndBuilder SetPid(string pid)
        {
            request.pid = pid;
            return this;
        }
        public GameEndBuilder SetPlayTime(int playTime)
        {
            request.playtime = playTime;
            return this;
        }
        public GameEndBuilder SetCheating(int cheating)
        {
            request.cheating = cheating;
            return this;
        }
        public GameEndBuilder SetCheatingDetail(string cheatingDetail)
        {
            request.cheatingDetail = cheatingDetail;
            return this;
        }
        public GameEndBuilder SetScore(int score)
        {
            request.score = score;
            return this;
        }

        public GameEndBuilder SetRecord(string record)
        {
            request.record = record;
            return this;
        }
        
        public GameEndBuilder SetReplay(string replay)
        {
            request.replay = replay;
            return this;
        }

        public GameEndBuilder SetPlayerType(string playerType)
        {
            request.playerType = playerType;
            return this;
        }

        public string ToString()
        {
            return JsonConvert.SerializeObject(request);
        }
    }
}