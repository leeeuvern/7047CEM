using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace AxieProBackground.Models.Payment
{

        public class PaymentRequest
        {
            public int total { get; set; }
            public List<PaymentResult> results { get; set; }
        }

        public class PaymentResult
        {
            public string from { get; set; }
            public string to { get; set; }
            public string value { get; set; }
            public string log_index { get; set; }
            public string tx_hash { get; set; }
            public int block_number { get; set; }
          [JsonConverter(typeof(UnixDateTimeConverter))]
            public DateTime timestamp { get; set; }
            public string token_address { get; set; }
            public int token_decimals { get; set; }
            public string token_name { get; set; }
            public string token_symbol { get; set; }
            public string token_type { get; set; }
        }
    public class UnixDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.UnixEpoch.AddSeconds(reader.GetInt64());
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue((value - DateTime.UnixEpoch).TotalMilliseconds + "000");
        }
    }


}
