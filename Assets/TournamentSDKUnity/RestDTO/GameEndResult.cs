namespace TournamentSDKUnity
{
    public class GameEndResult : RestResponseDTO
    {
        public GameEndResult()
        {
        }

        public GameEndResult(string token, string pid, int playtime, int cheating, string cheatingDetail, int score,
            string record)
        {
            
        }
        public GameEndResult(TournamentErrorException ex)
        {
            
            status = ex.status;
            code = ex.code;
            message = ex.message;
        }
    }

    public class IOGameEndResult : RestResponseDTO
    {
        public int remainBalance;
        public string entryCurrency;
        public int entryCharge;
        public int reward;
    }
    
    public class FreeGameEndResult : RestResponseDTO
    {
        public int reward;
    }
}