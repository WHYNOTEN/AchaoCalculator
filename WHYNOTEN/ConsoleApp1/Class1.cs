using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Threading.Tasks;
using System.Threading;
using System.Data;

namespace Calculator
{

    class Class1
    {
        //定义字符串类型保存+-*/
        private String Const = "+-*/";
        //记录算式
        public List<string> question = new List<string>();
        //存储算式
        public static List<string> vs = new List<string>();
        //定义静态随机对象
        private static Random rd = new Random(3);
        //定义Datable对象，用于计算符合规则的算式答案
        DataTable cal = new DataTable();
        //声明文件路径
        string path = "";
        public Class1(string a)
        {
            //传入txt文件保存路径
            this.path = a;
        }
        //记录操作数个数
        private int account = 0;
        //清空变量，防止堆栈溢出
        private void ClearVars()
        {
            this.question.Clear();
        }
        //随机赋值函数
        public void RandValues()
        {
            this.account = rd.Next(3, 5);
            if (this.account == 3)
            {
                this.Assit(5);
            }
            else
            {
                this.Assit(7);
            }
        }
        //辅助赋值函数
        private void Assit(int a)
        {
            for (int i = 1; i <= a; i++)
            {
                if (i % 2 == 0)
                {
                    //随机获得操作符
                    int index = rd.Next(0, 4);
                    this.question.Add(this.Const[index].ToString());
                }
                else
                {   //随机获得操作数
                    this.question.Add(rd.Next(1, 101).ToString());
                }
            }
        }
        //转存算式
        private List<string> CopyList()
        {
            List<string> temp = new List<string>();
            foreach (string _ in this.question)
            {
                temp.Add(_);
            }
            return temp;
        }
        //对减法与除法进行判断
        private bool Judge(int num1, string str1, int num2)
        {
            switch (str1)
            {
                case "/":
                    if (num1 % num2 == 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                case "-":
                    if (num1 < num2)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                default:
                    return false;
            }
        }
        //递归函数，判断算式中所有的除法是否为非整除
        private bool JudgeDiv(List<string> args)
        {
            int index = args.IndexOf("/");
            if (index == -1)
            {
                return true;
            }
            else
            {
                int num1 = Convert.ToInt32(args[index - 1]);
                int num2 = Convert.ToInt32(args[index + 1]);
                if (this.Judge(num1, "/", num2))
                {
                    args.RemoveAt(index - 1);
                    args.RemoveAt(index - 1);
                    args.RemoveAt(index - 1);
                    args.Insert(index - 1, (num1 / num2).ToString());
                    return JudgeDiv(args);
                }
                else
                {
                    return false;
                }
            }
        }
        //递归函数，当算式中除法符合规范时，对乘法进行计算
        //便于对减法进行判断
        private List<string> Multi(List<string> args_2)
        {
            int index = args_2.IndexOf("*");
            if (index == -1)
            {
                return args_2;
            }
            else
            {
                int num1 = Convert.ToInt32(args_2[index - 1]);
                int num2 = Convert.ToInt32(args_2[index + 1]);
                args_2.RemoveAt(index - 1);
                args_2.RemoveAt(index - 1);
                args_2.RemoveAt(index - 1);
                args_2.Insert(index - 1, (num1 / num2).ToString());
                return Multi(args_2);
            }
        }
        //对减法进行规则判定
        private bool Subtrace(List<string> args_3)
        {
            int index = args_3.IndexOf("-");
            if (index == -1)
            {
                return true;
            }
            else
            {
                int num1 = Convert.ToInt32(args_3[index - 1]);
                int num2 = Convert.ToInt32(args_3[index + 1]);
                if (this.Judge(num1, "-", num2))
                {
                    args_3.RemoveAt(index - 1);
                    args_3.RemoveAt(index - 1);
                    args_3.RemoveAt(index - 1);
                    args_3.Insert(index - 1, (num1 - num2).ToString());
                    return JudgeDiv(args_3);
                }
                else
                {
                    return false;
                }
            }
        }
        //综合函数依次进行除法判定，除法运算，减法判定
        public void Comprehensive()
        {
            this.RandValues();
            List<string> temp = new List<string>();
            temp = this.CopyList();
            //除法判断
            if (this.JudgeDiv(temp))
            {
                //除法判定通过，进行乘法运算
                this.Multi(temp);
                //进行减法判定
                if (this.Subtrace(temp))
                {
                    String question_ = "";
                    foreach (string _ in this.question)
                    {
                        question_ += _;
                    }
                    int answer = Convert.ToInt32(this.cal.Compute(question_, null));
                    //检查最终结果是否为负数
                    if (answer >= 0)
                    {
                        this.question.Add("=");
                        this.question.Add(Convert.ToString(answer));
                        this.question.Add("\n");
                        this.ToFile(this.question);
                        this.ClearVars();
                    }
                    else
                    {
                        this.ClearVars();
                        this.Comprehensive();
                    }
                }
                else
                {
                    //未通过则清空变量再次生成
                    this.ClearVars();
                    this.Comprehensive();
                }
            }
            else
            {
                //未通过则清空变量再次生成
                this.ClearVars();
                this.Comprehensive();
            }
        }
        ////写入文件
        private void ToFile(List<string> a)
        {
            FileInfo fileInfo = new FileInfo(this.path);
            // 通过则将算式保存，返回
            StreamWriter sw = fileInfo.AppendText();
            foreach (string _ in a)
            {
                sw.Write(_);
            }
            sw.Close();
        }
    }
}