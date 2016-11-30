using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Reflection;
using System.Configuration;
using GreenCore.GreenAop;
using GreenConfiguration;

namespace GreenCore.GreenIoc
{
    /// <summary>
    /// [功能描述: 容器类（实现了IOC）-静态类]<br></br>
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
    public  static class ObjectContainer
    {
        /// <summary>
        /// 对象的容器，存放注入的对象
        /// </summary>
        private static IDictionary<string, object> objList = new Dictionary<string, object>();

        /// <summary>
        /// 类型容器，存放注入的类型
        /// </summary>
        private static IDictionary<string, Type> typeList = new Dictionary<string, Type>();

        /// <summary>
        /// 框架配置管理器
        /// </summary>
        private static ConfigurationManager framConfMana = null;

        private static Dictionary<string, IocConfigDto> iocConfigList = null;
        private static Dictionary<string, AopConfigDto> aopConfigList = null;

        /// <summary>
        /// 注入和拦截配置文件路径
        /// </summary>
        //private static string iocAopConfigPath;

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="xmlFileName"></param>
        static ObjectContainer()
        {
           framConfMana=(ConfigurationManager)ConfigurationSettings.GetConfig("GreenFram");
           iocConfigList=framConfMana.CoreConfig.GreenIocConfiguration.GetIocConfigList();
           aopConfigList = framConfMana.CoreConfig.GreenAopConfiguration.GetAopConfigList();
            //注入对象类型
           InitTypes();
           //注入对象
           InitObjects();
           //注入属性
           DiObject();
           //注入代理
           InitProxyTypes();
           
        }

        /// <summary>
        /// 实例类型，注入Ioc配置的所有的类型
        /// </summary>
        private static void InitTypes()
        {
            //获得所有注入配置的程序集路径
            var paths = (from obj in iocConfigList.Values select obj.DllName).Distinct();
            //遍历所有配置下的程序集路径
            foreach (var asemblyPath in paths)
            {
                //从指定路径读取程序集
                Assembly assembly = Assembly.LoadFile(Environment.CurrentDirectory + "\\" + asemblyPath + ".dll");
                //把所有的类型加入类型字典
                foreach (var type in assembly.GetTypes())
                {
                    typeList.Add(type.FullName, type);
                }
            }
        }

       /// <summary>
        /// 实例类型，注入所有代理的类型
       /// </summary>
        private static void InitProxyTypes()
        {
            InterceptorProxyBuilder proxyBuilder = new InterceptorProxyBuilder();

            //遍历所有的需要代理的类型
            foreach (var aopConfig in aopConfigList)
            {
                try
                {
                    //调用生成代理的方法
                    var proxy = InterceptorProxyBuilder.CreateProxy(GetType(aopConfig.Value.Type));
                    //把生成的代理对象放入对象容器
                    objList.Add(aopConfig.Key, proxy);
                    //把类型放入类型容器
                    typeList.Add(proxy.GetType().FullName, proxy.GetType());
                }
                catch
                {
                    throw new Exception("创建" + aopConfig.Value.Type + "的代理失败");
                }
            }
            
        }

       /// <summary>
        /// 实例Ioc容器，先注入全部的对象，再对对象注入属性，防止注入时找不到对象
       /// </summary>
        private static void InitObjects()
        {
            //遍历所有的注入配置对象
            foreach (var iocConfig in iocConfigList)
            {
                //用类型名获得当前配置的类型
                Type type = GetType(iocConfig.Value.Type);
                object obj=Activator.CreateInstance(type);
                //把对象加入对像容器
                objList.Add(iocConfig.Key, obj);
            }
        }

