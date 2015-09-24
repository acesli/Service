using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;


namespace TechArt.Host
{
    [RunInstaller(true)]
    public class TServiceInstaller : Installer
    {
        private ServiceProcessInstaller processInstaller;
        private ServiceInstaller serviceInstaller;

        public TServiceInstaller()
        {
            this.processInstaller = new ServiceProcessInstaller();
            this.serviceInstaller = new ServiceInstaller();

            this.processInstaller.Account = ServiceAccount.LocalService;

            this.serviceInstaller.ServiceName = "TechArt Service Host(37000)";
            this.serviceInstaller.Description = "TechArt Service Host";
            this.serviceInstaller.StartType = ServiceStartMode.Automatic;

            this.Installers.Add(processInstaller);
            this.Installers.Add(serviceInstaller);

            //this.BeforeInstall+=serviceInstaller_BeforeInstall;
            //this.AfterUninstall+=serviceInstaller_AfterUninstall;
        }

        private void serviceInstaller_AfterUninstall(object sender, InstallEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void serviceInstaller_BeforeInstall(object sender, InstallEventArgs e)
        {
            throw new NotImplementedException();
        }

    }
}
