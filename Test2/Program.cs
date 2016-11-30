using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using GreenConfiguration;
using GreenEventNotification;

namespace Test2
{
    class Program
    {
        static void Main(string[] args)
        {
            ConfigurationManager framConfMana = (ConfigurationManager)ConfigurationSettings.GetConfig("GreenFram");
            string url = framConfMana.EventNotificationConfig.GetEventServerUrl();
            EventClient client = new EventClient(url);
            client.SubscribeEvent("test1", new EventClient.EventProcessingHandler(Test1_EventReceiver));
            client.SubscribeEvent("test2", new EventClient.EventProcessingHandler(Test2_EventReceiver));
            Console.ReadLine();
        }

        static void Test1_EventReceiver(string sender, object content)
        {
            Console.WriteLine("事件接收! " + sender +"内容为" + content.ToString());
        }


        static void Test2_EventReceiver(string sender, object content)
        {
            Console.WriteLine("事件接收!  " +sender + "内容为" + content.ToString());
        }
    }
}
