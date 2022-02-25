namespace AxieProBackground.Models.Battles
{

    public class Data
    {
        public Axies axies { get; set; }
    }

    public class Axies
    {
        public int total { get; set; }
        public List<Result> results { get; set; }
        public string __typename { get; set; }
    }

    public class Result
    {
        public string id { get; set; }
        public string name { get; set; }
        public int stage { get; set; }
        public string Class { get; set; }
        public int breedCount { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public Battleinfo battleInfo { get; set; }
        public Auction auction { get; set; }
        public List<Part> parts { get; set; }
        public string __typename { get; set; }
    }

    public class Battleinfo
    {
        public bool banned { get; set; }
        public string __typename { get; set; }
    }

    public class Auction
    {
        public string currentPrice { get; set; }
        public string currentPriceUSD { get; set; }
        public string __typename { get; set; }
    }

    //public class Part
    //{
    //    public string id { get; set; }
    //    public string name { get; set; }
    //    public string Class { get; set; }
    //    public string type { get; set; }
    //    public object specialGenes { get; set; }
    //    public string __typename { get; set; }
    //}
}
