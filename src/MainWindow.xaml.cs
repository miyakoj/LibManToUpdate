using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;

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
            // delete the log file if one exists from a previous session
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
        private async void inputFileButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "LibMan JSON|libman.json";
            openFileDialog.Multiselect = false;
            errorBox.Text = "";
            string combinedErrorMsg = string.Empty;

			if (openFileDialog.ShowDialog() == true) {
                inputFileField.Text = openFileDialog.FileName.Replace(@"\\", @"\");
                string json = File.ReadAllText(openFileDialog.FileName);
                LibManConfig libman = null;
                try {
                    libman = JsonConvert.DeserializeObject<LibManConfig>(json);
                }
                catch(JsonSerializationException) {
                    errorBox.Text = "Your libman.json is invalid.";
                    return;
                }
                catch(JsonReaderException) {
                    errorBox.Text = "Your json is invalid.";
                    return;
                }

                int totalLibraries = libman.Libraries.Count;
                int counter = 0;

                // from: https://stackoverflow.com/a/29872887/7843806
                var progress = new Progress<double>(value => progressBar.Value = value);
                await Task.Run(() =>
                {
                    foreach (var library in libman.Libraries)
                    {
                        string provider = libman.DefaultProvider;

                        string[] tempLibraryArray = library.Library.Split("@");
                        string libraryName = tempLibraryArray[0];
                        string tempLibraryVersion = tempLibraryArray[1];
                        string[] libraryVersion = tempLibraryVersion.Split(".");
                        string errorMsg = string.Format("{0} ({1}) could not be checked.", libraryName, provider);

                        try
                        {
                            int[] libraryVersionInt = { Int32.Parse(libraryVersion[0]), Int32.Parse(libraryVersion[1]),
                                Int32.Parse(libraryVersion[2]) };

                            if (library.Provider != null)
                            {
                                provider = library.Provider;
                            }

                            LibraryUpdateData data = CDNService.CheckLibrary(provider, libraryName);

                            if (data != null)
                            {
                                data.CurrentVersion = tempLibraryArray[1];
                                string[] mostRecentVersion = data.MostRecentVersion.Split(".");

                                try
                                {
                                    int[] mostRecentVersionInt = { Int32.Parse(mostRecentVersion[0]), Int32.Parse(mostRecentVersion[1]),
                                        Int32.Parse(mostRecentVersion[2]) };

                                    if (mostRecentVersionInt[0] > libraryVersionInt[0])
                                    {
                                        LibrariesToUpdate.Add(data);
                                    }
                                    else if (mostRecentVersionInt[0] == libraryVersionInt[0]
                                        && mostRecentVersionInt[1] > libraryVersionInt[1])
                                    {
                                        LibrariesToUpdate.Add(data);
                                    }
                                    else if (mostRecentVersionInt[0] == libraryVersionInt[0]
                                        && mostRecentVersionInt[1] == libraryVersionInt[1]
                                        && mostRecentVersionInt[2] > libraryVersionInt[2])
                                    {
                                        LibrariesToUpdate.Add(data);
                                    }
                                }
                                catch (Exception err)
                                {
                                    File.WriteAllText(App.LogFile, err.ToString());
                                    combinedErrorMsg += errorMsg + Environment.NewLine;
                                }
                            }
                            else
                            {
                                File.WriteAllText(App.LogFile, errorMsg);
                                combinedErrorMsg += errorMsg + Environment.NewLine;
                            }
                        }
                        catch (Exception err)
                        {
                            File.WriteAllText(App.LogFile, err.ToString());
                            combinedErrorMsg += errorMsg + Environment.NewLine;
                        }

                        counter++;
                        ((IProgress<double>)progress).Report(Math.Ceiling(((double)counter / (double)totalLibraries) * 100));
                    }
                });

                if (string.IsNullOrEmpty(combinedErrorMsg)) {
                    errorBox.Text += combinedErrorMsg;
                }

                // reset the progress bar value since the work is finished
                progressBar.Value = 0;

                if (LibrariesToUpdate.Count > 0) {
                    librariesWithUpdates.Text = LibrariesToUpdate.Count + " client libraries with updates:" 
                        + Environment.NewLine + Environment.NewLine;

                    foreach (var library in LibrariesToUpdate) {
                        librariesWithUpdates.Text += library.Library + " (" + library.CurrentVersion + " -> " 
                            + library.MostRecentVersion + ")" + Environment.NewLine;
                    }
                }
                else {
                    librariesWithUpdates.Text = "No client library updates were found.";
                }
            }
            else {
                errorBox.Text = "libman.json could not be loaded.";
            }
        }

        /// <summary>
        /// Opens a file save dialog for the libraries with updates results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void librariesWithUpdates_DoubleClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.FileName = "LibManUpdates";
            saveFileDialog.DefaultExt = ".txt";

            if (saveFileDialog.ShowDialog() == true) {
                string path = saveFileDialog.FileName;
                File.WriteAllText(path, librariesWithUpdates.Text);
            }
        }
    }
}
