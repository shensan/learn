using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GreenCore.GreenIoc;
using GreenConfiguration;
using MyDll;

namespace CoreTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("AOP拦截");
            Student student = (Student)ObjectContainer.GetSingletonObject("StudentProxy");
            Teacher t = (Teacher)ObjectContainer.GetSingletonObject("TeacherProxy");
            t.Say();
            if (student != null)
            {
                student.Say();
                Console.WriteLine("12=13={0}", student.Add(12, 13));
                Console.WriteLine("25的平方={0}", student.Sqrt(25));
                student.Spell("Hello");

            }

            Console.WriteLine("\n\n\n");
            Console.WriteLine("依赖注入");
            student = (Student)ObjectContainer.GetSingletonObject("Student");
            t = (Teacher)ObjectContainer.GetSingletonObject("Teacher");
            t.Say();
            if (student != null)
            {
                student.Say();
            }

            //Console.WriteLine("\n\n\n");
            //Console.WriteLine("数据访问,执行update emp set sal=sal*1");
            //GreenData data = new GreenData(new OracleClientInit(), new ReadConfig());
            //data.ExecuteNonQuery("update emp set sal=sal*1", null);
            //Console.WriteLine("数据访问成功");
            Console.Read();
        }
    }
}
