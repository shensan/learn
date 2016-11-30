using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyDll
{
    /// <summary>
    /// [功能描述: 测试老师]<br></br>
    /// [创建者:   张联珠]<br></br>
    /// [创建时间: 2012-12-31]<br></br>
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
    public class Teacher
    {
        private string name="xxx";

        public Teacher()
        {
        }

        public Teacher(string name)
        {
            this.name = name;
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
        public virtual void Say()
        {
            Console.WriteLine("我是老师"+name);
        }
    }
}
