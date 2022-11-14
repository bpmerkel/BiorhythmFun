// <copyright file="Program.cs" company="Brady P. Merkel">
// Copyright (c) Brady P. Merkel. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BiorhythmFun.Client;
using MatBlazor;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddMatBlazor();
builder.Services.AddBlazoredLocalStorage();
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var app = builder.Build();
await app.RunAsync();
