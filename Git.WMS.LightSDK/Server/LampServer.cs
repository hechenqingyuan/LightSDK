using Git.Framework.DataTypes;
using Git.Framework.Log;
using Git.Framework.Resource;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace Git.WMS.LightSDK.Server
{
    public partial class LampServer
    {
        public System.IO.Ports.SerialPort SerialLamp = null;
        private Log log = Log.Instance(typeof(LampServer));
        private System.Timers.Timer tmTimer = null; //蜂鸣器定定时

        private static LampServer InstanceEntity = null;

        public static LampServer Instance()
        {
            if (InstanceEntity == null)
            {
                InstanceEntity = new LampServer();
            }
            return InstanceEntity;
        }

        /// <summary>
        /// 初始化三色灯控制器
        /// </summary>
        public DataResult Init()
        {
            DataResult dataResult = new DataResult();

            try
            {
                if (this.SerialLamp == null)
                {
                    string COM = ResourceManager.GetSettingEntity("LightSDK_COM").Value;
                    string Rate = ResourceManager.GetSettingEntity("LightSDK_Rate").Value;
                    string DataPosition = ResourceManager.GetSettingEntity("LightSDK_DataPosition").Value;
                    this.SerialLamp = new System.IO.Ports.SerialPort(COM, ConvertHelper.ToType<int>(Rate, 9600), Parity.None, ConvertHelper.ToType<int>(DataPosition, 8), StopBits.One);
                    this.SerialLamp.RtsEnable = true;
                    this.SerialLamp.DataReceived += SerialLamp_DataReceived;
                    this.SerialLamp.Open();
                }
                else
                {
                    if (this.SerialLamp.IsOpen)
                    {

                    }
                    else
                    {
                        this.SerialLamp.Open();
                    }
                }
                this.CloseAll();
                dataResult.Code = (int)EResponseCode.Success;
                dataResult.Message = "串口连接成功";
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
                dataResult.Code = (int)EResponseCode.Exception;
                dataResult.Message = ex.Message;
            }

            return dataResult;
        }

        /// <summary>
        /// 关闭串口连接
        /// </summary>
        public DataResult Close()
        {
            DataResult dataResult = new DataResult();
            try
            {
                if (this.SerialLamp != null)
                {
                    if (this.SerialLamp.IsOpen)
                    {
                        //Task.Factory.StartNew(()=> { this.SerialLamp.Close(); });
                    }
                    this.SerialLamp = null;

                    dataResult.Code = (int)EResponseCode.Success;
                    dataResult.Message = "串口已经关闭";
                }
                else
                {
                    dataResult.Code = (int)EResponseCode.Exception;
                    dataResult.Message = "该串口未打开,无法进行关闭操作";
                }
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
                dataResult.Code = (int)EResponseCode.Exception;
                dataResult.Message = ex.Message;
            }

            return dataResult;
        }

        /// <summary>
        /// 三色灯控制响应
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SerialLamp_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            try
            {
                int n = this.SerialLamp.BytesToRead;
                byte[] by = new byte[n];
                this.SerialLamp.Read(by, 0, n);
                string str = string.Empty;
                if (by != null && by.Length > 0)
                {
                    str = System.Text.Encoding.Default.GetString(by);
                }
                Thread.Sleep(300);
                str = str.Replace(" ", "");
                log.Info("串口响应数据:" + str);
            }
            catch (Exception ex)
            {
                log.Info(ex.Message);
            }
        }

        /// <summary>
        /// 三色灯控制指令写入
        /// </summary>
        /// <param name="bCmd"></param>
        private DataResult WriteCom(byte[] bCmd)
        {
            DataResult dataResult = new DataResult();

            if (this.SerialLamp != null)
            {
                if (this.SerialLamp.IsOpen)
                {
                    Task.Factory.StartNew(() =>
                    {
                        this.SerialLamp.Write(bCmd, 0, bCmd.Length);
                    });
                    dataResult.Code = (int)EResponseCode.Success;
                    dataResult.Message = "已经发送指令到报警灯";
                }
                else
                {
                    this.Init();
                    if (this.SerialLamp != null)
                    {
                        if (this.SerialLamp.IsOpen)
                        {
                            Task.Factory.StartNew(() =>
                            {
                                this.SerialLamp.Write(bCmd, 0, bCmd.Length);
                            });
                            dataResult.Code = (int)EResponseCode.Success;
                            dataResult.Message = "已经发送指令到报警灯";
                        }
                        else
                        {
                            log.Error("未能够连接到三色灯");
                            dataResult.Code = (int)EResponseCode.Exception;
                            dataResult.Message = "未能够连接到三色灯";
                        }
                    }
                    else
                    {
                        dataResult.Code = (int)EResponseCode.Exception;
                        dataResult.Message = "未能够连接到三色灯";
                    }
                }
            }
            else
            {
                dataResult.Code = (int)EResponseCode.Exception;
                dataResult.Message = "未能够连接到三色灯";
            }

            return dataResult;
        }

        /// <summary>
        /// 关闭所有灯和蜂鸣器
        /// </summary>
        public DataResult CloseAll()
        {
            string LightSDK_Close = ResourceManager.GetSettingEntity("LightSDK_Close").Value;
            byte[] bCmd = this.hexStringToByte(LightSDK_Close);
            DataResult dataResult = this.WriteCom(bCmd);

            return dataResult;
        }

        /// <summary>
        /// 打开绿色灯，其他关闭
        /// </summary>
        public DataResult OpenGreen()
        {
            string LightSDK_Green = ResourceManager.GetSettingEntity("LightSDK_Green").Value;
            byte[] bCmd = this.hexStringToByte(LightSDK_Green);
            DataResult dataResult = this.WriteCom(bCmd);

            return dataResult;
        }

        /// <summary>
        /// 打开红灯
        /// </summary>
        public DataResult OpenRed()
        {
            string LightSDK_Red = ResourceManager.GetSettingEntity("LightSDK_Red").Value;
            byte[] bCmd = this.hexStringToByte(LightSDK_Red);
            DataResult dataResult = this.WriteCom(bCmd);

            return dataResult;
        }

        /// <summary>
        /// 打开黄色灯
        /// </summary>
        public DataResult OpenYellow()
        {
            string LightSDK_Yellow = ResourceManager.GetSettingEntity("LightSDK_Yellow").Value;
            byte[] bCmd = this.hexStringToByte(LightSDK_Yellow);
            DataResult dataResult = this.WriteCom(bCmd);

            return dataResult;
        }

        /// <summary>
        /// 十六进制转二进制数字
        /// </summary>
        /// <param name="hex"></param>
        /// <returns></returns>
        public byte[] hexStringToByte(string hex)
        {
            hex = hex.Replace(" ", "");
            int len = (hex.Length / 2);
            byte[] result = new byte[len];
            char[] achar = hex.ToCharArray();
            for (int i = 0; i < len; i++)
            {
                int pos = i * 2;
                result[i] = (byte)(toByte(achar[pos]) << 4 | toByte(achar[pos + 1]));
            }
            return result;
        }

        /// <summary>
        /// 字符转
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        private int toByte(char c)
        {
            byte b = (byte)"0123456789ABCDEF".IndexOf(c);
            return b;
        }
    }
}
