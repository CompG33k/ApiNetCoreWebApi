using AspNetCoreWebApi.BusinessLayer.Interfaces;
namespace AspNetCoreWebApi.BusinessLayer
{
    public class Summaries : ISummaries
    {
        private static readonly string[] _summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };
        public int GetLength() {  return _summaries.Length;  } 
            
        public string[] GetSummaries() { return _summaries; }
    }
}
