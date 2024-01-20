using System.Text;
using System.Windows.Input;

namespace gui;

public partial class MainPage : ContentPage
{
	ICommand OpenPaper;
	public MainPage(MainPageViewModel mainPageViewModel, IDeviceInfo deviceInfo)
	{
		InitializeComponent();
		if (deviceInfo.Platform == DevicePlatform.MacCatalyst)
		{
			AddToolbarItem("New", mainPageViewModel.NewCommand);
			AddToolbarItem("Open", mainPageViewModel.OpenCommand);
			AddToolbarItem("Save", mainPageViewModel.SaveCommand);
            AddToolbarItem("About", mainPageViewModel.AboutCommand);
            AddToolbarItem("Help", mainPageViewModel.HelpCommand);
            AddToolbarItem("ToggleTheme", mainPageViewModel.ToggleThemeCommand);
            AddToolbarItem("Quit", mainPageViewModel.QuitCommand);
            OpenPaper = mainPageViewModel.OpenPaperCommand;
		}
	}

	private void OnClicked(object sender, EventArgs e)
	{
		OpenPaper.Execute(null);
	}

	private void AddToolbarItem(string text, ICommand command)
	{
		var toolbarItem = new ToolbarItem()
		{
			Text = text,
			Command = command
		};
		ToolbarItems.Add(toolbarItem);
	}
}


