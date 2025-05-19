// <copyright file="Program.cs" company="Brady P. Merkel">
// Copyright (c) Brady P. Merkel. All rights reserved.
// </copyright>

/// <summary>
/// Main entry point for the application.
/// </summary>
/// <param name="args">The arguments.</param>
var builder = WebAssemblyHostBuilder.CreateDefault(args);

/// <summary>
/// Adds the root components to the builder.
/// </summary>
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

/// <summary>
/// Adds the Blazored LocalStorage service as a singleton.
/// </summary>
builder.Services.AddBlazoredLocalStorageAsSingleton();

/// <summary>
/// Adds and configures the MudBlazor services.
/// </summary>
/// <param name="config">The configuration for MudBlazor services.</param>
builder.Services.AddMudServices(config =>
{
    config.SnackbarConfiguration.PositionClass = Defaults.Classes.Position.BottomRight;
    config.SnackbarConfiguration.PreventDuplicates = false;
    config.SnackbarConfiguration.NewestOnTop = false;
    config.SnackbarConfiguration.ShowCloseIcon = true;
    config.SnackbarConfiguration.VisibleStateDuration = 5000;
    config.SnackbarConfiguration.HideTransitionDuration = 500;
    config.SnackbarConfiguration.ShowTransitionDuration = 500;
    config.SnackbarConfiguration.SnackbarVariant = Variant.Filled;
});

/// <summary>
/// Configures the animation options.
/// </summary>
/// <param name="options">The animation options.</param>
builder.Services.Configure<AnimateOptions>(options =>
{
    options.Animation = Animations.FadeDown;
    options.Duration = TimeSpan.FromMilliseconds(100);
});

/// <summary>
/// Builds and runs the application.
/// </summary>
var app = builder.Build();
await app.RunAsync();
