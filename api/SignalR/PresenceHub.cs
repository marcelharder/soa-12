using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using api.Entities;
using api.Interfaces.signalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace api.SignalR
{

    [Authorize]
    public class PresenceHub : Hub
    {
        private readonly IUserOnline _online;

        public PresenceHub(IUserOnline online)
        {
            _online = online;
        }
        public override async Task OnConnectedAsync()
        {
          /*   // find out if a onlineuser with the current name exists
            var user = await this._online.findUser(Context.User.Identity.Name);
            if (user != null)
            {
                var connections = user.ConnectionId;
                connections = connections + Context.ConnectionId;
                if (await _online.UpdateAsync(user) == 1)
                {

                };
            }
            else
            {
                var online_user = new Class_User_Online();
                online_user.Name = Context.User.Identity.Name;
                online_user.ConnectionId = Context.ConnectionId;
                if (await _online.AddAsync(user) == 1)
                {

                };
            }
 */

            await Clients.Others.SendAsync("UserIsOnline", Context.User.Identity.Name);

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
          /*   var currentConnectionId = Context.ConnectionId;
            var user = await this._online.findUser(Context.User.Identity.Name);
            if (user != null)
            {
                String[] connections = Regex.Split(user.ConnectionId, ", ");
                //var index = Array.Find(connections, x => x.Contains(Context.ConnectionId));
                connections = connections.Where(val => val != Context.ConnectionId).ToArray();
                if (connections.Length == 0)
                {
                    if (await _online.DeleteAsync(user) == 1) { };
                }
            } */

            await Clients.Others.SendAsync("UserIsOffline", Context.User.Identity.Name);
            await base.OnDisconnectedAsync(exception);
        }
    }
}