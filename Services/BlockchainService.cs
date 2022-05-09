using AuraIDE.Models;
using AuraIDE.Services;
using Microsoft.Extensions.Options;

namespace Aura.IDE.RustCompiler.Services
{
    public class BlockchainService : IBlockchainService
    {
        private readonly IHttpService _httpService;
        private readonly APIConfig _apiConfig;
        private readonly BlockchainAPIConfig _blockchainAPIConfig;
        private readonly ILogger<BlockchainService> _logger;

        public BlockchainService(IHttpService httpService, IOptions<APIConfig> apiConfig, ILogger<BlockchainService> logger, IOptions<BlockchainAPIConfig> blockchainAPIConfig)
        {
            _httpService = httpService;
            _apiConfig = apiConfig.Value;
            _logger = logger;
            _blockchainAPIConfig = blockchainAPIConfig.Value;
        }

        public async Task<string> Deposit(string auraWalletAddress, string coin)
        {
            try
            {
                var balance = await _httpService.GetAsync<GetBalanceResponse>($"{_apiConfig.BlockchainAPI}/{_blockchainAPIConfig.GetBalanceApi}/{auraWalletAddress}");
                if (balance != null && (string.IsNullOrEmpty(balance.Balances.First().Amount) || balance.Balances.First().Amount == "0"))
                {
                    var response = await _httpService.PostAsync<Object, TransferResponse>(_apiConfig.TokenFaucet, new { Address = auraWalletAddress, Coins = new List<string> { coin } });
                    balance = await _httpService.GetAsync<GetBalanceResponse>($"{_apiConfig.BlockchainAPI}/{_blockchainAPIConfig.GetBalanceApi}/{auraWalletAddress}");
                }
                return balance.Balances.First().Amount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return "";
            }
        }
    }
}
