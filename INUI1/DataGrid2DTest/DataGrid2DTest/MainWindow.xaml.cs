using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Windows.Themes;
using System.Collections;
using System.Collections.ObjectModel;
using DataGrid2DLibrary;

namespace DataGrid2DTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Declaration

        private int[][] m_int2DJaggedArray = null;
        private List<List<int>> m_int2DList = null;

        #endregion //Declaration

        #region Constructor

        public MainWindow()
        {
            InitializeArrays();
            InitializeComponent();            
            dataGrid2D.DataContext = this;
            noModifiedStyleDataGrid2D.DataContext = this;
        }

        #endregion

        #region Private Methods

        private void InitializeArrays()
        {
            DoubleList = new List<double>();
            Int2DList = new List<List<bool>>();
            ShortCollection = new ObservableCollection<short>();
            String2DCollection = new ObservableCollection<ObservableCollection<string>>();
            Float2DArray = new float[5, 10];
            ByteArray = new byte[8];
            Int2DJaggedArray = new int[5][];
            m_int2DJaggedArray = new int[5][];
            m_int2DList = new List<List<int>>();
            for (int i = 0; i < 5; i++)
            {
                DoubleList.Add(i * 1.11);
                Int2DList.Add(new List<bool>());
                ShortCollection.Add((short)i);
                String2DCollection.Add(new ObservableCollection<string>());
                Int2DJaggedArray[i] = new int[10];
                m_int2DJaggedArray[i] = new int[10];
                m_int2DList.Add(new List<int>());
                for (int j = 0; j < 10; j++)
                {
                    Int2DList[i].Add(i % 2 == 0);
                    String2DCollection[i].Add((i * 10 + j).ToString());
                    Int2DJaggedArray[i][j] = (i * 10 + j);
                    Float2DArray[i, j] = (i * 10 + j * 0.11f);
                    m_int2DJaggedArray[i][j] = (i * 10 + j);
                    if (j < 3)
                    {
                        m_int2DList[i].Add(i * 10 + j);
                    }
                    else if (j < 8)
                    {
                        ByteArray[j] = (byte)((new Random(1)).Next() % 256);
                    }
                }
            }
        }

        #endregion

        #region Properties

        public List<double> DoubleList { get; set; }
        public List<List<bool>> Int2DList { get; set; }
        public ObservableCollection<short> ShortCollection { get; set; }
        public ObservableCollection<ObservableCollection<string>> String2DCollection { get; set; }
        public byte[] ByteArray { get; set; }
        public int[][] Int2DJaggedArray { get; set; }
        public float[,] Float2DArray { get; set; }

        #endregion //Properties

        #region EventsHandlers

        private void propertiesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                ListBoxItem listBoxItem = e.AddedItems[0] as ListBoxItem;
                Binding datagrid2dBinding = new Binding();
                datagrid2dBinding.Path = new PropertyPath(listBoxItem.Content.ToString());
                dataGrid2D.SetBinding(DataGrid2D.ItemsSource2DProperty, datagrid2dBinding);
                noModifiedStyleDataGrid2D.SetBinding(DataGrid2D.ItemsSource2DProperty, datagrid2dBinding);
                memberVariablesListBox.SelectedIndex = -1;
            }
        }

        private void memberVariablesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count == 1)
            {
                ListBoxItem listBoxItem = e.AddedItems[0] as ListBoxItem;
                if (listBoxItem.Content.ToString() == "m_intJagged2DArray")
                {
                    dataGrid2D.ItemsSource2D = m_int2DJaggedArray;
                    noModifiedStyleDataGrid2D.ItemsSource2D = m_int2DJaggedArray;
                }
                else if (listBoxItem.Content.ToString() == "m_int2DList")
                {
                    dataGrid2D.ItemsSource2D = m_int2DList;
                    noModifiedStyleDataGrid2D.ItemsSource2D = m_int2DList;
                }
                propertiesListBox.SelectedIndex = -1;
            }
        }

        #endregion //EventsHandlers
    }
}
