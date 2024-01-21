using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Storage;
using Eva;
//using gui.Interfaces;

namespace gui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.UseMauiCommunityToolkit()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton(DeviceInfo.Current);
		builder.Services.AddSingleton(FolderPicker.Default); 
        builder.Services.AddSingleton<IRepository, InMemoryRepository>();
        builder.Services.AddTransient<IFileConvert, FileConvert>();
        builder.Services.AddTransient<IFileConvertService, FileConvertService>();
		builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();
		return builder.Build();
	}
}

