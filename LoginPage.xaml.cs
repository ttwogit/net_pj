using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace net_pj
{
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;
            string hashedPassword = HashPassword(username, password);
            if (string.IsNullOrEmpty(username))
            {
                await new MessageDialog("Vui lòng nhập tên đăng nhập.").ShowAsync();
                return;
            }

            if (!IsPasswordValid(password))
            {
                await new MessageDialog("Mật khẩu phải có ít nhất 8 ký tự và chứa ít nhất 1 số.").ShowAsync();
                return;
            }
            using (MySqlConnection conn = new MySqlConnection(Config.ConnStr))
            {
                try
                {
                    await conn.OpenAsync();

                    string query = "SELECT COUNT(*) FROM users WHERE username = @username AND password = @password";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);

                        object result = await cmd.ExecuteScalarAsync();
                        int count = Convert.ToInt32(result);

                        if (count > 0)
                        {
                            // Gán thông tin người chơi toàn cục
                            AppState.CurrentPlayer = new PlayerInfo
                            {
                                Username = username,
                                Token = 0,
                                OrderedFoods = new List<string>()
                            };
                            await new MessageDialog("Đăng nhập thành công!").ShowAsync();
                            Frame.Navigate(typeof(MainPage), username);
                        }
                        else
                        {
                            await new MessageDialog("Sai tên đăng nhập hoặc mật khẩu.").ShowAsync();
                        }
                    }
                }
                catch (Exception ex)
                {
                    await new MessageDialog("Lỗi: " + ex.Message).ShowAsync();
                }
            }
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(RegisterPage));
        }
        private bool IsPasswordValid(string password)
        {
            return password.Length >= 8 && System.Text.RegularExpressions.Regex.IsMatch(password, @"\d");
        }


        private string HashPassword(string username, string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                string combined = username + password;
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(combined));
                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(MainPage));
        }
    }
}
