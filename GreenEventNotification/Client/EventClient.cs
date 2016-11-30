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
	
    /// [��������: �¼�֪ͨ����ͻ������������¼�֪ͨ�����ͨѶ�ġ����ṩ�˿ͻ��˶��ģ�ȡ�����ĺͷ�����������]<br></br>
    /// [������:   ������]<br></br>
    /// [����ʱ��: 2013-9-17]<br></br>
    /// <˵��>
    ///    
    /// </˵��>
    /// <�޸ļ�¼>
    ///     <�޸�ʱ��></�޸�ʱ��>
    ///     <�޸�����>
    ///            
    ///     </�޸�����>
    /// </�޸ļ�¼>
    /// </summary>
	public class EventClient : MarshalByRefObject
	{
        //����֪ͨ��ί�У��¼������ί��
		public delegate void EventProcessingHandler(string eventName,object content);
		
		
        //�����Զ�̷���������
		private EventServer es;

        //����hastable���ָ��ٿͻ��Ѷ��ĵ���Ϣ
		private Hashtable repeatDelegate= new Hashtable();

        /// <summary>
        /// Ĭ�ϵĹ��캯��
        /// </summary>
        /// <param name="url"></param>
		public EventClient(string url)
		{			
			//����Զ��˫����ϵ
			IDictionary props = new Hashtable();
			props["port"] = 0;

			BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
			HttpChannel channel = new HttpChannel(props, clientProvider,null);      
			ChannelServices.RegisterChannel(channel);
            //��ô����Զ���¼�����������
			es = (EventServer)Activator.GetObject(typeof(EventServer),url);
	
		}
	

		/// <summary>
		/// ���ķ������ϵ��¼�
		/// </summary>
		/// <param name="eventName">�¼���</param>
		/// <param name="s">�ص���ί��</param>
		public void SubscribeEvent(string eventName, EventProcessingHandler s)
		{
			try
			{
				//����¼��Ƿ����ɿͻ�����
				Delegate handler =(Delegate)repeatDelegate[eventName];

				//����Ѿ�����, ���붩����
				//���򴴽��µ�ί�в���ӵ����ĵ�Hash��
				if (handler != null)
				{
					//����ί����
					handler = Delegate.Combine(handler, s);
                    ////�ڹ�ϣ��������ί����
					repeatDelegate[eventName] = handler;
				}
				else
				{
					repeatDelegate.Add(eventName,s);
					EventClient.EventProcessingHandler repeat = new EventClient.EventProcessingHandler(Repeat);
                    //���ķ������ϵ��¼�
					es.SubscribeEvent(eventName,repeat);
				}
			}
			catch (Exception ex)
			{
				 Console.WriteLine(ex.Message);
			}
		}

		/// <summary>
		/// ȡ�������¼�
		/// </summary>
		/// <param name="eventName">�¼���</param>
		/// <param name="s">�¼��ص�ί��</param>
		public void UnSubscribeEvent(string eventName, EventProcessingHandler s)
		{
			//��hash�����Ƴ�����
			Delegate handler =(Delegate)repeatDelegate[eventName];
			//�����Ϊ�գ�����˶��ĵ��Ƴ��ɷ���˼��֪ͨ������ʱ�Զ��Ƴ�
			if (handler != null)
			{
				handler = Delegate.Remove(handler, s);
                //���·������Ĺ�ϣ��
				repeatDelegate[eventName] = handler;
			}
		}

		/// <summary>
		/// ��Ϊһ���ͻ���֪ͨ�ĵ�����. �������ִ���������µ�֪ͨ��������ȵ�֪ͨ�����ڿͻ����ϵĴ������
		/// </summary>
		/// <param name="eventName">�¼���</param>
        /// <param name="content">֪ͨ������</param>
		public void Repeat(string eventName, object content)
		{
            //�ӹ�ϣ���л�ȡί��
			EventProcessingHandler eph = (EventProcessingHandler)repeatDelegate[eventName];
			if (eph !=null)
			{
				//ִ��ί��
				eph(eventName, content);
			}
		}

		/// <summary>
        /// �����¼�
		/// </summary>
		/// <param name="eventName">�¼���</param>
        /// <param name="content">֪ͨ������</param>
		public void RaiseEvent(string eventName, object content)
		{
			es.RaiseEvent(eventName,content);
		}
	}
}
