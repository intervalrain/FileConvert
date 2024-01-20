using System;
namespace Eva;

public class InMemoryRepository : IRepository
{
	private Dictionary<string, string> memory;

	public InMemoryRepository()
	{
		memory = new Dictionary<string, string>();
	}

    public string GetOutputById(string id)
    {
        if (!memory.ContainsKey(id)) throw new KeyNotFoundException();
        return memory[id];
    }

    public void Save(string id, string content)
    {
        if (!memory.ContainsKey(id))
        {
            memory.Add(id, content);
        }
    }
}
