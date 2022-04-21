using System;
using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.Handlers
{
    public class DependentIdGeneratorHandler : IRequestHandler<int, string>
    {
        private string GenerateFriendlyId()
        {
            var generatedId = new Random().Next(100000, 999999);
            return generatedId.ToString("D6");
        }
        public string Handle()
        {
            return GenerateFriendlyId();
        }
        public string Handle(int request)
        {
            throw new NotImplementedException();
        }
        public Task<string> HandleAsync()
        {
            return Task.FromResult(Handle());
        }
        public Task<string> HandleAsync(int request)
        {
            throw new NotImplementedException();
        }        
    }
}