        /// <summary>
        /// 属性注入
        /// </summary>
        private static void DiObject()
        {
            #region 给依赖注入注入属性
            //遍历所有的注入配置对象
            foreach (var iocConfig in iocConfigList)
            {
                //获得当前对象的属性
                var propertys=iocConfig.Value.Propertys;
                //如果有属性
                if (propertys != null)
                {
                    //用类型名获得当前配置的类型
                    Type type = GetType(iocConfig.Value.Type);
                    
                    //遍历所有属性
                    foreach (var property in propertys)
                    {
                        //要赋值给属性的对象
                        object obj = null;
                        //如果属性是引用型，获取引用的对象
                        if (property.Type == "ref")
                        {
                            obj = objList[property.Value];
                        }
                        //否则直接赋值
                        else
                        {
                            obj = property.Value;
                        }
                        //获得指定的属性
                        PropertyInfo proper = type.GetProperty(property.Key);
                        //设置属性值
                        proper.SetValue(objList[iocConfig.Key], obj, null);
                    }
                }
            }
            #endregion
            #region 给所有的拦截注入属性
            //遍历所有的注入配置对象
            foreach (var aopConfig in aopConfigList)
            {
                //获得当前对象的属性
                var propertys = aopConfig.Value.Propertys;
                //如果有属性
                if (propertys != null)
                {
                    //用类型名获得当前配置的类型
                    Type type = GetType(aopConfig.Value.Type);

                    //遍历所有属性
                    foreach (var property in propertys)
                    {
                        //要赋值给属性的对象
                        object obj = null;
                        //如果属性是引用型，获取引用的对象
                        if (property.Type == "ref")
                        {
                            obj = objList[property.Value];
                        }
                        //否则直接赋值
                        else
                        {
                            obj = property.Value;
                        }
                        //获得指定的属性
                        PropertyInfo proper = type.GetProperty(property.Key);
                        //设置属性值
                        proper.SetValue(objList[aopConfig.Key], obj, null);
                    }
                }
            }
            #endregion
        }

        /// <summary>
        /// 根据名字获得单例对象
        /// </summary>
        /// <param name="objName">对象标示名</param>
        /// <returns>返回的对象</returns>
        public static object GetSingletonObject(string objName)
        {
            object obj = null;
            if (objList.ContainsKey(objName))
            {
                obj = objList[objName];
            }
            return obj;
        }

        /// <summary>
        /// 根据名字获得新对象
        /// </summary>
        /// <param name="objName">对象标示名</param>
        /// <returns>返回的对象</returns>
        public static object GetNewObject(string objName)
        {
            object obj = null;
            if (objList.ContainsKey(objName))
            {
                obj = objList[objName];
            }
            return obj;
        }

        /// <summary>
        /// 根据给定类型获得该类型的单例对象
        /// </summary>
        /// <typeparam name="T">给定的类型</typeparam>
        /// <returns></returns>
        public static object GetSingletonObject<T>()
        {
            return null;
        }

         /// <summary>
        /// 根据给定类型获得该类型的单例对象
        /// </summary>
        /// <typeparam name="T">给定的类型</typeparam>
        /// <returns></returns>
        public static object GetNewObject<T>()
        {
            return null;
        }

        /// <summary>
        /// 通过类型全名获得类型,供内部实例化对象时使用
        /// </summary>
        /// <param name="typeFullName">类型全名</param>
        /// <returns>类型</returns>
        private static Type GetType(string typeFullName)
        {
            if (typeList.Keys.Contains(typeFullName))
            {
                return typeList[typeFullName];
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 通过类型获取在配置文件中为该类型配置的拦截器
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static object GetInterceptorByType(Type t)
        {
            List<IInterceptor> interList = new List<IInterceptor>();
            //遍历所有的注入配置对象
            foreach (var aopConfig in aopConfigList)
            {
                //如果当前配置的引用类型等于要找的类型，读取它的拦截器配置
                if (aopConfig.Value.Type == t.FullName)
                {
                    //获得所有的拦截器名
                    var interceptors=aopConfig.Value.Interceptors;
                    if (interceptors != null)
                    {
                        //遍历获得所有的拦截器对象
                        foreach (var interceptor in interceptors)
                        {
                            interList.Add((IInterceptor)objList[interceptor]);
                        }   
                    }
                    //退出，不用再往下找了
                    break;
                }
            }
            //返回找到的拦截器对象
            return interList;
        }

    }
}
