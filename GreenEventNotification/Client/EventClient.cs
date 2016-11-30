using System;
using System.Reflection;
using System.Collections;
using System.Threading;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels.Http;
using System.Runtime.Remoting.Channels;
using System.Runtime.Serialization;
using System.Runtime.Remoting.Activation;

namespace GreenEventNotification
{
	
    /// [功能描述: 事件通知服务客户端是用来和事件通知服务端通讯的。它提供了客户端订阅，取消订阅和发布的能力。]<br></br>
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
	public class EventClient : MarshalByRefObject
	{
        //推送通知的委托，事件处理的委托
		public delegate void EventProcessingHandler(string eventName,object content);
		
		
        //代理的远程服务器对象
		private EventServer es;

        //当地hastable保持跟踪客户已订阅的消息
		private Hashtable repeatDelegate= new Hashtable();

        /// <summary>
        /// 默认的构造函数
        /// </summary>
        /// <param name="url"></param>
		public EventClient(string url)
		{			
			//建立远程双边联系
			IDictionary props = new Hashtable();
			props["port"] = 0;

			BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
			HttpChannel channel = new HttpChannel(props, clientProvider,null);      
			ChannelServices.RegisterChannel(channel);
            //获得代理的远程事件服务器对象
			es = (EventServer)Activator.GetObject(typeof(EventServer),url);
	
		}
	

		/// <summary>
		/// 订阅服务器上的事件
		/// </summary>
		/// <param name="eventName">事件名</param>
		/// <param name="s">回调的委托</param>
		public void SubscribeEvent(string eventName, EventProcessingHandler s)
		{
			try
			{
				//检查事件是否已由客户订阅
				Delegate handler =(Delegate)repeatDelegate[eventName];

				//如果已经订阅, 加入订阅链
				//否则创建新的委托并添加到订阅的Hash表
				if (handler != null)
				{
					//接入委托链
					handler = Delegate.Combine(handler, s);
                    ////在哈希表中重置委托链
					repeatDelegate[eventName] = handler;
				}
				else
				{
					repeatDelegate.Add(eventName,s);
					EventClient.EventProcessingHandler repeat = new EventClient.EventProcessingHandler(Repeat);
                    //订阅服务器上的事件
					es.SubscribeEvent(eventName,repeat);
				}
			}
			catch (Exception ex)
			{
				 Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// 取消订阅事件
		/// </summary>
		/// <param name="eventName">事件名</param>
		/// <param name="s">事件回调委托</param>
		public void UnSubscribeEvent(string eventName, EventProcessingHandler s)
		{
			//从hash表里移除订阅
			Delegate handler =(Delegate)repeatDelegate[eventName];
			//如果不为空，服务端订阅的移除由服务端检查通知不可用时自动移除
			if (handler != null)
			{
				handler = Delegate.Remove(handler, s);
                //重新分配链的哈希表
				repeatDelegate[eventName] = handler;
			}
		}

		/// <summary>
		/// 作为一个客户端通知的调度者. 被服务端执行来推送新的通知并负责调度的通知所有在客户端上的处理程序
		/// </summary>
		/// <param name="eventName">事件名</param>
        /// <param name="content">通知的内容</param>
		public void Repeat(string eventName, object content)
		{
            //从哈希表中获取委托
			EventProcessingHandler eph = (EventProcessingHandler)repeatDelegate[eventName];
			if (eph !=null)
			{
				//执行委托
				eph(eventName, content);
			}
		}

		/// <summary>
        /// 引发事件
		/// </summary>
		/// <param name="eventName">事件名</param>
        /// <param name="content">通知的内容</param>
		public void RaiseEvent(string eventName, object content)
		{
			es.RaiseEvent(eventName,content);
		}
	}
}
