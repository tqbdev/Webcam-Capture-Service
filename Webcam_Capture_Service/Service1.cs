using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace Webcam_Capture_Service
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer();
            timer.Interval = 60000;
            timer.Elapsed += timer_Tick;
            timer.Enabled = true;
        }

        private void timer_Tick(object sender, ElapsedEventArgs args)
        {
            try
            { 
                Upload_FTP.Upload_Folder_FTP.Upload("C:\\temp\\");
            }
            catch (Exception ex)
            {
                Utilities.Log.WriteLogError(ex);
            }
        }

        protected override void OnStop()
        {
        }
    }
}
