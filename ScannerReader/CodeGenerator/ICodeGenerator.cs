namespace CodeGenerator
{
    public interface ICodeGenerator
    {
        void GenerateToFile(string message, string filePath);
    }
}