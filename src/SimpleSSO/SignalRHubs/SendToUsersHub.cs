using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static SimpleSSO.Setting;

namespace SimpleSSO.SignalRHubs
{
    // [Authorize(Roles = RoleConfig.InUserAdminRole)]
    [HubName("sendToUsersHub")]
    public class SendToUsersHub : Hub
    {
        public void SendToMe(string value)
        {
            Clients.User(Context.User.Identity.GetUserId()).hubMessage(value);
        }

        public void SendToUser(string userId, string value)
        {
            Clients.User(userId).hubMessage(value);
        }

        public void SendToUsers(IList<string> userIds, string value)
        {
            Clients.Users(userIds).hubMessage(value);
        }

    }
}