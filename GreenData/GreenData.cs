using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using System.Configuration;
using GreenConfiguration;


namespace GreenData
{
    /// <summary>
    /// [功能描述: Oracle数据访问实现]<br></br>
    /// [创建者:   张联珠]<br></br>
    /// [创建时间: 2013-9-5]<br></br>
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
    public class GreenData:IGreenData
    {
        /// <summary>
        /// 引擎初始化接口
        /// </summary>
        private IDataAccessInit dataAccessInit;

        /// <summary>
        /// 连接串
        /// </summary>
        private string conStr;

        /// <summary>
        /// 连接对象
        /// </summary>
        private IDbConnection conn;

        /// <summary>
        /// 命令对象
        /// </summary>
        private IDbCommand command;

        /// <summary>
        /// 数据适配器对象
        /// </summary>
        private IDataAdapter dataAdapter;

        /// <summary>
        /// 数据读取器对象
        /// </summary>
        private IDataReader dataReader;

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="dataAccessInit">引擎初始化接口</param>
        /// <param name="readConfig">读取配置文件接口</param>
        public GreenData(IDataAccessInit dataAccessInit)
        {
            this.dataAccessInit = dataAccessInit;
            ConfigurationManager framConfMana = (ConfigurationManager)ConfigurationSettings.GetConfig("GreenFram");
            this.conStr = framConfMana.DataBaseConfig.GetConnStr();
            this.conn = this.dataAccessInit.GetConnection();
            this.command = this.dataAccessInit.GetCommand();
            this.dataAdapter = this.dataAccessInit.GetDataAdapter();
            InitAccess();  
        }

        /// <summary>
        /// 初始化连接和命令对象
        /// </summary>
        private void InitAccess()
        {
            conn.ConnectionString = this.conStr;
            command.Connection = conn;
        }

        /// <summary>
        /// 创建数据参数对象
        /// </summary>
        /// <returns>参数对象</returns>
        public IDbDataParameter CreateDataParameter()
        {
            return this.dataAccessInit.GetDataParameter();
        }

        /// <summary>
        /// 用指定的sql和rowMapper查询数据
        /// </summary>
        /// <typeparam name="T">DTO的类型</typeparam>
        /// <param name="sql">sql语句</param>
        /// <param name="rowMapper"></param>
        /// <param name="para">参数</param>
        /// <returns>T类型数据的集合</returns>
        public IList<T> QueryWithRowMapper<T>(string sql, BaseRowMapper<T> rowMapper, IDictionary<string, object> para)
        {
            //创建指定类型的集合
            IList<T> list = new List<T>();
            //给命令对象设sql
            command.CommandText = sql;
            if (para != null && para.Count > 0)
            {
                //填充参数
                foreach (var v in para)
                {
                    IDbDataParameter pa = this.dataAccessInit.GetDataParameter();
                    pa.ParameterName = v.Key;
                    pa.Value = v.Value;
                    command.Parameters.Add(pa);
                }
            }
            try
            {
                //打开数据连接
                this.conn.Open();
                //获得数据读取器对象
                this.dataReader = command.ExecuteReader();
                //循环读取数据
                while (this.dataReader.Read())
                {
                    //创建指定类型的对象
                    T dto = (T)Activator.CreateInstance(typeof(T));
                    //往对象里填充数据
                    rowMapper.FieldMap(dto, this.dataReader);
                    //把填数据的对象加入集合
                    list.Add(dto);
                }
            }
            //捕获异常
            catch (Exception ex)
            {
                throw ex;
            }
            //关闭资源
            finally
            {
                if (!this.dataReader.IsClosed)
                {
                    this.dataReader.Close();
                }
                if (conn.State != ConnectionState.Closed)
                {
                    this.conn.Close();
                }
            }
            //返回集合
            return list;
        }

        /// <summary>
        /// 用指定的sql执行非查询
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="para">参数</param>
        /// <returns>影响行数</returns>
        public int ExecuteNonQuery(string sql, IDictionary<string, object> para)
        {
            //记录影像行数
            int num = 0;
            //给命令对象设sql
            command.CommandText = sql;
            if (para != null && para.Count > 0)
            {
                //填充参数
                foreach (var v in para)
                {
                    IDbDataParameter pa = this.dataAccessInit.GetDataParameter();
                    pa.ParameterName = v.Key;
                    pa.Value = v.Value;
                    command.Parameters.Add(pa);
                }
            }
            try
            {
                //打开数据连接
                this.conn.Open();
                num=this.command.ExecuteNonQuery();
            }
            //捕获异常
            catch (Exception ex)
            {
                throw ex;
            }
            //关闭资源
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    this.conn.Close();
                }
            }
            //返回影响行数
            return num;
        }

        /// <summary>
        /// 执行查询，并返回查询所返回的结果集中第一行的第一列。忽略额外的列或行。
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="para">参数</param>
        /// <returns>第一行第一列的数据</returns>
        public object ExecuteScalar(string sql, IDictionary<string, object> para)
        {
            //要返回的对象
            object obj = null;
            //给命令对象设sql
            command.CommandText = sql;
            if (para != null && para.Count > 0)
            {
                //填充参数
                foreach (var v in para)
                {
                    IDbDataParameter pa = this.dataAccessInit.GetDataParameter();
                    pa.ParameterName = v.Key;
                    pa.Value = v.Value;
                    command.Parameters.Add(pa);
                }
            }
            try
            {
                //打开数据连接
                this.conn.Open();
                obj=this.command.ExecuteScalar();
            }
            //捕获异常
            catch (Exception ex)
            {
                throw ex;
            }
            //关闭资源
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    this.conn.Close();
                }
            }
            //返会对象
            return obj;
        }
        
    }
}
