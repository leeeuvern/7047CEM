namespace AxieProBackground.Models.Axies
{
    public class Axie
    {

        public int Id { get; set; }
        public int AxieId { get; set; }
        public string Eyes { get; set; }
        public string Ears { get; set; }
        public string Back { get; set; }
        public string Mouth { get; set; }
        public string Horn { get; set; }
        public string Tail { get; set; }

        public string Hash { get; set; }
        public string Class { get; set; }
    }
    public class HashCombination
    {

        public string ClassHash { get; set; }
        public string BuildHash { get; set; }
    }
}
