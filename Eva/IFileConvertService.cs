using System;
namespace Eva;

public interface IFileConvertService
{
	public void New();

	public void Open(string path);

	public void Save();
}

