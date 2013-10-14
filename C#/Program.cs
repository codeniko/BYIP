using System;
using System.Threading;

namespace BYIP
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            while (true)
            {
                cURL curl = new cURL();
                try
                {
					//Attempt to record a new IP (if changed)
					String url = "http://www.example.com/by.php?write" 
                    String src = curl.get(url);
                }
                catch (Exception e)
                {
                    Thread.Sleep(60000);
                    continue;
                }
                Thread.Sleep(300000);
            }
        }
    }
}
