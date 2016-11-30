using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenCore.GreenAop;

namespace MyDll
{
    /// <summary>
    /// [功能描述: 拦截的测试事例]<br></br>
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
    public class StandardInterceptor2 : IInterceptor
    {

        /// <summary>
        /// 方法调用前
        /// </summary>
        /// <param name="operationName">方法名</param>
        /// <param name="inputs">参数</param>
        /// <returns>状态对象，用于调用后传入</returns>
        public void BeforeCall(string operationName, object[] inputs)
        {
            Console.WriteLine("拦截2");
            Console.WriteLine("调用方法{0}前 :", operationName);
            if (inputs != null)
            {
                int i = 0;
                foreach (var obj in inputs)
                {
                    i++;
                    Console.WriteLine("第{0}个参数为:{1}",i, obj);
                }
            }
            Console.WriteLine("拦截2");
        }

        /// <summary>
        /// 方法调用后
        /// </summary>
        /// <param name="operationName">方法名</param>
        /// <param name="returnValue">结果</param>
        /// <param name="correlationState">状态对象</param>
        public void AfterCall(string operationName, object returnValue, object correlationState)
        {
            correlationState = "正常结束";
            Console.WriteLine("拦截2");
            Console.WriteLine("调用方法{0}后:\n结果: {1}\n状态:{2}", operationName, returnValue ?? "空", correlationState);
            Console.WriteLine("拦截2");
        }
    }
}
