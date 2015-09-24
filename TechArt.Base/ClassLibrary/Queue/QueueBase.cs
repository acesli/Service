using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TechArt.Base.Interfaces;

namespace TechArt.Base.ClassLibrary.Queue
{
    public abstract class QueueBase<T> : IQueueBase, ITaskPool, IDisposable
        where T : IJob
    {
        private string _name = "";
        private QueueStatus _status;

        private static object _JobLock = new object();
        private static object _TaskLock = new object();
        private static  object _SleepTasksLock =new object(); 

        private List<Task> _taskList ;
        private HashSet<int> _sleepTasks;
        private List<T> _jobList;

        private int _maxTaskCount = 10;
        private int _sleepInterval = 1;
        private int _currentTaskCount = 0;

        public delegate void AfterStartEventHandler(object sender, EventArgs e);
        public delegate void AfterStopEventHandler(object sender, EventArgs e);

        public event AfterStartEventHandler Started;
        public event AfterStopEventHandler Stopped;

        #region Proporty
        public IDebugger Debugger { get; set; }
        #endregion

        #region Implement ITaskPool

        public int CurrentTaskCount
        {
            get { return _currentTaskCount; }
        }

        public int MaxTaskCount
        {
            get { return _maxTaskCount; }

            set
            {
                if (_status == QueueStatus.None || _status == QueueStatus.Stoped)
                {
                    if (value <= 0)
                    {
                        throw new InvalidDataException();
                    }
                    _maxTaskCount = value;
                }
                else
                {
                    throw new InvalidOperationException();
                }
            }
        }
        
        public int SleepInterval
        {
            get { return _sleepInterval; }

            set
            {
                if (_status == QueueStatus.None || _status == QueueStatus.Stoped)
                {
                    if (value < 0)
                    {
                        throw new InvalidDataException();
                    }

                    _sleepInterval = value;
                }
                else
                {
                    throw new InvalidOperationException();
                }

            }
        }

        #endregion

        #region  Implement IQueueBase

        public string Name {
            get { return _name; }
            set { _name = value; }
        }


        public QueueStatus Status
        {
            get { return _status; }
        }


        public void Start()
        {
            if (_status == QueueStatus.None || _status == QueueStatus.Stoped)
            {
                //set status to starting
                _status = QueueStatus.Starting;

                Debugger.AddMsg("Start Queue " + this.Name);

                //Init Task Pool
                TaskPool_Init();

                //Raise Event Started
                Started?.Invoke(this, EventArgs.Empty);


                //set status to started
                _status = QueueStatus.Started;
                Debugger.AddMsg("Start Queue  " + this.Name + " Successfully.");
            }
            else
            {
                throw new InvalidOperationException("Queue status must None when start queue");
            }
        }


        public void Stop()
        {
            if (_status == QueueStatus.Started)
            {
                Debugger.AddMsg("Stop Queue " + this.Name);

                //set status to stopping 
                _status = QueueStatus.Stopping;

                while (_jobList.Count > 0)
                {
                    //sleep 100ms when has job
                    Thread.Sleep(100);
                }

                //Raise Event Started
                this.Stopped?.Invoke(this, EventArgs.Empty);

                //set status to started
                _status = QueueStatus.Started;
                Debugger.AddMsg("Stop Queue  " + this.Name + " Successfully.");
            }
            else
            {
                throw new InvalidOperationException("Queue status must None when start queue");
            }
        }


        #endregion

        #region  Public Methods 

        public void AddJob(T job)
        {
            //Check Queue Start State
            if (_status == QueueStatus.Started)
            {
                Job_Add(job);
            }
        }
        
        #endregion

        #region Private Methods 

        #region Task Pool
        private void TaskPool_Init()
        {
            //init object
            this._taskList=new List<Task>();
            this._jobList=new List<T>();

            this._sleepTasks=new HashSet<int>();
            this._currentTaskCount = 0;
        }

        private void TaskPool_NewOne()
        {
            if (this._currentTaskCount < _maxTaskCount)
            {
                this._currentTaskCount += 1;
                Task task = new Task(ThreadPool_Worker);

                try
                {
                    //Lock Task
                    LockTask();

                    //Add this task to task list
                    this._taskList.Add(task);

                    //Start task
                    task.Start();
                }
                finally
                {
                    //Release Lock 
                    UnlockTask();
                }
            }
        }

        private void TaskPool_RemoveOne(int taskID)
        {
            try
            {
                //Lock Task
                LockTask();

                Task findTask = this._taskList.FirstOrDefault(x => x.Id == taskID);

                if (findTask != null)
                {
                    
                    //Add this task to task list
                    this._taskList.Remove(findTask);
                    this._currentTaskCount -= 1;
                }
            }
            finally
            {
                //Release Lock 
                UnlockTask();
            }
        }

        private bool TaskPool_TrySleepTask(int taskID)
        {
            bool result = true;
            try
            {
                LockSleepingTask();

                if (this._sleepTasks.Contains(taskID))
                {
                    this._sleepTasks.Remove(taskID);

                    //return false;
                    result = false;
                }
                else
                {
                    Thread.Sleep(_sleepInterval);
                }
            }
            finally
            {
                UnlockSleepingTask();
            }
            return result;
        }

        private void ThreadPool_Worker()
        {
            while (true)
            {
                T job = Job_Get();

                if (job == null)
                {
                    int? taskID = Task.CurrentId;

                    if (taskID.HasValue)
                    {
                        //check current task sleeped or not 
                        if (TaskPool_TrySleepTask(taskID.Value) == false)
                        {
                            //Try Sleep Task fail ,
                            //this mean this task has sleeped before this time
                            //break dead loop 

                            TaskPool_RemoveOne(taskID.Value);
                            break;
                        }
                    }
                }
                else
                {
                    //Execute Job
                    PerformJob(job);

                    //Remove job
                    Job_Remove(job.ID);
                }
               
            }
        }

        #endregion

        #region Job

        private void Job_Add(T job)
        {
            try
            {
                //Lock The Job List
                this.LockJob();

                this._jobList.Add(job);

                //there is not task to do job create new one
                if (_currentTaskCount == 0)
                {
                    TaskPool_NewOne();
                }
            }
            finally
            {
                //Release Lock
                this.UnlockJob();
            }
        }

        private void Job_Remove(Guid jobId)
        {
            try
            {
                //Lock The Job List
                this.LockJob();

                var currentJob = _jobList.FirstOrDefault((x) => x.ID.Equals(jobId));

                if (currentJob != null)
                {
                    _jobList.Remove(currentJob);
                }
            }
            finally
            {
                //Release Lock
                this.UnlockJob();
            }
        }

        private T Job_Get()
        {
            T job = default(T);
            try
            {
                //Lock The Job List
                this.LockJob();

                if (_jobList.Count > 0)
                {
                    job = _jobList[0];
                }
            }
            finally
            {
                //Release Lock
                this.UnlockJob();
            }

            return job;
        }

        #endregion

        #region Lock
        private void LockJob()
        {
            Monitor.Enter(_JobLock);
        }

        private void UnlockJob()
        {
            Monitor.Exit(_JobLock);
        }

        private void LockTask()
        {
            Monitor.Enter(_TaskLock);
        }

        private void UnlockTask()
        {
            Monitor.Exit(_TaskLock);
        }

        private void LockSleepingTask()
        {
            Monitor.Exit(_SleepTasksLock);
        }

        private void UnlockSleepingTask()
        {
            Monitor.Exit(_SleepTasksLock);
        }

        #endregion

        #endregion

        #region MustInherit Methods 

        protected abstract void PerformJob(T job);

        #endregion

        #region Implement IDisposable

        private void ClearUp()
        {
            this._jobList = null;
            this._sleepTasks = null;
            this._taskList = null;
        }

        public void Dispose()
        {
            // Do not change this code.  
            ClearUp();
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}