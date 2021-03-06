﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace TEditXna
{
    public static class ErrorLogging
    {
        #region ErrorLevel enum

        public enum ErrorLevel
        {
            Debug,
            Trace,
            Warn,
            Error,
            Fatal
        }

        #endregion

        public static void Log(string message)
        {
            File.AppendAllText("log.txt",
                               string.Format("{0} v{1} {2} {3}",
                               DateTime.Now.ToString("MM-dd-yyyy HH:mm"),
                               Version,
                               message,
                               Environment.NewLine));
        }

        public static void LogException(object ex)
        {
            if (ex is AggregateException)
            {
                var e = ex as AggregateException;
                foreach (var curE in e.Flatten().InnerExceptions) LogException(curE);
            }
            else if (ex is Exception)
            {
                var e = ex as Exception;
                // Log inner exceptions first
                if (e.InnerException != null)
                    LogException(e.InnerException);

                Log(String.Format("{0} - {1}\r\n{2}", ErrorLevel.Error, e.Message, e.StackTrace));
            }
        }

        public static string Version
        {
            get
            {
                Assembly asm = Assembly.GetExecutingAssembly();
                FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(asm.Location);
                return fvi.FileVersion;
            }
        }
    }
}