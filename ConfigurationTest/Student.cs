using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ConfigurationTest
{
    /// <summary>
    /// 学生类
    /// </summary>
    public class Student
    {
        public void Say()
        {
            Console.WriteLine("我是学生张联珠");
        }

        public void SayString(string str)
        {
            Console.WriteLine(str);
        }

        public void AnserQuestion()
        {
            Console.WriteLine("一加一等于二");
        }
    }
}
