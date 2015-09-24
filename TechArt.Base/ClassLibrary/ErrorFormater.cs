using System;
using System.Collections.Generic;

namespace TechArt.Base.ClassLibrary
{
    public class ErrorFormater
    {
        private ErrorFormater()
        {
            Timestamp = true;
            Message = true;
            StackTrace = true;
            AppDomain = true;
            Machine = true;
        }

        public static ErrorFormater GetDefaultInstance()
        {
            return new ErrorFormater();
        }

        public bool Timestamp { get; set; }

        public bool Message { get; set; }

        public bool StackTrace { get; set; }

        public bool AppDomain { get; set; }

        public bool Machine { get; set; }

        public string GetErrorLog(Exception exception)
        {
            string result = "";
            string formatter = "";
            List<Object> datas = new List<object>();

            if (Timestamp)
            {
                formatter += "Timestamp:{0}\r\n";
                datas.Add(DateTime.Now.ToLocalTime().ToString());
            }
            if (Message)
            {
                formatter += "Message:{0}\r\n";
                datas.Add(exception.Message);
            }
            if (AppDomain)
            {
                formatter += "AppDomain:{0}\r\n";
                datas.Add(System.AppDomain.CurrentDomain.ToString());
            }
            if (Machine)
            {
                formatter += "Machine:{0}\r\n";
                datas.Add(System.Environment.MachineName);
            }
            if (StackTrace)
            {
                formatter += "StackTrace:{0}\r\n";
                datas.Add(exception.StackTrace);
            }

            return String.Format(formatter, datas.ToArray());
        }
    }
}