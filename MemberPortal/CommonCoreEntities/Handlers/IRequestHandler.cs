using System.Threading.Tasks;

namespace StagwellTech.SEIU.CommonCoreEntities.Handlers
{
    public interface IRequestHandler<TRequest, TResponse>
    {
        Task<TResponse> HandleAsync(TRequest request);
        Task<TResponse> HandleAsync();
        TResponse Handle(TRequest request);
        TResponse Handle();
    }
}