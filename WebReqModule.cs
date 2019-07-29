using System;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web;

using WebSocketSharp;
using WebSocketSharp.Server;
using System.Collections.Generic;
using WebSocketSharp.Net;
using Newtonsoft.Json;

namespace SimFeedback.extension.webctrl
{
    public class WebReqModule
    {
        public HttpServer _httpsv = null;

        private List<ReqResponseMap> _reqResponses = new List<ReqResponseMap>();
        private HttpListenerRequest _req = null;

        public List<Parameters> ParseParameters(string rawUrl, string schema)
        {
            // schema look like this /log/{val:int}/{msg:string}
            // urls like this        /log/0/hello
            // we want schema parameter names, here 'val' and 'msg',
            // associate with their type (int, string) and the values
            // from the url --> '0' and 'hello'

            Globals.facade.LogDebug("in ParseParam");

            // store parameter name, type and string value in here
            List<Parameters> _parameters = new List<Parameters>();

            rawUrl = rawUrl.TrimEnd('/');
            schema = schema.TrimEnd('/');

            if (GetBasePath(schema) == rawUrl)
            {
                Globals.facade.LogDebug($"ParseParams: No paramters in URL of this endpoint");
                return _parameters;
            }

            // look for {name:val} patterns in schema
            var pattern = "\\{[a-zA-Z0-9]*:[a-zA-Z0-9]*\\}";
            MatchCollection matchList = Regex.Matches(schema, pattern);
            var schemaParams = matchList.Cast<Match>().Select(match => match.Value).ToList();
            Globals.facade.LogDebug($"ParseParams: got '{schemaParams.Count}' param matches in URL");

            if (schemaParams.Count > 0)
            {
                int a;
                int b = 1;
                foreach (string sp in schemaParams)
                {
                    Globals.facade.LogDebug($"ParseParams: handling url substr '{sp}'");
                    char[] trimChars = { '{', '}' };
                    var schemaParam = sp.Trim(trimChars);
                    var colpos = schemaParam.IndexOf(':');
                    var schemaParamName = schemaParam.Substring(0, colpos);
                    var schemaParamType = schemaParam.Substring(colpos + 1, schemaParam.Length - colpos - 1);

                    a = rawUrl.IndexOf('/', b);
                    b = rawUrl.IndexOf('/', a + 1);
                    b = b == -1 ? rawUrl.Length : b;
                    var urlVal = rawUrl.Substring(a, b - a).TrimStart('/');

                    Globals.facade.LogDebug($"ParseParam - Schema Name: '{schemaParamName}' - Schema Type: '{schemaParamType}' - Val from URL '{urlVal}'");

                    _parameters.Add(new Parameters
                    {
                        name = schemaParamName,
                        value = HttpUtility.UrlDecode(urlVal, new UTF8Encoding()),
                        typ = Type.GetType(schemaParamType)
                    });
                }
            }
            else
            {
                Globals.facade.LogDebug($"ParseParam: no parameters in schema for this route");
            }

            return _parameters;
        }

        public void Get(string p, Func<object, object> rf)
        {
            Globals.facade.LogDebug($"In Get: adding endpoint '{p}'");
            AddRESTEndpoint(p, rf);
        }

        public void Post(string p, Func<object, object> rf)
        {
            Globals.facade.LogDebug($"In Post: adding endpoint '{p}'");
            AddRESTEndpoint(p, rf);
        }

        public void AddRESTEndpoint(string p, Func<object, object> rf)
        {
            _reqResponses.Add(new ReqResponseMap
            {
                path = p,
                response = "",
                respFunc = rf,
            });            
        }

