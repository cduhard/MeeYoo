using System;
using System.Collections.Generic;
using System.Linq;
using MeeYoo.Inventory.Web.ViewModels;

namespace MeeYoo.Inventory.Web.Services
{
    public class InMemoryItemSearchRepository : Dictionary<Guid, ItemSearchResultViewModel>, IItemSearchRepository
    {
        #region IItemSearchRepository Members

        public ItemSearchResultsViewModel Search(string term, Guid warehouseId)
        {
            return new ItemSearchResultsViewModel(from item in Values
                                                  where item.WarehouseId == warehouseId
                                                        && item.SearchTerm.Contains(term.ToLowerInvariant())
                                                  select item);
        }

        #endregion
    }
}