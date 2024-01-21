using CommunityToolkit.Maui.Storage;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Eva;

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
        Application.Current?.MainPage.DisplayAlert("Fileds Clear!", "The files are cleared.", "OK");
    }

    [RelayCommand]
    async Task Open(CancellationToken cancellationToken)
    {
        var result  = await folderPicker.PickAsync(cancellationToken);
        result.EnsureSuccess();
        var root = result.Folder.Path;
        
        if (root is not null)
        {
            fileConvertService.Open(root);
        }

        Application.Current?.MainPage.DisplayAlert("Task Completed!", "The files are completed. Please click 'save'.", "OK");
    }

    [RelayCommand]
    async Task Save(CancellationToken cancellationToken)
    {
        fileConvertService.Save();
        Application.Current?.MainPage.DisplayAlert("Files Saved", "The files are saved.", "OK");
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
    void About()
    {
        var about = new About();
        Application.Current?.MainPage?.ShowPopup(about);
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
