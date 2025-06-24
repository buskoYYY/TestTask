#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace WMAO
{
    [ExecuteAlways]
    public static class Audio
    {
        private static int _proccessedFilesCount;
        private static string _path;
        private static string _root = "Assets";
        private static string[] _files;
        private static Dictionary<AudioExtensions, string> _fileExtensions = new Dictionary<AudioExtensions, string>
        {
            { AudioExtensions.MP3, "*.mp3" },
            { AudioExtensions.OGG, "*.ogg" },
            { AudioExtensions.WAV, "*.wav" }
        };

        private static AssetOptimizerSettings _settings;

        public static void OptimizeAudio()
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

            foreach (var extensionPair in _fileExtensions)
            {
                if (_settings.AudioExtensions.HasFlag(extensionPair.Key))
                {
                    _files = Directory.GetFiles(_root + _path, extensionPair.Value, SearchOption.AllDirectories);
                    foreach (var filePath in _files)
                    {
                        ProcessOptimization(filePath);
                    }
                }

            }
            Logger.CloseLog(_proccessedFilesCount, AssetType.Audio);
            Debug.Log($"{AssetType.Audio}: {_proccessedFilesCount} files has been optimized.");
        }

        private static void ProcessOptimization(string filePath)
        {
            AudioImporter audioImporter = AssetImporter.GetAtPath(filePath) as AudioImporter;

            if (audioImporter != null && IsImportRequired(audioImporter))
            {
                if (_settings.OverrideForceToMono) audioImporter.forceToMono = _settings.ForceToMono;
                //if (_settings.OverrideNormalize) audioImporter. = _settings.Normalize;
                if (_settings.OverrideAmbisonic) audioImporter.ambisonic = _settings.Ambisonic;
                if (_settings.OverrideLoadInBackground) audioImporter.loadInBackground = _settings.LoadInBackground;

                AudioImporterSampleSettings sampleSettings = audioImporter.defaultSampleSettings;
                if (_settings.OverrideLoadType) sampleSettings.loadType = _settings.LoadType;
                if (_settings.OverridePreloadAudioData) sampleSettings.preloadAudioData = _settings.PreloadAudioData;
                if (_settings.OverrideCompressionFormat) sampleSettings.compressionFormat = _settings.CompressionFormat;
                if (_settings.OverrideQuality) sampleSettings.quality = _settings.Quality;
                if (_settings.OverrideSampleRateSetting) sampleSettings.sampleRateSetting = _settings.SampleRateSetting;

                audioImporter.defaultSampleSettings = sampleSettings;
                AssetDatabase.ImportAsset(filePath);
                _proccessedFilesCount++;
                Logger.AddLog(filePath, AssetType.Audio);
            }
        }

        private static bool IsImportRequired(AudioImporter audioImporter)
        {
            if (_settings.OverrideForceToMono && audioImporter.forceToMono != _settings.ForceToMono) return true;
            if (_settings.OverrideAmbisonic && audioImporter.ambisonic != _settings.Ambisonic) return true;
            if (_settings.OverrideLoadInBackground && audioImporter.loadInBackground != _settings.LoadInBackground) return true;

            AudioImporterSampleSettings sampleSettings = audioImporter.defaultSampleSettings;
            if (_settings.OverrideLoadType && sampleSettings.loadType != _settings.LoadType) return true;
            if (_settings.OverridePreloadAudioData && sampleSettings.preloadAudioData != _settings.PreloadAudioData) return true;
            if (_settings.OverrideCompressionFormat && sampleSettings.compressionFormat != _settings.CompressionFormat) return true;
            if (_settings.OverrideQuality && sampleSettings.quality != _settings.Quality) return true;
            if (_settings.OverrideSampleRateSetting && sampleSettings.sampleRateSetting != _settings.SampleRateSetting) return true;

            return false;
        }

        private static bool IsProccessRequired()
        {
            if (_settings.OverrideForceToMono) return true;
            if (_settings.OverrideAmbisonic) return true;
            if (_settings.OverrideLoadInBackground) return true;
            if (_settings.OverrideLoadType) return true;
            if (_settings.OverridePreloadAudioData) return true;
            if (_settings.OverrideCompressionFormat) return true;
            if (_settings.OverrideQuality) return true;
            if (_settings.OverrideSampleRateSetting) return true;

            return false;
        }
    }
}
#endif
