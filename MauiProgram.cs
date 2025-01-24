using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ObenApp.Handlers;
using ObenApp.Interfaces;
using ObenApp.Services;
using ObenApp.Views;
using Plugin.Maui.Audio;

namespace ObenApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureMauiHandlers(handlers =>
                {
                    handlers.AddHandler(typeof(Entry), typeof(CustomEntryHandler));
                })
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<IAudioManager>(AudioManager.Current);
            builder.Services.AddSingleton<IProfileAdministrationService, ProfileAdministrationService>();
            builder.Services.AddSingleton<ICreatePositionAndBlockService, CreatePositionAndBlockService>();
            builder.Services.AddTransient<ProfileAdministration>();
            builder.Services.AddTransient<CreatePositionAndBlock>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
