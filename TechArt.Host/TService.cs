using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using TechArt.Host.Core;

namespace TechArt.Host
{
    public partial class TService : ServiceBase
    {
        public TService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            TServiceBase.Start();
        }

        protected override void OnStop()
        {
            TServiceBase.Stop();
        }
    }
}
