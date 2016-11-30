using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Reflection.Emit;
using GreenCore.GreenIoc;

namespace GreenCore.GreenAop
{
    /// <summary>
    /// [功能描述: 默认代理创建器]<br></br>
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
    public class InterceptorProxyBuilder
    {
        /// <summary>
        /// void类型
        /// </summary>
        private static readonly Type VoidType = Type.GetType("System.Void");

        /// <summary>
        /// 创建代理对象
        /// </summary>
        /// <typeparam name="T">要创建代理的类型</typeparam>
        /// <returns>代理类型</returns>
        public static object CreateProxy(Type type)
        {
            //获得T的类型
            Type classType = type;

            //代理类型的命名空间
            string name = classType.Namespace + ".Aop";
            //代理类型程序集的名字
            string fileName = name + ".dll";

            //定义程序集的唯一标识
            var assemblyName = new AssemblyName(name);
            //定义动态程序集
            var assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(assemblyName, AssemblyBuilderAccess.RunAndSave);
            //定义动态模块                                                                  
            var moduleBuilder = assemblyBuilder.DefineDynamicModule(name, fileName);
            //根据所给的类型和模块构建新类型（该类型的代理）
            var aopType = BulidType(classType, moduleBuilder);
            //保存程序集
            assemblyBuilder.Save(fileName);
            //返回构建的代理类型
            return Activator.CreateInstance(aopType);
        }

        /// <summary>
        /// 根据所给的类型和模块在该模块下构建该类型的代理类型
        /// </summary>
        /// <param name="classType">所要构建代理的类型</param>
        /// <param name="moduleBuilder">所在模块</param>
        /// <returns>构建的代理类型</returns>
        private static Type BulidType(Type classType, ModuleBuilder moduleBuilder)
        {
            //代理类型的名字
            string className = classType.Name + "Proxy";

            //根据所给的类型定义类型
            var typeBuilder = moduleBuilder.DefineType(className,
                                                       TypeAttributes.Public | TypeAttributes.Sealed | TypeAttributes.Class,
                                                       classType);
            //定义字段（拦截器） inspector
            var inspectorFieldBuilder = typeBuilder.DefineField("inspector", typeof(List<IInterceptor>),
                                                                FieldAttributes.Private | FieldAttributes.InitOnly);
            //构造代理的构造函数
            BuildCtor(classType, inspectorFieldBuilder, typeBuilder);

            //构造代理的方法
            BuildMethod(classType, inspectorFieldBuilder, typeBuilder);

            //得到构造之后的代理类型
            Type aopType = typeBuilder.CreateType();
            //返回生成的代理类型
            return aopType;
        }

