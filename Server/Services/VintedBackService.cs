using Microsoft.OData.ModelBuilder;
using Microsoft.OData.UriParser;
using Microsoft.Playwright;
using static System.Net.WebRequestMethods;

namespace MCrossList.Server.Services
{
    public class VintedBackService
    {
        string chromiumpath = @"chromium\bin\chrome.exe";
        private string site2 = "https://www.vinted.it/member/272115553";
        private string site = "https://www.vinted.it/member/71765100";
        public VintedBackService()
        {

        }

        public async Task<int> GetProducts()
        {
            var pw = await InitializePlaywrigth();
            await using var browser = await pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                ExecutablePath = chromiumpath,
                Headless = false
            });
            var page = await browser.NewPageAsync();

            await page.GotoAsync(site, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });

            int batch = 0;
            int prevCount = 0;
            int newCount = 0;
            while (true)
            {
                var items = page.Locator("xpath=/html/body/div[1]/div/main/div/div/div/div/div/div/div[1]/div/div[3]/div[3]/div[3]/div[1]");
                prevCount = await items.Locator("> div").CountAsync();
                await page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
                await page.WaitForTimeoutAsync(2000); // tempo per caricare

                newCount = await items.Locator("> div").CountAsync();
                if (newCount == prevCount)
                    break; // nessuna nuova immagine, finito

                batch++;
                Console.WriteLine($"Batch {batch}: trovate {newCount} immagini finora");
            }

            return newCount;

        }

        public async Task<int> GetProductsDetails()
        {
            var pw = await InitializePlaywrigth();
            await using var browser = await pw.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                ExecutablePath = chromiumpath,
                Headless = false
            });
            var page = await browser.NewPageAsync();

            await page.GotoAsync(site, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });


            int batch = 0;
            int prevCount = 0;
            int newCount = 0;
            while (true)
            {
                var items = page.Locator("xpath=/html/body/div[1]/div/main/div/div/div/div/div/div/div[1]/div/div[3]/div[3]/div[3]/div[1]");
                prevCount = await items.Locator("> div").CountAsync();

                for (int i = 0; i < prevCount; i++)
                {
                    // prendo il link dell'item
                    var div = items.Locator($"> div:nth-child({i + 1})");
                    var a = div.Locator("a[href]");
                    var href = await a.GetAttributeAsync("href");

                    // apro il link in una nuova pagina
                    var itemPage = await browser.NewPageAsync();
                    await itemPage.GotoAsync("https://www.vinted.it" + href, new PageGotoOptions { WaitUntil = WaitUntilState.NetworkIdle });

                    await itemPage.Locator("xpath=/html/body/div[17]/div[2]/div/div[1]/div/div[2]/div/button[1]").ClickAsync();

                    //Ottengo i dati

                    //Titolo
                    var h1 = itemPage.Locator("xpath=/html/body/div[1]/div/main/div/div/div/div/div/div/main/div[1]/aside/div[2]/div[1]/div/div/div/div/div/div[1]/div[1]/div[1]/h1");
                    var Title = await h1.InnerTextAsync();

                    //Categoria
                    var liCount = await itemPage.Locator("xpath=/html/body/div[1]/div/main/div/div/div/div/div/div/main/div[1]/section/div[2]/div[1]/div/div/ul").Locator("> li").CountAsync();
                    var span = itemPage.Locator($"xpath=/html/body/div[1]/div/main/div/div/div/div/div/div/main/div[1]/section/div[2]/div[1]/div/div/ul/li[{liCount}]/a/span");
                    var Category = await span.InnerTextAsync();
                    
                    
                    
                    int x = 0;
                    // chiudo la pagina
                    await itemPage.CloseAsync();

                }

                await page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight)");
                await page.WaitForTimeoutAsync(2000); // tempo per caricare

                newCount = await items.Locator("> div").CountAsync();
                if (newCount == prevCount)
                    break; // nessuna nuova immagine, finito

                batch++;
                Console.WriteLine($"Batch {batch}: trovate {newCount} immagini finora");
            }

            return 0;
        }

    
        

        private async Task<IPlaywright> InitializePlaywrigth()
        {
            var pw = await Playwright.CreateAsync();
            return pw;
        }
    }
}
