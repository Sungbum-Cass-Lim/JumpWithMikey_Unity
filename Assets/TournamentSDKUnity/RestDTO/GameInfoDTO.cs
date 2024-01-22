namespace TournamentSDKUnity
{
    public class GameInfo
    {
        public string gameId;
        public int tid;
        public int uid;
        public string pid;
        public GameInfo(string gameId, int tid, int uid, string pid){}
    }

    public class GameInfoBuilder
    {
        private GameInfo request;

        public GameInfoBuilder()
        {
            request = new GameInfo("", 0, 0, "");
        }
        
        public GameInfoBuilder SetGameId(string gameId)
        {
            request.gameId = gameId;
            return this;
        } 
        public GameInfoBuilder SetTID(int tid)
        {
            request.tid = tid;
            return this;
        }
 
        public GameInfoBuilder SetUID(int uid)
        {
            request.uid = uid;
            return this;
        }

        public GameInfoBuilder SetPID(string pid)
        {
            request.pid = pid;
            return this;
        }

        public string ToString()
        {
            return string.Format($"gameId={request.gameId}&tid={request.tid}&uid={request.uid}&pid={request.pid}");
        }
    }
}