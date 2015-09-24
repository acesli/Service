using System;
using TechArt.Base.Interfaces;

namespace TechArt.Base.ClassLibrary.Job
{
    public class DebuggerJob:IJob
    {
        public Guid ID { get; set; }
        public object Data { get; set; }
    }
}