using System;
using System.IO;
using System.Threading;
using TechArt.Base.ClassLibrary;
using TechArt.Base.ClassLibrary.Job;
using TechArt.Base.ClassLibrary.Queue;
using TechArt.Base.Interfaces;

namespace TechArt.Base.FW_Shared
{
    public class Debugger : IDebugger, IDisposable
    {
        private static object _syncLock = new object();

        private DebuggerQueue _debuggerQueue;
        private bool _queueInitialized;
        private string filePath = @"c://1.txt";
        private TextWriter _textWriter;
        private ErrorFormater _ErrorFormater;

        public Debugger(bool IsAsynchronous)
        {
            _textWriter = File.CreateText(filePath);
            _ErrorFormater = ErrorFormater.GetDefaultInstance();

            if (IsAsynchronous)
            {
                //initialize queue
                _debuggerQueue = new DebuggerQueue(_textWriter, _ErrorFormater);

                _debuggerQueue.MaxTaskCount = 1;
                _debuggerQueue.Name = "Debugger Queue";
                _debuggerQueue.Debugger = this;
                _debuggerQueue.Start();

                //set queueInitialized to ture
                _queueInitialized = true;
            }
            else
            {
                _queueInitialized = false;
            }
        }

        public void AddMsg(string message)
        {
            WriteLog(message);
        }

        public void AddError(Exception exception)
        {

            WriteLog(exception);
        }

        private void WriteLog(object data)
        {
            if (_queueInitialized)
            {
                _debuggerQueue.AddJob(new DebuggerJob() { Data = data, ID = Guid.NewGuid() });
            }
            else
            {
                string message = "";
                bool isException = false;
                try
                {
                    Monitor.Enter(_syncLock);
                    if (data is string)
                    {
                        message = data.ToString();
                       
                    }
                    else if ( data is Exception)
                    {
                        isException = true;
                        message = _ErrorFormater.GetErrorLog(data as Exception);
                    }

                    //Log to file
                    _textWriter.Write(message + "\r\n");
                    _textWriter.Flush();
                }
                finally
                {
                    Monitor.Exit(_syncLock);
                }

                if (isException)
                {
                    //TODO Logger to Database
                }
            }
        }

        public void Dispose()
        {
            if (_queueInitialized)
            {
                _debuggerQueue.Stop();
                _debuggerQueue.Dispose();
            }
        }
    }
}