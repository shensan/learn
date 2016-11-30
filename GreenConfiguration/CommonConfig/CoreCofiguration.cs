using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace GreenConfiguration
{

    /// [功能描述: 核心服务的配置。]<br></br>
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
    public class CoreCofiguration
    {
        #region 字段
        /// <summary>
        /// 节点
        /// </summary>
        private XmlNode cacheXml;

        /// <summary>
        /// 依赖注入的配置
        /// </summary>
        private IocConfiguration iocConfiguration = null;

        /// <summary>
        /// 拦截的配置
        /// </summary>
        private AopConfiguration aopConfiguration = null;
        #endregion

        #region 属性
        /// <summary>
        /// 依赖注入配置对象
        /// </summary>
        public IocConfiguration GreenIocConfiguration
        {
            get
            {
                return iocConfiguration;
            }
        }

        /// <summary>
        /// 拦截的配置对象
        /// </summary>
        public AopConfiguration GreenAopConfiguration
        {
            get
            {
                return aopConfiguration;
            }
        }
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="configData">配置数据</param>
        public CoreCofiguration(XmlNode configData)
        {
            cacheXml = configData;
            //获得所有依赖注入和拦截的配置路径
            IList<string> paths = GetIocAopPathList();
            //创建依赖注入配置对象
            iocConfiguration = new IocConfiguration(paths);
            //创建拦截配置对象
            aopConfiguration = new AopConfiguration(paths);
        }
        #endregion

        #region 方法

        /// <summary>
        /// 获得依赖注入和拦截的配置文件路径的集合
        /// </summary>
        /// <returns>路径集合</returns>
        public IList<string> GetIocAopPathList()
        {
            //选取所有IocAopPath下的子节点
            XmlNodeList objects = cacheXml.SelectNodes("IocAopPath");
            IList<string> pathList = new List<string>();
            //遍历所有节点，获取InnerText
            foreach(XmlNode obj in objects)
            {
                pathList.Add(obj.InnerText);
            }
            return pathList;
        }

        /// <summary>
        /// 获得自动依赖注入自动拦截的配置文件路径集合
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAutoIocAopPathList()
        {
            //选取所有IocAopPath下的子节点
            XmlNodeList objects = cacheXml.SelectNodes("AutoIocAopPath");
            IList<string> pathList = new List<string>();
            //遍历所有节点，获取InnerText
            foreach (XmlNode obj in objects)
            {
                pathList.Add(obj.InnerText);
            }
            return pathList;
        }

        /// <summary>
        /// 获得自动表配置文件路径的集合
        /// </summary>
        /// <returns>路径集合</returns>
        public IList<string> GetAutoTablePathList()
        {
            //选取所有IocAopPath下的子节点
            XmlNodeList objects = cacheXml.SelectNodes("AutoTable");
            IList<string> pathList = new List<string>();
            //遍历所有节点，获取InnerText
            foreach (XmlNode obj in objects)
            {
                pathList.Add(obj.InnerText);
            }
            return pathList;
        }
        #endregion

    }
}
