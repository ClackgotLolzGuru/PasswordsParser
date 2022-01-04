using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
//using System.Windows.Shapes;

namespace Parser
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Text Files (*.txt)|*.txt|All files (*.*)|*.*";
            if (sfd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(docBox.Document.ContentStart, docBox.Document.ContentEnd);
                using (FileStream fs = File.Create(sfd.FileName))
                {
                    if (Path.GetExtension(sfd.FileName).ToLower() == ".txt")
                        doc.Save(fs, DataFormats.Text);
                }
            }
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "RichText Files (*.txt)|*.txt|All files (*.*)|*.*";

            if (ofd.ShowDialog() == true)
            {
                TextRange doc = new TextRange(docBox.Document.ContentStart, docBox.Document.ContentEnd);
                using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
                {
                    if (Path.GetExtension(ofd.FileName).ToLower() == ".txt")
                    {
                        byte[] array = new byte[fs.Length];
                        fs.Read(array, 0, array.Length);
                        string textFromFile = Encoding.Default.GetString(array);

                        Regex regex = new Regex(@"Password: .*?(\n|\z|\r)");
                        var matches = regex.Matches(textFromFile);
                        List<string> passwords = new List<string>();
                        foreach (Match match in matches)
                        {
                            passwords.Add(match.Value.Replace("Password: ", ""));
                        }
                        if(passwords.Count == 0)
                        {
                            MessageBox.Show("Паролей не найдено");
                            return;
                        }

                        int oldCount = passwords.Count;
                        passwords = passwords.Distinct().ToList();
                        int newCount = passwords.Count;
                        MessageBox.Show($"Удалено {oldCount - newCount} дублей");
                        string result = "";
                        foreach (var password in passwords)
                        {
                            result += password;
                        }
                        doc.Text = result;

                        //doc.Load(fs, DataFormats.Text);
                    }

                }
            }
        }
    }
}
