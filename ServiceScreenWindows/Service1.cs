using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Timers;

namespace ServiceScreenWindows
{
    public partial class Service1 : ServiceBase
    {
        private Timer _t = null;
        private int _lenTime = 20000;
        private String _rutaPrograna;
        private uint _uidProcess =0;
        Process _process;



        public Service1()
        {

            InitializeComponent();
            _t = new Timer(_lenTime);
            _t.Elapsed += new ElapsedEventHandler(t_Elapsed);
        }

        protected override void OnStart(string[] args)
        {
           // System.Diagnostics.EventLog.WriteEntry("args", "args len : " + args.Count());
            if (args.Length >= 2)
            {
                _lenTime = int.Parse(args[1]) * 1000;
                _rutaPrograna = args[0];
                //System.Diagnostics.EventLog.WriteEntry("args", "args{0}: " + args[0]+ "args [1] = " +args[1]);

            }
            else if (args.Length == 1)
            {
                _rutaPrograna = args[0];
            }
            else
            {
                //_rutaPrograna = @"C:\program files\WatchGuard\Mobile VPN\ncpmon.exe";
                 _rutaPrograna = @"C:\ScreenService\App\ScreenCaptureApl.exe";
            }
          
            
            if (_uidProcess == 0)
            {
                bool _pass = OpenAPP(_rutaPrograna);
                if (!_pass)
                {
                    System.Diagnostics.EventLog.WriteEntry("ScreenService", "ERROR NO INICIO EL PROCESO");
                }

            }
            _t.Interval = _lenTime;
            _t.Start();
            //System.Diagnostics.EventLog.WriteEntry("ApplicationAP", "Codigo : " + _uidProcess);
        }

        protected override void OnStop()
        {
            _t.Stop();
            if (_uidProcess != 0)
            {
                try
                {
                    _process.Kill();
                }
                catch(Exception ex)
                {
                    System.Diagnostics.EventLog.WriteEntry("ApplicationException on Stop", "ERROR : " + ex.Message);
                }

                
            }
            

        }

        void t_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (_uidProcess == 0)
            {
                bool _pass = OpenAPP(_rutaPrograna);
                if (!_pass)
                {
                    System.Diagnostics.EventLog.WriteEntry("ScreenService", "ERROR NO INICIO EL PROCESO");
                }
                
            }
            else
            {
                try
                {

                    _process = Process.GetProcessById((int)_uidProcess);
                   

                }
                catch(Exception ex)
                {

                    _uidProcess = 0;
                    System.Diagnostics.EventLog.WriteEntry("ScreenService", "ERROR : " + ex.Message);
                }

               
            }
            
        }

        private bool OpenAPP(String _ruta)
        {
            bool inicio = false;
            try
            {

                ApplicationLoader.PROCESS_INFORMATION procInfo;

                String applicationName = _ruta;
                inicio = ApplicationLoader.StartProcessAndBypassUAC(applicationName, out procInfo);
                _uidProcess = procInfo.dwProcessId;
                _process = Process.GetProcessById((int)_uidProcess);
                System.Diagnostics.EventLog.WriteEntry("ScreenService", "Id Proceso : " + _process.Id);
                //t.Start();

                if (inicio)
                {
                    System.Diagnostics.EventLog.WriteEntry("ScreenService", "Inicio : " + _uidProcess);

                }
                else
                {
                    System.Diagnostics.EventLog.WriteEntry("ScreenService", "No inicio ");
                }

            }
            catch(Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry("ScreenService", "Error:  "+ ex.Message);
            }


           
            return inicio;
        }

        internal void TestStartupAndStop(String[] args)
        {
            this.OnStart(args);
            Console.ReadLine();
            this.OnStop();

        }

    }
}
