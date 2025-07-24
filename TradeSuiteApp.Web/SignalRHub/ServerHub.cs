using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeSuiteApp.Web.SignalRHub
{
    public class ServerHub : Hub
    {
        private readonly static ConnectionMapping<string> Connections = new ConnectionMapping<string>();

        public void PrintTextFile(string URL)
        {
            string ConnectionID = Context.ConnectionId;
            try
            {
                if (Connections.GetConnections(PrintUID).Count() == 0)
                {
                    Clients.Client(ConnectionID).Error("Could not find the printer");
                }

                foreach (var printConnectionID in Connections.GetConnections(PrintUID))
                {
                    Clients.Client(printConnectionID).Print(URL, ConnectionID);
                }
            }
            catch (Exception e)
            {
                Clients.Client(ConnectionID).error(e.Message);
            }
        }

        public void SendNotification(string Title, string Text, int[] UserIds)
        {
            foreach (var UserID in UserIds)
            {
                foreach (var userConnectionID in Connections.GetConnections(UserID.ToString()))
                {
                    Clients.Client(userConnectionID).OnNotificationReceived(Title, Text);
                }
            }

        }

        public void SendTickers(string Type, string Html, int[] UserIds)
        {
            foreach (var UserID in UserIds)
            {
                foreach (var userConnectionID in Connections.GetConnections(UserID.ToString()))
                {
                    Clients.Client(userConnectionID).OnTickerReceived(Type, Html);
                }
            }
        }

        public void NotifyError(string Message, string ConnectionID)
        {
            Clients.Client(ConnectionID).error(Message);
        }

        public void GetToken()
        {
            string ConnectionID = Context.ConnectionId;
            Clients.All.GetToken(ConnectionID);
        }

        private string SignalRUID
        {
            get
            {
                string _SignalRUID = "";

                if (Context.Request.Cookies["SignalRUID"] != null)
                {
                    _SignalRUID = Context.Request.Cookies["SignalRUID"].Value.ToString();
                }
                return _SignalRUID;
            }
        }

        private string PrintUID
        {
            get
            {
                string _PrintUID = "";

                if (Context.Request.Cookies["PrintUID"] != null)
                {
                    _PrintUID = Context.Request.Cookies["PrintUID"].Value.ToString();
                }
                return _PrintUID;
            }
        }

        private string UserID
        {
            get
            {
                return Context.User.Identity.GetUserId()?? "0";
            }
        }

        public override Task OnConnected()
        {
            try
            {
                Connections.Add(UserID, Context.ConnectionId);
            }
            catch (Exception)
            {
                //Desktop client
            }
            Connections.Add(SignalRUID, Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            try
            {
                Connections.Remove(UserID, Context.ConnectionId);
            }
            catch (Exception)
            {
                //Desktop client
            }
            Connections.Remove(SignalRUID, Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
            if (!Connections.GetConnections(UserID).Contains(Context.ConnectionId))
            {
                try
                {
                    Connections.Add(UserID, Context.ConnectionId);
                }
                catch (Exception)
                {
                    //Desktop client
                }
            }
            if (!Connections.GetConnections(SignalRUID).Contains(Context.ConnectionId))
            {
                Connections.Add(SignalRUID, Context.ConnectionId);
            }
            return base.OnReconnected();
        }

    }

    public class ConnectionMapping<T>
    {
        private readonly Dictionary<T, HashSet<string>> _connections = new Dictionary<T, HashSet<string>>();

        public int Count
        {
            get
            {
                return _connections.Count;
            }
        }

        public void Add(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    connections = new HashSet<string>();
                    _connections.Add(key, connections);
                }

                lock (connections)
                {
                    connections.Add(connectionId);
                }
            }
        }

        public IEnumerable<string> GetConnections(T key)
        {
            HashSet<string> connections;
            if (_connections.TryGetValue(key, out connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }

        public void Remove(T key, string connectionId)
        {
            lock (_connections)
            {
                HashSet<string> connections;
                if (!_connections.TryGetValue(key, out connections))
                {
                    return;
                }

                lock (connections)
                {
                    connections.Remove(connectionId);

                    if (connections.Count == 0)
                    {
                        _connections.Remove(key);
                    }
                }
            }
        }
    }
}