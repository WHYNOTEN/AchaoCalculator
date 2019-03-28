using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
namespace Calculator
{
    class Program
    {
        //获得用户要求的题目数目
        static int GetAccount()
        {
            Console.WriteLine("请输入生成题目数目:");
            string a = Console.ReadLine();
            try
            {
                int account = Convert.ToInt32(a);
                return account;
            }
            catch (SystemException e)
            {
                Console.WriteLine(e.Data);
                Console.WriteLine("输入错误，请重新输入(一个整数)");
                return GetAccount();

            }
        }
        static void Main(string[] args)
        {
            //实例化Class1并传入文件路径
            string path = @"C:\Users\Administrator\Desktop\WY\ConsoleApp1\ConsoleApp1\\result.txt";
            Class1 a = new Class1(path);
            int sum = GetAccount();
            //循环调用生成函数，进行多道题目生成
            for (int i = 0; i < sum; i++)
            {
                a.Comprehensive();
            }
            foreach (string _ in Class1.vs)
            {
                Console.Write(_);
            }
            Console.WriteLine("已生成题目，文件位于:{0}", path);
        }

    }
}
