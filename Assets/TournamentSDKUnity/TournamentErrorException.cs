using System;
using System.Runtime.Serialization;

namespace TournamentSDKUnity
{
    public class TournamentErrorException : Exception
    {
        public TournamentErrorException() : base()
        {
        }

        public TournamentErrorException(string message) : base(message)
        {
        }

        public TournamentErrorException(string message, Exception e) : base(message, e)
        {
        }

        public TournamentErrorException(SerializationInfo info, StreamingContext cxt) : base(info, cxt)
        {
        }
        
        public int status { get; set; }
        public int code { get; set; }
        public string message { get; set; }
    }
}