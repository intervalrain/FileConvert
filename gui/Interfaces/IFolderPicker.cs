namespace gui.Interfaces;

public interface IFolderPicker
{
	Task<string> PickFolder();
}