        public T Bind<T>()
        {
            Globals.facade.LogDebug("in Bind");
            if (_req == null)
                return (T)Convert.ChangeType(null, typeof(T));

            Stream dataStream = _req.InputStream;
            StreamReader reader = new StreamReader(dataStream);
            string result = reader.ReadToEnd();
            reader.Close();
            dataStream.Close();
            Globals.facade.LogDebug($"Bind - string result: '{result}'");
            if (result == "" || result == null)
            {
                return (T)Convert.ChangeType(null, typeof(T));
            }
            else
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(result);
                } catch (Exception e)
                {
                    Globals.facade.LogError($"Bind: unable to deserialize - probably malformed json - exception message '{e.Message}'");
                    return (T)Convert.ChangeType(null, typeof(T));
                }
            }
        }


        public void GetEvent(object sender, HttpRequestEventArgs e)
        {
            Globals.facade.LogDebug($"CurDir: '{Directory.GetCurrentDirectory()}'");
            Globals.facade.LogDebug("in OnGet");

            _req = e.Request;
            var path = _req.RawUrl;
            var res = e.Response;

            // Check for whether it is a REST-API call and if so handle it
            if (RestAPIResp(path, res))
            {
                Globals.facade.LogDebug($"GetEvent: handled endpoint '{path}'");
                return;
            }

            // No REST-API endpoint
            Globals.facade.LogDebug($"GetEvent - no REST endpoint but requested URL to load '{path}'");
            if (path == "/")
                path += "html/index.html";

            if (path == "/html")
                path += "/index.html";

            if (path == "/html/")
                path += "index.html";

            Globals.facade.LogDebug($"URL to load '{path}'");
            byte[] contents;

            if ((contents = _httpsv.GetFile(path)) == null)
            {
                res.StatusCode = (int)WebSocketSharp.Net.HttpStatusCode.NotFound;
                return;
            }

            if (path.EndsWith("index.html"))
            {
                res.ContentType = "text/html";
                res.ContentEncoding = Encoding.UTF8;

                // This might be a bit hacky but couldn't think of another way allowing
                // to change the websocket port dynamically. Here is how:
                // The index.html file embeds an invisible input element with a value set to
                // the pattern '#web#sock#port#'. We replace this pattern with the actual websocket
                // port and the JavaScript websocket code will read this value and open the websocket
                // with it.
                var strCont = res.ContentEncoding.GetString(contents);
                strCont = strCont.Replace("#web#sock#port#", PortData.Instance.webSvcPort.ToString());
                contents = res.ContentEncoding.GetBytes(strCont);
            }

            if (path.EndsWith(".html"))
            {
                res.ContentType = "text/html";
                res.ContentEncoding = Encoding.UTF8;
            }
            else if (path.EndsWith(".js"))
            {
                res.ContentType = "application/javascript";
                res.ContentEncoding = Encoding.UTF8;
            }
            else if (path.EndsWith(".css"))
            {
                res.ContentType = "text/css";
                res.ContentEncoding = Encoding.UTF8;
            }
            else if (path.EndsWith(".png"))
            {
                res.ContentType = "image/png";
                res.ContentEncoding = Encoding.UTF8;
            }

            res.WriteContent(contents);
        }


        public void PostEvent(object sender, HttpRequestEventArgs e)
        {
            Globals.facade.LogDebug($"CurDir: '{Directory.GetCurrentDirectory()}'");
            Globals.facade.LogDebug("in PostEvent");

            _req = e.Request;
            var path = _req.RawUrl;
            var res = e.Response;

            if (RestAPIResp(path, res))
                Globals.facade.LogDebug($"PostEvent: handled REST endpoint '{path}'");
            else
                Globals.facade.LogDebug("PostEvent: no URL match");
        }

        private bool RestAPIResp(string path, HttpListenerResponse res)
        {
            var basePath = GetBasePath(path);
            Globals.facade.LogDebug($"RestAPIResp: path '{path}' --> basepath '{basePath}'");

            foreach (ReqResponseMap respDef in _reqResponses)
            {
                var respDefBasePath = GetBasePath(respDef.path);
                Globals.facade.LogDebug($"RestAPIResp: respDef.path '{respDef.path}' --> respDefBasePath '{respDefBasePath}'");

                if (basePath == respDefBasePath)
                {
                    Globals.facade.LogDebug($"In RestAPIResp: Handling path '{path}'");
                    res.ContentType = "application/json";
                    res.ContentEncoding = Encoding.UTF8;
                    List<Parameters> _parameters = ParseParameters(path, respDef.path);
                    object response = null;
                    if (respDef.respFunc != null)
                    {
                        response = respDef.respFunc(_parameters);
                    }
                    var json = JsonConvert.SerializeObject(response);
                    Globals.facade.LogDebug($"In RestAPIResp: serialized response '{json}'");
                    res.WriteContent(Encoding.UTF8.GetBytes(json));
                    return true;
                }
            }
            return false;
        }

        /* Removes trailing paramter schema (i.e. '{...}' and slashes from path */
        private string GetBasePath(string path)
        {
            var idx = path.IndexOf('/', 1);
            var bp = path.Substring(0, idx == -1 ? path.Length : idx);
            return bp.TrimEnd('/');
        }
    }
}