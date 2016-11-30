using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GreenConfiguration
{

    /// [功能描述: 键值实体。]<br></br>
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
    public class KeyValueDto
    {
        /// <summary>
        /// 键
        /// </summary>
        public string Key
        {
            get;
            set;
        }

        /// <summary>
        /// 值
        /// </summary>
        public string Value
        {
            get;
            set;
        }
    }
}
