using Git.Framework.DataTypes.EnumAtttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Git.WMS.LightSDK.Server
{
    public enum EResponseCode
    {
        [EnumDescription("成功")]
        Success = 1,

        [EnumDescription("异常")]
        Exception = 3,
    }
}
