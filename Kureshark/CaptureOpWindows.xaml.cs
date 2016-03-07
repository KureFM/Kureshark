using System.Windows;
using System.Windows.Controls;

namespace Kureshark
{
    /// <summary>
    /// CaptureOpWindows.xaml 的交互逻辑
    /// </summary>
    public partial class CaptureOpWindows : Window
    {
        public CaptureOpWindows()
        {
            InitializeComponent();
        }

        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void deviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (deviceList.SelectedIndex >= 0)
            {
                startButton.IsEnabled = true;
            }
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.Visibility = Visibility.Hidden;
            e.Cancel = true;
        }
    }
}
