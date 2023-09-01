using System;
using System.Threading;
using Microsoft.Extensions.Hosting;

namespace CreditorGuard.Web;

public static class Program
{
    public static int Main(string[] args)
    {
        try
        {
            CreateWebHostBuilder(args)
                .Build()
                .Run();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine(e.StackTrace);

        }
        finally
        {

            Thread.Sleep(1000);

        }

        return 0;
    }

    public static IHostBuilder CreateWebHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args);
    }
}