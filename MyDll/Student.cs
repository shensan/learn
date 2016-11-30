using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MyDll
{
    /// <summary>
    /// [功能描述: 测试学生]<br></br>
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
    public class Student
    {
        private string name = "xxx";

        public virtual void Say()
        {
            Console.WriteLine("我是学生"+name);
            if (MyTeacher != null)
            {
                Console.WriteLine("我的老师是:" + MyTeacher.Name);
            }
            Console.WriteLine();
        }

        public Student()
        {
        }

        public string Name
        {
            get
            {
                return name;
            }
        }
        public Student(string name)
        {
            this.name = name;
        }

        /// <summary>
        /// 加
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public virtual double Add(double a,double b)
        {
            return a + b;
        }

        public virtual double Sqrt(double a)
        {
            return a * a;
        }

        public virtual  void Spell(string str)
        {
            Console.WriteLine("{0}说：{1}",name,str);
        }
        public Teacher MyTeacher
        {
            get;
            set;
        }
    }
}
