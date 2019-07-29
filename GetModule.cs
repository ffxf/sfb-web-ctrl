using WebSocketSharp.Server;

namespace SimFeedback.extension.webctrl
{
    public class GetModule : WebReqModule
    {
        public GetModule(HttpServer httpsv)
        {
            _httpsv = httpsv;

            Get("/status", _ =>
            {
                return new GetData()
                {
                    intensityIncrement = Globals.intensityIncrement,
                    isConnected = Globals.facade.IsTelemetryProviderConnected(),
                    isSFXOn = Globals.facade.IsRunning(),
                    lastMessage = Globals.lastMessage
                };
            });
            Globals.facade.LogDebug($"GetModule Constructor - added status");
        }
    }
}