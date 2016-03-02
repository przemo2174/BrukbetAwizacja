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

        private void txbIP_LostKeyboardFocus(object sender, RoutedEventArgs e)
        {
            TextBox txb = sender as TextBox;
            if (!Validation.IsIPValid(txb.Text))
                MessageBox.Show("Wrong ip address");
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
    }
}
