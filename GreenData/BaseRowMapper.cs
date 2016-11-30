using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;

namespace GreenData
{
    /// <summary>
    /// [功能描述: 数据映射器基类]<br></br>
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
    public abstract class BaseRowMapper<T> //where T : global::GreenData.BaseDto, new()
    {
        protected BaseRowMapper()
        {
        }

        /// <summary>
        /// 字段映射
        /// </summary>
        /// <param name="dto">数据实体</param>
        /// <param name="dataReader">数据读写器</param>
        public abstract void FieldMap(T dto, IDataReader dataReader);
  
        /// <summary>
        /// 将DataReader转换为Dto
        /// </summary>
        /// <param name="dataReader">数据库行读取器</param>
        /// <param name="rowNum">行编号</param>
        /// <returns>Dto</returns>
        public abstract T MapRow(DbDataReader dataReader, int rowNum);
     
    }
}
