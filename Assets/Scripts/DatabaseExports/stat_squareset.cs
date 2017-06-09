using System.Collections.Generic;
public class stat_squaresetRow
{
    public int id;
    public string name;
    public int needlv;
    public string description;
    public int season;
}
/// <summary>
/// Auto Generated By ZJRTool, Do Not Modify
/// </summary>
public class stat_squareset : DatabaseSerializer
{
    private static stat_squareset mInstance;
    public List<stat_squaresetRow> rowList = new List<stat_squaresetRow>();
    public static stat_squareset GetInstance()
    {
        return mInstance ?? (mInstance = new stat_squareset());
    }
    protected override void AddRow(object[] rowInfo)
    {
        stat_squaresetRow row = new stat_squaresetRow();
        row.id = GetInt(rowInfo[0]);
        row.name = GetString(rowInfo[1]);
        row.needlv = GetInt(rowInfo[2]);
        row.description = GetString(rowInfo[3]);
        row.season = GetInt(rowInfo[4]);
        rowList.Add(row);
    }
}
