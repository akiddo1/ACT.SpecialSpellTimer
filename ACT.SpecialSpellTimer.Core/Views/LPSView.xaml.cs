using System;
using System.Windows;
using System.Windows.Threading;
using ACT.SpecialSpellTimer.Config;
using FFXIV.Framework.WPF.Views;
using Prism.Mvvm;

namespace ACT.SpecialSpellTimer.Views
{
    /// <summary>
    /// LPSView.xaml の相互作用ロジック
    /// </summary>
    public partial class LPSView :
        Window,
        IOverlay
    {
        private static LPSView instance;

        public static void ShowLPS()
        {
            instance = new LPSView();

            instance.OverlayVisible = Settings.Default.LPSViewVisible;
            instance.ClickTransparent = Settings.Default.ClickThroughEnabled;

            instance.Show();
        }

        public static void CloseLPS()
        {
            if (instance != null)
            {
                instance.Close();
                instance = null;
            }
        }

        private DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Background);

        public LPSView()
        {
            this.InitializeComponent();

            this.DataContext = new LPSViewModel();

            this.ToNonActive();
            this.ToNotTransparent();

            this.MouseLeftButtonDown += (x, y) => this.DragMove();

            this.Loaded += this.LPSView_Loaded;
            this.Closed += this.LPSView_Closed;
#if !DEBUG
            this.LPSTextBlock.Text = string.Empty;
#endif
        }

        public LPSViewModel ViewModel => this.DataContext as LPSViewModel;

        private bool overlayVisible;
        private bool clickTranceparent;

        public bool OverlayVisible
        {
            get => this.overlayVisible;
            set => this.SetOverlayVisible(ref this.overlayVisible, value, Settings.Default.Opacity);
        }

        public bool ClickTransparent
        {
            get => this.clickTranceparent;
            set
            {
                if (this.clickTranceparent != value)
                {
                    this.clickTranceparent = value;

                    if (this.clickTranceparent)
                    {
                        this.ToTransparent();
                    }
                    else
                    {
                        this.ToNotTransparent();
                    }
                }
            }
        }

        private void LPSView_Loaded(object sender, RoutedEventArgs e)
        {
            this.timer.Interval = TimeSpan.FromSeconds(1.1);
            this.timer.Tick += this.Timer_Tick;
            this.timer.Start();
        }

        private void LPSView_Closed(object sender, EventArgs e)
        {
            this.timer.Stop();
            this.timer = null;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            var lps = PluginMainWorker.Instance?.LogBuffer?.LPS ?? 0;
            this.LPSTextBlock.Text = lps.ToString("N0");

            // 表示を切り替える
            this.OverlayVisible = Settings.Default.LPSViewVisible;

            // ついでにクリック透過を切り替える
            this.ClickTransparent = Settings.Default.ClickThroughEnabled;
        }
    }

    public class LPSViewModel :
        BindableBase
    {
        public Settings Config => Settings.Default;
    }
}
