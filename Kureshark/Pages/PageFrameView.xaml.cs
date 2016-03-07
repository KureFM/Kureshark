using Kureshark.Model;
using System.Windows.Controls;

namespace Kureshark.Pages
{
    /// <summary>
    /// PageFrameView.xaml 的交互逻辑
    /// </summary>
    public partial class PageFrameView : Page
    {
        public PageFrameView()
        {
            InitializeComponent();
            
        }

        private void frameDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (frameDataGrid.SelectedItem != null)
            {
                decodedFrameTreeView.ItemsSource = ((DecodedFrame)frameDataGrid.SelectedItem).TreeDisplay;
            }
        }
    }
}
