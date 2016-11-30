using System;
using System.Xml;
using System.Collections;


namespace GreenConfiguration
{

    /// [功能描述: 它负责加载代理对象，负责检索配置数据。]<br></br>
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
	public class ConfigurationAgentManager
	{
		private XmlNode configurationData;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configData">节点</param>
		public ConfigurationAgentManager(XmlNode configData)
		{
			configurationData = configData;
		}

		/// <summary>
        /// 对于一个给定的键，返回XML Configuraiton的设置
		/// </summary>
		/// <param name="key">给定的键<SAF.ClassFactory>. </param>
		/// <returns>设置信息</returns>
		public XmlNode GetData(string key)
		{
			XmlNode result=null;
			XmlAttribute agentAttribute =null;
			if (configurationData.SelectSingleNode(key) != null)
			{
				//检查是否有代理定义为一个特定的部分或关键
                //如果有，加载代理和检索数据
                //否则，只是将数据加载从Configuraiton的文件
				agentAttribute = configurationData.SelectSingleNode(key).Attributes["ConfigurationAgent"];
                //如果ConfigurationAgent属性为空，就使用键对应的节点
				if ( agentAttribute == null)
				{
					result = configurationData.SelectSingleNode(key);
				}
                //如果ConfigurationAgent属性不为空，就使用代理对象获取配置信息
				else
				{
                    //使用代理retrive数据
					string data = GetAgent(agentAttribute.Value).GetConfigurationSetting();
                    //创建xml文档
					XmlDocument xml = new XmlDocument();
                    //导入文档
					xml.LoadXml(data);
                    //把结果设为导入文档的父节点
					result = (XmlNode)xml.DocumentElement;
				}
			}
			return result;
		}

		/// <summary>
		/// 方法使用反射加载代理，并返回一个代理实例
        /// 给调用者
		/// </summary>
		/// <param name="agentName">代理的名称</param>
		/// <returns>返回的代理对象</returns>
		private IConfigurationAgent GetAgent(string agentName)
		{
            //获得该名称的节点
			XmlNode agentNode = configurationData.SelectSingleNode("//Agent[@name ='" + agentName +  "']");
            //获得该节点的类型
			Type type = Type.GetType(agentNode.Attributes["type"].Value);
            //创建该类型的代理实例
			IConfigurationAgent agent = (IConfigurationAgent)Activator.CreateInstance(type,null);
			//初始化方法设置的代理对象与指定的参数信息
            //在文件中所需要的代理来完成其工作
			agent.Initialize(agentNode);
            //返回代理对象
			return agent;
		}
	}
	/// <summary>
	/// 每个配置代理类必须实现的接口（用来读取外部数据资源）
    /// 它的两个方法在运行时由代理调用
	/// </summary>
	public interface IConfigurationAgent
	{
        /// <summary>
        /// 用来执行初始化操作
        /// </summary>
        /// <param name="xml"></param>
		void Initialize(XmlNode xml);

        /// <summary>
        /// 用来获取配置信息
        /// </summary>
        /// <returns></returns>
		string GetConfigurationSetting();
	}
}
