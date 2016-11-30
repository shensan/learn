using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;

namespace GreenData
{
    /// <summary>
    /// [功能描述: Oracle引擎初始化]<br></br>
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
    public class OracleClientInit:IDataAccessInit
    {
      
        /// <summary>
        /// 获得连接对象
        /// </summary>
        public IDbConnection GetConnection()
        {
            return new OracleConnection();
        }

        /// <summary>
        /// 获得命令对象
        /// </summary>
        public IDbCommand GetCommand()
        {
            return new OracleCommand();
        }

        /// <summary>
        /// 获得数据适配器对象
        /// </summary>
        public IDbDataAdapter GetDataAdapter()
        {
            return new OracleDataAdapter();
        }

        /// <summary>
        /// 获得数据参数对象
        /// </summary>
        public IDbDataParameter GetDataParameter()
        {
            return new OracleParameter();
        }

    }
}
