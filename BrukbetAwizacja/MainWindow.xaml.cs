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

namespace BrukbetAwizacja
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FileManager manager = new FileManager("D://");
            manager.Changed += ((sender, e) =>
            {
                MessageBox.Show("Event occured: " + e.ChangeType);
            });
            manager.Renamed += ((sender, e) =>
            {
                MessageBox.Show("Event occuredr: " + e.ChangeType);
            });
        }
    }
}
