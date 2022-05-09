namespace AuraIDE.ViewModels.Home
{
    public class CompileFileRequest
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public List<string> Childs { get; set; }
        public string Content { get; set; }
        public string Path { get; set; }
        public CompileFileRequest()
        {
            Childs = new List<string>();
        }
    }
}
