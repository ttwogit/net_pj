using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Gaming.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;


namespace net_pj
{
    public sealed partial class NavigationView : Page
    {
        private DispatcherTimer _timer;

        public NavigationView()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(UserInfoPage));
            if (!string.IsNullOrEmpty(AppState.CurrentPlayer.Username))
            {
                WelcomeText.Text = $"Chào {AppState.CurrentPlayer.Username}!";
            }

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += UpdateTokenTime;
            _timer.Start();
        }
        private void UpdateTokenTime(object sender, object e)
        {
            if (AppState.CurrentPlayer != null)
            {
                TimeTokenText.Text = $"Thời gian còn lại: {FormatTime.FormatTokenTime(AppState.CurrentPlayer.Token)}";
            }
        }
        private void NavView_ItemInvoked(Windows.UI.Xaml.Controls.NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {

            var selectedItem = NavView.SelectedItem;
    
            if (selectedItem == AppsPage)
            {
                ContentFrame.Navigate(typeof(MainPage));
            }
            else if (selectedItem == GamePage)
            {
                ContentFrame.Navigate(typeof(GameList));
            }
            else if (selectedItem == OderPage)
            {
                ContentFrame.Navigate(typeof(OrderPage));
            }
            if (args.IsSettingsInvoked)
            {
                ContentFrame.Navigate(typeof(UserInfoPage));
            }
        }


        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("Navigation failed: " + e.SourcePageType.FullName);
        }

    }
}
