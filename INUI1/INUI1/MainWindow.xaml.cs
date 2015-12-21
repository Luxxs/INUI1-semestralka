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
using INUI1.DesignTime;
using INUI1.Converters;
using INUI1.Logic;

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

        private void createMatrixButton_Click(object sender, RoutedEventArgs e)
        {
            int rows = 0;
            int cols = 0;
            if (int.TryParse(Dimension1.Text, out rows) && int.TryParse(Dimension2.Text, out cols) && rows > 1 && cols > 1)
            {
                MainViewModel.Columns = cols;
                MainViewModel.State.Clear();
                for (int i = 0; i < rows * cols; i++)
                {
                    MainViewModel.State.Add(new Cell(0, false));
                }
                CellNumberContentConverter.MinDimension = (rows < cols) ? rows : cols;
            }
            else
            {
                MessageBox.Show("Zadejte celočíselné rozměry větší než jedna.", Title);
            }
        }

        private void searchSolutionButton_Click(object sender, RoutedEventArgs e)
        {
            int[,] inputMatrix = new int[MainViewModel.State.Count / MainViewModel.Columns, MainViewModel.Columns];
            int col = 0;
            int row = 0;
            foreach(var cell in MainViewModel.State)
            {
                inputMatrix[row, col] = cell.Value;
                col = (col < MainViewModel.Columns - 1) ? col + 1 : 0;
                row = (col == 0) ? row + 1 : row;
            }
            var pathSearch = new HledaniCesty(inputMatrix);
            try {
                bool[,] result = pathSearch.Vypocti();
                col = 0;
                row = 0;
                foreach (var cell in MainViewModel.State)
                {
                    cell.InPath = result[row, col];
                    col = (col < MainViewModel.Columns - 1) ? col + 1 : 0;
                    row = (col == 0) ? row + 1 : row;
                }
            } catch(ArgumentException ex)
            {
                MessageBox.Show(ex.Message, Title);
            }
        }
    }
}
