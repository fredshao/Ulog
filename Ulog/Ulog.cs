using System;
using UnityEngine;
using System.IO;
using System.Text;

/// <summary>
/// Ulog封装了Unity3D的Debug.Log Debug.LogFormat, Debug.LogError, Debug.LogErrorFormat等，
/// 使其更易用，支持不定数量参数的打印，不再使用麻烦的字符串拼接
/// 使用时，请自行编译成DLL放到工程中Plugins目录下使用
/// 使用：
///     Ulog.Log("aaa",123,"ccc");
///     Ulog.Error("aaa",666,"898",222);
///     Ulog.LogFormat 和 Debug.LogFormat的使用完全一样
///     
/// Update: 2018.01.10
/// Author: FredShao
/// </summary>
public static class Ulog {
    /// <summary>
    /// 打开关闭在Editor里面显示日志
    /// </summary>
    public static bool logEnabled = true;

    /// <summary>
    /// 打开或关闭将日志写到一个文件里
    /// </summary>
    public static bool logWriteEnabled = false;

    /// <summary>
    /// 要将日志写到的文件绝对路径(如果这里未赋值，则无法写入)
    /// </summary>
    public static string logFilePath = string.Empty;

    /// <summary>
    /// 是否捕捉异常，并写到日志文件（Editor下不起作用，因为Editor下本身就可以看到异常）
    /// 使用此功能时 logWriteEnabled 必须为 true
    /// 此功能只建议应用在 Windows 导出版本的调试使用，正式版本不建议使用
    /// </summary>
    private static bool _catchFatalException = false;

    // 是否捕捉异常
    public static bool catchFatalException
    {
        get
        {
            return _catchFatalException;
        }
        set
        {
            _catchFatalException = value;
            if (_catchFatalException)
            {
                if (Application.platform != RuntimePlatform.OSXEditor ||
                    Application.platform != RuntimePlatform.WindowsEditor)
                {
                    Application.logMessageReceived += HandleLog;
                }
            }
            else
            {
                if (Application.platform != RuntimePlatform.OSXEditor ||
                    Application.platform != RuntimePlatform.WindowsEditor)
                {
                    Application.logMessageReceived -= HandleLog;
                }
            }
        }
    }



    /// <summary>
    /// 普通日志显示
    /// </summary>
    /// <param name="_args"></param>
    public static void Log(params object[] _args)
    {
        if (logEnabled || logWriteEnabled)
        {
            string logStr = "";
            if (_args != null)
            {
                for (int i = 0; i < _args.Length; ++i)
                {
                    if (_args == null)
                    {
                        logStr += "Null ";
                    }
                    else
                    {
                        logStr += _args[i] + " ";
                    }
                }
                if (logEnabled)
                {
                    string debugLogStr = System.DateTime.Now.ToString("T") + " " + logStr;
                    Debug.Log(debugLogStr);
                }

                if (logWriteEnabled)
                {
                    WriteLog(logStr, LogType.Log);
                }
            }
        }
    }

    /// <summary>
    /// 按一定格式日志显示
    /// </summary>
    /// <param name="_format"></param>
    /// <param name="_args"></param>
    public static void LogFormat(string _format, params object[] _args)
    {
        if(logEnabled || logWriteEnabled)
        {
            string logStr = String.Format(_format, _args);
            if (logEnabled)
            {
                string debugLogStr = System.DateTime.Now.ToString("T") + " " + logStr;
                Debug.Log(debugLogStr);
            }

            if (logWriteEnabled)
            {
                WriteLog(logStr, LogType.Log);
            }
        }
    }

    /// <summary>
    /// 普通Error日志显示
    /// </summary>
    /// <param name="_args"></param>
    public static void LogError(params object[] _args)
    {
        if (logEnabled || logWriteEnabled)
        {
            string logStr = "";
            if (_args != null)
            {
                for (int i = 0; i < _args.Length; ++i)
                {
                    if (_args == null)
                    {
                        logStr += "Null ";
                    }
                    else
                    {
                        logStr += _args[i] + " ";
                    }
                }
                if (logEnabled)
                {
                    string debugLogStr = System.DateTime.Now.ToString("T") + " " + logStr;
                    Debug.LogError(debugLogStr);
                }

                if (logWriteEnabled)
                {
                    WriteLog(logStr, LogType.Error);
                }
            }
        }
    }

    /// <summary>
    /// 按一定格式的Error日志显示
    /// </summary>
    /// <param name="_format"></param>
    /// <param name="_args"></param>
    public static void LogErrorFormat(string _format, params object[] _args)
    {
        if (logEnabled || logWriteEnabled)
        {
            string logStr = String.Format(_format, _args);
            if (logEnabled)
            {
                string debugLogStr = System.DateTime.Now.ToString("T") + " " + logStr;
                Debug.LogError(debugLogStr);
            }

            if (logWriteEnabled)
            {
                WriteLog(logStr, LogType.Log);
            }
        }
    }

    /// <summary>
    /// 将日志写到文件
    /// </summary>
    /// <param name="_logStr"></param>
    /// <param name="_type"></param>
    private static void WriteLog(string _logStr, LogType _type)
    {
        if (!logWriteEnabled)
        {
            return;
        }

        if (String.IsNullOrEmpty(logFilePath))
        {
            return;
        }

        string currTime = System.DateTime.Now.ToString();
        string logStr = _type.ToString() + " - [" + currTime + "] " + _logStr + "\n";
        try
        {
            using (FileStream fs = new FileStream(logFilePath, FileMode.Append, FileAccess.Write))
            {
                byte[] data = Encoding.UTF8.GetBytes(logStr);
                fs.Write(data, 0, data.Length);
                fs.Flush();
                fs.Close();
            }
        }catch(System.Exception e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// 捕捉异常
    /// </summary>
    /// <param name="_logString"></param>
    /// <param name="_stackTrace"></param>
    /// <param name="_type"></param>
    private static void HandleLog(string _logString, string _stackTrace, LogType _type)
    {
        if(logWriteEnabled == false)
        {
            return;
        }

        if (_type != LogType.Assert || _type != LogType.Exception)
        {
            return;
        }

        WriteLog(_logString + "\n" + _stackTrace, LogType.Exception);
    }


}
