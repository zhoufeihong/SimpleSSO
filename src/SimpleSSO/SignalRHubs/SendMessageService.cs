using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static SimpleSSO.Setting;

namespace SimpleSSO.SignalRHubs
{
    public class SendMessageService
    {
        private const string AdminRole = RoleConfig.AdminRole;

        public void SendToAdmin(IHubContext context, string value)
        {
            context.Clients.Group(AdminRole).hubMessage(value);
        }

        public void SendToGroup(IHubContext context, string groupName, string value)
        {
            context.Clients.Group(groupName).hubMessage(value);
        }

        public void SendToUser(IHubContext context, string userId, string value)
        {
            context.Clients.User(userId).hubMessage(value);
        }

        public void SendToUsers(IHubContext context, IList<string> userIds, string value)
        {
            context.Clients.Users(userIds).hubMessage(value);
        }

    }
}