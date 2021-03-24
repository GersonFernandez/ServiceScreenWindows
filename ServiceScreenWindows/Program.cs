using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace ServiceScreenWindows
{
    static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        static void Main()
        {

            if (Environment.UserInteractive)
            {
                String[] args= new string[] { };
                Service1 service = new Service1();
                service.TestStartupAndStop(args);
            }
            else
            {
                try
                {
                    ServiceBase[] ServicesToRun;
                    ServicesToRun = new ServiceBase[]
                    {
                new Service1()
                    };
                    ServiceBase.Run(ServicesToRun);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("ServiceScreen", "ERROR : " + ex.Message);
                }
            } 
        }
    }
}
