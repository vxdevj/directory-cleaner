using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace DirectoryCleaner
{
    class MainClass
    {
        public const string version = "1.1.0";

        [STAThread]
        static void Main(string[] args)
        {
            Sorting.Initialize();
            Dialog.IntroQuery();
        }       
    }
}
