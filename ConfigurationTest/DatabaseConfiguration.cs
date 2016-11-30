using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace ConfigurationTest
{
    /// <summary>
    /// 数据库配置
    /// </summary>
    public class DatabaseConfiguration
    {

        private XmlNode databaseXml;

        /// <summary>
        /// 被配置管理器调用的有参构造方法
        /// </summary>
        /// <param name="configData"></param>
        public DatabaseConfiguration(XmlNode configData)
        {
            databaseXml = configData;
        }
        /// <summary>
        /// method that parse the information out of the Xml
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseConnection()
        {
            return databaseXml.SelectSingleNode("ConnectionString").InnerText;
        }
    }
}
