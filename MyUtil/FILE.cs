using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyUtil
{
    public class FILE
    {

        public static bool saveToFile(string msg, string fileName)
        {
            bool success = true;
            StreamWriter file = null;
            try
            {
                file = File.AppendText(fileName);
                file.WriteLine(msg);
            }
            catch
            {
                success = false;
            }
            finally
            {
                if (file != null) file.Close();
            }
            return success;
        }

    }
}
