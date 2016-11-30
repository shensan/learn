using System;
using System.Xml;

namespace GreenConfiguration
{
	
    /// [功能描述: 事件通知配置，用来获取事件通知服务的配置信息。]<br></br>
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
	public class EventNotificationConfiguration
	{
        /// <summary>
        /// 节点
        /// </summary>
		private XmlNode enXml;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configData"></param>
		public EventNotificationConfiguration(XmlNode configData)
		{
			enXml = configData;
		}

        /// <summary>
        /// 获得服务的URL
        /// </summary>
        /// <returns></returns>
		public string GetEventServerUrl()
		{
			
			string server = GetServer();
			string port =GetPortNumber();
			string appName =GetApplicationName();
			string objectUri =GetObjectUri();
			return "http://" + server + ":" + port + "/" + appName + "/" + objectUri;
		}

        /// <summary>
        /// 获得端口号
        /// </summary>
        /// <returns></returns>
		public string GetPortNumber()
		{
			return enXml.SelectSingleNode("Port").InnerText;
		}

        /// <summary>
        /// 获得程序集名称
        /// </summary>
        /// <returns></returns>
		public string GetApplicationName()
		{
			return enXml.SelectSingleNode("ApplicationName").InnerText;
		}

        /// <summary>
        /// 获得Uri
        /// </summary>
        /// <returns></returns>
		public string GetObjectUri()
		{
			return enXml.SelectSingleNode("ObjectUri").InnerText;
		}

        /// <summary>
        /// 获得服务
        /// </summary>
        /// <returns></returns>
		public string GetServer()
		{
			return enXml.SelectSingleNode("Server").InnerText;
		}
	}
}
