using Syncfusion.UI.Xaml.Charts;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace StepTracker
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            infocardsGrid.CanUserAddRows = false;
            DataContext = new ApplicationViewModel();

        }

        private void infocardsGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Button_Export.IsEnabled = true;
            MySFChart.Series.Clear();
            DataGrid gd = (DataGrid)sender;
         //   ((MainWindow)Application.Current.MainWindow).Button_Export.IsVisible = true;
            ColumnSeries series = new ColumnSeries();

            series.ItemsSource = ((ViewInfoCard)gd.SelectedItem).Steps;

            series.XBindingPath = "Key";
            series.YBindingPath = "Value";

            MySFChart.Series.Add(series);         
        }

        private void Button_Export_Click(object sender, RoutedEventArgs e)
        {
            DataGrid gd = ((MainWindow)Application.Current.MainWindow).infocardsGrid;
            ApplicationViewModel applicationViewModel = new ApplicationViewModel();
            applicationViewModel.ExportData((ViewInfoCard)gd.SelectedItem);

            MessageBox.Show("User data info was exported successfully!");
        }
    }
}
