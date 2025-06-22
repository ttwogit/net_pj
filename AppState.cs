using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
