using OpenTK.Core;
using System;

namespace PrometheOSSkinEditor
{
    public static class Utility
    {
        public static uint RoundUpToNextPowerOf2(uint value)
        {
            value--;
            value |= value >> 1;
            value |= value >> 2;
            value |= value >> 4;
            value |= value >> 8;
            value |= value >> 16;
            value++;
            return value;
        }

        public static string? GetApplicationPath()
        {
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            if (exePath == null)
            {
                return null;
            }

            var result = Path.GetDirectoryName(exePath);
            return result;
        }

        public static string FormatLogMessage(LogMessage logMessage)
        {
            if (logMessage.Level == LogMessageLevel.None)
            {
                return "\n";
            }
            var formattedTime = logMessage.Time.ToString("HH:mm:ss");
            var message = $"{formattedTime} {logMessage.Level} - {logMessage.Message}";
            return $"{message}\n";
        }
    }
}
