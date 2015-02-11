using Microsoft.WindowsAzure.Mobile.Service;

namespace sharepointpromagfgw1Service.DataObjects
{
    public class BookieTwoPointOh : EntityData
    {
        public string Text { get; set; }
        public bool Complete { get; set; }
        public string FullName { get; set; }
        public bool RepayLoan { get; set; }
        public string TwitterHandle { get; set; }
    }
}