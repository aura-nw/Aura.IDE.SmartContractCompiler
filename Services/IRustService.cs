using AuraIDE.Models;
using AuraIDE.ViewModels.Home;

namespace AuraIDE.Services
{
    public interface IRustService
    {
        public RustBuildResultModel CompileFile(List<CompileFileRequest> model, string sessionId);
    }
}
