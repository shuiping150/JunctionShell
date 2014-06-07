using System.Windows.Forms;
using System;

namespace JunctionShell
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            JunctionHelper helper = new JunctionHelper();
            if (args.Length == 0)
            {
                helper.Reg();
                MessageBox.Show("注册成功");
            }
            else
            {
                if (args[0].StartsWith("/"))
                {
                    switch (args[0].Trim().ToLower())
                    {
                        case "/i":
                            helper.Reg();
                            MessageBox.Show("注册成功");
                    	break;
                        case "/u":
                            helper.UnReg();
                            MessageBox.Show("卸载成功");
                        break;
                        case "/p":
                            helper.Paste();
                        break;
                    }
                }
            }
        }
    }
}
