using System;
namespace Eva;

public class TimeOption
{
	private const string DefaultDateTimeFormat = "yyyy-MM-dd hh:mm:ss";

    public static TimeOption Default => new TimeOption(DefaultDateTimeFormat);

	public string DateTimeFormat { get; private set; }

    public TimeOption(string DateTimeFormat)
	{
		this.DateTimeFormat = DateTimeFormat;
	}
}
