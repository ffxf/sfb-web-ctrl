using System.IO;
using Newtonsoft.Json;

namespace SimFeedback.extension.webctrl
{
    /* Singleton pattern used here to avoid we have to instantiate the class
       everywhere it is being used while we have to de/serialize it to write
       it as a JSON file.
     */
    public class PortData
    {
        private static PortData _instance;
        public int httpPort { get; set; } = 8080;
        public int webSvcPort { get; set; } = 8181;

        public static PortData Instance
        {
            get { return _instance ?? (_instance = new PortData()); }
        }

        public static void ReadFile()
        {
            PortData pdata = JsonConvert.DeserializeObject<PortData>(File.ReadAllText(Globals.portFile));
            PortData.Instance.httpPort = pdata.httpPort;
            PortData.Instance.webSvcPort = pdata.webSvcPort;
        }

        public static void WriteFile()
        {
            File.WriteAllText(Globals.portFile, JsonConvert.SerializeObject(PortData.Instance));
        }
    }

    class Globals
    {
        public static SimFeedbackExtensionFacade facade { get; set; }
        public static int intensityIncrement { get; set; }
        public static string lastMessage { get; set; }
        public static string rootPath { get; set; } = "extensions/sfbctrl";
        public static string portFile { get; set; } = rootPath + "/" + "portdata.json";
    }
}