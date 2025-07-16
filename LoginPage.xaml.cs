using System;
using System.Security.Cryptography;
using System.Text;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MySqlConnector;

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

            MessageTextBlock.Text = ""; // Clear thông báo cũ


            if (string.IsNullOrEmpty(username))
            {
                MessageTextBlock.Text = "Vui lòng nhập tên đăng nhập.";
                return;
            }

            if (!IsPasswordValid(password))
            {
                MessageTextBlock.Text = "Mật khẩu phải có ít nhất 8 ký tự và chứa ít nhất 1 số.";
                return;
            }
            using (MySqlConnection conn = new MySqlConnection(Config.ConnStr))
            {
                try
                {
                    await conn.OpenAsync();

                    // Lấy token trực tiếp nếu username & password hợp lệ
                    string query = "SELECT TimeToken FROM users WHERE username = @username AND password = @password LIMIT 1";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@username", username);
                        cmd.Parameters.AddWithValue("@password", hashedPassword);

                        object result = await cmd.ExecuteScalarAsync();

                        if (result != null && result != DBNull.Value)
                        {
                            int token = Convert.ToInt32(result);

                            // Gán thông tin vào AppState
                            AppState.CurrentPlayer = new PlayerInfo
                            {
                                Username = username,
                                Token = token
                            };

                            MessageTextBlock.Text = "Đăng nhập thành công!";
                            Window.Current.Content = new NavigationView();
                        }
                        else
                        {
                            MessageTextBlock.Text = "Sai tên đăng nhập hoặc mật khẩu.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageTextBlock.Text = "Lỗi: " + ex.Message;
                }
            }
        }

            private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            Window.Current.Content = new RegisterPage();
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
    }
}
