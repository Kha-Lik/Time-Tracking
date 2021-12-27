using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Concurrent;
using System.IO.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Console;
using TimeTracking.Common.Abstract;
using TimeTracking.Common.Enums;
 
namespace TimeTracking.Common.Services
{
    public class TemplateStorageService : IWriteableTemplateStorageService, IReadOnlyTemplateStorageService
    {
        private readonly IFileSystem _fileSystem;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly ILogger _logger;

        public TemplateStorageService(IFileSystem fileSystem, IWebHostEnvironment webHostEnvironment)
        {
            _fileSystem = fileSystem;
            _webHostEnvironment = webHostEnvironment;
            ILoggerFactory loggerFactory = LoggerFactory.Create(c => c.AddDebug().AddConsole());
            _logger = loggerFactory.CreateLogger<TemplateStorageService>();
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
                _logger.LogInformation($"Current dir: {_fileSystem.Directory.GetCurrentDirectory()}"+
                                       $"Current dir parent: {_fileSystem.Directory.GetParent(_fileSystem.Directory.GetCurrentDirectory())}"+
                                       $"Full dir path: {dir}");
                dir = _fileSystem.Path.Combine(_fileSystem.Directory.GetParent(_fileSystem.Directory.GetCurrentDirectory()).
                    Parent.ToString(), "TimeTracking.Templates");
            }
            catch (NullReferenceException nre)
            {
                _logger.LogError(nre,$"Trying to fix directory issue according to docker context");
                dir = "/app";
                _logger.LogInformation($"Full dir path: {dir}");
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
                _logger.LogWarning($"{templateKey} not exists in TemplateTypePathMap");
                return String.Empty;
            }
            return TemplateTypePathMap[templateKey];
        }
 
        private bool CheckPathIsValid(string path)
        {
            if (!_fileSystem.File.Exists(path))
            {
                _logger.LogWarning($"File with path {path} not exists");
                return false;
            }
            _logger.LogInformation($"File with path {path} is valid");
            return true;
        }
    }
}
