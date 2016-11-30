using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenCore.GreenAop
{
    /// <summary>
    /// [功能描述: 通知的接口]<br></br>
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
    public interface IInterceptor
    {

        /// <summary>
        /// 方法调用前
        /// </summary>
        /// <param name="operationName">方法名</param>
        /// <param name="inputs">参数</param>
        /// <returns>状态对象，用于调用后传入</returns>
        void BeforeCall(string operationName, object[] inputs);


        /// <summary>
        /// 方法调用后
        /// </summary>
        /// <param name="operationName">方法名</param>
        /// <param name="returnValue">结果</param>
        /// <param name="correlationState">状态对象</param>
        void AfterCall(string operationName, object returnValue, object correlationState);
    }
}
