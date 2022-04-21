using StagwellTech.SEIU.CommonCoreEntities.Handlers;
using StagwellTech.SEIU.CommonCoreEntities.Handlers.Requests;
using StagwellTech.SEIU.CommonCoreEntities.Services;
using StagwellTech.SEIU.CommonEntities.ThirdPartyIntegrations.AzureServices;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace DocumentAPI.Handlers
{
    public class DownloadEnrollmentDocumentHandler : IRequestHandler<DownloadEnrollmentDocumentRequest, byte[]>
    {
        readonly ICloudStorageFileService fileService;

        public DownloadEnrollmentDocumentHandler(ICloudStorageFileService fileService)
        {
            this.fileService = fileService;
        }

        public byte[] Handle(DownloadEnrollmentDocumentRequest request)
        {
            throw new NotImplementedException();
        }

        public byte[] Handle()
        {
            throw new NotImplementedException();
        }

        public async Task<byte[]> HandleAsync(DownloadEnrollmentDocumentRequest request)
        {
            return await fileService.DownloadBlob(this.fileService.DependentEnrollmentBlobContainer, HttpUtility.UrlDecode(request.FileName));
        }

        public Task<byte[]> HandleAsync()
        {
            throw new NotImplementedException();
        }
    }

    
}
