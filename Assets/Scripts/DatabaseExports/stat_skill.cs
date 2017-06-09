using System.Collections.Generic;
public class stat_skillRow
{
    public int id;
    public string name;
    public string description;
    public string type;
    public int cast;
    public int volumn;
    public int time;
    public int animation;
}
/// <summary>
/// Auto Generated By ZJRTool, Do Not Modify
/// </summary>
public class stat_skill : DatabaseSerializer
{
    private static stat_skill mInstance;
    public List<stat_skillRow> rowList = new List<stat_skillRow>();
    public static stat_skill GetInstance()
    {
        return mInstance ?? (mInstance = new stat_skill());
    }
    protected override void AddRow(object[] rowInfo)
    {
        stat_skillRow row = new stat_skillRow();
        row.id = GetInt(rowInfo[0]);
        row.name = GetString(rowInfo[1]);
        row.description = GetString(rowInfo[2]);
        row.type = GetString(rowInfo[3]);
        row.cast = GetInt(rowInfo[4]);
        row.volumn = GetInt(rowInfo[5]);
        row.time = GetInt(rowInfo[6]);
        row.animation = GetInt(rowInfo[7]);
        rowList.Add(row);
    }
}