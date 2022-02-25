using static AxiePro.Models.Battles.MmrRequest;

namespace AxiePro.Models.Battles
{

    public class ProxyMmr
    {
        public bool success { get; set; }
        public long cache_last_updated { get; set; }
        public int draw_total { get; set; }
        public int lose_total { get; set; }
        public int win_total { get; set; }
        public int total_matches { get; set; }
        public int win_rate { get; set; }
        public int mmr { get; set; }
        public int rank { get; set; }
        public int ronin_slp { get; set; }
        public int total_slp { get; set; }
        public int raw_total { get; set; }
        public int in_game_slp { get; set; }
        public int last_claim { get; set; }
        public int lifetime_slp { get; set; }
        public string name { get; set; }
        public int next_claim { get; set; }
    }

    public class LeaderBoardRequest
    {

        public string _etag { get; set; }
        public List<MmrPlayer> _items { get; set; }
    }
    public class MmrRequest
    {


        public bool success { get; set; }
        public List<MmrPlayer> items { get; set; }
        public int offset { get; set; }
        public int limit { get; set; }
        public long update_time { get; set; }


        public class MmrPlayer
        {
            public string clientID { get; set; }

            public int elo { get; set; }
            public int rank { get; set; }
            public string name { get; set; }
        }
    }
}
