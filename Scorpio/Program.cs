using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using Scorpio.Forms;

namespace Scorpio
{
    static class Program
    {
        //导入Windows系统自带的user32.dll中的函数（API），设置Windows窗体应用程序支持高DPI
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            //检测当前系统版本是否大于Windows7
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetProcessDPIAware();
            }

            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;

            Process instance = RunningInstance();
            if (instance == null)
            {
                Utils.SaveLog("Scorpio srart up");


                //设置语言环境
                string lang = Utils.RegReadValue(Global.MyRegPath, Global.MyRegKeyLanguage, "zh-Hans");
            }
        }
    }
}
