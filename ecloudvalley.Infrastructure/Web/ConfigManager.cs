using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// 參考 appsettings.json 格式建立
/// </summary>
public class ConfigManager
{
    public SQLSettings SQLSettings { get; set; }
}

/// <summary>
/// 資料庫連線設定
/// </summary>
public class SQLSettings
{
    /// <summary>
    /// 資料庫的連線
    /// </summary>
    public string TestConnectionStrings { get; set; }
}