using System;
using System.Xml;

namespace GreenConfiguration
{
	
    /// [��������: �¼�֪ͨ���ã�������ȡ�¼�֪ͨ�����������Ϣ��]<br></br>
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
	public class EventNotificationConfiguration
	{
        /// <summary>
        /// �ڵ�
        /// </summary>
		private XmlNode enXml;

        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="configData"></param>
		public EventNotificationConfiguration(XmlNode configData)
		{
			enXml = configData;
		}

        /// <summary>
        /// ��÷����URL
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
        /// ��ö˿ں�
        /// </summary>
        /// <returns></returns>
		public string GetPortNumber()
		{
			return enXml.SelectSingleNode("Port").InnerText;
		}

        /// <summary>
        /// ��ó�������
        /// </summary>
        /// <returns></returns>
		public string GetApplicationName()
		{
			return enXml.SelectSingleNode("ApplicationName").InnerText;
		}

        /// <summary>
        /// ���Uri
        /// </summary>
        /// <returns></returns>
		public string GetObjectUri()
		{
			return enXml.SelectSingleNode("ObjectUri").InnerText;
		}

        /// <summary>
        /// ��÷���
        /// </summary>
        /// <returns></returns>
		public string GetServer()
		{
			return enXml.SelectSingleNode("Server").InnerText;
		}
	}
}
