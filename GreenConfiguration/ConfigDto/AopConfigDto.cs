using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GreenConfiguration
{

    /// [功能描述: 拦截配置实体。]<br></br>
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
    [Serializable]
    public class AopConfigDto
    {
        /// <summary>
        /// 要实现拦截的对象的类型，是包含命名空间的全名
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// 要拦截的对象的程序集名称
        /// </summary>
        public string DllName
        {
            get;
            set;
        }

        /// <summary>
        /// 拦截器的集合
        /// </summary>
        public List<string> Interceptors
        {
            get;
            set;
        }

        /// <summary>
        /// 要注入属性的集合
        /// </summary>
        public List<PropertyDto> Propertys
        {
            get;
            set;
        }
    }
}
