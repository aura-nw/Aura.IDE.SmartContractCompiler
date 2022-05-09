namespace Aura.IDE.RustCompiler.Services
{
    public interface IBlockchainService
    {
        public Task<string> Deposit(string auraWalletAddress, string coin);
    }
}
