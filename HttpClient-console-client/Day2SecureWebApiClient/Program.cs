using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Day2SecureWebApiClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();
            string url = "https://localhost:5001/api/user";
            var validToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJuYmYiOjE1OTM0MjAwNjQsImV4cCI6MTU5MzU5Mjg2NCwiaWF0IjoxNTkzNDIwMDY0fQ.svgVhGE6Ev2JC0u-IYbKxFXNZbJp5oPHLmw862Z-yHk";

            //httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {validToken}");

            //var response =  await httpClient.GetAsync(url);
            //if(response.IsSuccessStatusCode)
            //{
            //    var responseBody = await response.Content.ReadAsStringAsync();
            //    var jsonData = JArray.Parse(responseBody);
            //}


            var loginReq = new LoginRequest() {
                 Password = "1234567", UserName = "user1"
            };

            httpClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            var payload = new StringContent(JsonConvert.SerializeObject(loginReq));
            var response = await httpClient.PostAsync($"{url}/login", payload);
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var jsonData = JArray.Parse(responseBody);
            }

            Console.ReadKey();
        }
    }

    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
    }

    public class LoginRequest
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
