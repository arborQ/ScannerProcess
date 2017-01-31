using RepositoryServices.Interfaces;

namespace RepositoryServices.Models
{
    public class Machine : IMachine
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string EngineCodeA { get; set; }
        public string EngineCodeB { get; set; }
        public int? EnginePositionA { get; set; }
        public int? EnginePositionB { get; set; }
        public string ProgramType { get; set; }
        public string ImageA { get; set; }
        public string ImageB { get; set; }
        public string ImageC { get; set; }
        public string Comment { get; set; }
    }
}