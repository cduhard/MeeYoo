using System;
using System.Collections.Generic;
using MeeYoo.Inventory.Web.ViewModels;

namespace MeeYoo.Inventory.Web.Services
{
    public class InMemoryItemDetailRepository : Dictionary<Guid, ItemDetailViewModel>, IItemDetailRepository
    {
        #region IItemDetailRepository Members

        public ItemDetailViewModel Get(Guid warehouseItemId)
        {
            ItemDetailViewModel value;
            TryGetValue(warehouseItemId, out value);
            return value;
        }

        #endregion
    }
}