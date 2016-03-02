using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using BrukbetAwizacja;

namespace BrukbetAwizacja
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string filename;

        public MainWindow()
        {
            InitializeComponent();
            LoadUserSettings();
            FileManager manager = new FileManager("D://");
            manager.Changed += ((sender, e) =>
            {
                FileSystemWatcher watcher = sender as FileSystemWatcher;
                watcher.EnableRaisingEvents = false;
                MessageBox.Show("Event occured: " + e.ChangeType);
                watcher.EnableRaisingEvents = true;
            });
            manager.Renamed += ((sender, e) =>
            {
                MessageBox.Show("Event occuredr: " + e.ChangeType);
            });
        }
       
        private void LoadUserSettings()
        {
            txbIP.Text = Properties.Settings.Default.IpAddress;
            lblPath.Content = Properties.Settings.Default.FilePath;
        }

        private void SaveUserSettings()
        {
            Properties.Settings.Default.IpAddress = txbIP.Text;
            Properties.Settings.Default.FilePath = lblPath.Content.ToString();
            Properties.Settings.Default.Save();
        }

        private NotificationType ValidateCheckBoxes()
        {
            if (checkboxGreen.IsChecked == true && checkboxRed.IsChecked == false)
                return NotificationType.GreenNotification;
            else if (checkboxGreen.IsChecked == false && checkboxRed.IsChecked == true)
                return NotificationType.RedNotification;
            else
                return NotificationType.None;
        }

        private bool IsFileLoaded()
        {
            return lblPath.Content != null ? true : false;
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            fileDialog.DefaultExt = ".txt";
            fileDialog.Filter = "Pliki Tekstowe (*.txt)|*.txt";

            bool? result = fileDialog.ShowDialog();

            if(result == true)
            {
                filename = fileDialog.FileName;
                lblPath.Content = filename;
            }
        }     

        private void checkboxGreen_Checked(object sender, RoutedEventArgs e)
        {
            checkboxGreen.IsChecked = true;
            checkboxRed.IsChecked = false;
        }

        private void checkboxRed_Checked(object sender, RoutedEventArgs e)
        {
            checkboxRed.IsChecked = true;
            checkboxGreen.IsChecked = false;
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (Validation.IsIPValid(txbIP.Text))
            {
                if (IsFileLoaded())
                {
                    NotificationType notificationType = ValidateCheckBoxes();
                    if (notificationType != NotificationType.None)
                    {
                        try
                        {
                            TextReader textReader = new TextReader(lblPath.Content.ToString());
                            textReader.ReadText();
                            TextParser parser = new TextParser(textReader);
                            byte[] message = parser.CreateMessage(notificationType);
                            EthernetCommunication ethernet = new EthernetCommunication(txbIP.Text, 9000);
                            string response = ethernet.SendMessage(message);
                            LogFile logFile = new LogFile(".\\logs.txt");
                            logFile.AddLogMessage(response);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Wystąpił błąd: " + ex.Message);
                        }
                    }
                    else
                        MessageBox.Show("Nie zaznaczono statusu awizacji!");
                }
                else
                    MessageBox.Show("Nie wybrano żadnego pliku!");
                
            }
            else
                MessageBox.Show("Błędny format adresu IP!");
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveUserSettings();
        }
    }
}
