using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using VietQRHelper;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using ZXing.Common;
using ZXing;
using MySqlConnector;
using System.Threading.Tasks;


namespace net_pj
{
    public sealed partial class Item : UserControl
    {
        public string _Price;
        public string _OderID;
        public string _Type;
        public int _Value;
        public Item()
        {
            this.InitializeComponent();
        }

        public UIElementData Data
        {
            get => (UIElementData)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(
                nameof(Data),
                typeof(UIElementData),
                typeof(Item),
                new PropertyMetadata(null, OnDataChanged));

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Item;
            var data = e.NewValue as UIElementData;
            if (data != null)
            {

                control.NameText.Text = data.content;
                control.PriceText.Text = $"Giá Bán: {data.Price} VND";
                control._Price = data.Price.ToString();
                control._OderID = data.OderID.ToString();
                control._Type = data.type.ToString();
                control._Value = data.Value;

                string filename = Path.GetFileName(data.ImageUrl);
                string localPath = $"ms-appx:///Assets/{filename}";

                var bitmap = new BitmapImage(new Uri(localPath));
                bitmap.ImageFailed += (s, args) =>
                {
                    control.ProductImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.scale-200.png"));
                };

                control.ProductImage.Source = bitmap;
            }
        }

        private async void OrderButton_Click(object sender, RoutedEventArgs e)
        {
            var accountNumber = Config.accountNumber;
            var momoQR = QRPay.InitVietQR(
              bankBin: BankApp.BanksObject[BankKey.BANVIET].bin,
              bankNumber: accountNumber,
              amount: _Price,
              purpose: _OderID
            );

            // Trong mã QR của MoMo có chứa thêm 1 mã tham chiếu tương ứng với STK
            momoQR.additionalData.reference = "MOMOW2W" + accountNumber.Substring(10);

            // Mã QR của MoMo có thêm 1 trường ID 80 với giá trị là 3 số cuối của SỐ ĐIỆN THOẠI của tài khoản nhận tiền
            momoQR.SetUnreservedField("80", "570");



            string qrData = momoQR.Build(); // Chuỗi VietQR chuẩn EMV

            // Tạo mã QR bằng ZXing
            var writer = new BarcodeWriterPixelData
            {
                Format = BarcodeFormat.QR_CODE,
                Options = new EncodingOptions
                {
                    Height = 300,
                    Width = 300,
                    Margin = 0
                }
            };

            var pixelData = writer.Write(qrData);

            // Chuyển sang BitmapImage
            var bitmap = new WriteableBitmap(300, 300);
            using (var stream = bitmap.PixelBuffer.AsStream())
            {
                stream.Write(pixelData.Pixels, 0, pixelData.Pixels.Length);
            }

            // Hiển thị trong ContentDialog
            var image = new Image
            {
                Source = bitmap,
                Width = 200,
                Height = 200,
                Stretch = Windows.UI.Xaml.Media.Stretch.Uniform
            };

            var panel = new StackPanel { HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center };
            panel.Children.Add(image);
            panel.Children.Add(new TextBlock { Text = $"Bạn đã đặt món: {NameText.Text}", Margin = new Thickness(0, 10, 0, 0) });

            var dialog = new ContentDialog
            {
                Title = "QR Code Thanh Toán",
                Content = panel,
                CloseButtonText = "OK",
                XamlRoot = this.Content.XamlRoot
            };
            dialog.Closed += (s, args) =>
            {
                CloseCalling();
            };
            await dialog.ShowAsync();


        }
        private async void CloseCalling() {
            if (_Type != "GameTime") return;
            AppState.CurrentPlayer.Token += _Value;
            await UpdateUserTokenAsync.SyncUserTimeTokenAsync();
        }
    }
}
    