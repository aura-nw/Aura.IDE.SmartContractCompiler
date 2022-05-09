namespace AuraIDE.Models
{
    public class TransferResponse
    {
        public List<TransferItem> Transfers { get; set; }
    }
    public class TransferItem
    {
        public string Coin { get; set; }
        public string Status { get; set; }
    }
}