using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Concurrent;
using System.IO.Abstractions;
using TimeTracking.Common.Abstract;
using TimeTracking.Common.Enums;
 
namespace TimeTracking.Common.Services
{
    public class TemplateStorageService : IWriteableTemplateStorageService, IReadOnlyTemplateStorageService
    {
        private readonly IFileSystem _fileSystem;
        private readonly IWebHostEnvironment _webHostEnvironment;
 
        public TemplateStorageService(IFileSystem fileSystem, IWebHostEnvironment webHostEnvironment)
        {
            _fileSystem = fileSystem;
            _webHostEnvironment = webHostEnvironment;
        }
 
        private ConcurrentDictionary<string, string> templateTypePathMap;
 
        private ConcurrentDictionary<string, string> TemplateTypePathMap
        {
            get
            {
                if (templateTypePathMap is null)
                {
                    templateTypePathMap = new ConcurrentDictionary<string, string>();
                    InitDictionary();
                }
                return templateTypePathMap;
            }
        }
 
        private void InitDictionary()
        {
            TryUpSertTemplate(EmailPurpose.EmailConfirmation.ToString(),
                GetFile("Views/Emails/ConfirmEmail.cshtml"));
            TryUpSertTemplate(EmailPurpose.ResetPassword.ToString(),
                GetFile("Views/Emails/ResetPassword.cshtml"));
            TryUpSertTemplate(ReportType.ActivitiesReport.ToString(),
                GetFile("ReportsTemplates/time-log.xlsx"));
        }
 
        string GetFile(string path)
        {
            string dir = "";
            try
            {
                dir = _fileSystem.Path.Combine(_fileSystem.Directory.GetParent(_fileSystem.Directory.GetCurrentDirectory()).Name, "src/src/TimeTracking.Templates");
            }
            catch (NullReferenceException nre)
            {
                dir = _fileSystem.Path.Combine(_fileSystem.Directory.GetParent(_fileSystem.Directory.GetCurrentDirectory()).Parent.ToString(), "TimeTracking.Templates");
            }
            return _fileSystem.Path.Combine(dir, path.Replace('/', _fileSystem.Path.DirectorySeparatorChar));
        }
        public bool TryUpSertTemplate(string templateType, string path)
        {
            if (!CheckPathIsValid(path)) return false;
            return templateTypePathMap.TryAdd(templateType, path);
        }
 
        public string GetPathByKey(string templateKey)
        {
            if (!TemplateTypePathMap.ContainsKey(templateKey))
            {
                return String.Empty;
            }
            return TemplateTypePathMap[templateKey];
        }
 
        private bool CheckPathIsValid(string path)
        {
            if (!_fileSystem.File.Exists(path))
            {
                return false;
            }
            return true;
        }
 
    }
}
