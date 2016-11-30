using System;
using System.Collections;

namespace GreenEventNotification
{
	
    /// [功能描述: 事件通知服务端负责接收来自客户端的注册和当有消息发送到服务端时通知客户端。]<br></br>
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
	public class EventServer :  MarshalByRefObject
	{
        //hastable跟踪注册/订阅的所有事件
		private Hashtable delegateMap =new Hashtable();

        /// <summary>
        /// 空的构造函数
        /// </summary>
		public EventServer()
		{
		}

		/// <summary>
        /// 保持事件服务器永远活着
		/// </summary>
		/// <returns></returns>
		public override  object InitializeLifetimeService()
		{
            //反对无限期住
			return null;
		}
	
		/// <summary>
		/// 订阅事件
		/// </summary>
		/// <param name="eventName">事件名</param>
		/// <param name="handler">客户端委托，用于通知</param>
		public void SubscribeEvent(string eventName,EventClient.EventProcessingHandler handler)
		{
            //确保只有一个线程一次修改委托链。
			lock(this)
			{
                //检查是否已经注册该事件
				Delegate delegateChain = (Delegate)delegateMap[eventName];
				//如果委托链为空，说明没注册。添加客户端委托作为最初的处理程序。否则，添加委托链。
				if (delegateChain == null)
				{
					delegateMap.Add(eventName,handler);
				}
				else
				{
					//添加委托到委托链
					delegateChain = Delegate.Combine(delegateChain,handler);
                    //在哈希表中重置委托链
					delegateMap[eventName] = delegateChain;
				}
			}
		}
		
		/// <summary>
        ///取消订阅事件
		/// </summary>
		/// <param name="eventName">事件名称</param>
		/// <param name="handler">客户端委托</param>
		public void UnSubscribeEvent(string eventName,EventClient.EventProcessingHandler handler)
		{
            //确保只有一个线程一次修改委托链。
			lock(this)
			{
				Delegate delegateChain = (Delegate)delegateMap[eventName];
				//如果委托链不为空，从委托链删除委托
				if (delegateChain != null)
				{
                    //从委托链删除指定的委托
					delegateChain = Delegate.Remove(delegateChain,handler);
                    //在哈希表中重置委托链
					delegateMap[eventName] = delegateChain;
				}
			}
		}

		/// <summary>
		/// 对于一个特定的通知执行委托链
		/// </summary>
		/// <param name="eventName">事件名称</param>
		/// <param name="content">通知的内容</param>
		public void RaiseEvent(string eventName, object content)
		{
            //找到委托链
			Delegate delegateChain = (Delegate)delegateMap[eventName];
            if (delegateChain != null)
            {
                //从委托链检索委托列表
                IEnumerator invocationEnumerator = delegateChain.GetInvocationList().GetEnumerator();
                //遍历每个委托并调用它
                while (invocationEnumerator.MoveNext())
                {
                    //获得当前委托
                    Delegate handler = (Delegate)invocationEnumerator.Current;
                    try
                    {
                        //执行委托的方法
                        handler.DynamicInvoke(new object[] { eventName, content });
                    }
                    catch
                    {
                        //如果客户端接收事件通不可用，去掉其委托
                        delegateChain = Delegate.Remove(delegateChain, handler);
                        //在哈希表中重置委托链
                        delegateMap[eventName] = delegateChain;
                    }
                }
            }

			
		}
	}
}
