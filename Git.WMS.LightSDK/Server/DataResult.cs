using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.WMS.LightSDK.Server
{
    public partial class DataResult
    {
        public DataResult() { }

        /// <summary>
        /// 相应状态码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 业务状态码
        /// </summary>
        public int SubCode { get; set; }

        /// <summary>
        /// 业务相应消息
        /// </summary>
        public string SubMessage { get; set; }
    }
}
