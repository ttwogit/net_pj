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

namespace net_pj
{

    public sealed partial class MainPage : Page
    {
        private string currentUsername;


        public MainPage()
        {
            this.InitializeComponent();
            Name.Text = $"Tên người dùng: {AppState.CurrentPlayer.Username}";
            Email.Text = $"Email khiếu nại: {AppState.CurrentPlayer.Email}";
        }
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string type = (ComplaintTypeBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            string detail = DetailBox.Text.Trim();

            string username = AppState.CurrentPlayer.Username;
            string email = AppState.CurrentPlayer.Email;

            
            if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(detail))
            {
                var dialog = new ContentDialog
                {
                    Title = "Thiếu thông tin",
                    Content = "Vui lòng chọn loại khiếu nại và nhập chi tiết.",
                    CloseButtonText = "OK",
                    CornerRadius = new CornerRadius(5),
                    XamlRoot = this.XamlRoot
                };
                _ = dialog.ShowAsync();
                return;
            }

            // Xử lý gửi khiếu nại (giả lập)
            string complaintInfo = $"Người chơi: {username}\nEmail: {email}\nLoại: {type}\nChi tiết: {detail}";

            var successDialog = new ContentDialog
            {
                Title = "Gửi thành công",
                Content = $"Khiếu nại của bạn đã được ghi nhận.\n\n{complaintInfo}",
                CloseButtonText = "OK",
                CornerRadius = new CornerRadius(5),
                XamlRoot = this.XamlRoot
            };
            _ = successDialog.ShowAsync();

            // Reset nội dung
            ComplaintTypeBox.SelectedIndex = -1;
            DetailBox.Text = string.Empty;
        }

        private void ResetButton_Click(object sender, RoutedEventArgs e)
        {
            ComplaintTypeBox.SelectedIndex = -1;
            DetailBox.Text = string.Empty;
        }
    }
}
