﻿using System;
using System.Linq;
using MeeYoo.Inventory.Web.Services;
using MeeYoo.Inventory.Web.ViewModels;
using Nancy;

namespace MeeYoo.Inventory.Web.Modules
{
    public class WarehouseModule : NancyModule
    {
        public WarehouseModule(IItemSearchRepository items)
            : base("/warehouse")
        {
            Get["/{warehouseId}"] = p =>
            {
                Guid warehouseId = p.warehouseId;
                var warehouse = WarehouseListViewModel.Instance.SingleOrDefault(vm => vm.WarehouseId == warehouseId);
                if (warehouse == null) return 404;
                return Negotiate.WithModel(new WarehouseOverviewViewModel(warehouse.WarehouseId, warehouse.Name));
            };

            Get["/{warehouseId}/search-items"] = p =>
            {
                Guid warehouseId = p.warehouseId;
                string term = Request.Query.q;
                return items.Search(term, warehouseId);
            };
        }
    }
}