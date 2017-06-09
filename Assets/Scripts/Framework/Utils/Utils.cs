using System;
    





/// <summary>
/// 工具类 静态类 ，放置那些可以抽离的方法，最好为静态方法，最好不存储数据
/// </summary>
public class Utils
{
    /// <summary>
    /// 是否可以转换成int ，否则返回 0
    /// </summary>
    /// <returns>The int.</returns>
    /// <param name="value">Value.</param>
    static public int GetInt(object value)
    {
        int parseValue = 0;
        int.TryParse(value.ToString(), out parseValue);
        return parseValue;
    }
    /// <summary>
    /// 是否可以转换成 float，否则返回 0
    /// </summary>
    /// <returns>The float.</returns>
    /// <param name="value">Value.</param>
    static public float GetFloat(object value)
    {
        float parseValue = 0;
        float.TryParse(value.ToString(), out parseValue);
        return parseValue;
    }
    /// <summary>
    /// 是否可以转换成string 否则返回""
    /// </summary>
    /// <returns>The string.</returns>
    /// <param name="value">Value.</param>
    static public string GetString(object value)
    {
        if (value == null)
        {
            return "";
        }
        return value.ToString();
    }

}


