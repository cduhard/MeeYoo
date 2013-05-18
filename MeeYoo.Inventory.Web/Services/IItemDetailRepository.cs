using System;
using MeeYoo.Inventory.Web.ViewModels;

namespace MeeYoo.Inventory.Web.Services
{
    public interface IItemDetailRepository
    {
        ItemDetailViewModel Get(Guid warehouseItemId);
    }
}