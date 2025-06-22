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
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FoodOrderPage : Page
    {
        private string username;
        public FoodOrderPage()
        {
            this.InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            username = e.Parameter?.ToString();
            // Bạn có thể dùng username để hiển thị hoặc truy vấn dữ liệu
        }

        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.CurrentPlayer == null)
            {
                OrderMessage.Text = "❌ Không có người dùng nào đang đăng nhập.";
                return;
            }

            if (FoodComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string food = selectedItem.Content.ToString();
                AppState.CurrentPlayer.OrderedFoods.Add(food);
                OrderMessage.Text = $"✅ Đã gọi món: {food}";
            }
            else
            {
                OrderMessage.Text = "❗ Vui lòng chọn món ăn trước khi gọi.";
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
