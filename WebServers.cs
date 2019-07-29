using System.Threading.Tasks;

using WebSocketSharp;
using WebSocketSharp.Server;
using Newtonsoft.Json;

namespace SimFeedback.extension.webctrl
{
    public class SFBCtrlWebservices
    {
        public async Task StartWebServicesAsync()
        {
            ServerThreads st = new ServerThreads();

            // We keep restarting the web servers. They will terminate when ports have changed
            while (true)
                _= await st.CreateWebServersAsync();
        }
    }

    public class ServerThreads
    {
        public async Task<bool> CreateWebServersAsync()
        {
            int taskDelay = 100; // # of millisecs --> wake up tasks every 1/10th of a sec
            int updateFrequency = 30; // updateFrequency * taskDelay = 3000 millisecs = 3secs 

            int httpPort = PortData.Instance.httpPort;
            int webSvcPort = PortData.Instance.webSvcPort;

            Globals.facade.LogDebug("In CreateWebSocAsync");
            Globals.facade.LogDebug($"Starting WebSocketServer on port {webSvcPort} ...");
            var wssv = new WebSocketServer(webSvcPort);
            Globals.facade.LogDebug($"Starting HttpWebServer on port {httpPort} ...");
            var httpsv = new HttpServer(httpPort);
            wssv.AddWebSocketService<SFB_WebSock>("/websock");
            // Set the document root path.
            httpsv.RootPath = Globals.rootPath;
            Globals.facade.LogDebug($"Root path '{httpsv.RootPath}'");

            var getter = new GetModule(httpsv);
            httpsv.OnGet += (sender, e) => getter.GetEvent(sender, e);
            var poster = new PostModule(httpsv);
            httpsv.OnPost += (sender, e) => poster.PostEvent(sender, e);

            httpsv.Start();
            wssv.Start();
            Globals.facade.LogDebug($"Started WebSocketServer on port '{webSvcPort}'  and http server on '{httpPort}'...");

            SFB_Status sfb_status = new SFB_Status();
            int count = 0;
            while (true)
            {
                await Task.Delay(taskDelay);

                // Terminate task to restart the web servers if ports have changed
                if (httpPort != PortData.Instance.httpPort || webSvcPort != PortData.Instance.webSvcPort)
                    break;

                // SFB takes some time to dis/connect to the controller. So send updates only every few secs
                if (count % updateFrequency == 0)
                {
                    // The status of SFB is all we send via the web socket
                    sfb_status.isActivated = Globals.facade.IsTelemetryProviderConnected();
                    sfb_status.isRunning = Globals.facade.IsRunning();
                    var json = JsonConvert.SerializeObject(sfb_status);
                    wssv.WebSocketServices.Broadcast(json);
                }
                count++;
            }
            wssv.Stop();
            httpsv.Stop();
            return true;
        }
    }

    public class SFB_WebSock : WebSocketBehavior
    {
        public void Snd(string msg)
        {
            Send(msg);
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (!e.IsText)
            {
                return;
            }

            Send(e.Data);
        }

        protected override void OnOpen()
        {
            Globals.facade.LogDebug("Connection Established!");
            Send("Connected!");
            base.OnOpen();
        }

    }

    public class SFB_Status {
            public bool isRunning { get; set; }
            public bool isActivated { get; set; }
    }
}
