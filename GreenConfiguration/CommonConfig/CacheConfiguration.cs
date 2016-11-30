using System;
using System.Xml;

namespace GreenConfiguration
{
	
    /// [功能描述: 缓存服务的配置。]<br></br>
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
	public class CacheConfiguration
    {
        #region 字段
        /// <summary>
        /// 节点
        /// </summary>
		private XmlNode cacheXml;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configData">配置数据</param>
		public CacheConfiguration(XmlNode configData)
		{
			cacheXml = configData;
		}

        #endregion

        #region 放法
        /// <summary>
		/// 加载定义在配置文件中的缓存策略对象，并且返回给缓存
		/// </summary>
		/// <returns>cache strategy object</returns>
		public object GetCacheStrategy()
		{
			string typeName = cacheXml.SelectSingleNode("CacheStrategy").Attributes["type"].Value;
			Type type = Type.GetType(typeName);
			return Activator.CreateInstance(type,null);
        }
        #endregion
    }
}
