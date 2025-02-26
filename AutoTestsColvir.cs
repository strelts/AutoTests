using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using TestProject1;
using Newtonsoft.Json.Linq;

namespace ColvirAutoTests
{
    
    public class Colvir
    {   //Адрес сервиса колвир
        static String ColvirUrl = "http://localhost:8080"; 
        //Path запроса для поиска информации о кредите по иин
        static String loansEndpoint = ColvirUrl + "/ekassir/client/loans";
        //Path запроса для поиска информации по счетам по иин
        static String accListEndpoint = ColvirUrl + "/ekassir/client/acclist";
        public String GetColvirUrl()
        {
            return ColvirUrl;
        }
        public String GetLoansEndPoint()
        {
            return loansEndpoint;
        }
        public String GetaccListEndpoint()
        {
            return accListEndpoint;
        }



    }

 

    [TestClass]
    public sealed class ColvirAutoTests
    {
        Colvir Colvir = new Colvir();
        CustomAsserts pAssert = new CustomAsserts();

       

        [TestMethod]
        public async Task TestSuccessCheckByIIN()
        {
         
            using (HttpClient client = new HttpClient())
            {
                //Создаю тело запроса
                var RequestBody = new
                {
                    iin = "111111111111",
                    term_id = "204204"
                };

                var json = JsonConvert.SerializeObject(RequestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //Отправля Post запрос с телом запроса 
                client.DefaultRequestHeaders.Add("apiKey", "a785d625-d57b-4a1a-9ef4-8b83568dd228");
                HttpResponseMessage response = await client.PostAsync(Colvir.GetLoansEndPoint(), content);
                
               
                //Проверяю статус код ответа
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic parsedResponse = JsonConvert.DeserializeObject(responseContent);
                    
                    //Проверяю параметр name
                    Assert.AreEqual("OK", parsedResponse.message.ToString());
                   

                   
                    pAssert.CheckIsNumeric(parsedResponse.status);
                    pAssert.CheckIsString(parsedResponse.message);
                    pAssert.CheckIsNumeric(parsedResponse.data.loan_sum);
                    Assert.AreEqual(200, (int)parsedResponse.status);
                   
                   


                    Console.WriteLine(parsedResponse);
             

                }


            }
        } // Конец метода

        [TestMethod]
        public async Task TestNotSuccessCheckByIIN1() //Проверка что сервис отвечает с ошибкой если обязательный параметр имеет неверный тип данных
        {

            using (HttpClient client = new HttpClient())
            {
                //Создаю тело запроса
                var RequestBody = new
                {
                    iin = 122222222222,
                    term_id = "204204"
                };

                var json = JsonConvert.SerializeObject(RequestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //Отправля Post запрос с телом запроса 
                client.DefaultRequestHeaders.Add("apiKey", "a785d625-d57b-4a1a-9ef4-8b83568dd228");
                HttpResponseMessage response = await client.PostAsync(Colvir.GetLoansEndPoint(), content);

                //Проверяю статус код ответа
                Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic parsedResponse = JsonConvert.DeserializeObject(responseContent);

                  
                    Assert.AreNotEqual("OK", parsedResponse.message.ToString());
                    Assert.AreNotEqual(200, (int)parsedResponse.status);
                    Console.WriteLine(parsedResponse);


                }


            }
        } // Конец метода
        [TestMethod]
        public async Task TestNotSuccessCheckByIIN2() //Проверка что сервис отвечает с ошибкой если отсутстует обязательный параметр
        {

            using (HttpClient client = new HttpClient())
            {
                //Создаю тело запроса
                var RequestBody = new
                {
                    iin = "111111111111"
                    
                };

                var json = JsonConvert.SerializeObject(RequestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //Отправля Post запрос с телом запроса 
                client.DefaultRequestHeaders.Add("apiKey", "a785d625-d57b-4a1a-9ef4-8b83568dd228");
                HttpResponseMessage response = await client.PostAsync(Colvir.GetLoansEndPoint(), content);

                //Проверяю статус код ответа
                Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic parsedResponse = JsonConvert.DeserializeObject(responseContent);

                   
                    Assert.AreNotEqual("OK", parsedResponse.message.ToString());
                    Assert.AreNotEqual(200, (int)parsedResponse.status);
                    Console.WriteLine(parsedResponse);


                }


            }
        } // Конец метода

        [TestMethod]
        public async Task ПроверкаЧтоСервисОтветчаетОшибкойВслучаеЕслиОтсутствуетApiKey()
        {

            using (HttpClient client = new HttpClient())
            {
                //Создаю тело запроса
                var RequestBody = new
                {
                    iin = "111111111111",
                    term_id = "204204"
                };

                var json = JsonConvert.SerializeObject(RequestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //Отправля Post запрос с телом запроса 
              //  client.DefaultRequestHeaders.Add("apiKey", "a785d625-d57b-4a1a-9ef4-8b83568dd228");
                HttpResponseMessage response = await client.PostAsync(Colvir.GetLoansEndPoint(), content);

                //Проверяю статус код ответа
                Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic parsedResponse = JsonConvert.DeserializeObject(responseContent);

                    
                    Assert.AreNotEqual("OK", parsedResponse.message.ToString());
                    Assert.AreNotEqual(200, (int)parsedResponse.status);
                    Console.WriteLine(parsedResponse);
                    


                }


            }
        } // Конец метода

        [TestMethod]
        public async Task ПроверкаЧтоСервисОтветчаетОшибкойВслучаеЕслиApiKeyНекорректный()
        {

            using (HttpClient client = new HttpClient())
            {
                //Создаю тело запроса
                var RequestBody = new
                {
                    iin = "111111111111",
                    term_id = "204204"
                };

                var json = JsonConvert.SerializeObject(RequestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                //Отправля Post запрос с телом запроса 
                client.DefaultRequestHeaders.Add("apiKey", "we2385d625-d57b-4a1a-9ef4-8b83568dd228"); //Добавляю неверный ApiKey
                HttpResponseMessage response = await client.PostAsync(Colvir.GetLoansEndPoint(), content);

                //Проверяю статус код ответа
                Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic parsedResponse = JsonConvert.DeserializeObject(responseContent);

                   
                    Assert.AreNotEqual("OK", parsedResponse.message.ToString());
                    Assert.AreNotEqual(200, (int)parsedResponse.status);
                    Console.WriteLine(parsedResponse);


                }


            }
        } // Конец метода



    }
}
