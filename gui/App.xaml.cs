namespace gui;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();
	}

    protected override Window CreateWindow(IActivationState activationState)
    {
		var window = base.CreateWindow(activationState);
		window.Title = "KIT File Converter";
		const int width = 480;
		const int height = 640;

		window.Width = width;
		window.MaximumWidth = width;
		window.MinimumWidth = width;

		window.Height = height;
		window.MaximumHeight = height;
		window.MinimumHeight = height;

		return window;
    }
}

