using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace net_pj
{
    public sealed partial class Item : UserControl
    {
        public int OrderID;
        public Item()
        {
            this.InitializeComponent();
        }
        public async void BindData(UIElementData data)
        {
            if (data == null) return;
            NameText.Text = data.content;
            PriceText.Text = $"{data.Price} VND";
            string filename = Path.GetFileName(data.ImageUrl);
            string localPath = $"ms-appx:///Assets/{filename}";
            OrderID = data.OrderID;
            try
            {
                var testFile = await StorageFile.GetFileFromApplicationUriAsync(new Uri(localPath));
                ProductImage.Source = new BitmapImage(new Uri(localPath));
            }
            catch 
            {
                ProductImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.scale-200.png"));
            }
        }
        private void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Thông báo",
                Content = $"Bạn đã đặt món: {NameText.Text}",
                CloseButtonText = "OK"
            };

            _ = dialog.ShowAsync();
        }
    }
}
