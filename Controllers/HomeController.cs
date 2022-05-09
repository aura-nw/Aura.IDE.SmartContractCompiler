using Aura.IDE.RustCompiler.Services;
using AuraIDE.Models;
using AuraIDE.Services;
using AuraIDE.ViewModels.Home;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Diagnostics;

namespace AuraIDE.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRustService _rustService;
        private readonly IBlockchainService _blockchainService;

        public HomeController(ILogger<HomeController> logger, IRustService rustService, IBlockchainService blockchainService)
        {
            _logger = logger;
            _rustService = rustService;
            _blockchainService = blockchainService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [Route("/compile")]
        public async Task<IActionResult> Compile([FromBody] List<CompileFileRequest> model)
        {
            //if (!HttpContext.Session.Keys.Contains("SessionId"))
            //{
            //    HttpContext.Session.SetString("SessionId", HttpContext.Session.Id);
            //}
            //return Ok(_rustService.CompileFile(model, HttpContext.Session.GetString("SessionId")));
            return Ok(_rustService.CompileFile(model, HttpContext.Session.Id));
        }

        [HttpPost]
        [Route("/deposit")]
        public async Task<IActionResult> Deposit([FromBody] TransferRequest request)
        {
            return Ok(await _blockchainService.Deposit(request.Address, request.Coin));
        }
    }
}
