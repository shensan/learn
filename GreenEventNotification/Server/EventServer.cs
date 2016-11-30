using System;
using System.Collections;

namespace GreenEventNotification
{
	
    /// [��������: �¼�֪ͨ����˸���������Կͻ��˵�ע��͵�����Ϣ���͵������ʱ֪ͨ�ͻ��ˡ�]<br></br>
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
	public class EventServer :  MarshalByRefObject
	{
        //hastable����ע��/���ĵ������¼�
		private Hashtable delegateMap =new Hashtable();

        /// <summary>
        /// �յĹ��캯��
        /// </summary>
		public EventServer()
		{
		}

		/// <summary>
        /// �����¼���������Զ����
		/// </summary>
		/// <returns></returns>
		public override  object InitializeLifetimeService()
		{
            //����������ס
			return null;
		}
	
		/// <summary>
		/// �����¼�
		/// </summary>
		/// <param name="eventName">�¼���</param>
		/// <param name="handler">�ͻ���ί�У�����֪ͨ</param>
		public void SubscribeEvent(string eventName,EventClient.EventProcessingHandler handler)
		{
            //ȷ��ֻ��һ���߳�һ���޸�ί������
			lock(this)
			{
                //����Ƿ��Ѿ�ע����¼�
				Delegate delegateChain = (Delegate)delegateMap[eventName];
				//���ί����Ϊ�գ�˵��ûע�ᡣ��ӿͻ���ί����Ϊ����Ĵ�����򡣷������ί������
				if (delegateChain == null)
				{
					delegateMap.Add(eventName,handler);
				}
				else
				{
					//���ί�е�ί����
					delegateChain = Delegate.Combine(delegateChain,handler);
                    //�ڹ�ϣ��������ί����
					delegateMap[eventName] = delegateChain;
				}
			}
		}
		
		/// <summary>
        ///ȡ�������¼�
		/// </summary>
		/// <param name="eventName">�¼�����</param>
		/// <param name="handler">�ͻ���ί��</param>
		public void UnSubscribeEvent(string eventName,EventClient.EventProcessingHandler handler)
		{
            //ȷ��ֻ��һ���߳�һ���޸�ί������
			lock(this)
			{
				Delegate delegateChain = (Delegate)delegateMap[eventName];
				//���ί������Ϊ�գ���ί����ɾ��ί��
				if (delegateChain != null)
				{
                    //��ί����ɾ��ָ����ί��
					delegateChain = Delegate.Remove(delegateChain,handler);
                    //�ڹ�ϣ��������ί����
					delegateMap[eventName] = delegateChain;
				}
			}
		}

		/// <summary>
		/// ����һ���ض���ִ֪ͨ��ί����
		/// </summary>
		/// <param name="eventName">�¼�����</param>
		/// <param name="content">֪ͨ������</param>
		public void RaiseEvent(string eventName, object content)
		{
            //�ҵ�ί����
			Delegate delegateChain = (Delegate)delegateMap[eventName];
            if (delegateChain != null)
            {
                //��ί��������ί���б�
                IEnumerator invocationEnumerator = delegateChain.GetInvocationList().GetEnumerator();
                //����ÿ��ί�в�������
                while (invocationEnumerator.MoveNext())
                {
                    //��õ�ǰί��
                    Delegate handler = (Delegate)invocationEnumerator.Current;
                    try
                    {
                        //ִ��ί�еķ���
                        handler.DynamicInvoke(new object[] { eventName, content });
                    }
                    catch
                    {
                        //����ͻ��˽����¼�ͨ�����ã�ȥ����ί��
                        delegateChain = Delegate.Remove(delegateChain, handler);
                        //�ڹ�ϣ��������ί����
                        delegateMap[eventName] = delegateChain;
                    }
                }
            }

			
		}
	}
}
