using Kureshark.BLL;
using Kureshark.Model;
using Kureshark.Pages;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Kureshark
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        #region 私有字段

        private ObservableCollection<NetworkDevice> netDevictList;
        private ObservableCollection<DecodedFrame> decodedFrameList;
        private PageWelcome welcomePage;
        private PageFrameView frameViewPage;
        private PcapOperate pcapCapture;
        private PcapOperate pcapLoadFile;
        private DeviceListWindow deviceListWindow;
        private CaptureOpWindows captureOpWindows;
        private int selectedDeviceIndex = -1;
        private string filter="";
        private string fileName="";

        #endregion

        public MainWindow()
        {
            InitializeComponent();
            welcomePage = new PageWelcome();
            frameViewPage = new PageFrameView();
            decodedFrameList = new ObservableCollection<DecodedFrame>();
            welcomePage.deviceList.MouseDoubleClick += deviceList_MouseDoubleClick;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            netDevictList = new ObservableCollection<NetworkDevice>(PcapOperate.GetAllDevices());
            welcomePage.deviceList.ItemsSource = netDevictList;
            welcomePage.deviceList.SelectionChanged += deviceList_SelectionChanged;
            welcomePage.startButton.Click += startButton_Click;
            welcomePage.captureOpButton.Click += captureOpButton_Click;
            welcomePage.deviceListButton.Click += deviceListButton_Click;
            frame.Navigate(welcomePage);
            DisabledButton(startButton);
            DisabledButton(stopButton);
            DisabledButton(restartButton);
            DisabledButton(welcomePage.startButton);
            //temp目录存放临时捕获的文件
            if (!Directory.Exists("temp"))
            {
                Directory.CreateDirectory("temp");
            }
        }

        #region welcomePage.deviceList事件绑定函数
        //选择项改变
        void deviceList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (deviceListWindow != null)
            {
                deviceListWindow.deviceList.SelectedIndex = welcomePage.deviceList.SelectedIndex;
            }
            if (captureOpWindows != null)
            {
                captureOpWindows.deviceList.SelectedIndex = welcomePage.deviceList.SelectedIndex;
            }
            if (welcomePage.deviceList.SelectedIndex >= 0)
            {
                if (startButton.IsEnabled == false)
                {
                    EnableButton(startButton);
                    EnableButton(welcomePage.startButton);
                }
                selectedDeviceIndex = welcomePage.deviceList.SelectedIndex;
            }
        }
        //双击
        void deviceList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (welcomePage.deviceList.SelectedItem != null)
            {
                StartCapture();
                frame.Navigate(frameViewPage);
            }
        }

        #endregion

        #region 工具栏按钮事件绑定函数

        #region 打开设备列表
        private void deviceListButton_Click(object sender, RoutedEventArgs e)
        {

            //绑定控件
            if (deviceListWindow != null)
            {
                //start=false&&stop=true
                if ((!startButton.IsEnabled) && stopButton.IsEnabled)
                {
                    deviceListWindow.deviceList.IsEnabled = false;
                }
                deviceListWindow.deviceList.SelectedIndex = selectedDeviceIndex;
                deviceListWindow.stopButton.IsEnabled = stopButton.IsEnabled;
                deviceListWindow.startButton.IsEnabled = startButton.IsEnabled;
                if (deviceListWindow.Visibility == Visibility.Hidden)
                {
                    deviceListWindow.Visibility = Visibility.Visible;
                }


                deviceListWindow.Activate();

                return;
            }
            deviceListWindow = new DeviceListWindow();
            deviceListWindow.deviceList.ItemsSource = netDevictList;
            deviceListWindow.startButton.Click += startButton_Click;
            deviceListWindow.stopButton.Click += stopButton_Click;
            deviceListWindow.optionsButton.Click += captureOpButton_Click;
            deviceListWindow.deviceList.SelectionChanged += (object s, SelectionChangedEventArgs sc) =>
                {
                    welcomePage.deviceList.SelectedIndex = deviceListWindow.deviceList.SelectedIndex;
                    if (!startButton.IsEnabled)
                    {
                        EnableButton(startButton);
                        EnableButton(welcomePage.startButton);
                    }
                    selectedDeviceIndex = welcomePage.deviceList.SelectedIndex;

                };

            if ((!startButton.IsEnabled) && stopButton.IsEnabled)
            {
                deviceListWindow.deviceList.IsEnabled = false;
            }

            deviceListWindow.deviceList.SelectedIndex = selectedDeviceIndex;
            deviceListWindow.stopButton.IsEnabled = stopButton.IsEnabled;
            deviceListWindow.startButton.IsEnabled = startButton.IsEnabled;
            deviceListWindow.Show();

        }

        #endregion

        #region 打开捕获设置
        private void captureOpButton_Click(object sender, RoutedEventArgs e)
        {
            //绑定控件
            if (captureOpWindows != null)
            {
                //start=false&&stop=true
                if ((!startButton.IsEnabled) && stopButton.IsEnabled)
                {
                    captureOpWindows.deviceList.IsEnabled = false;
                }
                captureOpWindows.deviceList.SelectedIndex = selectedDeviceIndex;
                captureOpWindows.startButton.IsEnabled = startButton.IsEnabled;
                if (captureOpWindows.Visibility == Visibility.Hidden)
                {
                    captureOpWindows.Visibility = Visibility.Visible;
                }


                captureOpWindows.Activate();

                return;
            }
            captureOpWindows = new CaptureOpWindows();
            captureOpWindows.deviceList.ItemsSource = netDevictList;
            captureOpWindows.startButton.Click += startButton_Click;
            captureOpWindows.deviceList.SelectionChanged += (object s, SelectionChangedEventArgs sc) =>
            {
                welcomePage.deviceList.SelectedIndex = captureOpWindows.deviceList.SelectedIndex;
                if (!startButton.IsEnabled)
                {
                    EnableButton(startButton);
                    EnableButton(welcomePage.startButton);
                }
                selectedDeviceIndex = welcomePage.deviceList.SelectedIndex;

            };
            captureOpWindows.filterTextBox.TextChanged += filterTextBox_TextChanged;

            //start=false&&stop=true
            if ((!startButton.IsEnabled) && stopButton.IsEnabled)
            {
                captureOpWindows.deviceList.IsEnabled = false;
            }

            captureOpWindows.deviceList.SelectedIndex = selectedDeviceIndex;
            captureOpWindows.startButton.IsEnabled = startButton.IsEnabled;

            captureOpWindows.Show();
        }

        void filterTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (captureOpWindows.filterTextBox.Text != filter)
            {
                filter = captureOpWindows.filterTextBox.Text;
            }
        }

        #endregion

        #region Start/Stop/Restart
        private void startButton_Click(object sender, RoutedEventArgs e)
        {
            StartCapture();
            if ("Kureshark.Pages.PageWelcome" == frame.NavigationService.Content.ToString())
            {
                frame.Navigate(frameViewPage);
            }
        }

        private void stopButton_Click(object sender, RoutedEventArgs e)
        {
            StopCapture();
        }

        private void restartButton_Click(object sender, RoutedEventArgs e)
        {
            StopCapture();
            StartCapture();
        }
        #endregion

        #endregion

        #region 私有封装函数

        #region 开始/停止捕获
        private void StartCapture()
        {
            if (selectedDeviceIndex >= 0)
            {
                decodedFrameList.Clear();
                frameViewPage.decodedFrameTreeView.ItemsSource = null;
                pcapCapture = new PcapOperate(netDevictList[selectedDeviceIndex]);
                pcapCapture.DecodedQueue.HasEnqueue += DecodedQueue_HasEnqueue;
                frameViewPage.frameDataGrid.ItemsSource = decodedFrameList;
                DisabledButton(startButton);
                EnableButton(stopButton);
                EnableButton(restartButton);
                if (deviceListWindow != null)
                {
                    deviceListWindow.startButton.IsEnabled = false;
                    deviceListWindow.stopButton.IsEnabled = true;
                }
                if (captureOpWindows != null)
                {
                    captureOpWindows.startButton.IsEnabled = false;
                }
                fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".pcap";
                pcapCapture.Start(System.IO.Path.Combine("temp", fileName), filter);
                SetTitle(netDevictList[selectedDeviceIndex].Name);
            }
        }

        void DecodedQueue_HasEnqueue(object sender, EventArgs e)
        {
            stopButton.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() =>
            {
                int count = 50;
                while (pcapCapture.DecodedQueue.Count > 0 && count > 0)
                {
                    DecodedFrame dFrame = pcapCapture.DecodedQueue.Dequeue();
                    decodedFrameList.Add(dFrame);
                    count -= 1;
                }
            }));
        }

        private void StopCapture()
        {
            DisabledButton(stopButton);
            DisabledButton(restartButton);
            EnableButton(startButton);
            if (deviceListWindow != null)
            {
                deviceListWindow.startButton.IsEnabled = true;
                deviceListWindow.stopButton.IsEnabled = false;
                deviceListWindow.deviceList.IsEnabled = true;
            }
            if (captureOpWindows != null)
            {
                captureOpWindows.startButton.IsEnabled = true;
                captureOpWindows.deviceList.IsEnabled = true;
            }
            pcapCapture.Stop();
        }
        #endregion

        private void SetTitle(string title)
        {
            this.Title = title + " - Kureshark";
        }

        #region 启用/禁用Button
        private void DisabledButton(Button button)
        {
            button.IsEnabled = false;
            button.Opacity = 0.2;
        }

        private void EnableButton(Button button)
        {
            button.IsEnabled = true;
            button.Opacity = 1;
        }
        #endregion

        #endregion


        private void mainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (pcapCapture != null)
            {
                pcapCapture.Stop();
            }
            Application.Current.Shutdown();
        }

        private void exitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            mainWindow_Closing(sender, new System.ComponentModel.CancelEventArgs());
        }

        private void openMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (pcapCapture != null && !startButton.IsEnabled)
            {
                StopCapture();
            }
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.DefaultExt = "pcap";
            dialog.Filter = LocRes.Language.PcapFilter;
            dialog.Multiselect = false;
            if (dialog.ShowDialog() ?? false)
            {
                fileName = dialog.FileName;          
                pcapLoadFile = new PcapOperate(fileName);
                pcapLoadFile.DecodedQueue.HasEnqueue += LoadDecodedQueue_HasEnqueue;
            }
            decodedFrameList.Clear();
            frameViewPage.frameDataGrid.ItemsSource = decodedFrameList;
            SetTitle(fileName);
            if ("Kureshark.Pages.PageWelcome" == frame.NavigationService.Content.ToString())
            {
                frame.Navigate(frameViewPage);
            }
        }

        void LoadDecodedQueue_HasEnqueue(object sender, EventArgs e)
        {
            this.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                int count = 50;
                while (pcapLoadFile.DecodedQueue.Count > 0 && count > 0)
                {
                    DecodedFrame dFrame = pcapLoadFile.DecodedQueue.Dequeue();
                    decodedFrameList.Add(dFrame);
                    count -= 1;
                }
            }));
        }

        private void saveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExt = "pcap";
                dialog.Title = "保存";
                dialog.Filter = LocRes.Language.PcapFilter;
                if (dialog.ShowDialog() ?? false)
                {
                    if (fileName != dialog.FileName)
                    {
                        System.IO.File.Move(fileName, dialog.FileName);
                        fileName = dialog.FileName;
                        SetTitle(fileName);
                    }
                }
            }
        }

        private void saveAsMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                SaveFileDialog dialog = new SaveFileDialog();
                dialog.DefaultExt = "pcap";
                dialog.Title = "另存为";
                dialog.Filter = LocRes.Language.PcapFilter;
                if (dialog.ShowDialog() ?? false)
                {
                    if (fileName != dialog.FileName)
                    {
                        System.IO.File.Copy(fileName, dialog.FileName);
                        fileName = dialog.FileName;
                        SetTitle(fileName);
                    }
                }
            }
        }
    }
}
