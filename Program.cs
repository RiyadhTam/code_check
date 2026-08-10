using System;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml;

namespace OWASP
{
    class Program
    {
        static void Main(string[] args)
        {
            /*Console.WriteLine("Hello World!");
            Console.Write("Enter username: ");
            string input = Console.ReadLine();

            Process process = new Process();

            process.StartInfo.FileName = "calc.exe";//"whoami.exe";
            process.StartInfo.Arguments = "-user " + input + " -role user";

            process.Start();
            */
            Console.Write("Enter username: ");
            string input = Console.ReadLine();

            //Regex usernameRegex = new Regex(@"^[A-Za-z0-9]{1,20}$");
            Regex usernameRegex = new Regex(@"^[0-9 @#$]{1,10}$");

            if (usernameRegex.IsMatch(input))
            {
                Process process = new Process();

                //process.StartInfo.FileName = "exportLegacy.exe";
                process.StartInfo.FileName = "calc.exe";
                process.StartInfo.Arguments = "-user " + input + " -role user";

                process.Start();
            }
            else
            {
                Console.WriteLine("Invalid username.");
            }
        }
        void asd()
        {
            Console.Write("Enter Device ID: ");
            string input = Console.ReadLine();

            XmlDocument doc = new XmlDocument();
            doc.XmlResolver = null;
            doc.Load("config.xml");

            string xpath =
                "/Config/Devices/Device[Id='" + input + "']";

            XmlNodeList results = doc.SelectNodes(xpath);

            foreach (XmlNode node in results)
            {
                Console.WriteLine(node["Name"].InnerText);
            }
        }
    }
}
