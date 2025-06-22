using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MySqlConnector;

namespace net_pj
{
    public sealed partial class RegisterPage : Page
    {
        public RegisterPage()
        {
            this.InitializeComponent();
        }

        private async void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = NewUsernameTextBox.Text.Trim();
            string password = NewPasswordBox.Password;
            string email = EmailTextBox.Text.Trim();

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

            if (string.IsNullOrEmpty(email) || !IsValidEmail(email))
            {
                MessageTextBlock.Text = "Vui lòng nhập địa chỉ email hợp lệ.";
                return;
            }

            string hashedPassword = HashPassword(username, password);

            try
            {
                using (MySqlConnection conn = new MySqlConnection(Config.ConnStr))
                {
                    await conn.OpenAsync();

                    string checkQuery = "SELECT COUNT(*) FROM users WHERE username = @username";
                    using (MySqlCommand checkCmd = new MySqlCommand(checkQuery, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@username", username);
                        int count = Convert.ToInt32(await checkCmd.ExecuteScalarAsync());

                        if (count > 0)
                        {
                            MessageTextBlock.Text = "Tên đăng nhập đã tồn tại, vui lòng chọn tên khác.";
                            return;
                        }
                    }

                    string insertQuery = "INSERT INTO users (username, password, email) VALUES (@username, @password, @email)";
                    using (MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn))
                    {
                        insertCmd.Parameters.AddWithValue("@username", username);
                        insertCmd.Parameters.AddWithValue("@password", hashedPassword);
                        insertCmd.Parameters.AddWithValue("@email", email);

                        await insertCmd.ExecuteNonQueryAsync();
                    }
                }

                MessageTextBlock.Text = "Đăng ký thành công!";
                Frame.Navigate(typeof(LoginPage));
            }
            catch (Exception ex)
            {
                MessageTextBlock.Text = "Lỗi: " + ex.Message;
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(LoginPage));
        }

        private bool IsPasswordValid(string password)
        {
            return password.Length >= 8 && Regex.IsMatch(password, @"\d");
        }

        private bool IsValidEmail(string email)
        {
            return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$");
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
