using System.Collections.Generic;
using System.Threading.Tasks;

namespace TechArt.Base.Interfaces
{
    public interface ITaskPool
    {
       
        /// <summary>
        /// Task Pool Max Count
        /// </summary>
        int MaxTaskCount { get; set; }


        /// <summary>
        /// Sleep Interval 
        /// </summary>
        int SleepInterval { get; set; }


        int CurrentTaskCount { get; }
    }
}