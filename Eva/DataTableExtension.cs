using System.Data;
using System.Text;

namespace Eva;

public static class DataTableExtension
{
	public static string ToCSV(this DataTable dt, TimeOption? option = null)
	{
		if (option == null) option = TimeOption.Default;
		var sb = new StringBuilder();
		var colNames = dt.Columns.Cast<DataColumn>().Select(col => col.ColumnName);
		sb.AppendLine(string.Join(',', colNames));

		foreach (DataRow row in dt.Rows)
		{
			var fields = row.ItemArray.Select(f =>
			{
				string text;
				if (f is DateTime)
				{
					text = DateTime.Parse(Convert.ToString(f)!).ToString(option.DateTimeFormat);
                }
				else
				{
                    text = Convert.ToString(f)!;
                }
				text = string.Concat("\"", text?.Replace("\"", "\"\""), "\"");
				return text;
            });
			sb.AppendLine(string.Join(',', fields));
		}
		return sb.ToString();
	}
}
