using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using GreenConfiguration;

namespace ConfigurationTest
{
    /// <summary>
    /// 程序配置管理器
    /// </summary>
    public class AppConfigurationManager
    {
        public DatabaseConfiguration DatabaseConfig;

        private XmlNode configurationData;
        /// <summary>
        /// 开始创建所有运用程序配置对象的链
        /// </summary>
        /// <param name="sections">节点信息</param>
        public AppConfigurationManager(XmlNode sections)
        {
            configurationData = sections;
            ConfigurationAgentManager cam = new ConfigurationAgentManager(configurationData);
            DatabaseConfig = new DatabaseConfiguration(cam.GetData("Application.Database"));
        }
    }
}
