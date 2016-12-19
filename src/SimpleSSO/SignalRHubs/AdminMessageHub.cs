using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin.Security.OAuth;
using SimpleSSO.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using static SimpleSSO.Setting;

namespace SimpleSSO.SignalRHubs.Admin
{
    //[Authorize(Roles = RoleConfig.AdminRole)]
    [HubName("adminMessageHub")]
    public class AdminMessageHub : Hub
    {
        private const string AdminRole = RoleConfig.AdminRole;

        public override Task OnConnected()
        {
            var connectionId = Context.ConnectionId;
            if (Context.User.IsInRole(AdminRole))
            {
                Groups.Add(Context.ConnectionId, AdminRole);
            }
            return base.OnConnected();
        }

        public override Task OnReconnected()
        {
            if (Context.User.IsInRole(AdminRole))
            {
                Groups.Remove(Context.ConnectionId, AdminRole);
            }
            return base.OnReconnected();
        }

        public void SendToAdmin(string value)
        {
            Clients.Group(AdminRole).hubMessage(value);
        }

        public void SendToGroup(string groupName, string value)
        {
            Clients.Group(groupName).hubMessage(value);
        }

    }
}