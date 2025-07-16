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

    public class PlayerInfo
    {
        public string Username { get; set; }
        public int Token { get; set; }
        public List<string> OrderedFoods { get; set; } = new List<string>();
    }
    public static class UpdateUserTokenAsync {
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

    }
}
