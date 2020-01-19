using Microsoft.Win32;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;
using System.Security.Principal;
using Scorpio.Base;

namespace Scorpio
{
    class Utils
    {

        #region JSON操作

        /// <summary>
        /// 获取嵌入文本资源
        /// </summary>
        /// <param name="res"></param>
        /// <returns></returns>
        public static string GetEmbedText(string res)
        {
            string result = string.Empty;

            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                using (Stream stream = assembly.GetManifestResourceStream(res))
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// 获取存储资源
        /// </summary>
        /// <returns></returns>
        public static string LoadResource(string res)
        {
            string result = string.Empty;

            try
            {
                using (StreamReader reader = new StreamReader(res))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// 发序列化对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="strJson"></param>
        /// <returns></returns>
        public static T FromJson<T>(string strJson)
        {
            try
            {
                T obj = JsonConvert.DeserializeObject<T>(strJson);
                return obj;
            }
            catch
            {
                return JsonConvert.DeserializeObject<T>("");
            }
        }

        /// <summary>
        /// 序列化成Json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJson(Object obj)
        {
            string result = string.Empty;
            try
            {
                result = JsonConvert.SerializeObject(obj, Formatting.Indented,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// 保存成json文件
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static int ToJsonFile(Object obj, string filePath)
        {
            int result = -1;
            try
            {
                using (StreamWriter file = System.IO.File.CreateText(filePath))
                {
                    JsonSerializer serializer = new JsonSerializer() { Formatting = Formatting.Indented };
                    serializer.Serialize(file, obj);
                }
                result = 0;
            }
            catch
            {
                result = -1;
            }
            return result;
        }
        #endregion

        #region 数据格式转换

        /// <summary>
        /// List<string>转逗号分隔的字符串
        /// </summary>
        /// <param name="lst"></param>
        /// <returns></returns>
        public static string List2String(List<string> lst, bool wrap = false)
        {
            try
            {
                if (wrap)
                {
                    return string.Join(",\r\n", lst.ToArray());
                }
                else
                {
                    return string.Join(",", lst.ToArray());
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 逗号分隔的字符串,转List<string>
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static List<string> String2List(string str)
        {
            try
            {
                str = str.Replace("\r\n", "");
                return new List<string>(str.Split(new string[] { "," }, StringSplitOptions.RemoveEmptyEntries));
            }
            catch
            {
                return new List<string>();
            }
        }

        /// <summary>
        /// Base64编码
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Encode(string plainText)
        {
            try
            {
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception ex)
            {
                SaveLog("Base64Encode", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// Base64解码
        /// </summary>
        /// <param name="plainText"></param>
        /// <returns></returns>
        public static string Base64Decode(string plainText)
        {
            try
            {
                plainText = plainText.Trim()
                     .Replace("\n", "")
                    .Replace("\r\n", "")
                    .Replace("\r", "")
                    .Replace(" ", "");

                if (plainText.Length % 4 > 0)
                {
                    plainText = plainText.PadRight(plainText.Length + 4 - plainText.Length % 4, '=');
                }

                byte[] data = Convert.FromBase64String(plainText);
                return Encoding.UTF8.GetString(data);
            }
            catch (Exception ex)
            {
                SaveLog("Base64Decode", ex);
                return string.Empty;
            }
        }

        /// <summary>
        /// 转Int
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static int ToInt(object obj)
        {
            try
            {
                return Convert.ToInt32(obj);
            }
            catch
            {
                return 0;
            }
        }

        public static string ToString(object obj)
        {
            try
            {
                return (obj == null ? string.Empty : obj.ToString());
            }
            catch
            {
                return string.Empty;
            }
        }

        public static void DedupServerList(List<Mode.VmessItem> source, out List<Mode.VmessItem> result)
        {
            var list = new List<Mode.VmessItem>();
            foreach (var item in source)
            {
                if (!list.Exists(i => item.address == i.address && item.port == i.port && item.path == i.path))
                {
                    list.Add(item);
                }
            }
            result = list;
        }

        #endregion

        #region 数据检查

        /// <summary>
        /// 判断输入的是否是数字
        /// </summary>
        /// <param name="oText"></param>
        /// <returns></returns>
        public static bool IsNumberic(string oText)
        {
            try
            {
                int varl = Utils.ToInt(oText);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 文本
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return true;
            }
            if (text.Equals("null"))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 验证IP地址是否合法
        /// </summary>
        /// <param name="ip"></param>   
        public static bool IsIP(string ip)
        {
            //如果为空
            if (IsNullOrEmpty(ip))
            {
                return false;
            }

            //清除要验证字符串中的空格
            //ip = ip.TrimEx();
            //可能是CIDR
            if (ip.IndexOf(@"/") > 0)
            {
                var cidr = ip.Split('/');
                if (cidr.Length == 2)
                {
                    if (!IsNumberic(cidr[0]))
                    {
                        return false;
                    }
                    ip = cidr[0];
                }
            }

            //模式字符串
            string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";

            //验证
            return IsMatch(ip, pattern);
        }

        /// <summary>
        /// 验证Domain地址是否合法
        /// </summary>
        /// <param name="domain"></param>
        /// <returns></returns>
        public static bool IsDomain(string domain)
        {
            //如果为空
            if (IsNullOrEmpty(domain))
            {
                return false;
            }

            //清除要验证字符串中的空格
            //domain = domain.TrimEx();

            //模式字符串
            string pattern = @"^(?=^.{3,255}$)[a-zA-Z0-9][-a-zA-Z0-9]{0,62}(\.[a-zA-Z0-9][-a-zA-Z0-9]{0,62})+$";

            //验证
            return IsMatch(domain, pattern);
        }

        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern">模式字符串</param>
        /// <returns></returns>
        public static bool IsMatch(string input, string pattern)
        {
            return Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }

        #region

        #region 开机自启

        private static string autoRunName = "Scorpio";
        private static string autoRunRegPath
        {
            get
            {
                return @"Software\Microsoft\Windows\CurrentVersion\Run";
            }
        }

        /// <summary>
        /// 开机自启
        /// </summary>
        /// <param name="run"></param>
        /// <returns></returns>
        public static int SetAutoRun(bool run)
        {
            try
            {
                if (run)
                {
                    string exePath = GetExePath();
                    RegWriteValue(autoRunRegPath, autoRunName, exePath);
                }
                else
                {
                    RegWriteValue(autoRunRegPath, autoRunName, "");
                }
            }
            catch
            {
            }
            return 0;
        }

        /// <summary>
        /// 是否已经设置开机自启
        /// </summary>
        /// <returns></returns>
        public static bool IsAutoRun()
        {
            try
            {
                var value = RegReadValue(autoRunRegPath, autoRunName.Length, "");
                string exePath = GetExePath();
                if (value?.Equals(exePath) == true)
                {
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        /// <summary>
        /// 获取启动了应用程序的可执行文件的路径
        /// </summary>
        /// <returns></returns>
        public static string GetPath(string fileName)
        {
            string startupPath = startupPath();
            if (Utils.IsNullOrEmpty(fileName))
            {
                return startupPath
            }
            return Path.Combine(startupPath, fileName);
        }

        /// <summary>
        /// 获取启动了应用程序的可执行文件路径及文件名
        /// </summary>
        /// <returns></returns>
        public static string GetExePath()
        {
            return Application.ExecutablePath;
        }

        public static string StartupPath()
        {
            try
            {
                string exePath = GetExePath();
                return exePath.Substring(0, exePath.LastIndexOf("\\", StringComparison.Ordinal));
            }
            catch
            {
                return Application.StartupPath;
            }
        }

        public static string RegReadValue(string path, string name, string def)
        {
            RegistryKey regKey = null;
            try
            {
                regKey = Registry?.CurrentUser.OpenSubKey(path, false);
                string value = regKey?.GetValue(name) as string;
                if (IsNullOrEmpty(value))
                {
                    return def;
                }
                else
                {
                    return value;
                }
            }
            catch
            {
            }
            finally
            {
                regKey?.Close();
            }
            return def;
        }

        public static void RegWriteValue(string path, string name, string value)
        {
            RegistryKey regKey = null;
            try
            {
                regKey = Registry.CurrentUser.OpenSubKey(path);
                if (IsNullOrEmpty(value))
                {
                    regKey?.DeleteValue(name, false);
                }
                else
                {
                    regKey?.SetValue(name, value);
                }
            }
            catch
            {
            }
            finally
            {
                regKey?.Close();
            }
        }

        #endregion

        #region 延迟测试

        /// <summary>
        /// ping
        /// </summary>
        /// <param name="host"></param>
        /// <returns></returns>
        public static long Ping(string host)
        {
            long roundtripTime = -1;
            try
            {
                int timeout = 120;
                int echoNum = 2;
                Ping PingSender = new Ping();
                for (int i = 0; i < echoNum; i++)
                {
                    PingReply reply = PingSender.Send(host, timeout);
                    if (reply.Status == IPStatus.Success)
                    {
                        if (reply.RoundtripTime < 0)
                        {
                            continue;
                        }
                        if (roundtripTime < 0 || reply.RoundtripTime < roundtripTime)
                        {
                            roundtripTime = reply.RoundtripTime;
                        }
                    }
                }
            }
            catch
            {
                return -1;
            }
            return roundtripTime;
        }

        /// <summary>
        /// 获取本机IP地址
        /// </summary>
        /// <returns></returns>
        public static List<string> GetHostIPAddress()
        {
            List<string> lstIPAddress = new List<string>();
            try
            {
                IPHostEntry IpEntry = Dns.GetHostEntry(Dns.GetHostName());
                foreach (IPAddress ipa in IpEntry.AddressList)
                {
                    if (ipa.AddressFamily == AddressFamily.InterNetwork)
                        lstIPAddress.Add(ipa.ToString());
                }
            }
            catch
            { 
            }
            return lstIPAddress;
        }

        #endregion

        #region 其他

        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns></returns>
        public static string GetVersion()
        {
            try
            {
                string location = GetExePath();
                return string.Format("Scorpio - V{0} -{1}",
                    FileVersionInfo.GetVersionInfo(location).FileVersion.ToString(),
                    File.GetLastWriteTime(location).ToString("yyyy/MM/dd"));
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 深度拷贝
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T obj)
        {
            Object retval;
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter bf = new BinaryFormatter();
                //序列化成流
                bf.Serialize(ms, obj);
                ms.Seek(0, SeekOrigin.Begin);
                //反序列化成对象
                retval = bf.Deserialize(ms);
                ms.Close();
            }
            return (T)retval;
        }

        /// <summary>
        /// IsAdministrator
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
            try
            {
                WindowsIdentity current = WindowsIdentity.GetCurrent();
                WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
                return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// TempPath
        /// </summary>
        /// <returns></returns>
        public static string _tempPath = null;

        public static string GetTempPath()
        {
            if (_tempPath == null)
            {
                Directory.CreateDirectory(Path.Combine(StartupPath(), "Scorpio_temp"));
                _tempPath = Path.Combine(StartupPath(), "Scorpio_temp");
            }
            return _tempPath;
        }

        public static string GetTempPath(string filename)
        {
            return Path.Combine(GetTempPath(), filename);
        }

        public static void ClearTempPath()
        {
            //Directory.Delete(GetTempPath(), true);
            //_tempPath = null;
        }

        public static string UnGzip(byte[] buf)
        {
            byte[] buffer = new byte[1024];
            int n;
            using (MemoryStream sb = new MemoryStream())
            {
                using (GZipStream input = new GZipStream(new MemoryStream(buf),
                    CompressionMode.Decompress,
                    false))
                {
                    while ((n = input.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        sb.Write(buffer, 0, n);
                    }
                }
                return System.Text.Encoding.UTF8.GetString(sb.ToArray());
            }
        }

        /// <summary>
        /// Log位置
        /// </summary>
        /// <returns></returns>
        public static void SaveLog(string strContent)
        {
            SaveLog("info", new Exception(strContent));
        }
        public static void SaveLog(string strTitle, Exception ex)
        {
            try
            {
                string path = Path.Combine(StartupPath(), "Scorpio_logs");
                string FilePath = Path.Combine(path, DateTime.Now.ToString("yyyyMMdd") + ".txt");
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (!File.Exists(FilePath))
                {
                    FileStream FsCreate = new FileStream(FilePath, FileMode.Create);
                    FsCreate.Close;
                    FsCreate.Dispose();
                }
                FileStream FsWrite = new FileStream(FilePath, FileMode.Append, FileAccess.Write);
                StreamWriter SwWrite = new StreamWriter(FsWrite);

                string strContent = ex.ToString();

                SwWrite.WriteLine(string.Format("{0}{1}[{2}]{3}", "--------------------------------", strTitle, DateTime.Now.ToString("HH:mm:ss"), "--------------------------------"));
                SwWrite.Write(strContent);
                SwWrite.WriteLine("\r\n");
                SwWrite.WriteLine(" ");
                SwWrite.Flush();
                SwWrite.Close();
            }
            catch
            { 
            }
        }

        #region
    }
}