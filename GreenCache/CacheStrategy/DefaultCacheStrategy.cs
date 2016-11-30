using System;
using System.Collections;

namespace GreenCache
{
	
    /// [��������: Ĭ�ϵĻ�����Զ���ΪĬ�ϵĻ�������ṩ����]<br></br>
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
	public class DefaultCacheStrategy : ICacheStrategy
	{
		private Hashtable objectTable;
		
		/// <summary>
		/// ���캯��
		/// </summary>
		public DefaultCacheStrategy()
		{
            //��ʼ����ϣ��
			objectTable = new Hashtable();
		}

		/// <summary>
		/// ���һ��������ײ�洢
		/// </summary>
		/// <param name="objId">����ؼ���</param>
		/// <param name="o">Ҫ��ӵĶ���</param>
		public void AddObject(string objId, object o)
		{
			objectTable.Add(objId,o);
		}

		/// <summary>
		/// �ӵײ�洢�Ƴ�һ������
		/// </summary>
        /// <param name="objId">����ؼ���</param>
		public void RemoveObject(string objId)
		{
			objectTable.Remove(objId);
		}

		/// <summary>
		/// �ӵײ�洢����һ������
		/// </summary>
        /// <param name="objId">����ؼ���</param>
		/// <returns>���صĶ���</returns>
		public object RetrieveObject(string objId)
		{
			return objectTable[objId];
		}
	}
}
