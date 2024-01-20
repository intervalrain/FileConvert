using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eva;
using gui.Interfaces;

namespace gui;

public partial class MainPageViewModel : ObservableObject
{
    private readonly IFileConvertService fileConvertService;
    private readonly IFolderPicker folderPicker; 

    public MainPageViewModel(IFileConvertService fileConvertService, IFolderPicker folderPicker)
    {
        this.fileConvertService = fileConvertService;
        this.folderPicker = folderPicker;
    }

    [RelayCommand]
    void New()
    {
        fileConvertService.New();
    }

    [RelayCommand]
    async Task Open(CancellationToken cancellationToken)
    {
        var root = await folderPicker.PickFolder();
        
        if (root is not null)
        {
            fileConvertService.Open(root);
        }
    }

    [RelayCommand]
    async Task Save(CancellationToken cancellationToken)
    {
        string root = await folderPicker.PickFolder();
        fileConvertService.Save();
    }

    [RelayCommand]
    void Quit()
    {
        Application.Current?.Quit();
    }

    [RelayCommand]
    Task Help()
    {
        return Launcher.OpenAsync("https://intervalrain@gmail.com/");
    }

    [RelayCommand]
    Task About()
    {
        return Launcher.OpenAsync("https://intervalrain@gmail.com/");
    }

    [RelayCommand]
    void ToggleTheme()
    {
        if (Application.Current is not null)
        {
            Application.Current.UserAppTheme = Application.Current.UserAppTheme == AppTheme.Light ? AppTheme.Dark : AppTheme.Light;
        }
    }

    [RelayCommand]
    Task OpenPaper()
    {
        return Launcher.OpenAsync("https://onedrive.live.com/edit?id=B3DC443F55EC1A8C!1465&resid=B3DC443F55EC1A8C!1465&authkey=!ABF5bTS3nC4tu2M&wdPid=407b3f73&wdo=2&cid=b3dc443f55ec1a8c");
    }
}
