﻿using System;
using EventStore.ClientAPI;
using MeeYoo.Inventory.Web.GetEventStore;
using Raven.Client;

namespace MeeYoo.Inventory.Web.ViewWriters
{
    public class ImmediateDocumentSessionObserver<TView> : IObserver<Action<IDocumentSession>>,
                                                           IGetEventStorePositionRepository
    {
        private readonly IDocumentStore documentStore;

        public ImmediateDocumentSessionObserver(IDocumentStore documentStore)
        {
            this.documentStore = documentStore;
        }

        #region IGetEventStorePositionRepository Members

        public Position? GetLastProcessedPosition()
        {
            return documentStore.GetPositionFromRaven<TView>();
        }

        #endregion

        #region IObserver<Action<IDocumentSession>> Members

        public void OnNext(Action<IDocumentSession> value)
        {
            using (var session = documentStore.OpenSession())
            {
                value(session);
                session.SaveChanges();
            }
        }

        public void OnError(Exception error)
        {
        }

        public void OnCompleted()
        {
        }

        #endregion
    }
}