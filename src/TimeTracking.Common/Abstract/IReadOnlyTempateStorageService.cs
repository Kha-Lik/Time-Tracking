namespace TimeTracking.Common.Abstract
{
    public interface IReadOnlyTemplateStorageService
    {
        string GetPathByKey(string templateKey);
    }
}