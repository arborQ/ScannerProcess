namespace RepositoryServices.Interfaces
{
    public interface IMachine : IBaseElement
    {
        string Code { get; }

        string EngineCodeA { get; }

        string EngineCodeB { get; }

        int? EnginePositionA { get; }

        int? EnginePositionB { get; }

        string ProgramType { get; }

        string ImageA { get; }

        string ImageB { get; }

        string ImageC { get; }

        string Comment { get; }
    }
}