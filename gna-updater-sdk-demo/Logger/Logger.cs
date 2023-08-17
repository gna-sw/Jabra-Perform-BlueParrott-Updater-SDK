using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Diagnostics;
using System.Linq;

namespace GNAUpdaterSDK_Demo.Logger
{
    public enum LogLevels
    {
        Fatal = 0,
        Error = 1,
        Warn = 2,
        Info = 3,
        Debug = 4,
        Trace = 5
    }

    public static class DemoLogWriter
    {
        public static NLog.Logger Instance = LogManager.GetCurrentClassLogger();

        public const string GNA_DEMO_DEFAULT_LOG_FILE_PATH = "logs/GNAUpdaterSDK-Demo.log";
        public const string GNA_DEMO_DEFAULT_LAYOUT = @"${date:format=MM-dd-yyyy HH\:mm\:ss.ffff} | ${level:uppercase=true} | ${message}";
        public const LogLevels GNA_DEMO_DEFAULT_LOG_LEVEL = LogLevels.Info;

        static DemoLogWriter()
        {
            LogManager.Configuration = LoggerConfiguration();
            LogManager.ReconfigExistingLoggers();
        }

        public static LoggingConfiguration LoggerConfiguration()
        {
            return LoggerConfiguration(GNA_DEMO_DEFAULT_LOG_FILE_PATH, GNA_DEMO_DEFAULT_LOG_LEVEL, GNA_DEMO_DEFAULT_LAYOUT);
        }

        public static LoggingConfiguration LoggerConfiguration(LogLevels loglevel)
        {
            return LoggerConfiguration(GNA_DEMO_DEFAULT_LOG_FILE_PATH, loglevel, GNA_DEMO_DEFAULT_LAYOUT);
        }

        public static LoggingConfiguration LoggerConfiguration(string loggingDir)
        {
            return LoggerConfiguration(loggingDir, GNA_DEMO_DEFAULT_LOG_LEVEL, GNA_DEMO_DEFAULT_LAYOUT);
        }

        public static LoggingConfiguration LoggerConfiguration(string loggingDir, LogLevels loglevel)
        {
            return LoggerConfiguration(loggingDir, loglevel, GNA_DEMO_DEFAULT_LAYOUT);
        }

        public static LoggingConfiguration LoggerConfiguration(string loggingDir, LogLevels loglevel, string layout)
        {
            FileTarget fileTarget = new FileTarget(Instance.Name);
            fileTarget.FileName = loggingDir;
            fileTarget.Layout = layout;

            LogLevel currentLogLevel = getLoggerLevel(loglevel);
            LoggingRule loggingRule = new LoggingRule(Instance.Name, currentLogLevel, fileTarget);

            LoggingConfiguration config = LogManager.Configuration ?? new LoggingConfiguration();

            if (config.AllTargets.Any(t => t.Name.Equals(Instance.Name)))
            {
                config.RemoveTarget(Instance.Name);
            }
            config.AddTarget(Instance.Name, fileTarget);

            var existingRules = config.LoggingRules.Where(r => r.NameMatches(Instance.Name));
            foreach (var rule in existingRules)
            {
                config.RemoveRuleByName(rule.RuleName);
            }

            config.LoggingRules.Add(loggingRule);

            LogManager.Configuration = config;

            return config;
        }

        public static void SetLoggerConfiguration(LoggingConfiguration configuration)
        {
            LogManager.Configuration = configuration;
            LogManager.ReconfigExistingLoggers();
        }

        private static LogLevel getLoggerLevel(LogLevels loglevel)
        {
            if (loglevel == LogLevels.Fatal)
            {
                return LogLevel.Fatal;
            }
            else if (loglevel == LogLevels.Error)
            {
                return LogLevel.Error;
            }
            else if (loglevel == LogLevels.Info)
            {
                return LogLevel.Info;
            }
            else if (loglevel == LogLevels.Warn)
            {
                return LogLevel.Warn;
            }
            else if (loglevel == LogLevels.Debug)
            {
                return LogLevel.Debug;
            }
            else if (loglevel == LogLevels.Trace)
            {
                return LogLevel.Trace;
            }
            return LogLevel.Trace;
        }

        public static String GetExceptionString(Exception ex)
        {
            try
            {
                return "Exception = " + ex.Message + ", StackTrace = " + ex.StackTrace;
            }
            catch (Exception)
            {
            }
            return String.Empty;
        }


        public static void Fatal(String message)
        {
            try
            {
                string trace_msg = PrepareMessage(message, GetCallingMethodName());
                Instance.Fatal(trace_msg);
            }
            catch (Exception)
            {
            }
        }

        public static void Error(String message)
        {
            try
            {
                    string trace_msg = PrepareMessage(message, GetCallingMethodName());
                    Instance.Error(trace_msg);
            }
            catch (Exception)
            {
            }
        }

        public static void Warn(String message)
        {
            try
            {
                    string trace_msg = PrepareMessage(message, GetCallingMethodName());
                    Instance.Warn(trace_msg);
            }
            catch (Exception)
            {
            }
        }

        public static void Info(String message)
        {
            try
            {
                    string trace_msg = PrepareMessage(message, GetCallingMethodName());
                    Instance.Info(trace_msg);
            }
            catch (Exception)
            {
            }
        }

        public static void Debug(String message)
        {
            try
            {
                    string trace_msg = PrepareMessage(message, GetCallingMethodName());
                    Instance.Debug(trace_msg);
            }
            catch (Exception)
            {
            }
        }

        public static void Trace(String message)
        {
            try
            {
                    string trace_msg = PrepareMessage(message, GetCallingMethodName());
                    Instance.Trace(trace_msg);
            }
            catch (Exception)
            {
            }
        }

        private static string GetCallingMethodName()
        {
            try
            {
                StackTrace stackTrace = new StackTrace();
                return stackTrace.GetFrame(2).GetMethod().ReflectedType.Namespace + "." + stackTrace.GetFrame(2).GetMethod().ReflectedType.Name + " " + stackTrace.GetFrame(2).GetMethod().Name + "(): ";
            }
            catch (Exception)
            {
            }
            return "Unknown(): ";
        }
        private static string PrepareMessage(String message, String method)
        {
            try
            {
                String str = method + message;
                return " | " + str;
            }
            catch (Exception)
            {
            }
            return " | " + method + message;
        }
    }
}
