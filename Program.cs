using ContactManagerWebApi.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ContactManagerWebApi;

/// <summary>
/// Handles the starting of the app (by default with the web server Kestrel).
/// </summary>
public class Program
{
    public static IHostBuilder CreateHostBuilder(string[] args)
        => Host
            .CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());

    public static void Main(string[] args)
        => CreateHostBuilder(args).Build().Run();
}