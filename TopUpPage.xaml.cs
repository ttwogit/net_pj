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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace net_pj
{
    public sealed partial class TopUpPage : Page
    {
        private string username;

        public TopUpPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            username = e.Parameter?.ToString();
            // Bạn có thể dùng username để hiển thị hoặc truy vấn dữ liệu
            

        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(TokenAmountTextBox.Text, out int amount) && amount > 0)
            {
                // 👉 Thêm đoạn kiểm tra null ở đây:
                if (AppState.CurrentPlayer != null)
                {
                    AppState.CurrentPlayer.Token += amount;
                    MessageTextBlock.Text = $"Đã nạp {amount} token!";
                }
                else
                {
                    MessageTextBlock.Text = "Không có người dùng đang đăng nhập.";
                }
            }
            else
            {
                MessageTextBlock.Text = "Vui lòng nhập số token hợp lệ.";
            }
        }
        

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
