using System;
using System.Xml;
using System.Collections;

namespace GreenConfiguration
{
   
    /// [功能描述: 处理对特定的配置节的访问。]<br></br>
    /// [创建者:   张联珠]<br></br>
    /// [创建时间: 2013-9-17]<br></br>
    /// <说明>
    ///    
    /// </说明>
    /// <修改记录>
    ///     <修改时间></修改时间>
    ///     <修改内容>
    ///            
    ///     </修改内容>
    /// </修改记录>
    /// </summary>
    public class ConfigurationHandler : System.Configuration.IConfigurationSectionHandler
    {
        /// <summary>
        /// 实现的接口方法
        /// </summary>
        /// <param name="parent">父对象</param>
        /// <param name="configContext">配置上下文对象</param>
        /// <param name="section">节 XML 节点</param>
        /// <returns>返回结果</returns>
        public object Create(Object parent, object configContext, XmlNode section)
        {
            //读配置获得段处理器的类型
            Type type = System.Type.GetType(section.Attributes["type"].Value);
            //把段处理器配置装入参数
            object[] parameters = { section };
            //调用配置对象的构造函数
            object configObject = null;
            try
            {
                //根据段处理器的配置创建实体
                configObject = Activator.CreateInstance(type, parameters);
            }
            catch (Exception ex)
            {
                //创建实体异常
                string x = ex.Message;
                return null;
            }
            //返回配置所要产生的对象
            return configObject;

        }
    }

    /// <summary>
    /// 根配置对象，为框架组件提供配置对象的访问
    /// </summary>
    public class ConfigurationManager
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        public DatabaseConfiguration DataBaseConfig;

        /// <summary>
        /// 缓存配置
        /// </summary>
        public CacheConfiguration CacheConfig;

        /// <summary>
        /// 核心配置
        /// </summary>
        public CoreCofiguration CoreConfig;

        /// <summary>
        /// 事件通知服务配置
        /// </summary>
        public EventNotificationConfiguration EventNotificationConfig;

        /// <summary>
        /// 节点信息
        /// </summary>
        private XmlNode configurationData;


        /// <summary>
        /// 通过该配置管理器初始化所有的配置对象
        /// </summary>
        /// <param name="sections"></param>
        public ConfigurationManager(XmlNode sections)
        {
            configurationData = sections;
            ConfigurationAgentManager cam = new ConfigurationAgentManager(configurationData);
            if (cam.GetData("Green.Core") != null)
            {
                CoreConfig = new CoreCofiguration(cam.GetData("Green.Core"));
            }
            if (cam.GetData("Green.Cache") != null)
            {
                CacheConfig = new CacheConfiguration(cam.GetData("Green.Cache"));
            }
            if (cam.GetData("Green.DataBase") != null)
            {
                DataBaseConfig = new DatabaseConfiguration(cam.GetData("Green.DataBase"));
            }
            if (cam.GetData("Green.EventNotification") != null)
            {
                EventNotificationConfig = new EventNotificationConfiguration(cam.GetData("Green.EventNotification"));
            }
        }
    }
}
