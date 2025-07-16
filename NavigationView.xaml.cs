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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace net_pj
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class NavigationView : Page
    {

        public NavigationView()
        {
            this.InitializeComponent();
            ContentFrame.Navigate(typeof(UserInfoPage));
            if (!string.IsNullOrEmpty(AppState.CurrentPlayer.Username))
            {
                WelcomeText.Text = $"Chào {AppState.CurrentPlayer.Username}!";
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
                ContentFrame.Navigate(typeof(TopUpPage));
            }
            else if (selectedItem == AudioPage)
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
