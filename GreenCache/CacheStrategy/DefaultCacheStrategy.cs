using System;
using System.Collections;

namespace GreenCache
{
	
    /// [功能描述: 默认的缓存策略对象，为默认的缓存服务提供服务。]<br></br>
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
	public class DefaultCacheStrategy : ICacheStrategy
	{
		private Hashtable objectTable;
		
		/// <summary>
		/// 构造函数
		/// </summary>
		public DefaultCacheStrategy()
		{
            //初始化哈希表
			objectTable = new Hashtable();
		}

		/// <summary>
		/// 添加一个对象进底层存储
		/// </summary>
		/// <param name="objId">对象关键字</param>
		/// <param name="o">要添加的对象</param>
		public void AddObject(string objId, object o)
		{
			objectTable.Add(objId,o);
		}

		/// <summary>
		/// 从底层存储移除一个对象
		/// </summary>
        /// <param name="objId">对象关键字</param>
		public void RemoveObject(string objId)
		{
			objectTable.Remove(objId);
		}

		/// <summary>
		/// 从底层存储返回一个对象
		/// </summary>
        /// <param name="objId">对象关键字</param>
		/// <returns>返回的对象</returns>
		public object RetrieveObject(string objId)
		{
			return objectTable[objId];
		}
	}
}
