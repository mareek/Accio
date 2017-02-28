using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
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
using System.Windows.Shell;

namespace Accio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly DateTime PeriodeSeekedStart = new DateTime(2017, 4, 5);
        private static readonly DateTime PeriodeSeekedEnd = new DateTime(2017, 4, 10);

        private readonly Scrapper _scrapper;
        private bool _isWatching = false;

        public MainWindow()
        {
            InitializeComponent();
            _scrapper = new Scrapper(new WebBrowserPageDownloader(Browser));
        }

        private async void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            DownloadButton.IsEnabled = false;
            _isWatching = true;
            StopButton.IsEnabled = true;
            await CheckAvaliability(TimeSpan.FromMinutes(15));
            DownloadButton.IsEnabled = true;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            _isWatching = false;
            StopButton.IsEnabled = false;
        }

        private async Task CheckAvaliability(TimeSpan delayBetweenCall)
        {
            var perfomrancesAvailableBefore = await GetAvailablePerformances();
            OutputPerformances(perfomrancesAvailableBefore);

            while (_isWatching)
            {
                await Task.Delay(delayBetweenCall);
                var performancesAvailable = await GetAvailablePerformances();
                OutputPerformances(performancesAvailable);
                if (performancesAvailable.Except(perfomrancesAvailableBefore).Any())
                {
                    FlashInTaskBar();
                }

                perfomrancesAvailableBefore = performancesAvailable;
            }
        }

        private void OutputPerformances(Performance[] performancesAvailable)
        {
            Dispatcher.Invoke(() =>
            {
                OutputBox.Text = string.Join("\n", performancesAvailable.Select(p => p.ToListItem()));
                if (performancesAvailable.Any(p => PeriodeSeekedStart <= p.DateAndHour && p.DateAndHour <= PeriodeSeekedEnd))
                {
                    FlashInTaskBar();
                    TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Normal;
                    TaskbarItemInfo.ProgressValue = 1d;
                    OutputBox.Background = Brushes.Chartreuse;
                }
                else
                {
                    TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;
                    OutputBox.Background = Brushes.White;
                }
            });
        }

        private async Task<Performance[]> GetAvailablePerformances()
        {
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.Indeterminate;
            var performances = await _scrapper.DownloadPerformances();
            TaskbarItemInfo.ProgressState = TaskbarItemProgressState.None;

            return performances.Where(p => p.Availability != PerformanceAvailability.Full).ToArray();
        }

        private void OutputBox_GotFocus(object sender, RoutedEventArgs e)
        {
            StopFlashInTaskBar();
        }

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        [StructLayout(LayoutKind.Sequential)]
        public struct FLASHWINFO
        {
            public UInt32 cbSize;
            public IntPtr hwnd;
            public UInt32 dwFlags;
            public UInt32 uCount;
            public UInt32 dwTimeout;
        }

        public const UInt32 FLASHW_ALL = 3;
        public const UInt32 FLASHW_STOP = 0;

        private void FlashInTaskBar() => SetFlashMode(FLASHW_ALL);

        private void StopFlashInTaskBar() => SetFlashMode(FLASHW_STOP);

        private void SetFlashMode(UInt32 flashMode)
        {
            var fInfo = new FLASHWINFO
            {
                cbSize = Convert.ToUInt32(Marshal.SizeOf(default(FLASHWINFO))),
                hwnd = new System.Windows.Interop.WindowInteropHelper(this).Handle,
                dwFlags = flashMode,
                uCount = UInt32.MaxValue,
                dwTimeout = 0
            };

            Dispatcher.Invoke(() => FlashWindowEx(ref fInfo));
        }
    }
}