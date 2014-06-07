using System;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Win32;
using Monitor.Core.Utilities;

namespace JunctionShell
{
    internal sealed class JunctionHelper
    {
        public void Paste()
        {
            var paths = Clipboard.GetFileDropList();
            foreach (string path in paths)
            {
                DirectoryInfo dir = new DirectoryInfo(path);
                DirectoryInfo ddir = new DirectoryInfo(Environment.CurrentDirectory + "\\" + dir.Name);

                // 获取path的原始路径
                string cur = path;
                string next = path;
                do 
                {
                    cur = next;
                    next = JunctionPoint.GetTarget(next);
                } while (next != null);

                if (cur != path)
                {
                    dir = new DirectoryInfo(cur);
                }

                if (dir.Exists)
                {
                    if (ddir.Exists)
                    {
                        if (MessageBox.Show(dir.Name + "已经存在,是否覆盖?", "提问", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1) != DialogResult.Yes)
                        {
                            continue;
                        }
                        ddir.Delete(true);
                    }
                    JunctionPoint.Create(ddir.FullName, dir.FullName, true);
                }
            }
        }

        public void Reg()
        {
            var key = Registry.ClassesRoot.OpenSubKey(@"Directory\Background\shell", true);
            var junc = key.OpenSubKey("junction", true);
            if (junc == null)
            {
                junc = key.CreateSubKey("junction");
            }
            junc.SetValue(string.Empty, "JunctionPaste");
            var command = junc.OpenSubKey("Command", true);
            if (command == null)
            {
                command = junc.CreateSubKey("Command");
            }
            command.SetValue(string.Empty, "\"" + CurrentPath + "\\" + FileName + "\" /p");
        }

        public void UnReg()
        {
            var key = Registry.ClassesRoot.OpenSubKey(@"Directory\Background\shell", true);
            if (key.OpenSubKey("junction") != null)
            {
                key.DeleteSubKeyTree("junction");
            }
        }

        public static string CurrentPath
        {
            get
            {
                return Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase).Replace("file:\\", "");
            }
        }

        public static string FileName
        {
            get
            {
                return Path.GetFileName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            }
        }

    }
}
