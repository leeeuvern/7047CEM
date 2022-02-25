namespace AxieProBackground.Models.Axies
{
    public class BattleHistory
    {
        public int Id { get; set; }
        public string BattleUid { get; set; }
        public string WinnerUserUid { get; set; }
        public string LoserUserUid { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public string WinnerTeamHash { get; set; }
        public string LoserTeamHash { get; set; }
        public bool Draw { get; set; }
        public bool? LeaderboardGame { get; set; }
    }
}
