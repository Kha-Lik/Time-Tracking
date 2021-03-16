namespace TimeTracking.Common.Abstract
{
    public interface IWriteableTemplateStorageService
    {
        bool TryUpSertTemplate(string templateType, string path);
    }
}