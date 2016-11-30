using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace GreenData
{
    /// <summary>
    /// [功能描述: 框架的数据引擎初始化接口接口]<br></br>
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
    public interface IDataAccessInit
    {
        /// <summary>
        /// 获得连接对象
        /// </summary>
        IDbConnection GetConnection();
        

        /// <summary>
        /// 获得命令对象
        /// </summary>
        IDbCommand GetCommand();
        

        /// <summary>
        /// 获得数据适配器对象
        /// </summary>
        IDbDataAdapter GetDataAdapter();
        

        /// <summary>
        /// 获得数据参数对象
        /// </summary>
        IDbDataParameter GetDataParameter();
       
    }
}
