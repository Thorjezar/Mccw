using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;


namespace McwdService
{
    [RunInstaller(true)]
    public partial class Installer1 : System.Configuration.Install.Installer
    {
        public Installer1()
        {
            InitializeComponent();
        }
        protected override void OnBeforeInstall(IDictionary savedState)
        {
            base.OnBeforeInstall(savedState);
        }

        private void ProjectInstaller_Committed(object sender, InstallEventArgs e)
        {
            //System.ServiceProcess.ServiceController controller = new System.ServiceProcess.ServiceController("NMLXWindowsHosting");
            //controller.Start();
        }

        /// <summary>
        /// 重写安装提交事件
        /// </summary>
        /// <param name="savedState"></param>
        public override void Commit(IDictionary savedState)
        {
            base.Commit(savedState);
            ServiceController sc = new ServiceController("McwdService");
            if (sc.Status.Equals(ServiceControllerStatus.Stopped))
            {

                SetWindowsServiceStartType("McwdService", 2);//设置服务类型为自动启动
                sc.Start();//安装完成后启动服务
            }
        }

        /// <summary>
        /// 设置服务启动类型
        /// </summary>
        /// <param name="sServiceName"></param>
        /// <param name="iStartType"></param>
        /// <returns></returns>
        public Boolean SetWindowsServiceStartType(String sServiceName, int iStartType)
        {
            try
            {
                System.Diagnostics.ProcessStartInfo objProcessInf = new System.Diagnostics.ProcessStartInfo();

                objProcessInf.FileName = "cmd.exe";

                objProcessInf.CreateNoWindow = false;
                objProcessInf.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;

                string sStartState = "boot";

                switch (iStartType)
                {
                    case 1:
                        {
                            sStartState = "system";//默认
                            break;
                        }
                    case 2:
                        {
                            sStartState = "auto";//自动
                            break;
                        }
                    case 3:
                        {
                            sStartState = "demand";//手动
                            break;
                        }
                    case 4:
                        {
                            sStartState = "disabled";//禁用
                            break;
                        }
                    default:
                        {
                            break;
                        }
                }

                objProcessInf.Arguments = "/c sc config " + sServiceName + " start= " + sStartState;

                System.Diagnostics.Process.Start(objProcessInf);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private void serviceProcessInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }

        private void serviceInstaller1_AfterInstall(object sender, InstallEventArgs e)
        {

        }
    }
}
