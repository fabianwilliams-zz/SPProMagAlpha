using System;
using Newtonsoft.Json;

namespace sharepointpromagfgw1
{
    public class BookItem
    {
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }
        [JsonProperty(PropertyName = "complete")]
        public bool Complete { get; set; }
        [JsonProperty(PropertyName = "fullname")]
        public string FullName { get; set; }
        [JsonProperty(PropertyName = "repayloan")]
        public bool RepayLoan { get; set; }
        [JsonProperty(PropertyName = "twitterhandle")]
        public string TwitterHandle { get; set; }
    }
}
