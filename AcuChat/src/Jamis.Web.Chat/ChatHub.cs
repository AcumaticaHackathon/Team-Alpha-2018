using Microsoft.AspNet.SignalR;
using PX.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;

namespace Jamis.Web.Chat
{
    /// <examples>
    /// See aditional docs here:
    /// https://docs.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/hubs-api-guide-server
    /// https://docs.microsoft.com/en-us/aspnet/signalr/overview/guide-to-the-api/mapping-users-to-connections
    /// </examples>
    [Authorize]
    public class ChatHub : Hub
    {
        private static IDictionary<string, User> ActiveUsers = new Dictionary<string, User>();

        public User GetCurrentUser()
        {
            return GetUser(Membership.GetUser(Context.User.Identity.Name) as MembershipUserExt);
        }

        public IEnumerable<User> GetActiveUsers()
        {
            return ActiveUsers.Values;
        }

        public void SendMessage(string userId, string message)
        {
            var user = this.GetCurrentUser();

            if (user != null)
            {
                this.Clients.Group(userId).receiveMessage(user.DisplayName, message);
            }
        }

        public void SendAll(string message)
        {
            var user = this.GetCurrentUser();

            if (user != null)
            {
                this.Clients.All.receiveMessage(user.DisplayName, message);
            }
        }

        public override System.Threading.Tasks.Task OnConnected()
        {
            var user = this.GetCurrentUser();

            if (user != null)
            {
                Groups.Add(Context.ConnectionId, user.Id);

                if (ActiveUsers.ContainsKey(user.UserName) == false)
                {
                    ActiveUsers.Add(user.UserName, user);
                }

                this.Clients.Group(user.Id).currentUser(user);

                this.Clients.All.activeUsers(GetActiveUsers());
            }

            return base.OnConnected();
        }

        public override System.Threading.Tasks.Task OnDisconnected(bool stopCalled)
        {
            var userName = Context.User.Identity.Name;

            if (ActiveUsers.ContainsKey(userName))
            {
                ActiveUsers.Remove(userName);

                this.Clients.All.activeUsers(GetActiveUsers());
            }

            return base.OnDisconnected(stopCalled);
        }

        public override System.Threading.Tasks.Task OnReconnected()
        {
            var user = GetCurrentUser();

            if (ActiveUsers.ContainsKey(user.UserName))
            {
                ActiveUsers.Add(user.UserName, user);

                this.Clients.All.activeUsers(GetActiveUsers());
            }

            return base.OnReconnected();
        }

        private User GetUser(MembershipUserExt user)
        {
            if (user != null)
            {
                return new User
                {
                    Id = user.ProviderUserKey.ToString(),
                    DisplayName = user.DisplayName,
                    UserName = user.UserName + "@" + PXAccess.GetCompanyName()
                };
            }

            return null;
        }

        public class User
        {
            public string Id { get; set; }
            public string UserName { get; set; }
            public string DisplayName { get; set; }
        }
    }
}
