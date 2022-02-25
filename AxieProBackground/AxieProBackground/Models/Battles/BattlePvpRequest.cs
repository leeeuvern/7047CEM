namespace AxieProBackground.Models.Battles
{

    public class BattlePvpRequest
    {
      public List<Battles> battles { get; set; }
    }
    public class Battles
    {
        public string battle_uuid { get; set; }
        public DateTime game_started { get; set; }
        public DateTime game_ended { get; set; }
        public string winner { get; set; }
        public string first_client_id { get; set; }
        public string first_team_id { get; set; }
        public List<int> first_team_fighters { get; set; }
        public string second_client_id { get; set; }
        public string second_team_id { get; set; }
        public List<int> second_team_fighters { get; set; }
        public List<Eloanditem> eloAndItem { get; set; }
        public string pvp_type { get; set; }
    }
    public class Eloanditem
    {
        public string player_id { get; set; }
        public int new_elo { get; set; }
        public int old_elo { get; set; }
        public string result_type { get; set; }
    }

}
