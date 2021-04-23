using Git.Framework.Log;
using Git.Framework.Resource;
using Git.WMS.LightSDK.Server;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Git.WMS.LightSDK
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Log log = Log.Instance(typeof(Form1));

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 开启API服务
        /// </summary>
        public void Start()
        {
            string baseAddress = ResourceManager.GetSettingEntity("API_URL").Value;
            Microsoft.Owin.Hosting.WebApp.Start<Startup>(url: baseAddress);
            Console.WriteLine("程序已启动,按任意键退出");
            LampServer.Instance().Init();
            Console.ReadLine();
        }

        /// <summary>
        /// 开启服务
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStart_Click(object sender, EventArgs e)
        {
            this.Start();


        }

        /// <summary>
        /// 连接报警灯连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            LampServer.Instance().Init();
        }

        /// <summary>
        /// 关闭报警灯连接
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            LampServer.Instance().Close();
        }

        /// <summary>
        /// 关闭所有灯光
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            LampServer.Instance().CloseAll();
        }

        /// <summary>
        /// 开红灯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRed_Click(object sender, EventArgs e)
        {
            LampServer.Instance().OpenRed();
        }

        /// <summary>
        /// 开黄灯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnYellow_Click(object sender, EventArgs e)
        {
            LampServer.Instance().OpenYellow();
        }

        /// <summary>
        /// 开绿灯
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGreen_Click(object sender, EventArgs e)
        {
            LampServer.Instance().OpenGreen();
        }

        /// <summary>
        /// 关闭窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            LampServer.Instance().CloseAll();
            LampServer.Instance().Close();
            Environment.Exit(0);
        }
    }
}
