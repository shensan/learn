using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GreenConfiguration
{
  
    /// [功能描述: 数据库配置。]<br></br>
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
    public class DatabaseConfiguration 
	{
		
		private XmlNode databaseXml;

		/// <summary>
		/// 被配置管理器调用的有参构造方法
		/// </summary>
		/// <param name="configData"></param>
		public DatabaseConfiguration (XmlNode configData)
		{
			databaseXml = configData;	
		}

		/// <summary>
		/// 获得数据库连接串
		/// </summary>
		/// <returns></returns>
		public string GetConnStr()
		{
			return databaseXml.SelectSingleNode("ConnectionString").InnerText;
		}
	}
}
