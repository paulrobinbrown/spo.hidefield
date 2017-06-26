using System;
using System.IO;
using System.Reflection;

namespace SPO.HideField
{
    class MessageLog
    {
        public static void Write(string message)
        {
            StreamWriter sw = null;

            try
            {
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                path += "\\log.txt";

                if (!File.Exists(path))
                    sw = File.CreateText(path);
                else
                    sw = File.AppendText(path);

                Console.WriteLine(message);
                sw.WriteLine("[" + DateTime.Now.ToString("dd:MM:yyyy HH:mm") + "] " + message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing to log:\n{0}", ex.Message);
            }
            finally
            {
                if (sw != null)
                {
                    sw.Flush();
                    sw.Close();
                }
            }
        }
    }
}
