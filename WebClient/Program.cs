// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var baseUrl = "https://localhost:7133/api/";

var client = new HttpClient();



var data = client.GetStringAsync(baseUrl + "todo/list").Result;
Console.WriteLine(data);
Console.ReadKey();

data = client.GetStringAsync(baseUrl + "todo/List?Title=string").Result;
Console.WriteLine(data);
Console.ReadKey();


data = client.GetStringAsync(baseUrl + "Todo/2").Result;
Console.WriteLine(data);
Console.ReadKey();

data = client.GetStringAsync(baseUrl + "Todo/5").Result;
Console.WriteLine(data);
Console.ReadKey();




