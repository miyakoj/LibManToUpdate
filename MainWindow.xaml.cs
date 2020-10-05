using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;

namespace LibmanToUpdate
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

        /// <summary>
        /// Opens a file selection dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "LibMan JSON file (libman.json)";
            openFileDialog.Multiselect = false;

			if (openFileDialog.ShowDialog() == true) {
                string json = File.ReadAllText(openFileDialog.FileName);
                JsonTextReader reader = new JsonTextReader(new StringReader(json));

                while (reader.Read())
                {
                    Console.WriteLine("Token: {0}", reader.TokenType);
                }
            }
        }

        /// <summary>
        /// Opens a file save dialog for the libraries with updates results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void librariesToUpdate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
