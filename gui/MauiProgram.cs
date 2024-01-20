using Eva;
using gui.Interfaces;

namespace gui;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});
		builder.Services.AddSingleton(DeviceInfo.Current);
#if MACCATALYST
		builder.Services.AddSingleton<IFolderPicker, Platforms.MacCatalyst.FolderPicker>();
#endif 
		builder.Services.AddSingleton<IRepository, InMemoryRepository>();
        builder.Services.AddSingleton<IFileConvert, FileConvert>();
        builder.Services.AddSingleton<IFileConvertService, FileConvertService>();
		builder.Services.AddSingleton<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();
		return builder.Build();
	}
}

