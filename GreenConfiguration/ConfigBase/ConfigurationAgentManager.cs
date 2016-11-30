using System;
using System.Xml;
using System.Collections;


namespace GreenConfiguration
{

    /// [��������: ��������ش�����󣬸�������������ݡ�]<br></br>
    /// [������:   ������]<br></br>
    /// [����ʱ��: 2013-9-17]<br></br>
    /// <˵��>
    ///    
    /// </˵��>
    /// <�޸ļ�¼>
    ///     <�޸�ʱ��></�޸�ʱ��>
    ///     <�޸�����>
    ///            
    ///     </�޸�����>
    /// </�޸ļ�¼>
    /// </summary>
	public class ConfigurationAgentManager
	{
		private XmlNode configurationData;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="configData">�ڵ�</param>
		public ConfigurationAgentManager(XmlNode configData)
		{
			configurationData = configData;
		}

		/// <summary>
        /// ����һ�������ļ�������XML Configuraiton������
		/// </summary>
		/// <param name="key">�����ļ�<SAF.ClassFactory>. </param>
		/// <returns>������Ϣ</returns>
		public XmlNode GetData(string key)
		{
			XmlNode result=null;
			XmlAttribute agentAttribute =null;
			if (configurationData.SelectSingleNode(key) != null)
			{
				//����Ƿ��д�����Ϊһ���ض��Ĳ��ֻ�ؼ�
                //����У����ش���ͼ�������
                //����ֻ�ǽ����ݼ��ش�Configuraiton���ļ�
				agentAttribute = configurationData.SelectSingleNode(key).Attributes["ConfigurationAgent"];
                //���ConfigurationAgent����Ϊ�գ���ʹ�ü���Ӧ�Ľڵ�
				if ( agentAttribute == null)
				{
					result = configurationData.SelectSingleNode(key);
				}
                //���ConfigurationAgent���Բ�Ϊ�գ���ʹ�ô�������ȡ������Ϣ
				else
				{
                    //ʹ�ô���retrive����
					string data = GetAgent(agentAttribute.Value).GetConfigurationSetting();
                    //����xml�ĵ�
					XmlDocument xml = new XmlDocument();
                    //�����ĵ�
					xml.LoadXml(data);
                    //�ѽ����Ϊ�����ĵ��ĸ��ڵ�
					result = (XmlNode)xml.DocumentElement;
				}
			}
			return result;
		}

		/// <summary>
		/// ����ʹ�÷�����ش���������һ������ʵ��
        /// ��������
		/// </summary>
		/// <param name="agentName">���������</param>
		/// <returns>���صĴ������</returns>
		private IConfigurationAgent GetAgent(string agentName)
		{
            //��ø����ƵĽڵ�
			XmlNode agentNode = configurationData.SelectSingleNode("//Agent[@name ='" + agentName +  "']");
            //��øýڵ������
			Type type = Type.GetType(agentNode.Attributes["type"].Value);
            //���������͵Ĵ���ʵ��
			IConfigurationAgent agent = (IConfigurationAgent)Activator.CreateInstance(type,null);
			//��ʼ���������õĴ��������ָ���Ĳ�����Ϣ
            //���ļ�������Ҫ�Ĵ���������乤��
			agent.Initialize(agentNode);
            //���ش������
			return agent;
		}
	}
	/// <summary>
	/// ÿ�����ô��������ʵ�ֵĽӿڣ�������ȡ�ⲿ������Դ��
    /// ������������������ʱ�ɴ������
	/// </summary>
	public interface IConfigurationAgent
	{
        /// <summary>
        /// ����ִ�г�ʼ������
        /// </summary>
        /// <param name="xml"></param>
		void Initialize(XmlNode xml);

        /// <summary>
        /// ������ȡ������Ϣ
        /// </summary>
        /// <returns></returns>
		string GetConfigurationSetting();
	}
}
