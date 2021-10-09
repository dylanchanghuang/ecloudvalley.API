using System;
using System.Collections.Generic;
using System.Text;

/// <summary>
/// appsettings.json的靜態宣告，可以讓網站直接呼叫用
/// </summary>
public static class ConfigProvider
{
    private static ConfigManager _configManager;

    public static SQLSettings SQLSettings => _configManager.SQLSettings;

    /// <summary>
    /// 搬入靜態
    /// </summary>
    /// <param name="configManager">class ConfigManager</param>
    public static void Configure(ConfigManager configManager)
    {
        _configManager = configManager;
    }
}