namespace AuraIDE.Models
{
    public class RustBuildResultModel
    {
        public bool Success { get; set; }
        public List<string> Message { get; set; }
        public string Wasm { get; set; }
        public List<string> Schemas { get; set; }
    }
}
