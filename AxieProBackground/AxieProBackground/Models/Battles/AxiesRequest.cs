using System.Text.Json;
using System.Text.Json.Serialization;

namespace AxieProBackground.Models.Battles
{
   
        public class AxieRequest
        {
            public string id { get; set; }
            public string name { get; set; }
            public string genes { get; set; }
            public string owner { get; set; }
            public int birthDate { get; set; }
            public string bodyShape { get; set; }
            [JsonPropertyName("class")]
            public string Class { get; set; }
            public int sireId { get; set; }
            public string sireClass { get; set; }
            public int matronId { get; set; }
            public string matronClass { get; set; }
            public int stage { get; set; }
            public string title { get; set; }
            public int breedCount { get; set; }
            public int level { get; set; }
            public Figure figure { get; set; }
            public Part[] parts { get; set; }
            public Stats stats { get; set; }
            public object auction { get; set; }
            public Ownerprofile ownerProfile { get; set; }
            public Child[] children { get; set; }
            public string __typename { get; set; }
        }

        public class Figure
        {
            public string atlas { get; set; }
            public string model { get; set; }
            public string image { get; set; }
            public string __typename { get; set; }
        }

        public class Stats
        {
            public int hp { get; set; }
            public int speed { get; set; }
            public int skill { get; set; }
            public int morale { get; set; }
            public string __typename { get; set; }
        }

        public class Ownerprofile
        {
            public string name { get; set; }
            public string __typename { get; set; }
        }

        public class Part
        {
            public string id { get; set; }
            public string name { get; set; }
            public string _class { get; set; }
            public string type { get; set; }
            public int stage { get; set; }
            public Ability[] abilities { get; set; }
            public string __typename { get; set; }
        }

        public class Ability
        {
            public string id { get; set; }
            public string name { get; set; }
            public int attack { get; set; }
            public int defense { get; set; }
            public int energy { get; set; }
            public string description { get; set; }
            public string backgroundUrl { get; set; }
            public string effectIconUrl { get; set; }
            public string __typename { get; set; }
        }

        public class Child
        {
            public string id { get; set; }
            public string name { get; set; }
            public string _class { get; set; }
            public string image { get; set; }
            public string title { get; set; }
            public int stage { get; set; }
            public string __typename { get; set; }
        }

    }