        /// <summary>
        /// 构造代理的方法
        /// </summary>
        /// <param name="classType">原类型</param>
        /// <param name="inspectorFieldBuilder">拦截器</param>
        /// <param name="typeBuilder">代理类型</param>
        private static void BuildMethod(Type classType, FieldBuilder inspectorFieldBuilder, TypeBuilder typeBuilder)
        {
            //获取原类型的所有方法信息
            var methodInfos = classType.GetMethods();
            //遍历所有的方法信息
            foreach (var methodInfo in methodInfos)
            {
                //如果不是虚方法和抽象放法，进入下一个
                if (!methodInfo.IsVirtual && !methodInfo.IsAbstract) continue;
                //如果是Object的ToString方法，进入下一个
                if (methodInfo.Name == "ToString") continue;
                //如果是Object的GetHashCode方法，进入下一个
                if (methodInfo.Name == "GetHashCode") continue;
                //如果是Object的Equals方法，进入下一个
                if (methodInfo.Name == "Equals") continue;

                //获取该方法的所有参数信息
                var parameterInfos = methodInfo.GetParameters();
                //用Lamada表达式获取每个参数的类型
                var parameterTypes = parameterInfos.Select(p => p.ParameterType).ToArray();
                //获取参数的个数
                var parameterLength = parameterTypes.Length;
                //获取方法是否有返回值
                var hasResult = methodInfo.ReturnType != VoidType;

                //创建和该方法一样的方法
                var methodBuilder = typeBuilder.DefineMethod(methodInfo.Name,
                                                             MethodAttributes.Public | MethodAttributes.Final |
                                                             MethodAttributes.Virtual
                                                             , methodInfo.ReturnType
                                                             , parameterTypes);
                //得到IL加载器
                var il = methodBuilder.GetILGenerator();

                //定义三个局部变量，第一个记状态，第二个记结果，第三个记参数
                il.DeclareLocal(typeof(object)); //记状态
                il.DeclareLocal(typeof(object)); //记结果
                il.DeclareLocal(typeof(object[])); //记参数

                //object BeforeCall(string operationName, object[] inputs);前置通知方法申明
                il.Emit(OpCodes.Ldstr, methodInfo.Name);//把方法名加载到堆栈上

                if (parameterLength == 0)//判断方法参数长度，如果方法参数长度为0，将空引用推送到堆栈上
                {
                    il.Emit(OpCodes.Ldnull);//null -> 参数 inputs（把输入参数设为空）
                }
                //如果参数长度不为0，加入参数到前置通知的参数里
                else
                {
                    //创建new object[parameterLength];
                    il.Emit(OpCodes.Ldc_I4, parameterLength);//把参数的长度入栈
                    il.Emit(OpCodes.Newarr, typeof(Object));//创建长度为参数长度的Object数组
                    il.Emit(OpCodes.Stloc_2);//把创建的Object数组压入局部变量2（前面创的参数的局部变量，相当于实例化参数的局部变量）

                    //循环加入每个参数，加入参数局部变量数组
                    for (int i = 0, j = 1; i < parameterLength; i++, j++)
                    {
                        il.Emit(OpCodes.Ldloc_2);//把前面创建的参数的局部变量入栈
                        il.Emit(OpCodes.Ldc_I4, i);//把i入栈,后面用
                        il.Emit(OpCodes.Ldarg, j);//把j处的参数入栈
                        if (parameterTypes[i].IsValueType)//如果第i号参数是值类型
                        {
                            il.Emit(OpCodes.Box, parameterTypes[i]);//对值类型装箱
                        }
                        il.Emit(OpCodes.Stelem_Ref);//用(OpCodes.Ldarg, j)替换参数局部变量il.Emit(OpCodes.Ldloc_2)的第i个元素
                    }
                    il.Emit(OpCodes.Ldloc_2);//取出局部变量2 parameters-> 参数 inputs（参数变量入栈，前面方法名已入栈）
                }
                il.Emit(OpCodes.Ldarg_0);//将索引为0的参数加载到堆栈上（this指针）
                il.Emit(OpCodes.Ldfld, inspectorFieldBuilder);
                il.Emit(OpCodes.Callvirt, typeof(AopMethod).GetMethod("ExcutInterceptorBeforeCall"));//调用BeforeCall（通过前面部分已经给这个方法所需的参数入栈了）
                //il.Emit(OpCodes.Stloc_0);//建返回压入局部变量0 correlationState（把方法执行后放在栈上的参数存入索引为0的局部变量里）

                //执行方法
                il.Emit(OpCodes.Ldarg_0);//将索引为0的参数加载到堆栈上（this指针）
                //获取参数表
                for (int i = 1, length = parameterLength + 1; i < length; i++)
                {
                    il.Emit(OpCodes.Ldarg_S, i);//循环，把索引为i的参数依次加入栈
                }
                il.Emit(OpCodes.Call, methodInfo);//执行方法
                //将返回值压入 局部变量1result void就压入null
                if (!hasResult)//如果没有返回值，说明方法没往堆栈上放值
                {
                    il.Emit(OpCodes.Ldnull);//将空引用加到堆栈上
                }
                else if (methodInfo.ReturnType.IsValueType)//如果返回值是值类型，进行装箱
                {
                    il.Emit(OpCodes.Box, methodInfo.ReturnType);//对值类型装箱
                }
                il.Emit(OpCodes.Stloc_1);//把放在对上的空值或返回值放入编号为1的局部变量（存放返回值的变量）

                //AfterCall(string operationName, object returnValue, object correlationState);
                il.Emit(OpCodes.Ldstr, methodInfo.Name);//把方法名入栈
                il.Emit(OpCodes.Ldloc_1);//把索引为1的局部变量入栈（返回值）
                il.Emit(OpCodes.Ldloc_0);// 把索引为0的局部变量入栈（状态）
                il.Emit(OpCodes.Ldarg_0);//将索引为0的参数加载到堆栈上（this指针）
                il.Emit(OpCodes.Ldfld, inspectorFieldBuilder);
                il.Emit(OpCodes.Callvirt, typeof(AopMethod).GetMethod("ExcutInterceptorAfterCall"));//调用BeforeCall（通过前面部分已经给这个方法所需的参数入栈了）
                //il.Emit(OpCodes.Callvirt, typeof(IInterceptor).GetMethod("AfterCall"));//调用方法

                //result
                if (!hasResult)//如果没有返回值，返回方法
                {
                    il.Emit(OpCodes.Ret);//返回方法
                    continue;
                }
                il.Emit(OpCodes.Ldloc_1);//把索引为1的局部变量入栈（即返回值局部变量入栈）
                if (methodInfo.ReturnType.IsValueType)//如果返回值是值类型就进行装箱
                {
                    il.Emit(OpCodes.Unbox_Any, methodInfo.ReturnType);//对值类型拆箱
                }
                il.Emit(OpCodes.Ret);//返回，此时把堆栈的返回值给返回回去了
            }
        }

        /// <summary>
        /// 构造代理的构造函数
        /// </summary>
        /// <param name="classType">原类型</param>
        /// <param name="inspectorFieldBuilder">拦截器</param>
        /// <param name="typeBuilder">代理类型</param>
        private static void BuildCtor(Type classType, FieldBuilder inspectorFieldBuilder, TypeBuilder typeBuilder)
        {
            {
                //定义代理的构造函数
                var ctorBuilder = typeBuilder.DefineConstructor(MethodAttributes.Public, CallingConventions.HasThis,
                                                                Type.EmptyTypes);
                //获得IL指令器
                var il = ctorBuilder.GetILGenerator();

                il.Emit(OpCodes.Ldarg_0);//把索引为0的参数加载到堆栈上（this指针）
                il.Emit(OpCodes.Call, classType.GetConstructor(Type.EmptyTypes));//调用base的默认构造函数
                il.Emit(OpCodes.Ldarg_0);//把索引为0的参数加载到堆栈上（this指针）
                //将typeof(classType)压入计算堆
                il.Emit(OpCodes.Ldtoken, classType);//类型入栈
                il.Emit(OpCodes.Call, typeof(Type).GetMethod("GetTypeFromHandle", new[] { typeof(RuntimeTypeHandle) }));

                #region 创建并实例化拦截器
                //调用DefaultInterceptorFactory.Create(type)
                il.Emit(OpCodes.Call, typeof(ObjectContainer).GetMethod("GetInterceptorByType", new[] { typeof(Type) }));//调用IOC，获得拦截器
                //将结果保存到字段inspector
                il.Emit(OpCodes.Stfld, inspectorFieldBuilder);//把获得的拦截器赋给拦截器变量
                #endregion
                il.Emit(OpCodes.Ret);//返回
            }
        }
    }
}
