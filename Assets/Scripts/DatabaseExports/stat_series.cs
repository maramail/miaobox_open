using System.Collections.Generic;
public class stat_seriesRow
{
    public int id;
    public string name;
}
/// <summary>
/// Auto Generated By ZJRTool, Do Not Modify
/// </summary>
public class stat_series : DatabaseSerializer
{
    private static stat_series mInstance;
    public List<stat_seriesRow> rowList = new List<stat_seriesRow>();
    public static stat_series GetInstance()
    {
        return mInstance ?? (mInstance = new stat_series());
    }
    protected override void AddRow(object[] rowInfo)
    {
        stat_seriesRow row = new stat_seriesRow();
        row.id = GetInt(rowInfo[0]);
        row.name = GetString(rowInfo[1]);
        rowList.Add(row);
    }
}
