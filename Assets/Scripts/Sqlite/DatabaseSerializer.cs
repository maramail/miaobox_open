/********************************************************************
	created:	2016/08/18
	created:	18:8:2016   22:21
	filename: 	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\sqlite\databaseserializer.cs
	file path:	f:\users\administrator\projects\miaobox\miaobox\miaoboxmvc\assets\scripts\sqlite
	file base:	databaseserializer
	file ext:	cs
	author:		Zhou Jingren
	
	purpose:	读取数据库配置 并序列化class
*********************************************************************/
using UnityEngine;
using System.Collections;
using Mono.Data.Sqlite;

public class DatabaseSerializer
{
    public void ReadData(DbAccess dbAccess)
    {
        SqliteDataReader reader = dbAccess.ReadFullTable(GetType().ToString());
        while(reader.Read())
        {
            object[] objects = new object[reader.FieldCount];
            for(int i = 0; i < reader.FieldCount; i++)
            {
                objects[i] = reader[i];
            }
            AddRow(objects);
        }
    }
    protected virtual void AddRow(object[] rowInfo)
    {

    }
    public int GetInt(object value)
    {
        int parseValue = 0;
        int.TryParse(value.ToString(), out parseValue);
        return parseValue;
    }
    public float GetFloat(object value)
    {
        float parseValue = 0;
        float.TryParse(value.ToString(), out parseValue);
        return parseValue;
    }
    public string GetString(object value)
    {
        if (value == null)
        {
            return "";
        }
        return value.ToString();
    }
}
