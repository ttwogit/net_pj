using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using static QRCoder.PayloadGenerator;

namespace net_pj
{
    public sealed partial class UserInfoPage : Page
    {
        public UserInfoPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var player = AppState.CurrentPlayer;
            if (player != null)
            {
                UsernameTextBlock.Text = player.Username;
                TokenTextBlock.Text = FormatTime.FormatTokenTime(player.Token);
                EmailTextBlock.Text = player.Email;
            }
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            // Xóa thông tin người dùng
            AppState.CurrentPlayer = null;

            // Quay lại trang đăng nhập
            Window.Current.Content = new LoginPage();
            Window.Current.Activate();

        }

        private async void EmailChange_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog emailDialog = new ContentDialog
            {
                Title = "Đổi Email",
                PrimaryButtonText = "Xác nhận",
                CloseButtonText = "Hủy",
                CornerRadius = new CornerRadius(5),
                XamlRoot = this.Content.XamlRoot,
                DefaultButton = ContentDialogButton.Primary
            };

            StackPanel panel = new StackPanel();

            TextBlock instruction = new TextBlock
            {
                Text = "Nhập email mới:",
                Margin = new Thickness(0, 0, 0, 10)
            };

            TextBox emailTextBox = new TextBox
            {
                PlaceholderText = "Email mới...",
                Width = 250
            };

            panel.Children.Add(instruction);
            panel.Children.Add(emailTextBox);

            emailDialog.Content = panel;

            var result = await emailDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string newEmail = emailTextBox.Text.Trim();

                if (Regex.IsMatch(newEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$")) 
                {
                    // Gọi hàm đổi email trong DB
                    await UpdateUserAsync.UpdateUserEmailAsync(newEmail);

                    // Cập nhật trong AppState
                    AppState.CurrentPlayer.Email = newEmail;
                    EmailTextBlock.Text = AppState.CurrentPlayer.Email;

                    // Thông báo
                    ContentDialog successDialog = new ContentDialog
                    {
                        Title = "Thành công",
                        Content = $"Email đã được cập nhật thành {newEmail}.",
                        CloseButtonText = "OK",
                        CornerRadius = new CornerRadius(5),
                        XamlRoot = this.Content.XamlRoot
                    };
                    await successDialog.ShowAsync();
                }
                else
                {
                    ContentDialog errorDialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = "Vui lòng nhập email hợp lệ!",
                        CloseButtonText = "OK",
                        CornerRadius = new CornerRadius(5),
                        XamlRoot = this.Content.XamlRoot
                    };
                    await errorDialog.ShowAsync();
                }
            }
        }

    }
}
