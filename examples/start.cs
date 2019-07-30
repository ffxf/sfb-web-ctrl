using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

/* Simple Console program to start the SFX-100 system */

namespace SFXRest
{
    class StartSFX
    {
        public static void Main()
        {
            _ = new StartSFX().StartAsync();
            Console.ReadLine();
        }

        public async Task<bool> StartAsync()
        {
            HttpClient client = new HttpClient();

            var content = new StringContent("{}", Encoding.UTF8, "application/json");

            var response = await client.PostAsync("http://127.0.0.1:8080/start/1", content);

            var responseString = await response.Content.ReadAsStringAsync();

            Console.WriteLine(responseString);

            return true;
        }
    }
}