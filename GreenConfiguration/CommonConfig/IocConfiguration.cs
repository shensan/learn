using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace GreenConfiguration
{

    /// [功能描述: 依赖注入配置管理器。]<br></br>
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
    public class IocConfiguration
    {
        /// <summary>
        /// 配置对象集合
        /// </summary>
        private Dictionary<string, IocConfigDto> iocConfigList = new Dictionary<string, IocConfigDto>();

        /// <summary>
        /// 够造函数
        /// </summary>
        /// <param name="paths">配置文件的所有路径</param>
        public IocConfiguration(IList<string> paths)
        {
            //遍历所有的路径
            foreach (var path in paths)
            {
                //读取根节点
                XElement root = XElement.Load(path);
                //读取根节点分支IOC下的所有子节点
                var elements = root.Element("IOC").Elements();
                #region 读取所有的配置信息
                foreach (var element in elements)
                {
                    //创建一个Ioc配置实体
                    IocConfigDto dto = new IocConfigDto();
                    #region 读取类型和程序集信息
                    //写入注入对象的类型
                    dto.Type = element.Attribute("type").Value;
                    //写入注入对象的动态链接库名
                    dto.DllName = element.Attribute("dllName").Value;
                    #endregion
                    #region 读取属性信息
                    //找到所有属性节点
                    var propertys = element.Elements("property");
                    if (propertys != null)
                    {
                        //遍历所有属性节点
                        foreach (var property in propertys)
                        {
                            //创建一个属性配置实体
                            PropertyDto propDto = new PropertyDto();
                            //设置属性的名字
                            propDto.Key = property.Attribute("name").Value;
                            //如果属性值是引用类型
                            if (property.Attribute("ref") != null)
                            {
                                //写入属性类型
                                propDto.Type = "ref";
                                //写入属性值
                                propDto.Value = property.Attribute("ref").Value;
                            }
                            //如果属性值是值类性
                            else
                            {
                                //写入属性类型
                                propDto.Type = "value";
                                //写入属性值
                                propDto.Value = property.Attribute("value").Value;
                            }


                        }
                    }
                    #endregion
                    #region 写入配置集合
                    //写入配置集合
                    iocConfigList.Add(element.Attribute("id").Value, dto);
                    #endregion
                }
                #endregion
            }
        }

        /// <summary>
        /// 获得Ioc配置对象集合
        /// </summary>
        /// <returns>Ioc配置对象集合</returns>
        public Dictionary<string, IocConfigDto> GetIocConfigList()
        {
            return iocConfigList;
        }
    }
}
