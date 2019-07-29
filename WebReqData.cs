using System;

namespace SimFeedback.extension.webctrl
{
    public class ReqResponseMap
    {
        public string path { get; set; }
        public object response { get; set; }
        public Func<object, object> respFunc { get; set; }
    }

    public class Parameters
    {
        public string name { get; set; }
        public string value { get; set; }
        public Type typ { get; set; }

        public T GetVal<T>() {
            return (T)Convert.ChangeType(value, typeof(T));
            /*
            if (typeof(T).Equals(typ))
            {
                return value
            }
            */
        }
    }

    /*
    public class RequestResponseData
    {
        public string msg { get; set; }
        public int val { get; set; }
    }
    */
}