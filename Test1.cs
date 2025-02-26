using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;

namespace TestProject1
{

    public class TestTools
    {
       public async Task<dynamic> ReadAndDeserializeResponseAsync(HttpResponseMessage response)
    {
        if (response == null)
        {
            throw new ArgumentNullException(nameof(response), "Ответ не может быть null.");
        }

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException($"Запрос завершился с кодом {response.StatusCode}.");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject(responseContent);
    }
    }
   // [TestClass]
    public sealed class Example_Tests
    {
        [TestMethod]
        public async Task FirstTest()
        {
            using (HttpClient client = new HttpClient())
            {
                //Создаю тело запроса
                var RequestBody = new { 
                    name2 = "Artem123" 
                };
                
                var json = JsonConvert.SerializeObject(RequestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //Отправля Post запрос с телом запроса 
               // HttpResponseMessage response = await client.PostAsync("http://localhost:8080/ekassir", content);
                HttpResponseMessage response = await client.GetAsync("https://api.agify.io/?name=Artem");
                //Проверяю статус код ответа
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic parsedResponse = JsonConvert.DeserializeObject(responseContent);
                    
                    //Проверяю параметр name
                   Assert.AreEqual("Artem", parsedResponse.name.ToString());
                   Assert.AreEqual(39, (int)parsedResponse.age);
                    Console.WriteLine(parsedResponse);

                }


            }
        }



        [TestMethod]
        public async Task SecondTest()
        {
            using (HttpClient client = new HttpClient())
            {
                //Создаю тело запроса
                var RequestBody = new
                {
                    TestNotFound = "TestNotFound"
                };

                var json = JsonConvert.SerializeObject(RequestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //Отправля Post запрос с телом запроса 
                HttpResponseMessage response = await client.PostAsync("http://localhost:8080/ekassir", content);
                //Проверяю статус код ответа
                Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic parsedResponse = JsonConvert.DeserializeObject(responseContent);
                    //Проверяю параметр name
                    //Assert.AreEqual("Артём", parsedResponse.name.ToString());

                }


            }
        }







    }
}
