using System;
using System.Xml;
using System.Configuration;
using System.Collections;
using GreenConfiguration;

namespace GreenCache
{

    /// [功能描述: 对象缓存服务呈现在一个层次结构中的高速缓存的对象。它采用了可插拔的对象存储或缓存策略存储对象。]<br></br>
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
	public class Cache
	{
		private  XmlElement objectXmlMap ;
        private static GreenCache.ICacheStrategy cs;
		private static Cache cache;
		private XmlDocument rootXml = new XmlDocument();


		/// <summary>
		/// 私有构造方法, 需要单例设计模式
		/// </summary>
		private Cache()
		{
			//从配置文件读取配置
            ConfigurationManager cm = (ConfigurationManager)ConfigurationSettings.GetConfig("GreenFram");
			//加载缓存策略对象
			cs = (ICacheStrategy)cm.CacheConfig.GetCacheStrategy();
			//创建一个物理存储XML作为缓存在XML表达和对象之间的映射
			objectXmlMap = rootXml.CreateElement("Cache");
			//创建内部的xml文档
			rootXml.AppendChild(objectXmlMap);
		
		}

		/// <summary>
        /// 方法用来返回Singlton Cache类的实例
		/// </summary>
		/// <returns>缓存对象的实例</returns>
		public static Cache GetSAFCacheService()
		{
			if (cache == null)
			{
				cache = new Cache();
			}
			return cache;
		}

		/// <summary>
        /// 添加对象到底层存储和XML映射文件
		/// </summary>
        /// <param name="xpath">xml文件中的对象的分层位置 </param>
		/// <param name="o">要缓存的对象</param>
		public virtual void AddObject(string xpath, object o)
		{
            //清理XPath表达式
			string newXpath = PrepareXpath(xpath);
			int separator = newXpath.LastIndexOf("/");
			//找到组名
			string group = newXpath.Substring(0,separator  );
			//找到项目名
			string element = newXpath.Substring(separator + 1);
			
			XmlNode groupNode = objectXmlMap.SelectSingleNode(group);
            //如果组没有存在，创建一个。
			if (groupNode == null)
			{
				lock(this)
				{
					//创建xml树
					groupNode = CreateNode(group);
				}
			}
			//得到一个唯一的关键字，它是用来映射
            //XML和对象之间关系的
			string objectId = System.Guid.NewGuid().ToString();
            //创建一个新的元素和新的属性
			XmlElement objectElement = objectXmlMap.OwnerDocument.CreateElement(element);
			XmlAttribute objectAttribute =objectXmlMap.OwnerDocument.CreateAttribute("objectId");
			objectAttribute.Value = objectId;
			objectElement.Attributes.Append(objectAttribute);
            //添加对象元素的XML文档
			groupNode.AppendChild(objectElement);

            //通过缓存策略的对象添加到底层存储
			cs.AddObject(objectId,o);
			


		}

		/// <summary>
        /// 使用分层位置(xpath)获取缓存的对象
		/// </summary>
		/// <param name="xpath">分层位置信息</param>
		/// <returns>缓存对象</returns>
		public virtual object GetObject(string xpath)
		{
			object obj = null;
			XmlNode node =objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
			//如果xml里存在该节点，则返回该对象，否则返回空
			if ( node != null)
			{
				string objectId = node.Attributes["objectId"].Value;
				//通过缓存策略换回要找的对象
				obj = cs.RetrieveObject(objectId);
			}
			return obj;
			
		}

		/// <summary>
		/// 从缓存移除对象，并从xml里移除该对象的节点
		/// </summary>
		/// <param name="xpath">分层位置信息</param>
		public virtual void RemoveObject(string xpath)
		{
			XmlNode result = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
			//检查如果XPath是指一组（容器）还是
            //缓存对象的实际元素
			if (result.HasChildNodes)
			{
				//移除该节点下所有的缓存对象e
				XmlNodeList objects = result.SelectNodes("*[@objectId]");
				string objectId ="";
				foreach (XmlNode node in objects)
				{
					objectId = node.Attributes["objectId"].Value;
					node.ParentNode.RemoveChild(node);
					//使用缓存策略移除存在底层的对象
					cs.RemoveObject(objectId);
					
				}
				
			}
			else
			{
                //只是删除元素节点和关联的对象
				string objectId = result.Attributes["objectId"].Value;
				result.ParentNode.RemoveChild(result);
				cs.RemoveObject(objectId);
			
	
			}
		}

		/// <summary>
        /// 获取指定分层位置的对象的列表
		/// </summary>
		/// <param name="xpath">分层位置信息</param>
		/// <returns>对象数组</returns>
		public virtual object[] GetObjectList(string xpath)
		{
			XmlNode group = objectXmlMap.SelectSingleNode(PrepareXpath(xpath));
			XmlNodeList results = group.SelectNodes(PrepareXpath(xpath) + "/*[@objectId]");
			ArrayList objects = new ArrayList();
			string objectId= null;
			//遍历每个节点和链接对象
            //并通过缓存策略读取对象
			foreach (XmlNode result in results)
			{
				objectId = result.Attributes["objectId"].Value;
				objects.Add(cs.RetrieveObject(objectId));
			}
			//把ArrayList转换成对象数组
			return (object[])objects.ToArray(typeof(System.Object));
		}

		
		/// <summary>
		/// CreateNode是负责创建XML树
        /// 和分层位置中的对象
		/// </summary>
		/// <param name="xpath">分层位置信息</param>
		/// <returns></returns>
		private XmlNode CreateNode(string xpath)
		{
			string[] xpathArray = xpath.Split('/');
			string root = "";
			XmlNode parentNode = (XmlNode)objectXmlMap;
			//循环数组元素，并创建为每个级别相对应的节点
            //跳过根节点。
			for (int i = 1; i < xpathArray.Length; i ++)
			{
				XmlNode node = objectXmlMap.SelectSingleNode(root + "/" + xpathArray[i]);
				// 如果当前位置不存在则创建一个
				//否则设置当前位置到它孩子的位置
				if (node == null)
				{
					XmlElement newElement= objectXmlMap.OwnerDocument.CreateElement(xpathArray[i]);
					parentNode.AppendChild(newElement);
				}
                //设置低一个级别的新的位置
				root = root + "/" + xpathArray[i];
				parentNode = objectXmlMap.SelectSingleNode(root);
			}
			return parentNode;
		}

		/// <summary>
		/// 清理xpath，使额外的'\'被删除
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
	///缓存策略的接口
    ///缓存策略必须实现它，来为缓存提供服务
	/// </summary>
	public interface ICacheStrategy
	{
		void AddObject(string objId, object o);
		void RemoveObject(string objId);
		object RetrieveObject(string objId);
	}
}
