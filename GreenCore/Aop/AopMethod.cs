using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GreenCore.GreenAop
{
    /// <summary>
    /// [功能描述: 供Aop调的方法]<br></br>
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
    public static class AopMethod
    {
        /// <summary>
        /// 执行拦截器集合的前置通知
        /// </summary>
        /// <param name="operationName">拦截的方法名</param>
        /// <param name="inputs">拦截的输入参数</param>
        /// <param name="interList">拦截器集合</param>
        public static void ExcutInterceptorBeforeCall(string operationName, object[] inputs, List<IInterceptor> interList)
        {
            foreach(IInterceptor obj in interList)
            {
                obj.BeforeCall(operationName, inputs);
            }
        }

        /// <summary>
        /// 执行拦截器集合的后置通知
        /// </summary>
        /// <param name="operationName">拦截的方法名</param>
        /// <param name="returnValue">拦截方法的返回值</param>
        /// <param name="correlationState">拦截方法的状态</param>
        /// <param name="interList">拦截器集合</param>
        public static void ExcutInterceptorAfterCall(string operationName, object returnValue, object correlationState, List<IInterceptor> interList)
        {
            foreach (IInterceptor obj in interList)
            {
                obj.AfterCall(operationName, returnValue, correlationState);
            }
        }
    }
}
