using System;
using System.IO;
using System.Threading;
using TechArt.Base.ClassLibrary.Job;
using TechArt.Base.Interfaces;

namespace TechArt.Base.ClassLibrary.Queue
{
    public class DebuggerQueue : QueueBase<DebuggerJob>
    {
        private TextWriter _textWriter;
        private object _StreamLock = new object();
        private ErrorFormater _ErrorFormater;

        public DebuggerQueue(TextWriter textWriter, ErrorFormater errorFormater)
        {
            this._ErrorFormater= errorFormater;
            this._textWriter = textWriter;
        }

        public DebuggerQueue(TextWriter textWriter)
        {
            this._ErrorFormater = ErrorFormater.GetDefaultInstance();
            this._textWriter = textWriter;
        }

        protected override void PerformJob(DebuggerJob job)
        {
            string message = "";
            bool isException = false;
            try
            {
                Monitor.Enter(_StreamLock);
              
                if (job.Data is string)
                {
                    message = job.Data.ToString();

                }
                else if (job.Data is Exception)
                {
                    isException = true;
                    message = _ErrorFormater.GetErrorLog(job.Data as Exception);
                }

                //Log to file
                _textWriter.Write(message + "\r\n");
                _textWriter.Flush();
            }
            finally
            {
                Monitor.Exit(_StreamLock);
            }


            if (isException)
            {
                //TODO Logger to Database
            }
        }
    }
}