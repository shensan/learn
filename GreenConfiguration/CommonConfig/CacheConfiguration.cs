using System;
using System.Xml;

namespace GreenConfiguration
{
	
    /// [��������: �����������á�]<br></br>
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
	public class CacheConfiguration
    {
        #region �ֶ�
        /// <summary>
        /// �ڵ�
        /// </summary>
		private XmlNode cacheXml;
        #endregion

        #region ���캯��
        /// <summary>
        /// ���캯��
        /// </summary>
        /// <param name="configData">��������</param>
		public CacheConfiguration(XmlNode configData)
		{
			cacheXml = configData;
		}

        #endregion

        #region �ŷ�
        /// <summary>
		/// ���ض����������ļ��еĻ�����Զ��󣬲��ҷ��ظ�����
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
