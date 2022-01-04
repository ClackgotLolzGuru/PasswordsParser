using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace PasswordsParser
{
    class Program
    {
        static void Main(string[] args)
        {
            Regex regex = new Regex(@"Password: .*?(\n|\z|\r)");
            string txt = File.ReadAllText("аа.txt");
            var matches = regex.Matches(txt);
            List<string> passwords = new List<string>();
            foreach (Match match in matches)
            {
                passwords.Add(match.Value.Replace("Password: ", ""));
            }
            foreach (var password in passwords)
            {
                Console.WriteLine(password);
            }
        }
    }
}
