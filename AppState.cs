using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZXing.Aztec.Internal;

namespace net_pj
{
    public static class AppState
    {
        public static PlayerInfo CurrentPlayer { get; set; }
    }
    public static class FormatTime
    {
        public static string FormatTokenTime(int seconds)
        {
            var time = TimeSpan.FromSeconds(seconds);
            return $"{(int)time.TotalHours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
        }
    }

    public class PlayerInfo
    {
        public string Username { get; set; }
        public int Token { get; set; }
        public string Email { get; set; }
    }
    public static class UpdateUserAsync {
        public static async Task SyncUserTimeTokenAsync()
        {
            if (AppState.CurrentPlayer == null)
                throw new InvalidOperationException("No user is currently logged in.");

            using (MySqlConnection conn = new MySqlConnection(Config.ConnStr))
            {
                await conn.OpenAsync();
                string query = "UPDATE users SET TimeToken = @token WHERE username = @username";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@token", AppState.CurrentPlayer.Token);
                    cmd.Parameters.AddWithValue("@username", AppState.CurrentPlayer.Username);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }
        public static async Task UpdateUserEmailAsync(string newEmail)
        {
            if (AppState.CurrentPlayer == null)
                throw new InvalidOperationException("No user is currently logged in.");

            using (MySqlConnection conn = new MySqlConnection(Config.ConnStr))
            {
                await conn.OpenAsync();
                string query = "UPDATE users SET Email = @Email WHERE Username = @Username";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", newEmail);
                    cmd.Parameters.AddWithValue("@Username", AppState.CurrentPlayer.Username);

                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
