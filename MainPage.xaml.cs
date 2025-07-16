using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;



// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace net_pj
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string currentUsername;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            currentUsername = e.Parameter as string;

            if (!string.IsNullOrEmpty(currentUsername))
            {
                WelcomeTextBlock.Text = $"Chào {currentUsername}!";
            }
        }

        public MainPage()
        {
            this.InitializeComponent();

        }
        private void TopUpButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(TopUpPage), currentUsername);
        }

        private void FoodButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(OrderPage), currentUsername);
        }

        private void UserInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UserInfoPage), currentUsername);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Xóa thông tin người dùng
            AppState.CurrentPlayer = null;

            // Quay lại trang đăng nhập
            Window.Current.Content = new LoginPage();
            Window.Current.Activate();

        }
    }
}
