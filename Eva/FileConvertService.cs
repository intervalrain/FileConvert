using System;
namespace Eva;

public class FileConvertService : IFileConvertService
{
	private IFileConvert fc;

	public FileConvertService(IFileConvert fileConvert)
	{
		fc = fileConvert;
	}

	public void New()
	{
		fc.Reset();
	}

	public void Open(string path)
	{
		fc.Read(path);
	}

	public void Save()
	{
		fc.Write();
	}
}
