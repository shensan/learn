using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels;
using GreenConfiguration;
using System.Collections;
using System.Runtime.Serialization.Formatters;
using GreenEventNotification;


namespace GreenEventNotificationServer
{
    /// <summary>
    /// 事件通知服务服务端宿主
    /// </summary>
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ConfigurationManager cm = (ConfigurationManager)ConfigurationSettings.GetConfig("GreenFram");
            EventNotificationConfiguration enc = cm.EventNotificationConfig;
            //建立网络渠道接受的事件客户端的请求
            
            IDictionary props = new Hashtable();
            props["port"] = Int32.Parse(enc.GetPortNumber());

            BinaryServerFormatterSinkProvider provider = new BinaryServerFormatterSinkProvider();
            provider.TypeFilterLevel = TypeFilterLevel.Full;
            HttpChannel channel = new HttpChannel(props, null, provider);

            ChannelServices.RegisterChannel(channel);

            //注册remote对象类型
            WellKnownServiceTypeEntry wste = new WellKnownServiceTypeEntry(typeof(EventServer), enc.GetObjectUri(), WellKnownObjectMode.Singleton);
            RemotingConfiguration.ApplicationName = enc.GetApplicationName();
            RemotingConfiguration.RegisterWellKnownServiceType(wste);
            Console.WriteLine("事件通知服务运行中...");
            Console.WriteLine("Press Enter to exit");
            Console.ReadLine();
        }
    }
}
