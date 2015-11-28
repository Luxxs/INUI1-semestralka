using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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
using INUI1.ViewModel;
using INUI1.Model;

namespace INUI1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel MainViewModel { get { return mainViewModel; } }
        private MainViewModel mainViewModel = new MainViewModel();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void CreateMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            MainViewModel.Matrix = new Cell[int.Parse(Dimension1.Text), int.Parse(Dimension2.Text)];
        }
    }


}
