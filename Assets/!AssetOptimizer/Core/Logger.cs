#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;

namespace WMAO
{
    [ExecuteAlways]
    public static class Logger
    {
        private static readonly string _logPath = @"Assets\!AssetOptimizer";
        private static readonly string _logFullPath = @"Assets\!AssetOptimizer\AssetOptimizerLog.txt";

        private static StreamWriter _streamWriter;
        private static bool _logStarted = false;

        private static void CreateLogFile()
        {
            if (Directory.Exists(_logPath) == false)
                Directory.CreateDirectory(_logPath);

            if (File.Exists(_logFullPath) == false)
            {
                File.Create(_logFullPath).Dispose();
                Debug.Log("Log file created: " + _logFullPath);
            }
        }

        public static void AddLog(string line, AssetType type)
        {
            if (_logStarted == false)
            {
                CreateLogFile();
                _streamWriter = File.AppendText(_logFullPath);
                _logStarted = true;

                string logStart = $"{type} optimization started at {DateTime.Now}";
                _streamWriter.WriteLine(logStart);
                _streamWriter.WriteLine("---------------------------------------");
            }

            _streamWriter.WriteLine(line);
        }

        public static void CloseLog(int filesCount, AssetType type)
        {
            if (_streamWriter == null)
                return;

            _logStarted = false;
            string logEnd = $"{type} optimization finished at {DateTime.Now}\n{filesCount} files has been optimized.";
            _streamWriter.WriteLine("---------------------------------------");
            _streamWriter.WriteLine(logEnd + "\n\n");
            _streamWriter.Close();
            _streamWriter = null;
        }
    }
}
#endif
