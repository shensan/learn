using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace GreenData
{
    /// <summary>
    /// [功能描述: 框架的数据访问接口]<br></br>
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
    public interface IGreenData
    {

        /// <summary>
        /// 创建数据参数对象
        /// </summary>
        /// <returns>参数对象</returns>
        IDbDataParameter CreateDataParameter();

        /// <summary>
        /// 用指定的sql和rowMapper查询数据
        /// </summary>
        /// <typeparam name="T">DTO的类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="rowMapper"></param>
        /// <param name="para">参数</param>
        /// <returns>T类型数据的集合</returns>
        IList<T> QueryWithRowMapper<T>(string sql,BaseRowMapper<T> rowMapper,IDictionary<string,object> para);

        /// <summary>
        /// 用指定的sql执行非查询
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="para">参数</param>
        /// <returns>影响行数</returns>
        int ExecuteNonQuery(string sql, IDictionary<string, object> para);

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="para">参数</param>
        /// <returns>第一行第一列的数据</returns>
        object ExecuteScalar(string sql, IDictionary<string, object> para);
    }
}
