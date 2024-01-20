using System;
namespace Eva;

public interface IRepository
{
	public string GetOutputById(string id);

	public void Save(string file, string context);
}
