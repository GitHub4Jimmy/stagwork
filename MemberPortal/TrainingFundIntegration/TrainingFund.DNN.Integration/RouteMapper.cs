using DotNetNuke.Web.Api;

namespace TrainingFund.DNN.Integration
{
    public class RouteMapper : IServiceRouteMapper
    {
        public void RegisterRoutes(IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute("TrainingFund", "default", "{controller}/{action}", new[] { "TrainingFund.DNN.Integration.Components" });
        }

    }
}
