#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace WMAO
{
    [CreateAssetMenu(fileName = nameof(AssetOptimizerSettings), menuName = "Asset Optimizer Settings")]
    public class AssetOptimizerSettings : ScriptableObject
    {
        #region Audio Variables
        public bool ForceToMono = false;
        //public bool Normalize = true;
        public bool LoadInBackground = true;
        public bool Ambisonic = false;
        public bool PreloadAudioData = false;
        public AudioClipLoadType LoadType = AudioClipLoadType.DecompressOnLoad;
        public AudioCompressionFormat CompressionFormat = AudioCompressionFormat.Vorbis;
        public AudioSampleRateSetting SampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
        [HideInInspector] public int CompressionFormatInt = 1;
        public float Quality = 0.01f;
        public AudioExtensions AudioExtensions = (AudioExtensions)(-1);

        public bool OverrideForceToMono = false;
        //public bool OverrideNormalize = false;
        public bool OverrideLoadInBackground = true;
        public bool OverrideAmbisonic = false;
        public bool OverridePreloadAudioData = false;
        public bool OverrideLoadType = true;
        public bool OverrideCompressionFormat = true;
        public bool OverrideSampleRateSetting = false;
        public bool OverrideQuality = true;
        #endregion

        #region Materials Variables
        public bool OverrideGPUInstancing = true;
        public bool EnableGPUInstancing = false;
        public bool ChangeShaders = false;
        public string OldShader = "Standard";
        public string NewShader = "Legacy Shaders/Diffuse";
        #endregion

        #region Models Variables
        public ModelImporterMeshCompression MeshCompression = ModelImporterMeshCompression.Medium;
        public bool OverrideMeshCompression = true;
        #endregion

        #region Textures Variables
        public bool GenerateMipMaps = false;
        public int MaxSize = 1024;
        public int MaxSizeInt = 5;
        public TextureResizeAlgorithm ResizeAlgorithm = TextureResizeAlgorithm.Mitchell;
        public TextureImporterFormat Format = TextureImporterFormat.Automatic;
        public TextureImporterCompression Compression = TextureImporterCompression.Compressed;
        public int CompressionInt = 2;
        public bool UseCrunchCompression = true;
        public float CompressorQuality = 50f;
        public bool DisableOverrideForPlatforms = false;
        public Platforms Platforms = (Platforms)(-1);
        public bool ResizeTextures = true;
        public bool SkipIfReadWrite = true;
        public TextureExtensions TextureExtensions = (TextureExtensions)(-1);
        public List<string> ExcludeFoldersList = new List<string> { "WebGLTemplates" };
        public bool ExcludeFolders = true;
        public List<string> IncludeFoldersList = new List<string>();
        public bool IncludeFolders = false;
        public TextureImporterTypeFlags TextureImporterType = (TextureImporterTypeFlags)(-1);

        public bool OverrideGenerateMipMaps = true;
        public bool OverrideMaxSize = false;
        public bool OverrideResizeAlgorithm = false;
        public bool OverrideFormat = false;
        public bool OverrideCompression = true;
        public bool OverrideUseCrunchCompression = true;
        public bool OverrideCompressorQuality = true;
        #endregion

        public static AssetOptimizerSettings GetSettings() => Resources.Load<AssetOptimizerSettings>(nameof(AssetOptimizerSettings));
    }
}
#endif