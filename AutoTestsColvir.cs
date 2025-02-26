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
        static String customerNameEndpoint = ColvirUrl + "/ekassir/client/detail/list";
        public String GetColvirUrl()
        {
            return ColvirUrl;
        }
        public String GetCustomerNameEndPoint()
        {
            return customerNameEndpoint;
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
    public sealed class LoansTests
    {
        Colvir Colvir = new Colvir();
        CustomAsserts pAssert = new CustomAsserts();



        [TestMethod]
        [TestCategory("POST /ekassir/client/loans")]
        public async Task TestSuccessLoans()
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
        [TestCategory("POST /ekassir/client/loans")]
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
        [TestCategory("POST /ekassir/client/loans")]
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
        [TestCategory("POST /ekassir/client/loans")]
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
        [TestCategory("POST /ekassir/client/loans")]
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
    [TestClass]
    public sealed class accListTests
    {
        Colvir Colvir = new Colvir();
        CustomAsserts pAssert = new CustomAsserts();



        [TestMethod]
        [TestCategory("POST /ekassir/client/accList")]
        public async Task TestSuccessAccList()
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
                HttpResponseMessage response = await client.PostAsync(Colvir.GetaccListEndpoint(), content);


                //Проверяю статус код ответа
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic parsedResponse = JsonConvert.DeserializeObject(responseContent);


                    Assert.AreEqual("OK", parsedResponse.message.ToString());


                    //Проверка типа полученных данных в ответе
                    pAssert.CheckIsNumeric(parsedResponse.status);
                    pAssert.CheckIsString(parsedResponse.message);
                    pAssert.CheckIsString(parsedResponse.code);
                    pAssert.CheckIsString(parsedResponse.request_id);
                    pAssert.CheckIsString(parsedResponse.version);
                    pAssert.CheckIsString(parsedResponse.timestamp);
                    pAssert.CheckIsString(parsedResponse.request_url);
                    Assert.AreEqual(200, (int)parsedResponse.status);




                    Console.WriteLine(parsedResponse);


                }


            }
        } // Конец метода

        [TestMethod]
        [TestCategory("POST /ekassir/client/accList")]
        public async Task TestNotAuthAccList()
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
                client.DefaultRequestHeaders.Add("apiKey", "B785d625-d57b-4a1a-9ef4-8b83568dd228");
                HttpResponseMessage response = await client.PostAsync(Colvir.GetaccListEndpoint(), content);


                //Проверяю статус код ответа
                Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic parsedResponse = JsonConvert.DeserializeObject(responseContent);
                    Assert.AreEqual(401, (int)parsedResponse.status, "Ожидаемый код не 401");

                    Assert.AreEqual("Not Autrozation", (String)parsedResponse.message, "Ожидаемое сообщение не соответствует.");
                    Console.WriteLine(parsedResponse);
                }



            }
        } // Конец метода

        [TestMethod]
        [TestCategory("POST /ekassir/client/accList")]
        public async Task TestNotSuccessAccList()
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
                HttpResponseMessage response = await client.PostAsync(Colvir.GetaccListEndpoint(), content);


                //Проверяю статус код ответа
                Assert.AreNotEqual(HttpStatusCode.OK, response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic parsedResponse = JsonConvert.DeserializeObject(responseContent);


                    Assert.AreNotEqual("OK", parsedResponse.message.ToString());


                }


            }
        } // Конец метода
    }

    [TestClass]
    public sealed class DetailListTests
    {
        Colvir Colvir = new Colvir();
        CustomAsserts pAssert = new CustomAsserts();



        [TestMethod]
        [TestCategory("/ekassir/client/detail/list")]
        public async Task TestSuccessDetailList()
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
                HttpResponseMessage response = await client.PostAsync(Colvir.GetCustomerNameEndPoint(), content);


                //Проверяю статус код ответа
                Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
                if (response.IsSuccessStatusCode)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic parsedResponse = JsonConvert.DeserializeObject(responseContent);

                    Assert.AreEqual("OK", parsedResponse.message.ToString());


                    //Проверка типа полученных данных в ответе
                    pAssert.CheckIsNumeric(parsedResponse.status);
                    pAssert.CheckIsString(parsedResponse.message);
                    pAssert.CheckIsString(parsedResponse.code);
                    pAssert.CheckIsString(parsedResponse.request_id);
                    pAssert.CheckIsString(parsedResponse.version);
                    pAssert.CheckIsString(parsedResponse.timestamp);
                    pAssert.CheckIsString(parsedResponse.request_url);
                    Assert.AreEqual(200, (int)parsedResponse.status);




                    Console.WriteLine(parsedResponse);


                }


            }
        } // Конец метода

        [TestMethod]
        [TestCategory("/ekassir/client/detail/list")]
        public async Task TestNotSuccessDetailList()
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
                HttpResponseMessage response = await client.PostAsync(Colvir.GetCustomerNameEndPoint(), content);


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
        [TestCategory("/ekassir/client/detail/list")]
        public async Task TestNotAuthDetailList()
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
                client.DefaultRequestHeaders.Add("apiKey", "B785d625-d57b-4a1a-9ef4-8b83568dd228");
                HttpResponseMessage response = await client.PostAsync(Colvir.GetCustomerNameEndPoint(), content);


                //Проверяю статус код ответа
                Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    dynamic parsedResponse = JsonConvert.DeserializeObject(responseContent);
                    Assert.AreEqual(401, (int)parsedResponse.status);
                    Assert.AreEqual("Not Authorization", (String)parsedResponse.message);


                    




                    Console.WriteLine(parsedResponse);


                }


            }
        } // Конец метода
    }
}

    
