using System;

namespace Api.LogDealer.Models
{
    public enum eLogTypeMessage
    {
        LogLevelInfo = 0,
        LogLevelWarning = 1,
        LogLevelError = 2,
        LogLevelDebug = 3
    }
    public class LogMessageModel
    {
        public int LogId { get; private set; }
        public string Data { get; set; }
        public eLogTypeMessage TypeMessage { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public string ApplicationName{ get; set; }
        public string MethodName { get; set; }
        public string IPName { get; set; }
        public string ServerName { get; set; }
        public int IdRemoteUser { get; set; }       
        public DateTime CreationDate { get; set; }
        public DateTime ModifierDate { get; set; }        
    }
}
