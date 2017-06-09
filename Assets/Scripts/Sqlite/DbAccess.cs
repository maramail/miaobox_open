using UnityEngine;
using System;
using System.Collections;
using Mono.Data.Sqlite;
using System.IO;
/*
来源
http://www.xuanyusong.com/archives/831
稍微修改了下
*/
public class DbAccess
{
    /// <summary>
    /// 声明一个连接
    /// </summary>
    private SqliteConnection dbConnection;
    /// <summary>
    /// 声明一个操作数据库的命令
    /// </summary>
    private SqliteCommand dbCommand;
    /// <summary>
    /// 声明一个读取结果集的一个或者多个结果流
    /// </summary>
    private SqliteDataReader reader;
    public const string dbInfoname = "info.db";
    public const string dbStatname = "stat.db";

    public DbAccess(string connectionString)
    {
        // 临时放在Plugins下 等有了服务器之后访问服务器数据库
#if UNITY_EDITOR
        string path = Application.dataPath + @"/StreamingAssets/DataBase/";
        string appDBPrefix = "URI=file:";
#elif UNITY_STANDALONE_WIN
        string path = Application.dataPath + "/StreamingAssets/DataBase";
        string appDBPrefix="Data Source=";
#endif
        OpenDB(Path.Combine(appDBPrefix + path, connectionString));
    }

    public DbAccess()
    {
        // 临时放在Plugins下 等有了服务器之后访问服务器数据库
#if UNITY_EDITOR
        string path = Application.dataPath + @"/StreamingAssets/DataBase/";
        string appDBPrefix = "URI=file:";
#elif UNITY_STANDALONE_WIN
        string path = Application.dataPath + "/StreamingAssets/DataBase";
        string appDBPrefix="Data Source=";
#endif
        OpenDB(Path.Combine(appDBPrefix + path, dbInfoname));
    }

    public void OpenDB(string connectionString)
    {
        try
        {
            dbConnection = new SqliteConnection(connectionString);
            dbConnection.Open();
            Debug.LogWarning("Connected to db");
        }
        catch (Exception e)
        {
            string error = e.ToString();
            Debug.LogError(error);
        }
    }

    public void CloseSqlConnection()
    {
        if (dbCommand != null)
        {
            dbCommand.Dispose();
        }
        dbCommand = null;
        if (reader != null)
        {
            reader.Dispose();
        }
        reader = null;
        if (dbConnection != null)
        {
            dbConnection.Close();
        }
        dbConnection = null;
        Debug.Log("Disconnected from db.");
    }
    /// <summary>
    /// 执行查询sqlite 语句操作
    /// </summary>
    /// <returns>The query.</returns>
    /// <param name="sqlQuery">Sql query.</param>
    public SqliteDataReader ExecuteQuery(string sqlQuery)
    {
        dbCommand = dbConnection.CreateCommand();
        dbCommand.CommandText = sqlQuery;
        reader = dbCommand.ExecuteReader();
        return reader;
    }
    /// <summary>
    /// 查询该表的所有数据
    /// </summary>
    /// <returns>The full table.</returns>
    /// <param name="tableName">Table name.</param>
    public SqliteDataReader ReadFullTable(string tableName)
    {
        string query = "SELECT * FROM " + tableName;
        return ExecuteQuery(query);
    }
    /// <summary>
    /// 查询该表的最大ID
    /// </summary>
    /// <param name="tableName"></param>
    /// <returns></returns>
    public SqliteDataReader ReadMaxIDFromTable(string tableName)
    {
        string query = " SELECT MAX(id) FROM " + tableName;
        return ExecuteQuery(query);

    }

