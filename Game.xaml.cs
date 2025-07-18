using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;


namespace net_pj
{
    public sealed partial class Game : UserControl
    {
        private DispatcherTimer _tokenTimer;
        private int _syncCounter = 0;
        private ContentDialog _timeDialog;
        private TextBlock _dialogContent;
        private string _Name;
        private object _appid;
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
                control._Name = data.name;
                control._appid = data.appid;
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
            if (AppState.CurrentPlayer.Token <= 0)
            {
                await ShowTokenExpiredDialog();
                return;
            }
            StartTokenCountdown();
        }
        private async void StartTokenCountdown()
        {

            if (_tokenTimer == null)
            {
                _tokenTimer = new DispatcherTimer
                {
                    Interval = TimeSpan.FromSeconds(1)
                };

                _tokenTimer.Tick += async (s, e) =>
                {
                    if (AppState.CurrentPlayer.Token > 0)
                    {
                        AppState.CurrentPlayer.Token--;
                        _syncCounter++;

                        if (_dialogContent != null)
                        {
                            _dialogContent.Text = $"Thời gian còn lại: {FormatTime.FormatTokenTime(AppState.CurrentPlayer.Token)}";
                        }

                        if (_syncCounter >= 30)
                        {
                            _syncCounter = 0;
                            await UpdateUserAsync.SyncUserTimeTokenAsync();
                        }
                    }
                    else
                    {
                        _tokenTimer.Stop();
                        await UpdateUserAsync.SyncUserTimeTokenAsync();
                        if (_timeDialog != null)
                        {
                            _timeDialog.Hide();
                            _timeDialog = null;
                        }
                        await ShowTokenExpiredDialog();
                    }
                };
            }

            if (!_tokenTimer.IsEnabled)
            {
                _tokenTimer.Start();
                OpenUrl($"https://store.steampowered.com/app/{ _appid}");
                _dialogContent = new TextBlock
                {
                    Text = $"Thời gian còn lại: {FormatTime.FormatTokenTime(AppState.CurrentPlayer.Token)}",
                    FontSize = 18,
                    Margin = new Thickness(0, 10, 0, 0)
                };

                _timeDialog = new ContentDialog
                {
                    Title = $"Bạn đang chơi: {_Name}",
                    Content = _dialogContent,
                    CloseButtonText = "Đóng",
                    CornerRadius = new CornerRadius(5),
                    XamlRoot = this.Content.XamlRoot
                };
                _timeDialog.Closed += async (s, args) =>
                {
                    _tokenTimer.Stop();
                    await UpdateUserAsync.SyncUserTimeTokenAsync();
                };
                await _timeDialog.ShowAsync();
            }
        }

        private async Task ShowTokenExpiredDialog()
        {
            var dialog = new ContentDialog
            {
                Title = "Thông báo",
                Content = "Bạn đã sử dụng hết thời gian, vui lòng nạp thêm!",
                CloseButtonText = "OK",
                CornerRadius = new CornerRadius(5),
                XamlRoot = this.Content.XamlRoot
            };
            await dialog.ShowAsync();
        }
        private async void OpenUrl(string Url)
        {
            //thay thế cho hàm khởi động game :(((
            var uri = new Uri(Url);
            await Launcher.LaunchUriAsync(uri);
        }
    }
}
