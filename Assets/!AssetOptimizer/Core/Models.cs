#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

namespace WMAO
{
    [ExecuteAlways]
    public static class Models
    {
        private static int _proccessedFilesCount;
        private static string _path;
        private static string _root = "Assets";
        private static string[] _files;

        private static AssetOptimizerSettings _settings;

        public static void OptimizeModels()
        {
            _settings = AssetOptimizerSettings.GetSettings();

            if (_settings == null)
            {
                Debug.LogError("Asset Optimizer Settings can't be found. Create and put it on Resources folder.");
                return;
            }

            if (IsProccessRequired() == false)
                return;

            _proccessedFilesCount = 0;

            _files = Directory.GetFiles(_root + _path, "*.fbx", SearchOption.AllDirectories);
            foreach (var filePath in _files)
                ProcessOptimization(filePath);

            Logger.CloseLog(_proccessedFilesCount, AssetType.Models);
            Debug.Log($"{AssetType.Models}: {_proccessedFilesCount} files has been optimized.");
        }

        private static void ProcessOptimization(string filePath)
        {
            ModelImporter modelImporter = AssetImporter.GetAtPath(filePath) as ModelImporter;

            if (modelImporter != null && IsImportRequired(modelImporter))
            {
                if (_settings.OverrideMeshCompression) modelImporter.meshCompression = _settings.MeshCompression;

                AssetDatabase.ImportAsset(filePath);
                _proccessedFilesCount++;
                Logger.AddLog(filePath, AssetType.Models);
            }
        }

        private static bool IsImportRequired(ModelImporter modelImporter)
        {
            if (_settings.OverrideMeshCompression && modelImporter.meshCompression != _settings.MeshCompression) return true;
            
            return false;
        }

        private static bool IsProccessRequired()
        {
            if (_settings.OverrideMeshCompression) return true;

            return false;
        }
    }
}
#endif
