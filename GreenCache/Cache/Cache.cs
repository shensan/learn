using System;
using System.Xml;
using System.Configuration;
using System.Collections;
using GreenConfiguration;

namespace GreenCache
{

    /// [��������: ���󻺴���������һ����νṹ�еĸ��ٻ���Ķ����������˿ɲ�εĶ���洢�򻺴���Դ洢����]<br></br>
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
	public class Cache
	{
		private  XmlElement objectXmlMap ;
        private static GreenCache.ICacheStrategy cs;
		private static Cache cache;
		private XmlDocument rootXml = new XmlDocument();


		/// <summary>
		/// ˽�й��췽��, ��Ҫ�������ģʽ
		/// </summary>
		private Cache()
		{
			//�������ļ���ȡ����
            ConfigurationManager cm = (ConfigurationManager)ConfigurationSettings.GetConfig("GreenFram");
			//���ػ�����Զ���
			cs = (ICacheStrategy)cm.CacheConfig.GetCacheStrategy();
			//����һ������洢XML��Ϊ������XML���Ͷ���֮���ӳ��
			objectXmlMap = rootXml.CreateElement("Cache");
			//�����ڲ���xml�ĵ�
			rootXml.AppendChild(objectXmlMap);
		
		}

		/// <summary>
        /// ������������Singlton Cache���ʵ��
		/// </summary>
		/// <returns>��������ʵ��</returns>
		public static Cache GetSAFCacheService()
		{
			if (cache == null)
			{
				cache = new Cache();
			}
			return cache;
		}

		/// <summary>
        /// ��Ӷ��󵽵ײ�洢��XMLӳ���ļ�
		/// </summary>
        /// <param name="xpath">xml�ļ��еĶ���ķֲ�λ�� </param>
		/// <param name="o">Ҫ����Ķ���</param>
		public virtual void AddObject(string xpath, object o)
		{
            //����XPath���ʽ
			string newXpath = PrepareXpath(xpath);
			int separator = newXpath.LastIndexOf("/");
			//�ҵ�����
			string group = newXpath.Substring(0,separator  );
			//�ҵ���Ŀ��
			string element = newXpath.Substring(separator + 1);
			
			XmlNode groupNode = objectXmlMap.SelectSingleNode(group);
            //�����û�д��ڣ�����һ����
			if (groupNode == null)
			{
				lock(this)
				{
					//����xml��
					groupNode = CreateNode(group);
				}
			}
			//�õ�һ��Ψһ�Ĺؼ��֣���������ӳ��
            //XML�Ͷ���֮���ϵ��
			string objectId = System.Guid.NewGuid().ToString();
            //����һ���µ�Ԫ�غ��µ�����
			XmlElement objectElement = objectXmlMap.OwnerDocument.CreateElement(element);
			XmlAttribute objectAttribute =objectXmlMap.OwnerDocument.CreateAttribute("objectId");
			objectAttribute.Value = objectId;
			objectElement.Attributes.Append(objectAttribute);
            //��Ӷ���Ԫ�ص�XML�ĵ�
			groupNode.AppendChild(objectElement);

            //ͨ��������ԵĶ�����ӵ��ײ�洢
			cs.AddObject(objectId,o);
			


		}

		/// <summary>
        /// ʹ�÷ֲ�λ��(xpath)��ȡ����Ķ���
		/// </summary>
		/// <param name="xpath">�ֲ�λ����Ϣ</param>
		/// <returns>�������</returns>
		public virtual object GetObject(string xpath)
		{
			object obj = null;
			XmlNode node =objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
			//���xml����ڸýڵ㣬�򷵻ظö��󣬷��򷵻ؿ�
			if ( node != null)
			{
				string objectId = node.Attributes["objectId"].Value;
				//ͨ��������Ի���Ҫ�ҵĶ���
				obj = cs.RetrieveObject(objectId);
			}
			return obj;
			
		}

