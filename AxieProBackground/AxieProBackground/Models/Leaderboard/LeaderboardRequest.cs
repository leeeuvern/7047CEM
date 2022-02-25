using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AxieProBackground.Models.Leaderboard
{
    public class LeaderboardRequestProxy
    {


        public bool success { get; set; }
        public List<LeaderboardPlayer> items { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }


        public class LeaderboardPlayer
        {
            public string client_id { get; set; }
            public int win_total { get; set; }
            public int draw_total { get; set; }
            public int lose_total { get; set; }
            public int elo { get; set; }
            public int rank { get; set; }
            public string name { get; set; }
        }

    }
}
