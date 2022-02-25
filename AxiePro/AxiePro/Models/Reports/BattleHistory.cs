namespace AxiePro.Models.Reports
{
    public class BattleHistoryReport
    {
        public string BattleUid { get; set; }
        public string UserUid { get; set; }
        public string OpponentUid { get; set; }
        public string OpponentBuildHash { get; set; }
        public string OpponentClassHash { get; set; }
        public string BuildHash { get; set; }
        public string Result { get; set; }
        public DateTime BattleDateTime { get; set; }
        public int FirstOpponentFighter { get; set; }
        public int SecondOpponentFighter { get; set; }
        public int ThirdOpponentFighter { get; set; }
        public int FirstUserFighter { get; set; }
        public int SecondUserFighter { get; set; }
        public int ThirdUserFighter { get; set; }
        public string TimeAgo { get; set; }
        public bool Draw { get; set; }
        public bool? LeaderboardGame { get; set; }
    }
}
