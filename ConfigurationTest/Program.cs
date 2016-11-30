using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using GreenConfiguration;
using GreenCache;
using GreenEventNotification;

namespace ConfigurationTest
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            ConfigurationManager framConfMana = (ConfigurationManager)ConfigurationSettings.GetConfig("GreenFram");
            IList<string> iocAopPath = framConfMana.CoreConfig.GetIocAopPathList();
            Console.WriteLine("IocAop配置文件路径");
            foreach (var path in iocAopPath)
            {
                Console.WriteLine(path);
            }

            IList<string> autoIocAopPath = framConfMana.CoreConfig.GetIocAopPathList();
            Console.WriteLine("AutoIocAop配置文件路径");
            foreach (var path in iocAopPath)
            {
                Console.WriteLine(path);
            }

            IList<string> autoTablePath = framConfMana.CoreConfig.GetAutoTablePathList();
            Console.WriteLine("自动表配置文件路径");
            foreach (var path in autoTablePath)
            {
                Console.WriteLine(path);
            }

            Dictionary<string,IocConfigDto> iocDic=framConfMana.CoreConfig.GreenIocConfiguration.GetIocConfigList();
            Console.WriteLine("注入配置读取");
            foreach (var obj in iocDic)
            {
                Console.WriteLine(obj.Key + ":" + obj.Value.Type + ":" + obj.Value.Type);
                if (obj.Value.Propertys != null)
                {
                    foreach (var pro in obj.Value.Propertys)
                    {
                        Console.WriteLine("属性");
                        Console.WriteLine(pro.Key + ":" + pro.Type + ":" + pro.Value);
                    }
                }
            }
            Console.WriteLine("拦截配置读取");
            Dictionary<string, AopConfigDto> aopDic = framConfMana.CoreConfig.GreenAopConfiguration.GetAopConfigList();
            foreach (var obj in aopDic)
            {
                Console.WriteLine(obj.Key + ":" + obj.Value.Type + ":" + obj.Value.Type);
                if (obj.Value.Propertys != null)
                {
                    foreach (var pro in obj.Value.Propertys)
                    {
                        Console.WriteLine("属性");
                        Console.WriteLine(pro.Key + ":" + pro.Type + ":" + pro.Value);
                    }
                }
                if (obj.Value.Interceptors != null)
                {
                    foreach (var inter in obj.Value.Interceptors)
                    {
                        Console.WriteLine("拦截");
                        Console.WriteLine(inter);
                    }
                }
            }
            Console.WriteLine("数据库串读取");
            AppConfigurationManager appConfMana = (AppConfigurationManager)ConfigurationSettings.GetConfig("UserApplication");
            Console.WriteLine(appConfMana.DatabaseConfig.GetDatabaseConnection());
            
            Cache cache=Cache.GetSAFCacheService();
            Console.WriteLine("写入缓存");
            cache.AddObject("Student",new Student());
            Student student = (Student)cache.GetObject("Student");
            student.Say();
            Console.WriteLine("读取缓存");
            student.SayString("从缓存获得的我");

            Console.WriteLine("事件通知服务");
            Teacher teacher = new Teacher();
            string url = framConfMana.EventNotificationConfig.GetEventServerUrl();
            EventClient client = new EventClient(url);
            //client.SubscribeEvent("test1", new EventClient.EventProcessingHandler(Test1_EventReceiver));
            //client.SubscribeEvent("test2", new EventClient.EventProcessingHandler(Test2_EventReceiver));

            ////事件客户端发布或引发一些事件
            Console.WriteLine("张联珠发出的事件通知");
            client.RaiseEvent("test1", "张联珠发出的事件通知");
            Console.WriteLine("张联珠发出的事件通知1");
            client.RaiseEvent("test2", "张珠珠发出的事件通知1");

            Console.Read();
        }
    }
}
