using System;
using System.Collections.Generic;

using WebSocketSharp.Server;

namespace SimFeedback.extension.webctrl
{
    public class PostModule : WebReqModule
    {
        public PostModule(HttpServer httpsv)
        {
            Globals.facade.LogDebug($"PostModule Constructor");
            _httpsv = httpsv;

            Post("/start/{val:int}", x =>
            {
                int start = 0;
                string msg = "";

                List<Parameters> pl = (List<Parameters>)x;
                if (pl.Count == 1)
                {
                    foreach (Parameters p in pl)
                    {
                        Globals.facade.LogDebug($"In start action: handling url param '{p.name}'");
                        if (p.name == "val")
                            start = p.GetVal<int>();
                        else
                            Globals.facade.LogError($"Unknown paramter '{p.name}' in paramter list of 'start' post call.");
                    }
                }
                else
                {
                    PostData recievedData = this.Bind<PostData>();
                    Globals.facade.LogDebug($"Log: val received in message body '{recievedData.val}'");
                    start = recievedData.val;
                    msg = recievedData.message;
                }

                if (start == 1)
                    Globals.facade.Start();
                else
                    Globals.facade.Stop();

                Globals.lastMessage = msg;

                return new { success = true, message = $"Message recieved = {msg}", val = start };
            });
            Globals.facade.LogDebug($"PostModule Constructor - added start");


            Post("/save", _ =>
            {
                Globals.facade.SaveConfig();
                Globals.facade.LogDebug("/save route: saved configuration.");

                Globals.lastMessage = "";

                return new { success = true, message = $"", val = 0 };
            });
            Globals.facade.LogDebug($"PostModule Constructor - added save");


            Post("/log/{val:int}/{message:string}", x =>
            {
                int loglvl = 0;
                string msg = "";

                List<Parameters> pl = (List<Parameters>)x;
                if (pl.Count == 2)
                {
                    foreach (Parameters p in pl)
                    {
                        Globals.facade.LogDebug($"In log action: handling url param '{p.name}'");
                        if (p.name == "val")
                            loglvl = p.GetVal<int>();
                        else if (p.name == "message")
                            msg = p.value;
                        else
                            Globals.facade.LogError($"Unknown paramter '{p.name}' in paramter list of 'log' post call.");
                    }
                }
                else
                {
                    PostData recievedData = this.Bind<PostData>();
                    Globals.facade.LogDebug($"Log: val received in message body '{recievedData.val}'");
                    loglvl = recievedData.val;
                    msg = recievedData.message;
                }
                switch (loglvl)
                {
                    case 0:
                        Globals.facade.Log(msg);
                        break;
                    case 1:
                        Globals.facade.LogDebug(msg);
                        break;
                    case 2:
                        Globals.facade.LogError(msg);
                        break;
                    default:
                        Globals.facade.LogError($"Unknown logging type '{loglvl}' - trying to log message '{msg}'");
                        break;
                }
                
                Globals.lastMessage = msg;

                return new { success = true, message = $"Message recieved = {msg}", val = loglvl };
            });
            Globals.facade.LogDebug($"PostModule Constructor - added log");


            Post("/enable/{val:int}", x =>
            {
                int enable = 0;
                string msg = "";

                List<Parameters> pl = (List<Parameters>)x;
                if (pl.Count == 1)
                {
                    foreach (Parameters p in pl)
                    {
                        Globals.facade.LogDebug($"In enable action: handling url param '{p.name}'");
                        if (p.name == "val")
                            enable = p.GetVal<int>();
                        else
                            Globals.facade.LogError($"Unknown paramter '{p.name}' in paramter list of 'enable' post call.");
                    }
                }
                else
                {
                    PostData recievedData = this.Bind<PostData>();
                    Globals.facade.LogDebug($"Log: val received in message body '{recievedData.val}'");
                    enable = recievedData.val;
                    msg = recievedData.message;
                }

                if (enable == 1)
                {
                    Globals.facade.LogDebug("Enabling all effects");
                    Globals.facade.EnableAllEffects();

                }
                else
                {
                    Globals.facade.LogDebug("Disabling all effects");
                    Globals.facade.DisableAllEffects();
                }

                Globals.lastMessage = msg;

                return new { success = true, message = $"Message recieved = {msg}", val = enable };

            });
            Globals.facade.LogDebug($"PostModule Constructor - added enable");


            Post("/intensity/{val:int}", x =>
            {
                int intensity = 0;
                string msg = "";

                List<Parameters> pl = (List<Parameters>)x;
                if (pl.Count == 1)
                {
                    foreach (Parameters p in pl)
                    {
                        Globals.facade.LogDebug($"In itensity action: handling url param '{p.name}'");
                        if (p.name == "val")
                            intensity = p.GetVal<int>();
                        else
                            Globals.facade.LogError($"Unknown paramter '{p.name}' in paramter list of 'itensity' post call.");
                    }
                }
                else
                {
                    PostData recievedData = this.Bind<PostData>();
                    Globals.facade.LogDebug($"Log: val received in message body '{recievedData.val}'");
                    intensity = recievedData.val;
                    msg = recievedData.message;
                }

                if (intensity > 0)
                {
                    Globals.facade.LogDebug($"Incrementing intensity {intensity}x");
                    for (int i = 0; i < Math.Abs(intensity); i++)
                        Globals.facade.IncrementOverallIntensity();
                }
                else
                {
                    Globals.facade.LogDebug($"Decrementing intensity {intensity}x");
                    for (int i = 0; i < Math.Abs(intensity); i++)
                        Globals.facade.DecrementOverallIntensity();
                }

                Globals.lastMessage = msg;
                Globals.intensityIncrement = intensity;

                return new { success = true, message = $"Message recieved = {msg}", val = intensity };

            });
            Globals.facade.LogDebug($"PostModule Constructor - added intensity");
        }
    }
}