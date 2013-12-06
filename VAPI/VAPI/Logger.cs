using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace VAPI
{
    public class Logger
    {
        public static StreamWriter LogFile;
        public static void Init()
        {
            LogFile = new StreamWriter("log.txt");
        }

        public static void Write(string Text)
        {
            Debug.WriteLine(Text);
            LogFile.WriteLine(Text);

        }
    }
}
