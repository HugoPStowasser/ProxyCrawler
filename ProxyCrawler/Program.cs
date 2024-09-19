using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace ProxyCrawler
{
    public class Proxy
    {
        public string Protocol { get; set; }
        public string IP { get; set; }
        public string Port { get; set; }
        public string Code { get; set; }
        public string Country { get; set; }
        public string Anonymity { get; set; }
        public bool Https { get; set; }
        public string Latency { get; set; }
        public string LastChecked { get; set; }
    }

    class Program
    {
        static async Task Main(string[] args)
        {
            var options = new ChromeOptions();
            //options.AddArgument("--headless"); // Executar o navegador em modo headless (sem GUI)
            using var driver = new ChromeDriver(options);
            driver.Navigate().GoToUrl("https://pt-br.proxyscrape.com/lista-de-procuradores-gratuitos");

            // Aguarde a tabela ser carregada (você pode ajustar conforme a estrutura da página)
            var proxyRows = driver.FindElements(By.CssSelector("table tbody tr"));

            var proxies = new List<Proxy>();

            foreach (var row in proxyRows)
            {
                var columns = row.FindElements(By.TagName("td"));
                if (columns.Count > 0)
                {
                    var httpsCell = columns[6];
                    var httpsIconPresent = false;

                    var svgElements = httpsCell.FindElements(By.TagName("svg"));
                    foreach (var svg in svgElements)
                    {
                        var width = svg.GetAttribute("width");
                        if (width == "22") // Se o SVG possui largura 22 o https é verdadeiro
                        {
                            httpsIconPresent = true;
                            break;
                        }
                    }

                    var proxy = new Proxy
                    {
                        Protocol = columns[0].Text,
                        IP = columns[1].Text,
                        Port = columns[2].Text,
                        Code = columns[3].Text,
                        Country = columns[4].Text,
                        Anonymity = columns[5].Text,
                        Https = httpsIconPresent,
                        Latency = columns[7].Text,
                        LastChecked = columns[8].Text
                    };
                    proxies.Add(proxy);
                }
            }

            Console.WriteLine(proxies);

            driver.Quit();

            using var httpClient = new HttpClient();
            var response = await httpClient.PostAsJsonAsync("https://localhost:7203/api/proxy", proxies);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("Dados enviados com sucesso!");
            }
            else
            {
                Console.WriteLine("Erro ao enviar os dados: " + response.StatusCode);
            }
        }
    }
}
