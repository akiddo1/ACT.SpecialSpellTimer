using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using ACT.SpecialSpellTimer.resources;
using FFXIV.Framework.Globalization;

namespace ACT.SpecialSpellTimer.Config.Views
{
    /// <summary>
    /// TimelineAnalyzerView.xaml の相互作用ロジック
    /// </summary>
    public partial class TimelineAnalyzerView :
        UserControl,
        ILocalizable,
        INotifyPropertyChanged
    {
        public TimelineAnalyzerView()
        {
            this.InitializeComponent();
            this.SetLocale(Settings.Default.UILocale);
        }

        #region ILocalizebale

        public void SetLocale(Locales locale) => this.ReloadLocaleDictionary(locale);

        #endregion ILocalizebale

        #region INotifyPropertyChanged

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(
            [CallerMemberName]string propertyName = null)
        {
            this.PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));
        }

        protected virtual bool SetProperty<T>(
            ref T field,
            T value,
            [CallerMemberName]string propertyName = null)
        {
            if (Equals(field, value))
            {
                return false;
            }

            field = value;
            this.PropertyChanged?.Invoke(
                this,
                new PropertyChangedEventArgs(propertyName));

            return true;
        }

        #endregion INotifyPropertyChanged
    }
}
