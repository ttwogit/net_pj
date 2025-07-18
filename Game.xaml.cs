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
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace net_pj
{
    public sealed partial class Game : UserControl
    {
        
        public Game()
        {
            this.InitializeComponent();
        }
        public GameElementData Data
        {
            get => (GameElementData)GetValue(DataProperty);
            set => SetValue(DataProperty, value);
        }

        public static readonly DependencyProperty DataProperty =
            DependencyProperty.Register(
                nameof(Data),
                typeof(GameElementData),
                typeof(Game),
                new PropertyMetadata(null, OnDataChanged));
        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as Game;
            var data = e.NewValue as GameElementData;
            if (data != null)
            {

                control.Name.Text = data.name;
                control.Description.Text = data.description;
               
                string filename = Path.GetFileName($"{data.appid}.jpg");
                string localPath = $"ms-appx:///Assets/Appimg/{filename}";
                //System.Diagnostics.Debug.WriteLine(localPath);
                var bitmap = new BitmapImage(new Uri(localPath));
                bitmap.ImageFailed += (s, args) =>
                {
                    control.ProductImage.Source = new BitmapImage(new Uri("ms-appx:///Assets/Square150x150Logo.scale-200.png"));
                };

                control.ProductImage.Source = bitmap;
            }
        }

        private async void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (AppState.CurrentPlayer.Token <= 0) {
                var panel = new StackPanel { HorizontalAlignment = Windows.UI.Xaml.HorizontalAlignment.Center };
                    panel.Children.Add(new TextBlock { Text = $"Bạn đã sử dụng hết thời gian, vui lòng nạp thêm", Margin = new Thickness(0, 10, 0, 0) });
                var dialog = new ContentDialog
                {
                    Title = "Thông báo: ",
                    Content = panel,
                    CloseButtonText = "OK",
                    CornerRadius = new CornerRadius(5),
                    XamlRoot = this.Content.XamlRoot
                };
                await dialog.ShowAsync();
                return; }
        }
    }
}
