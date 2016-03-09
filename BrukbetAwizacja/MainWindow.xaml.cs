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
        private byte[] actualHash;
        private FileManager fileManager;
        System.Windows.Threading.DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            LoadUserSettings();
            if (lblPath.Content.ToString() != "Nie wybrano żadnego pliku" && lblPath.Content.ToString() != "")
                actualHash = FileManager.CalcualteMD5(lblPath.Content.ToString());
            //InitializeFileWatcher(System.IO.Path.GetDirectoryName(Properties.Settings.Default.FilePath));
            InitializeTimer();
        }

        private void InitializeTimer()
        {
            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += Timer_Tick;
            timer.Interval = new TimeSpan(0, 0, 8);
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.FilePath != "" && Properties.Settings.Default.FilePath != "Nie wybrano żadnego pliku")
            {
                byte[] hash = FileManager.CalcualteMD5(lblPath.Content.ToString());
                if (!FileManager.AreMD5Equal(actualHash, hash))
                {
                    listBox.Items.Clear();
                    Send(NotificationType.Both);
                    actualHash = hash;
                }
            }
        }

        private void InitializeFileWatcher(string path)
        {
            fileManager = new FileManager(path);
            fileManager.Changed += ((sender, e) =>
            {
                FileSystemWatcher watcher = sender as FileSystemWatcher;
                watcher.EnableRaisingEvents = false;

                Dispatcher.Invoke(() =>
                {
                    if(ValidateAllInput())
                        Send(NotificationType.Both);
                });

                watcher.EnableRaisingEvents = true;
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

        private NotificationType GetCheckBoxesValue()
        {
            if (checkboxGreen.IsChecked == true && checkboxRed.IsChecked == false)
                return NotificationType.GreenNotification;
            else if (checkboxGreen.IsChecked == false && checkboxRed.IsChecked == true)
                return NotificationType.RedNotification;
            else if (checkboxGreen.IsChecked == true && checkboxRed.IsChecked == true)
                return NotificationType.Both;
            else
                return NotificationType.None;
        }

        private bool AreCheckBoxesChecked()
        {
            if (checkboxGreen.IsChecked == false && checkboxRed.IsChecked == false)
                return false;
            return true;
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
                actualHash = FileManager.CalcualteMD5(lblPath.Content.ToString());
                Properties.Settings.Default.FilePath = lblPath.Content.ToString();
                //InitializeFileWatcher(System.IO.Path.GetDirectoryName(filename));
            }
            
        }           

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateAllInput())
            {
                listBox.Items.Clear();
                NotificationType notificationType = GetCheckBoxesValue();
                Send(notificationType);
            }
                
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            SaveUserSettings();
        }

        private void SaveAndPrintLogs(string message, NotificationType notificationType)
        {
            try
            {
                LogFile logFile = new LogFile(".\\logs.txt");
                string log;
                log = logFile.AddLogMessage(message, notificationType);
                listBox.Items.Add(log);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message);
            }
            
        }

        private void SendMessage(NotificationType notificationType, TextParser parser)
        {
            try
            {
                byte[] message = parser.CreateMessage(notificationType);
                EthernetCommunication ethernet = new EthernetCommunication(txbIP.Text, 23);
                string response = ethernet.SendMessage(message);
                SaveAndPrintLogs(response, notificationType);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message);
            }
            
        }

        private bool ValidateAllInput()
        {
            if (!Validation.IsIPValid(txbIP.Text))
            {
                MessageBox.Show("Błędny format adresu IP");
                return false;
            }
            if(!IsFileLoaded())
            {
                MessageBox.Show("Nie wybrano żadnego pliku!");
                return false;
            }
            if(!AreCheckBoxesChecked())
            {
                MessageBox.Show("Nie zaznaczono statusu awizacji!");
                return false;
            }
            return true;
        }

        private void Send(NotificationType notificationType)
        {
            try
            {
                TextReader textReader = new TextReader(lblPath.Content.ToString());
                textReader.ReadText();
                TextParser parser = new TextParser(textReader);
                if (notificationType == NotificationType.Both)
                {
                    SendMessage(NotificationType.GreenNotification, parser);
                    SendMessage(NotificationType.RedNotification, parser);
                }
                else
                    SendMessage(notificationType, parser);
            }

            catch (Exception ex)
            {
                MessageBox.Show("Wystąpił błąd: " + ex.Message);
            }
        }
    }
}
