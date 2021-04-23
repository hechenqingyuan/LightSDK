using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Git.WMS.LightSDK.Server
{
    public partial class LightController: ApiController
    {
        /// <summary>
        /// 连接报警灯
        /// </summary>
        [HttpGet]
        public DataResult Con()
        {
            DataResult dataResult=LampServer.Instance().Init();

            return dataResult;
        }

        /// <summary>
        /// 关闭报警灯连接
        /// </summary>
        [HttpGet]
        public DataResult DisCon()
        {
            DataResult dataResult = LampServer.Instance().Close();
            return dataResult;
        }

        /// <summary>
        /// 打开红灯
        /// </summary>
        [HttpGet]
        public DataResult OpenRed()
        {
            DataResult dataResult = LampServer.Instance().OpenRed();
            return dataResult;
        }

        /// <summary>
        /// 打开黄灯
        /// </summary>
        [HttpGet]
        public DataResult OpenYellow()
        {
            DataResult dataResult = LampServer.Instance().OpenYellow();
            return dataResult;
        }

        /// <summary>
        /// 打开绿灯
        /// </summary>
        [HttpGet]
        public DataResult OpenGreen()
        {
            DataResult dataResult = LampServer.Instance().OpenGreen();
            return dataResult;
        }

        /// <summary>
        /// 关闭所有灯
        /// </summary>
        [HttpGet]
        public DataResult CloseAll()
        {
            DataResult dataResult = LampServer.Instance().CloseAll();
            return dataResult;
        }
    }
}
