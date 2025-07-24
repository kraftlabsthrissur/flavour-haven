using BusinessLayer;
using Microsoft.AspNet.SignalR;
using PresentationContractLayer;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TradeSuiteApp.Web.SignalRHub
{
    public class PrintHub : Hub
    {
        private readonly static ConnectionMapping<string> Connections = new ConnectionMapping<string>();

        public void TextFile(string FilePath) {
            foreach (var connectionId in Connections.GetConnections(PrintUID))
            {
                Clients.Client(connectionId).Print(FilePath);
            }
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

        public override Task OnConnected()
        {
            Connections.Add(SignalRUID, Context.ConnectionId);
            return base.OnConnected();
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            Connections.Remove(SignalRUID, Context.ConnectionId);
            return base.OnDisconnected(stopCalled);
        }

        public override Task OnReconnected()
        {
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