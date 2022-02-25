namespace AxieProBackground.Models.Axies
{
    public class AxieTeam
    {

        public int Id { get; set; }
        public string TeamHash { get; set; }
        public string? BuildHash { get; set; }
        public string? ClassHash { get; set; }
        public string? OwnerUserUid { get; set; }
        public bool Active { get; set; }
        public int FirstFighter { get; set; }
        public int SecondFighter { get; set; }
        public int ThirdFighter { get; set; }
    }
}
