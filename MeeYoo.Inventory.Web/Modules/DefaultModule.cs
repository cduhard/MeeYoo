using MeeYoo.Inventory.Web.ViewModels;
using Nancy;

namespace MeeYoo.Inventory.Web.Modules
{
    public class DefaultModule : NancyModule
    {
        public DefaultModule()
        {
            Get["/"] = _ => Negotiate
                                .WithModel(WarehouseListViewModel.Instance)
                                .WithView("home");
        }
    }
}