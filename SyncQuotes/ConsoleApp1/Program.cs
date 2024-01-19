// See https://aka.ms/new-console-template for more information
using StoreCoinMarket;




try
{
    Console.WriteLine("Begin");


    ManageCoins mc = new ManageCoins();

    mc.InsertCoin("BTC");
    mc.InsertCoin("ETH");
    mc.InsertCoin("DOGE");
    mc.InsertCoin("SOL");

}
catch(Exception ex)
{
    Console.WriteLine(ex.Message);
}
Console.WriteLine("2");