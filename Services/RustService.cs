using Aura.IDE.RustCompiler.Core.Constants;
using Aura.IDE.RustCompiler.Services;
using Aura.IDE.RustCompiler.Utils;
using AuraIDE.Models;
using AuraIDE.ViewModels.Home;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace AuraIDE.Services
{
    public class RustService : IRustService
    {
        private readonly AuraConfig _auraConfig;
        private readonly ILogger<RustService> _logger;

        public RustService(IOptions<AuraConfig> auraConfig, ILogger<RustService> logger)
        {
            _auraConfig = auraConfig.Value;
            _logger = logger;
        }

        public RustBuildResultModel CompileFile(List<CompileFileRequest> model, string sessionId)
        {
            var smartContractDirectory = $"{Directory.GetCurrentDirectory()}/SmartContracts/{sessionId}";
            try
            {
                var rootDir = model[0].Name;
                CreateFileFromTree(model, sessionId);

                List<string> compileCommands = new List<string>
                {
                    "cargo wasm",
                    $"export RPC=\"{_auraConfig.RPC}\"",
                    $"export CHAIN_ID={_auraConfig.ChainId}",
                    $"export NODE={_auraConfig.Node}",
                    $"export TXFLAG={_auraConfig.TxFlag}"
                };

                List<string> compileMessages = TerminalUtil.RunScripts(compileCommands, $"{smartContractDirectory}/{rootDir}");
                bool success = true;
                foreach (var message in compileMessages)
                {
                    if (message.Contains(CompileConstant.COMPILE_ERROR_MESSAGE, StringComparison.OrdinalIgnoreCase))
                    {
                        success = false;
                    }
                }

                List<string> schemas = new List<string>();
                DirectoryInfo schemaDir = new DirectoryInfo($"{smartContractDirectory}/{rootDir}/{CompileConstant.SCHEMA_PATH}");
                if (schemaDir.Exists)
                {
                    foreach (var schemaFile in schemaDir.GetFiles())
                    {
                        if (schemaFile.FullName.EndsWith(".json"))
                        {
                            schemas.Add(Convert.ToBase64String(System.IO.File.ReadAllBytes(schemaFile.FullName)));
                        }
                    }
                }

                string wasmFilePath = string.Empty;
                DirectoryInfo directoryInfo = new DirectoryInfo($"{smartContractDirectory}/{rootDir}/{CompileConstant.RELEASE_PATH}");
                foreach (var file in directoryInfo.GetFiles())
                {
                    if (file.FullName.EndsWith(".wasm"))
                    {
                        wasmFilePath = file.FullName;
                    }
                }

                var wasmBase64 = Convert.ToBase64String(System.IO.File.ReadAllBytes(wasmFilePath));

                var response = new RustBuildResultModel { Message = compileMessages, Success = success, Wasm = wasmBase64, Schemas = schemas };
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return new RustBuildResultModel { Success = false };
            }
            finally
            {
                Directory.Delete(smartContractDirectory, true);
            }
        }

        private void CreateFileFromTree(List<CompileFileRequest> files, string sessionId)
        {
            if (files == null || files.Count == 0)
            {
                return;
            }
            var currentDir = $"{Directory.GetCurrentDirectory()}/SmartContracts/{sessionId}/{files.First().Name}";

            foreach (var file in files.Skip(1))
            {
                if (file.Type.ToLower() == "file")
                {
                    FileInfo fileInfo = new FileInfo(currentDir + "/" + file.Path);
                    fileInfo.Directory.Create();
                    System.IO.File.WriteAllBytes(fileInfo.FullName, Convert.FromBase64String(file.Content));
                }
                else
                {
                    Directory.CreateDirectory($"{currentDir}/{file.Path}");
                }
            }
        }
    }
}
