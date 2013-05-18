using System;
using MeeYoo.Inventory.Web.ViewModels;

namespace MeeYoo.Inventory.Web.Services
{
    public interface IItemSearchRepository
    {
        ItemSearchResultsViewModel Search(string term, Guid warehouseId);
    }
}