namespace AuraIDE.Models
{
    public class GetBalanceResponse
    {
        public List<Balance> Balances { get; set; }
    }

    public class Balance
    {
        public string Denom { get; set; }
        public string Amount { get; set; }
    }
}