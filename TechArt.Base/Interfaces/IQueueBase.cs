using TechArt.Base.ClassLibrary.Queue;

namespace TechArt.Base.Interfaces
{
    public interface IQueueBase
    {

        string Name { get; set; }

        QueueStatus Status { get;}

        void Start();

        void Stop();
    }
}