using Autofac;
using Autofac.Extensions.DependencyInjection;
using chessAPI;
using chessAPI.business.interfaces;
using chessAPI.models.game;
using chessAPI.models.player;
using Microsoft.AspNetCore.Authorization;
using Serilog;
using Serilog.Events;

//Serilog logger (https://github.com/serilog/serilog-aspnetcore)
Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateBootstrapLogger();

try
{
    Log.Information("chessAPI starting");
    var builder = WebApplication.CreateBuilder(args);

    var connectionStrings = new connectionStrings();
    builder.Services.AddOptions();
    builder.Services.Configure<connectionStrings>(builder.Configuration.GetSection("ConnectionStrings"));
    builder.Configuration.GetSection("ConnectionStrings").Bind(connectionStrings);

    // Two-stage initialization (https://github.com/serilog/serilog-aspnetcore)
    builder.Host.UseSerilog((context, services, configuration) => configuration.ReadFrom
             .Configuration(context.Configuration)
             .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning).ReadFrom
             .Services(services).Enrich
             .FromLogContext().WriteTo
             .Console());

    // Autofac como inyección de dependencias
    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
    builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.RegisterModule(new chessAPI.dependencyInjection<int, int>()));
    var app = builder.Build();
    app.UseSerilogRequestLogging();
    app.UseMiddleware(typeof(chessAPI.customMiddleware<int>));

    #region "REST routes"
    app.MapGet("/", () => Results.BadRequest());

    #region "Player REST Commands"
    app.MapGet("game/{id}", async (IGameBusiness bs, long id) =>
    {
        var x = await bs.getGame(id).ConfigureAwait(false);
        return x != null ? Results.Ok(x) : Results.NotFound();
    });

    app.MapPost("player",
    [AllowAnonymous] async (IPlayerBusiness<int> bs, clsNewPlayer newPlayer)
        => Results.Ok(await bs.addPlayer(newPlayer).ConfigureAwait(false)));
    #endregion

    #region "Game REST Commands"
    app.MapPost("game",
    [AllowAnonymous] async (IGameBusiness bs, clsNewGame newGame) =>
    {
        await bs.startGame(newGame).ConfigureAwait(false);
        return Results.Ok();
    });
    app.MapPut("/game/{id}/swapturn",
    [AllowAnonymous] async (IGameBusiness bs, long id) =>
    {
        var didSwap = await bs.swapTurn(id).ConfigureAwait(false);
        return didSwap ? Results.Ok() : Results.BadRequest();
    });
    #endregion
    #endregion

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "chessAPI terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
