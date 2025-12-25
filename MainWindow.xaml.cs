using System.Windows;

namespace NewYearGarlandGuide
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            int selectedType = GarlandTypeSelector.SelectedIndex;

            if (selectedType == -1) selectedType = 0;

            new GuideWindow(selectedType).Show();
            Close();
        }

        private void Exit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void GarlandTypeSelector_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void GarlandTypeSelector_SelectionChanged_1(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }
}