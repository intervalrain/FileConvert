using CommunityToolkit.Maui.Views;

namespace gui;

public partial class About : Popup
{
	public About()
	{
		InitializeComponent();
		var mainDisplayInfo = DeviceDisplay.Current.MainDisplayInfo;
        Size = new Size(Math.Min(500, mainDisplayInfo.Width / mainDisplayInfo.Density), Math.Min(500, mainDisplayInfo.Height / mainDisplayInfo.Density));
    }
}
