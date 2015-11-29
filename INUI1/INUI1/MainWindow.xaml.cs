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
                (FindByName("cellGrid", box) as System.Windows.Controls.Primitives.UniformGrid).Columns = cols;
                MainViewModel.State.Clear();
                for (int i = 0; i < rows * cols; i++)
                {
                    MainViewModel.State.Add(new Cell(0, false));
                }
                CellNumberContentConverter.MinDimension = (rows < cols) ? rows : cols;
            }
            else
            {
                MessageBox.Show("Zadejte celočíselné rozměry větší než jedna.", "Hlavolam");
            }
        }
        private FrameworkElement FindByName(string name, FrameworkElement root)
        {
            Stack<FrameworkElement> tree = new Stack<FrameworkElement>();
            tree.Push(root);

            while (tree.Count > 0)
            {
                FrameworkElement current = tree.Pop();
                if (current.Name == name)
                    return current;

                int count = VisualTreeHelper.GetChildrenCount(current);
                for (int i = 0; i < count; ++i)
                {
                    DependencyObject child = VisualTreeHelper.GetChild(current, i);
                    if (child is FrameworkElement)
                        tree.Push((FrameworkElement)child);
                }
            }

            return null;
        }

        private void searchSolutionButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }


}
