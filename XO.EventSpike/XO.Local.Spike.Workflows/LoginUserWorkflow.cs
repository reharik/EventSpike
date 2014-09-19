﻿using System;
using System.Net;
using EventStore.ClientAPI;
using XO.Local.Domain;
using XO.Local.Spike.Domain.AggregateRoots;

namespace XO.Local.Spike.Workflows
{
    public interface ILoginUserWorkflow
    {
        void LoginUser(LoginUser loginUser);
    }

    public class LoginUserWorkflow : ILoginUserWorkflow
    {
        private readonly IGetEventStoreRepository _getEventStoreRepository;

        public LoginUserWorkflow(IGetEventStoreRepository getEventStoreRepository)
        {
            _getEventStoreRepository = getEventStoreRepository;
        }

        public void LoginUser(LoginUser loginUser)
        {
            //            var byId = _getEventStoreRepository.GetById<User>(new Guid("f036aadb-d427-4478-ac8c-c168e2d55d9f"));
            var user = _getEventStoreRepository.GetById<User>(loginUser.Id);
            user.Handle(loginUser);
            _getEventStoreRepository.Save(user, Guid.NewGuid(), a => { });
        }
    }
}