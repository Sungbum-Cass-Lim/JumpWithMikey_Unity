using Unity.VisualScripting;

namespace TournamentSDKUnity
{
   
        public class StartSingleGameResult : RestResponseDTO
        {
            public string pid { get; set; }
            public string bestScore { get; set; }

            public StartSingleGameResult()
            {
            }

            public StartSingleGameResult(TournamentErrorException ex)
            {
            
                status = ex.status;
                code = ex.code;
                message = ex.message;
            }
        }
    
        public class StartFreeGameResult : RestResponseDTO
        {
            public string pid { get; set; }
            public int startTime { get; set; }
            public int scoreRate { get; set; }
            public int max { get; set; }
        }
    
        public class StartIOGameResult : RestResponseDTO
        {
            public string pid { get; set; }
            public string nickName { get; set; }
            public string imageUrl { get; set; }
        }
    
        public class StartPVPGameResult : RestResponseDTO
        {
            public string pid { get; set; }
            public StartPVPGameResult()
            {
            }
            public StartPVPGameResult(TournamentErrorException ex)
            {
            
                status = ex.status;
                code = ex.code;
                message = ex.message;
            }
        }
    
}