using System.Collections.Generic;
public class stat_missionpatternsRow
{
    public int id;
    public int type;
    public int time;
    public string name;
    public string description;
    public string spheres;
    public string users;
}
/// <summary>
/// Auto Generated By ZJRTool, Do Not Modify
/// </summary>
public class stat_missionpatterns : DatabaseSerializer
{
    private static stat_missionpatterns mInstance;
    public List<stat_missionpatternsRow> rowList = new List<stat_missionpatternsRow>();
    public static stat_missionpatterns GetInstance()
    {
        return mInstance ?? (mInstance = new stat_missionpatterns());
    }
    protected override void AddRow(object[] rowInfo)
    {
        stat_missionpatternsRow row = new stat_missionpatternsRow();
        row.id = GetInt(rowInfo[0]);
        row.type = GetInt(rowInfo[1]);
        row.time = GetInt(rowInfo[2]);
        row.name = GetString(rowInfo[3]);
        row.description = GetString(rowInfo[4]);
        row.spheres = GetString(rowInfo[5]);
        row.users = GetString(rowInfo[6]);
        rowList.Add(row);
    }
}