		/// <summary>
		/// �ӻ����Ƴ����󣬲���xml���Ƴ��ö���Ľڵ�
		/// </summary>
		/// <param name="xpath">�ֲ�λ����Ϣ</param>
		public virtual void RemoveObject(string xpath)
		{
			XmlNode result = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
			//������XPath��ָһ�飨����������
            //��������ʵ��Ԫ��
			if (result.HasChildNodes)
			{
				//�Ƴ��ýڵ������еĻ������e
				XmlNodeList objects = result.SelectNodes("*[@objectId]");
				string objectId ="";
				foreach (XmlNode node in objects)
				{
					objectId = node.Attributes["objectId"].Value;
					node.ParentNode.RemoveChild(node);
					//ʹ�û�������Ƴ����ڵײ�Ķ���
					cs.RemoveObject(objectId);
					
				}
				
			}
			else
			{
                //ֻ��ɾ��Ԫ�ؽڵ�͹����Ķ���
				string objectId = result.Attributes["objectId"].Value;
				result.ParentNode.RemoveChild(result);
				cs.RemoveObject(objectId);
			
	
			}
		}

		/// <summary>
        /// ��ȡָ���ֲ�λ�õĶ�����б�
		/// </summary>
		/// <param name="xpath">�ֲ�λ����Ϣ</param>
		/// <returns>��������</returns>
		public virtual object[] GetObjectList(string xpath)
		{
			XmlNode group = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
			XmlNodeList results = group.SelectNodes(PrepareXpath(xpath) + "/*[@objectId]");
			ArrayList objects = new ArrayList();
			string objectId= null;
			//����ÿ���ڵ�����Ӷ���
            //��ͨ��������Զ�ȡ����
			foreach (XmlNode result in results)
			{
				objectId = result.Attributes["objectId"].Value;
				objects.Add(cs.RetrieveObject(objectId));
			}
			//��ArrayListת���ɶ�������
			return (object[])objects.ToArray(typeof(System.Object));
		}

		
		/// <summary>
		/// CreateNode�Ǹ��𴴽�XML��
        /// �ͷֲ�λ���еĶ���
		/// </summary>
		/// <param name="xpath">�ֲ�λ����Ϣ</param>
		/// <returns></returns>
		private XmlNode CreateNode(string xpath)
		{
			string[] xpathArray = xpath.Split('/');
			string root = "";
			XmlNode parentNode = (XmlNode)objectXmlMap;
			//ѭ������Ԫ�أ�������Ϊÿ���������Ӧ�Ľڵ�
            //�������ڵ㡣
			for (int i = 1; i < xpathArray.Length; i ++)
			{
				XmlNode node = objectXmlMap.SelectSingleNode(root + "/" + xpathArray[i]);
				// �����ǰλ�ò������򴴽�һ��
				//�������õ�ǰλ�õ������ӵ�λ��
				if (node == null)
				{
					XmlElement newElement= objectXmlMap.OwnerDocument.CreateElement(xpathArray[i]);
					parentNode.AppendChild(newElement);
				}
                //���õ�һ��������µ�λ��
				root = root + "/" + xpathArray[i];
				parentNode = objectXmlMap.SelectSingleNode(root);
			}
			return parentNode;
		}

		/// <summary>
		/// ����xpath��ʹ�����'\'��ɾ��
		/// </summary>
		/// <param name="xpath">hierarchical location</param>
		/// <returns></returns>
		private string PrepareXpath(string xpath)
		{
			string[] xpathArray = xpath.Split('/');
			xpath ="/Cache";
			foreach (string s in xpathArray)
			{
				if (s != "")
				{
					xpath = xpath + "/" + s ;
				}
			}
			return xpath;
		}
	}
	
	
	
	/// <summary>
	///������ԵĽӿ�
    ///������Ա���ʵ��������Ϊ�����ṩ����
	/// </summary>
	public interface ICacheStrategy
	{
		void AddObject(string objId, object o);
		void RemoveObject(string objId);
		object RetrieveObject(string objId);
	}
}
