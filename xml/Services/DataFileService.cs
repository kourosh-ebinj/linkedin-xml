using Json.Path;
using LinkedIn_XML.Helpers;

namespace LinkedIn_XML.Services
{
    public interface IDataFileService
    {
        string FilePath { get; }
        string GetData();
        TResult? GetPrimitiveValueByPath<TResult>(string jsonPath) where TResult : struct;
        IEnumerable<TResult?> GetItemsByPath<TResult>(string jsonPath);

    }

    public class DataFileService : IDataFileService
    {
        public virtual string FilePath => "Data\\Data.xml";
        public virtual string GetData()
        {
            return FileHelper.LoadJsonFile(FilePath);
        }

        public TResult? GetPrimitiveValueByPath<TResult>(string jsonPath) where TResult : struct
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(jsonPath, nameof(jsonPath));

            return JsonHelper.GetPrimitiveValueByPath<TResult>(GetData(), jsonPath);
        }

        public IEnumerable<TResult?> GetItemsByPath<TResult>(string jsonPath)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(jsonPath, nameof(jsonPath));

            return JsonHelper.GetItemsByPath<TResult>(GetData(), jsonPath);
        }

    }
}
