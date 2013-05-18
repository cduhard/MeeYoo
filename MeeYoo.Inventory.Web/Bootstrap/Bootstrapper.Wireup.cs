using System;
using MeeYoo.Inventory.Application;
using MeeYoo.Inventory.Domain;
using MeeYoo.Inventory.Messages;
using MeeYoo.Inventory.Web.GetEventStore;
using MeeYoo.Inventory.Web.Projections;
using MeeYoo.Inventory.Web.Services;
using MeeYoo.Inventory.Web.ViewModels;
using MeeYoo.Inventory.Web.ViewWriters;
using Nancy.TinyIoc;
using Raven.Client;

namespace MeeYoo.Inventory.Web.Bootstrap
{
    public partial class Bootstrapper
    {
        private void RegisterProjections(TinyIoCContainer container)
        {
            var itemDetailRepository = new InMemoryItemDetailRepository();
            container.Register<IItemDetailRepository>(itemDetailRepository);
            var itemDetailViewWriter = new InMemoryViewWriter<Guid, ItemDetailViewModel>(itemDetailRepository);
            var itemDetails = new ItemDetailResultProjection(
                itemDetailViewWriter);

            var itemDetailSubscription = new GetEventStoreEventDispatcher(
                EventStoreConnection, serializerSettings, itemDetailViewWriter, bus);
            itemDetailSubscription.Subscribe<ItemTracked>(itemDetails.Handle);
            itemDetailSubscription.Subscribe<ItemPicked>(itemDetails.Handle);
            itemDetailSubscription.Subscribe<ItemLiquidated>(itemDetails.Handle);
            itemDetailSubscription.Subscribe<ItemReceived>(itemDetails.Handle);
            itemDetailSubscription.Subscribe<ItemQuantityAdjusted>(itemDetails.Handle);
            itemDetailSubscription.Subscribe<CycleCountStarted>(itemDetails.Handle);
            itemDetailSubscription.Subscribe<CycleCountCompleted>(itemDetails.Handle);

            itemDetailSubscription.StartDispatching();

            container.Register<IItemSearchRepository>(
                (c, n) => new RavenItemSearchRepository(c.Resolve<IDocumentSession>()));
            var itemSearchSessionObserver = new CatchUpDocumentSessionObserver<ItemSearchResultViewModel>(DocumentStore);
            bus.Register(itemSearchSessionObserver);
            var itemSearch = new ItemSearchResultProjection(
                new RavenDbViewWriter<Guid, ItemSearchResultViewModel>(
                    itemSearchSessionObserver));

            var itemSearchSubscription = new GetEventStoreEventDispatcher(
                EventStoreConnection, serializerSettings, itemSearchSessionObserver, bus);
            itemSearchSubscription.Subscribe<ItemTracked>(itemSearch.Handle);

            itemSearchSubscription.StartDispatching();
        }

        private void RegisterInventoryHandlers()
        {
            var handler = new InventoryHandlers(new GetEventStoreRepository<Reminiscence>(
                                                    EventStoreConnection, "inventory"));

            Register<TrackItem>(handler);
            Register<PickItem>(handler);
            Register<StartCycleCount>(handler);
            Register<CompleteCycleCount>(handler);
            Register<AdjustItemQuantity>(handler);
            Register<LiquidateItem>(handler);
            Register<ReceiveItem>(handler);
        }
    }
}