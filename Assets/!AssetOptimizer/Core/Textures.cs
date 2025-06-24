#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.U2D;
using UnityEngine;
using UnityEngine.U2D;

namespace WMAO
{
    [ExecuteAlways]
    public static class Textures
    {
        private static int _proccessedFilesCount;
        private static string _root = "Assets";
        private static string[] _files;
        private static Dictionary<TextureExtensions, string> _fileExtensions = new Dictionary<TextureExtensions, string>
        {
            { TextureExtensions.BMP, "*.bmp" },
            { TextureExtensions.EXR, "*.exr" },
            { TextureExtensions.JPG, "*.jpg" },
            { TextureExtensions.PNG, "*.png" },
            { TextureExtensions.PSB, "*.psb" },
            { TextureExtensions.PSD, "*.psd" },
            { TextureExtensions.TGA, "*.tga" },
            { TextureExtensions.TIF, "*.tif" },
            { TextureExtensions.SpriteAtlas, "*.spriteatlasv2" },
            { TextureExtensions.JPEG, "*.jpeg" },
        };

        private static readonly Dictionary<Platforms, string> _platforms = new Dictionary<Platforms, string>
        {
            { Platforms.WebGL, "WebGL" },
            { Platforms.Android, "Android" },
            { Platforms.iPhone, "iPhone" },
            { Platforms.Windows, "Standalone" },
            { Platforms.DedicatedServer, "Server" }
        };

        private static TextureExtensions _currentExtension;
        private static AssetOptimizerSettings _settings;
        private static bool _needReimport;
        private static string _currentLogLine;

        public static void OptimizeTextures()
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
                if (_settings.TextureExtensions.HasFlag(extensionPair.Key))
                {
                    _currentExtension = extensionPair.Key;
                    _files = GetFiles(extensionPair.Value);

                    foreach (var filePath in _files)
                    {
                        if (IsPathValid(filePath))
                            ProcessOptimization(filePath, extensionPair.Key);
                    }
                }
            }

