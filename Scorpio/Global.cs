
namespace Scorpio
{
    class Global
    {
        #region 定义常量

        //定义关于和更新网址
        public const string AboutUrl = @"https://monitor.neverstop.club";
        public const string UpdateUrl = AboutUrl + @"/releases";

        //定义延迟测试网址
        public const string SpeedTestUtl = @"https://www.google.com/generate_204";

        //定义GFW更新网址
        public const string GFWLIST_URL = "https://raw.githubusercontent.com/gfwlist/gfwlist/master/gfwlist.txt";

        //定义配置文件
        public const string ConfigFileName = "SConfig.json";
        public const string v2rayConfigFileName = "config.json";

        //定义默认设置
        public const string DefaultSecurity = "auto";
        public const string DefaultNetwork = "ws";
        public const string StreamSecurity = "tls";
        public const string TcpHeaderHttp = "http";
        public const string None = "none";

        public const string InboundSocks = "socks";
        public const string InboundHttp = "http";
        public const string InboundAPITagName = "api";
        public const string InboundAPIProtocal = "dokodemo-door";
        public const string Loopback = "127.0.0.1";

        //定义Tag值
        public const string agentTag = "proxy";
        public const string directTag = "direct";
        public const string blockTag = "block";

        //定义协议
        public const string vmessProtocol = "vmess://";
        public const string socksProtocol = "socks://";
        public const string httpProtocol = "http://";
        public const string httpsProtocol = "https://";

        //定义其他选项
        public const string pacFILE = "pac.txt";
        public const string MyRegPath = "Software\\Scorpio";
        public const string MyRegKeyLanguage = "CurrentLanguage";
        public const string CustomIconName = "Scorpio.ico";

        #endregion

        #region 定义变量

        //定义启用http代理
        public static bool sysAgent
        {
            get; set;
        }
        
        //定义socks端口
        public static int socksPort
        {
            get; set;
        }

        //定义pac端口
        public static int pacPort
        {
            get; set;
        }

        #endregion
    }
}
