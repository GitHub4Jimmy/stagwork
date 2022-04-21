using Microsoft.Extensions.Logging;
using StagwellTech.SEIU.CommonCoreEntities.Handlers;
using StagwellTech.SEIU.CommonCoreEntities.Handlers.Requests;
using StagwellTech.SEIU.CommonCoreEntities.Services;
using StagwellTech.SEIU.CommonEntities.DataModels.DBO;
using StagwellTech.SEIU.CommonEntities.DataModels.Enums;
using StagwellTech.SEIU.CommonEntities.DataModels.Portal.Document;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentAPI.Handlers
{
    public class UploadEnrollmentDocumentHandler : IRequestHandler<UploadEnrollmentDocumentHandlerRequest, PortalEnrollmentFormDocumentResponse>
    {
        private readonly ICloudStorageFileService _fileService;

        public UploadEnrollmentDocumentHandler(ICloudStorageFileService fileService)
        {
            _fileService = fileService;
        }

        public PortalEnrollmentFormDocumentResponse Handle(UploadEnrollmentDocumentHandlerRequest request)
        {
            if (String.IsNullOrEmpty(request.Request.Extension))
            {
                var parts = request.FileName.Split('.');
                request.Request.Extension = parts[^1];
            }

            if (request.Request.DocumentId == Guid.Empty)
            {
                request.Request.DocumentId = Guid.NewGuid();
            }
            var container = _fileService.DependentEnrollmentBlobContainer;
            var filePath = $"" +
                $"{request.Request.PersonId}" +
                $"/{request.Request.FriendlyId}" +
                $"/{request.Request.DocumentType}" +
                $"-{request.Request.PersonId}" +
                $"-{request.Request.DocumentId}" +
                $".{request.Request.Extension}";
            var location = container + "/" + filePath;

            try
            {
                _fileService.UploadBlob(container, filePath, request.FileStream);
                return new PortalEnrollmentFormDocumentResponse()
                {
                    FriendlyId = request.Request.FriendlyId,
                    DocumentType = request.Request.DocumentType,
                    DocumentId = request.Request.DocumentId,
                    Location = location
                };
            } 
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;
        }

        public PortalEnrollmentFormDocumentResponse Handle()
        {
            throw new NotImplementedException();
        }

        public Task<PortalEnrollmentFormDocumentResponse> HandleAsync(UploadEnrollmentDocumentHandlerRequest request)
        {
            return Task.FromResult(Handle(request));
        }

        public Task<PortalEnrollmentFormDocumentResponse> HandleAsync()
        {
            throw new NotImplementedException();
        }
    }
}