            Logger.CloseLog(_proccessedFilesCount, AssetType.Textures);
            Debug.Log($"{AssetType.Textures}: {_proccessedFilesCount} files has been optimized.");
        }

        private static string[] GetFiles(string ext)
        {
            if (_settings.IncludeFolders)
            {
                List<string> result = new List<string>();
                foreach (var folder in _settings.IncludeFoldersList)
                    result.AddRange(Directory.GetFiles(_root + "\\" + folder, ext, SearchOption.AllDirectories));

                return result.ToArray();
            }
            else
                return Directory.GetFiles(_root, ext, SearchOption.AllDirectories);
        }

        private static bool IsPathValid(string filePath)
        {
            if (_settings.ExcludeFolders == false)
                return true;

            foreach (var folderName in _settings.ExcludeFoldersList)
                if (filePath.StartsWith(_root + "\\" + folderName))
                    return false;

            return true;
        }

        private static bool IsValidTextureType(TextureImporterType type)
        {
            return _settings.TextureImporterType.HasFlag(ConvertImporterType(type));
        }

        private static TextureImporterTypeFlags ConvertImporterType(TextureImporterType type)
        {
            switch (type)
            {
                case TextureImporterType.Default: return TextureImporterTypeFlags.Default;
                case TextureImporterType.Cookie: return TextureImporterTypeFlags.Cookie;
                case TextureImporterType.Cursor: return TextureImporterTypeFlags.Cursor;
                case TextureImporterType.DirectionalLightmap: return TextureImporterTypeFlags.DirectionalLightmap;
                case TextureImporterType.GUI: return TextureImporterTypeFlags.GUI;
                case TextureImporterType.Lightmap: return TextureImporterTypeFlags.Lightmap;
                case TextureImporterType.NormalMap: return TextureImporterTypeFlags.NormalMap;
                case TextureImporterType.Shadowmask: return TextureImporterTypeFlags.Shadowmask;
                case TextureImporterType.SingleChannel: return TextureImporterTypeFlags.SingleChannel;
                case TextureImporterType.Sprite: return TextureImporterTypeFlags.Sprite;
            }

            return TextureImporterTypeFlags.Default;
        }

        private static void ProcessOptimization(string filePath, TextureExtensions textureType)
        {
            _needReimport = false;

            if (textureType == TextureExtensions.SpriteAtlas)
            {
                ProcessOptimizationSpriteAtlas(filePath);
                return;
            }

            TextureImporter textureImporter = AssetImporter.GetAtPath(filePath) as TextureImporter;

            if (textureImporter != null)
            {
                if (IsValidTextureType(textureImporter.textureType) == false)
                    return;

                _currentLogLine = string.Empty;

                if (IsImportRequired(textureImporter))
                {
                    if (_settings.OverrideGenerateMipMaps) textureImporter.mipmapEnabled = _settings.GenerateMipMaps;
                    if (_settings.OverrideMaxSize) textureImporter.maxTextureSize = _settings.MaxSize;
                    if (_settings.OverrideCompression) textureImporter.textureCompression = _settings.Compression;
                    if (_settings.OverrideUseCrunchCompression && !(_settings.SkipIfReadWrite && textureImporter.isReadable)) textureImporter.crunchedCompression = _settings.UseCrunchCompression;
                    if (_settings.OverrideCompressorQuality) textureImporter.compressionQuality = (int)_settings.CompressorQuality;

                    if (_settings.OverrideResizeAlgorithm)
                    {
                        var settings = textureImporter.GetDefaultPlatformTextureSettings();
                        settings.resizeAlgorithm = _settings.ResizeAlgorithm;
                        textureImporter.SetPlatformTextureSettings(settings);
                    }

                    if (_settings.OverrideFormat)
                    {
                        var settings = textureImporter.GetDefaultPlatformTextureSettings();
                        settings.format = _settings.Format;
                        textureImporter.SetPlatformTextureSettings(settings);
                    }

                    _needReimport = true;
                }

                if (_settings.DisableOverrideForPlatforms)
                    DisableOverrideForPlatforms(textureImporter);

                if (_settings.ResizeTextures && (_currentExtension == TextureExtensions.PNG || _currentExtension == TextureExtensions.JPG))
                {
                    if (!(_settings.SkipIfReadWrite && textureImporter.isReadable))
                    {
                        Texture2D originalTexture = LoadTexture(filePath);

                        if (IsResizeRequired(originalTexture, textureImporter.maxTextureSize, out int width, out int height))
                        {
                            _currentLogLine = $"({originalTexture.width}, {originalTexture.height}) | ({width}, {height}) ";
                            RenderTexture rt = RenderTexture.GetTemporary(width, height);
                            RenderTexture.active = rt;
                            Graphics.Blit(originalTexture, rt);
                            Texture2D resizedTexture = new Texture2D(width, height);
                            resizedTexture.ReadPixels(new Rect(0, 0, width, height), 0, 0);
                            resizedTexture.Apply();
                            RenderTexture.active = null;
                            RenderTexture.ReleaseTemporary(rt);
                            SaveTextureToFile(resizedTexture, filePath);
                            _needReimport = true;
                        }

                        Object.DestroyImmediate(originalTexture);
                        originalTexture = null;
                        Resources.UnloadUnusedAssets();
                    }
                }

                if (_needReimport)
                {
                    AssetDatabase.ImportAsset(filePath);
                    _proccessedFilesCount++;
                    _currentLogLine += filePath;
                    Logger.AddLog(_currentLogLine, AssetType.Textures);
                }
            }
            else
            {
                Debug.LogError($"Problem with texture import: {filePath}");
            }
        }

        private static void ProcessOptimizationSpriteAtlas(string filePath)
        {
            SpriteAtlasImporter spriteAtlasImporter = AssetImporter.GetAtPath(filePath) as SpriteAtlasImporter;
            
            TextureImporterPlatformSettings platformSettings = spriteAtlasImporter.GetPlatformSettings("DefaultTexturePlatform");
            SpriteAtlasTextureSettings textureSettings = spriteAtlasImporter.textureSettings;

            _currentLogLine = string.Empty;

            if (IsImportRequired(platformSettings, textureSettings))
            {
                if (_settings.OverrideGenerateMipMaps) textureSettings.generateMipMaps = _settings.GenerateMipMaps;
                if (_settings.OverrideMaxSize) platformSettings.maxTextureSize = _settings.MaxSize;
                if (_settings.OverrideCompression) platformSettings.textureCompression = _settings.Compression;
                if (_settings.OverrideUseCrunchCompression && !(_settings.SkipIfReadWrite && textureSettings.readable)) platformSettings.crunchedCompression = _settings.UseCrunchCompression;
                if (_settings.OverrideCompressorQuality) platformSettings.compressionQuality = (int)_settings.CompressorQuality;
                if (_settings.OverrideResizeAlgorithm) platformSettings.resizeAlgorithm = _settings.ResizeAlgorithm;
                if (_settings.OverrideFormat) platformSettings.format = _settings.Format;

                _needReimport = true;
            }

            if (_settings.DisableOverrideForPlatforms)
                DisableOverrideForPlatforms(spriteAtlasImporter);

            if (_needReimport)
            {
                spriteAtlasImporter.textureSettings = textureSettings;
                spriteAtlasImporter.SetPlatformSettings(platformSettings);

                AssetDatabase.ImportAsset(filePath);
                _proccessedFilesCount++;
                _currentLogLine += filePath;
                Logger.AddLog(_currentLogLine, AssetType.SpriteAtlas);
            }
        }

        private static bool IsResizeRequired(Texture2D texture, int maxSize, out int width, out int height)
        {
            width = 0;
            height = 0;
            if (texture.width < 4 || texture.height < 4)
            {
                texture = null;
                return false;
            }

            int newWidth = texture.width;
            int newHeight = texture.height;

            if (maxSize >= texture.width && maxSize >= texture.height)
            {
                newWidth = texture.width - texture.width % 4;
                newHeight = texture.height - texture.height % 4;

                if (texture.width % 4 != 0) newWidth += 4;
                if (texture.height % 4 != 0) newHeight += 4;
            }
            else
            {
                float ratio = 0f;
                CalculateNewMaxSize(ref newWidth, ref newHeight, ref ratio, maxSize);
                int origWidth = newWidth;
                int origHeight = newHeight;
                newWidth = origWidth - origWidth % 4;
                newHeight = origHeight - origHeight % 4;

                if (origWidth % 4 != 0) newWidth += 4;
                if (origHeight % 4 != 0) newHeight += 4;

                newWidth = (int)Mathf.Round((float)newWidth * ratio);
                newHeight = (int)Mathf.Round((float)newHeight * ratio);
            }

            if (newWidth == texture.width && newHeight == texture.height)
            {
                texture = null;
                return false;
            }

            newWidth = Mathf.Max(4, newWidth);
            newHeight = Mathf.Max(4, newHeight);

            width = newWidth;
            height = newHeight;

            return true;
        }

        private static void SaveTextureToFile(Texture2D texture, string path)
        {
            if (_currentExtension == TextureExtensions.PNG)
            {
                byte[] bytes = texture.EncodeToPNG();
                File.WriteAllBytes(path, bytes);
            }
            else if (_currentExtension == TextureExtensions.JPG)
            {
                byte[] bytes = texture.EncodeToJPG();
                File.WriteAllBytes(path, bytes);
            }
        }

        private static void CalculateNewMaxSize(ref int width, ref int height, ref float ratio, int maxSize)
        {
            float widthRatio = (float)width / (float)maxSize;
            float heightRatio = (float)height / (float)maxSize;

            if (widthRatio >= heightRatio)
            {
                ratio = widthRatio;
                width = maxSize;
                height = (int)((float)height / widthRatio);
            }
            else
            {
                ratio = heightRatio;
                height = maxSize;
                width = (int)((float)width / heightRatio);
            }
        }

        private static Texture2D LoadTexture(string path)
        {
            byte[] fileData = File.ReadAllBytes(path);
            Texture2D texture = new Texture2D(2, 2);
            texture.LoadImage(fileData);
            return texture;
        }

        private static void DisableOverrideForPlatforms(TextureImporter textureImporter)
        {
            foreach (var platformPair in _platforms)
            {
                if (_settings.Platforms.HasFlag(platformPair.Key))
                {
                    TextureImporterPlatformSettings settings = textureImporter.GetPlatformTextureSettings(platformPair.Value);
                    if (settings.overridden == true)
                    {
                        settings.overridden = false;
                        textureImporter.SetPlatformTextureSettings(settings);
                        _needReimport = true;
                    }
                }
            }
        }

        private static void DisableOverrideForPlatforms(SpriteAtlasImporter textureImporter)
        {
            foreach (var platformPair in _platforms)
            {
                if (_settings.Platforms.HasFlag(platformPair.Key))
                {
                    TextureImporterPlatformSettings settings = textureImporter.GetPlatformSettings(platformPair.Value);
                    if (settings.overridden == true)
                    {
                        settings.overridden = false;
                        textureImporter.SetPlatformSettings(settings);
                        _needReimport = true;
                    }
                }
            }
        }

        private static bool IsImportRequired(TextureImporter textureImporter)
        {
            if (_settings.OverrideGenerateMipMaps && textureImporter.mipmapEnabled != _settings.GenerateMipMaps) return true;
            if (_settings.OverrideMaxSize && textureImporter.maxTextureSize != _settings.MaxSize) return true;
            if (_settings.OverrideCompression && textureImporter.textureCompression != _settings.Compression) return true;
            if (_settings.OverrideUseCrunchCompression
                && !(_settings.SkipIfReadWrite && textureImporter.isReadable)
                && textureImporter.crunchedCompression != _settings.UseCrunchCompression) return true;
            if (_settings.OverrideCompressorQuality && textureImporter.compressionQuality != _settings.CompressorQuality) return true;
            if (_settings.OverrideResizeAlgorithm && textureImporter.GetDefaultPlatformTextureSettings().resizeAlgorithm != _settings.ResizeAlgorithm) return true;
            if (_settings.OverrideFormat && textureImporter.GetDefaultPlatformTextureSettings().format != _settings.Format) return true;

            return false;
        }

        private static bool IsImportRequired(TextureImporterPlatformSettings platformSettings, SpriteAtlasTextureSettings textureSettings)
        {
            if (_settings.OverrideGenerateMipMaps && textureSettings.generateMipMaps != _settings.GenerateMipMaps) return true;
            if (_settings.OverrideMaxSize && platformSettings.maxTextureSize != _settings.MaxSize) return true;
            if (_settings.OverrideCompression && platformSettings.textureCompression != _settings.Compression) return true;
            if (_settings.OverrideUseCrunchCompression && !(_settings.SkipIfReadWrite && textureSettings.readable) && platformSettings.crunchedCompression != _settings.UseCrunchCompression) return true;
            if (_settings.OverrideCompressorQuality && platformSettings.compressionQuality != (int)_settings.CompressorQuality) return true;
            if (_settings.OverrideResizeAlgorithm && platformSettings.resizeAlgorithm != _settings.ResizeAlgorithm) return true;
            if (_settings.OverrideFormat && platformSettings.format != _settings.Format) return true;

            return false;
        }

        private static bool IsProccessRequired()
        {
            if (_settings.OverrideGenerateMipMaps) return true;
            if (_settings.OverrideMaxSize) return true;
            if (_settings.OverrideCompression) return true;
            if (_settings.OverrideUseCrunchCompression) return true;
            if (_settings.OverrideCompressorQuality) return true;
            if (_settings.OverrideResizeAlgorithm) return true;
            if (_settings.OverrideFormat) return true;
            if (_settings.ResizeTextures) return true;
            if (_settings.DisableOverrideForPlatforms && _settings.Platforms != 0) return true;

            return false;
        }
    }
}
#endif
