using System.Data;
using System.Text;
using Newtonsoft.Json.Linq;
using Eva.Models;

namespace Eva;

public class FileConvert : IFileConvert
{
    private string root;

    private readonly string src = "Download/";
    private readonly string edit = "Questions/";
    private readonly string output = "Converted/";

    private DataTable table;
    private Dictionary<string, int> columns;
    private Dictionary<string, int> columnOrd;
    private IRepository Repository;

    private string[] files = new string[]
    {
        "111001",
        "111002",
        "111003",
        "111004",
        "111006",
        "111007",
        "111008"
    };

    public FileConvert(IRepository repository)
    {
        Repository = repository;
    }

    public void Read(string root)
    {
        this.root = root;
        foreach (var file in files)
        {
            Init(file);
            Prepare(file);
            var result = ReadData(file);
            Repository.Save(file, result);
        }
    }

    private void Init(string file)
    {
        table = new DataTable(file);
        table.Columns.Add("Sample_id", typeof(string));
        table.Columns.Add("time", typeof(DateTime));

        columns = new Dictionary<string, int>();
    }

    private void Prepare(string file)
    {
        string editPath = root + edit + file + ".csv";
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var reader = new StreamReader(editPath, Encoding.GetEncoding(950));
        reader.ReadLine();
        string note = string.Empty;
        while (reader.Peek() != -1)
        {
            var line = reader.ReadLine()!;
            var arr = line.Split(',');
            var model = new KitModel(
                Id: Convert.ToInt32(arr[0]),
                Sid: Convert.ToInt32(arr[1]),
                Text: arr[2],
                Type: (KitType)Convert.ToInt32(arr[3]),
                OptText: arr[4],
                OptValue: arr[5],
                Annotation: arr[6],
                Note: arr[7]
            );
            var column = "Q" + model.Id + "-" + model.Sid;
            if (!string.IsNullOrEmpty(note) && !string.Equals(note, column))
            {
                AddColumn(note + "-a", typeof(string));
                note = string.Empty;
            }

            switch (model.Type)
            {
                case KitType.Single:
                    AddColumn(column, typeof(int));
                    break;

                case KitType.Multi:
                    AddColumn(column, typeof(int), multi: true);
                    break;

                case KitType.Number:
                    AddColumn(column, typeof(int));
                    break;

                case KitType.String:
                    AddColumn(column, typeof(string));
                    break;

                case KitType.Time:
                    AddColumn(column, typeof(string));
                    break;

                case KitType.Location:
                    AddColumn(column + "-city", typeof(string));
                    AddColumn(column + "-town", typeof(string));
                    break;

                case KitType.Set:
                case KitType.NextPage:
                    break;
                default:
                    throw new InvalidOperationException();
            }
            if (model.Note == "1") note = column;
        }

        columnOrd = table.Columns.Cast<DataColumn>().ToDictionary(col => col.ColumnName, col => col.Ordinal);
    }

    private string ReadData(string file)
    {
        string srcPath = root + src + file + "final.txt"; 
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        using var reader = new StreamReader(srcPath, Encoding.UTF8);
        reader.ReadLine();
        while (reader.Peek() != -1)
        {
            var line = reader.ReadLine()!;
            var map = new Dictionary<string, object>();
            int len = line.IndexOf('{');
            if (len == -1) continue; // data is null
            var info = line[..len].Split(',');
            var content = line[len..];
            content = Format(content);

            var model = new surveyModel(
                RecordId: Convert.ToInt32(info[0]),
                SampleId: info[1],
                time: Convert.ToDateTime(info[2]),
                Record: info[3],
                Remark: info[4]);
            var jobj = JObject.Parse(content);
            foreach (var prop in jobj)
            {
                string qid = "Q" + prop.Key;
                if (prop.Value.HasValues)
                {
                    var hashSet = prop.Value.Select(v => Convert.ToInt32(v)).ToHashSet();
                    int cnt = columns[qid];
                    for (int i = 1; i <= cnt; i++)
                    {
                        map.Add($"Q{prop.Key}-{i}", hashSet.Contains(i) ? 1 : 0);
                    }
                }
                else if (qid.Count(x => x == '-') > 1 && int.TryParse(qid.Split('-')[2], out _))
                {
                    var arr = qid.Split('-');
                    arr[2] = "a";
                    qid = string.Join('-', arr);
                    map.Add(qid, prop.Value);
                }
                else 
                {
                    map.Add(qid, prop.Value);
                }
            }

            var row = table.NewRow();
            row[0] = model.SampleId;
            row[1] = model.time;
            foreach (var kvp in map)
            {
                string colName = kvp.Key;
                object value = kvp.Value;
                int index = columnOrd[colName];
                row[index] = value;
            }
            table.Rows.Add(row);
        }
        return table.ToCSV();
    }

    public void Write()
    {
        foreach (var file in files)
        {
            string outPath = root + output + file + ".csv";
            string text = Repository.GetOutputById(file);
            using var writer = new StreamWriter(outPath);
            writer.Write(text);
        }
    }

    public void Reset()
    {
        root = string.Empty;
        table.Dispose();
        columns.Clear();
    }

    private void AddColumn(string column, Type type, bool multi = false)
    {
        if (multi)
        {
            if (!columns.ContainsKey(column))
            {
                columns.Add(column, 1);
            }
            else
            {
                columns[column]++;
            }
            string columnName = column + "-" + columns[column];
            table.Columns.Add(columnName, type);
        }
        else
        {
            if (!columns.ContainsKey(column))
            {
                columns.Add(column, 1);
                table.Columns.Add(column, type);
            }
        }
    }

    private string Format(string text)
    {
        text = text.Replace("},{", ",");
        text = text.Replace(",", "，");
        text = text.Replace("\"，\"", "\",\"");  //","
        text = text.Replace("]，[", "],[");      //],[
        text = text.Replace("\"，[", "\",[");    //",[
        text = text.Replace("]，\"", "],\"");    //],"

        return text;
    }
}
