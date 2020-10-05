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
using Newtonsoft.Json;
using System.IO;
using System.Windows.Controls.Primitives;

namespace LibmanToUpdate
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static List<LibraryUpdateData> LibrariesToUpdate = new List<LibraryUpdateData>();

        public MainWindow()
        {
            // set up log file
            if (File.Exists(App.LogFile))
            {
                File.Delete(App.LogFile);
            }

            File.Create(App.LogFile);

            InitializeComponent();
        }

        /// <summary>
        /// Opens a file selection dialog.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inputFileButton_Click(object sender, RoutedEventArgs e)
        {
            /*Popup errorPopup = new Popup {
                Width = 100,
                Height = 75,
                Placement = PlacementMode.Center,
                PlacementTarget = this,
                IsOpen = true,
                Child = new AccessText {
                    Text = "Test"
                }
            };*/

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "LibMan JSON|libman.json";
            openFileDialog.Multiselect = false;

			if (openFileDialog.ShowDialog() == true) {
                inputFileField.Text = openFileDialog.FileName.Replace(@"\\", @"\");
                string json = File.ReadAllText(openFileDialog.FileName);
                LibManConfig libman = JsonConvert.DeserializeObject<LibManConfig>(json);
                int totalLibraries = libman.Libraries.Count;
                int counter = 0;

                foreach (var library in libman.Libraries) {
                    string provider = libman.DefaultProvider;

                    string[] tempLibraryArray = library.Library.Split("@");
                    string libraryName = tempLibraryArray[0];
                    string tempLibraryVersion = tempLibraryArray[1];
                    string[] libraryVersion = tempLibraryVersion.Split(".");

                    try {
                        int[] libraryVersionInt = { Int32.Parse(libraryVersion[0]), Int32.Parse(libraryVersion[1]), 
                            Int32.Parse(libraryVersion[2]) };

                        if (library.Provider != null) {
                            provider = library.Provider;
                        }

                        LibraryUpdateData data = CDNService.CheckLibrary(provider, libraryName);

                        if (data != null) {
                            string [] mostRecentVersion = data.MostRecentVersion.Split(".");

                            try {
                                int[] mostRecentVersionInt = { Int32.Parse(mostRecentVersion[0]), Int32.Parse(mostRecentVersion[1]), 
                                    Int32.Parse(mostRecentVersion[2]) };

                                if (mostRecentVersionInt[0] > libraryVersionInt[0]) {
                                    LibrariesToUpdate.Add(data);
                                }
                                else if (mostRecentVersionInt[0] > libraryVersionInt[0] 
                                    && mostRecentVersionInt[1] > libraryVersionInt[1]) {
                                    LibrariesToUpdate.Add(data);
                                }
                                else if (mostRecentVersionInt[0] > libraryVersionInt[0] 
                                    && mostRecentVersionInt[1] > libraryVersionInt[1] 
                                    && mostRecentVersionInt[2] > libraryVersionInt[2]) {
                                    LibrariesToUpdate.Add(data);
                                }
                            }
                            catch (Exception err) {
                                File.WriteAllText(App.LogFile, err.ToString());
                            }
                        }
                        else {
                            File.WriteAllText(App.LogFile, string.Format("{0} ({1}) could not be checked.", libraryName, provider));                            
                        }
                    }
                    catch (Exception err) {
                        LibraryUpdateData data = CDNService.CheckLibrary(provider, libraryName);
                        File.WriteAllText(App.LogFile, err.ToString());
                    }

                    counter++;
                    progressBar1.Value = Math.Floor(((float)counter / (float)totalLibraries) * 100);
                }

                if (LibrariesToUpdate.Count > 0) {
                    librariesToUpdate.Text = "";
                }
                else {
                    librariesToUpdate.Text = "No updates were found.";
                }
            }
            else {
                /*AccessText popupText = new AccessText {
                    Text = "No file was loaded."
                };
                errorPopup.Child = popupText;
                errorPopup.IsOpen = true;*/
            }
        }

        /// <summary>
        /// Opens a file save dialog for the libraries with updates results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void librariesToUpdate_DoubleClick(object sender, RoutedEventArgs e)
        {

        }
    }
}