    public SqliteDataReader InsertInto(string tableName, string[] values)
    {
        string query = "INSERT INTO " + tableName + " VALUES (" + values[0];
        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + values[i];
        }
        query += ")";
        //Debug.Log(query);
        return ExecuteQuery(query);
    }

    public SqliteDataReader UpdateInto(string tableName, string[] cols, string[] colsvalues, string selectkey, string selectvalue)
    {
        string query = "UPDATE " + tableName + " SET " + cols[0] + " = '" + colsvalues[0];
        for (int i = 1; i < colsvalues.Length; ++i)
        {
            query += "', " + cols[i] + " = '" + colsvalues[i];
        }
        query += "' WHERE " + selectkey + " = " + selectvalue + " ";
        return ExecuteQuery(query);
    }

    public SqliteDataReader Delete(string tableName, string[] cols, string[] colsvalues)
    {
        string query = "DELETE FROM " + tableName + " WHERE " + cols[0] + " = " + colsvalues[0];
        for (int i = 1; i < colsvalues.Length; ++i)
        {
            query += " or " + cols[i] + " = " + colsvalues[i];
        }
        Debug.Log(query);
        return ExecuteQuery(query);
    }

    /// <summary>
    /// 添加表配置
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="cols"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public SqliteDataReader InsertIntoSpecific(string tableName, string[] cols, string[] values)
    {
        string query = StrInsertInto(tableName, cols, values);
        return ExecuteQuery(query);
    }

    /// <summary>
    /// 登录的特殊添加 默认添加users表， 额外添加system
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="cols"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public SqliteDataReader InsertLogin(string tableName, string[] cols, string[] values)
    {
        string query = StrInsertInto(tableName, cols, values);
        query += ";insert into system (userid) select max(id) from info_users";
        return ExecuteQuery(query);
    }

    /// <summary>
    /// 直接sql语句请求
    /// </summary>
    /// <param name="tableName"></param>
    /// <param name="cols"></param>
    /// <param name="values"></param>
    /// <returns></returns>
    public SqliteDataReader SqlRequest(string query)
    {
        return ExecuteQuery(query);
    }

    public string StrInsertInto(string tableName, string[] cols, string[] values)
    {
        if (cols.Length != values.Length)
        {
            throw new SqliteException("columns.Length != values.Length");
        }
        string query = "INSERT INTO " + tableName + "(" + cols[0];
        for (int i = 1; i < cols.Length; ++i)
        {
            query += ", " + cols[i];
        }
        query += ") VALUES (" + values[0];
        for (int i = 1; i < values.Length; ++i)
        {
            query += ", " + "'" + values[i] + "'";
        }
        query += ")";
        return query;
    }

    public SqliteDataReader DeleteContents(string tableName)
    {
        string query = "DELETE FROM " + tableName;
        return ExecuteQuery(query);
    }

    public SqliteDataReader DropTable(string tableName)
    {
        string query = "DROP TABLE " + tableName;
        return ExecuteQuery(query);
    }

    public SqliteDataReader CreateTable(string name, string[] col, string[] colType)
    {
        if (col.Length != colType.Length)
        {
            throw new SqliteException("columns.Length != colType.Length");
        }
        string query = "CREATE TABLE " + name + " (" + col[0] + " " + colType[0];
        for (int i = 1; i < col.Length; ++i)
        {
            query += ", " + col[i] + " " + colType[i];
        }
        query += ")";
        return ExecuteQuery(query);
    }

    public SqliteDataReader SelectWhere(string tableName)
    {
        string query = "SELECT *  FROM " + tableName;
        return ExecuteQuery(query);
    }

    public SqliteDataReader SelectWhere(string tableName, string[] items, string[] col, string[] operation, string[] values)
    {
        if (col.Length != operation.Length || operation.Length != values.Length)
        {
            throw new SqliteException("col.Length != operation.Length != values.Length");
        }
        string query = "SELECT " + items[0];
        for (int i = 1; i < items.Length; ++i)
        {
            query += ", " + items[i];
        }
        query += " FROM " + tableName + " WHERE " + col[0] + operation[0] + "'" + values[0] + "' ";
        for (int i = 1; i < col.Length; ++i)
        {
            query += " AND " + col[i] + operation[i] + "'" + values[i] + "'";
        }

        return ExecuteQuery(query);
    }

    //自己添加的,判断表是否存在,因为重复创建会出错
    public bool IsHaveTable(string tableName)
    {
        bool isHave = false;
        //SELECT COUNT(*) FROM sqlite_master where type='table' and name='DBInfo'";
        string query = "SELECT COUNT(*) FROM sqlite_master where type='table' and name='" + tableName.Trim() + "' ";

        SqliteCommand mDbCmd = dbConnection.CreateCommand();
        mDbCmd.CommandText = query;
        if (0 < Convert.ToInt32(mDbCmd.ExecuteScalar()))
        {
            isHave = true;
        }
        return isHave;
    }
}