using System.Collections.Generic;
public class system_catgroupRow
{
    public int id;
    public string name;
}
/// <summary>
/// Auto Generated By ZJRTool, Do Not Modify
/// </summary>
public class system_catgroup : DatabaseSerializer
{
    private static system_catgroup mInstance;
    public List<system_catgroupRow> rowList = new List<system_catgroupRow>();
    public static system_catgroup GetInstance()
    {
        return mInstance ?? (mInstance = new system_catgroup());
    }
    protected override void AddRow(object[] rowInfo)
    {
        system_catgroupRow row = new system_catgroupRow();
        row.id = GetInt(rowInfo[0]);
        row.name = GetString(rowInfo[1]);
        rowList.Add(row);
    }
}
