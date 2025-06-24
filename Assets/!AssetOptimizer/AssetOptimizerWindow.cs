#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace WMAO
{
    public class AssetOptimizerWindow : EditorWindow
    {
        #region Audio Variables
        private bool _forceToMono = false;
        //private bool _normalize = true;
        private bool _loadInBackground = true;
        private bool _ambisonic = false;
        private bool _preloadAudioData = false;
        private AudioClipLoadType _loadType = AudioClipLoadType.DecompressOnLoad;
        private AudioCompressionFormat _compressionFormat = AudioCompressionFormat.Vorbis;
        private int _compressionFormatInt = 1;
        private AudioSampleRateSetting _sampleRateSetting = AudioSampleRateSetting.PreserveSampleRate;
        private float _quality = 1f;
        private AudioExtensions _audioExtensions = (AudioExtensions)(-1);

        private bool _overrideForceToMono = false;
        private bool _overrideNormalize = false;
        private bool _overrideLoadInBackground = true;
        private bool _overrideAmbisonic = false;
        private bool _overridePreloadAudioData = false;
        private bool _overrideLoadType = true;
        private bool _overrideCompressionFormat = true;
        private bool _overrideSampleRateSetting = false;
        private bool _overrideQuality = true;
        #endregion

        #region Materials Variables
        private bool _overrideGPUInstancing = true;
        private bool _enableGPUInstancing = false;
        private bool _changeShaders = false;
        private string _oldShader = string.Empty;
        private string _newShader = string.Empty;
        #endregion

        #region Models Variables
        private ModelImporterMeshCompression _meshCompression = ModelImporterMeshCompression.Medium;
        private bool _overrideMeshCompression = true;
        #endregion

        #region Textures Variables
        private bool _generateMipMaps = false;
        private int _maxSize = 1024;
        private int _maxSizeInt = 5;
        private string[] _maxSizeOptions = { "32", "64", "128", "256", "512", "1024", "2048", "4096", "8192", "16384"};
        private TextureResizeAlgorithm _resizeAlgorithm = TextureResizeAlgorithm.Mitchell;
        private TextureImporterFormat _format = TextureImporterFormat.Automatic;
        private TextureImporterCompression _compression = TextureImporterCompression.Compressed;
        private int _compressionInt = 1;
        private string[] _compressionOptions = { "None", "Low Quality", "Normal Quality", "High Quality"};
        private bool _useCrunchCompression = true;
        private float _compressorQuality = 50f;
        private bool _disableOverrideForPlatforms = false;
        private Platforms _platforms = (Platforms)(-1);
        private bool _resizeTextures = true;
        private bool _skipIfReadWrite = true;
        private TextureExtensions _textureExtensions = (TextureExtensions)(-1);
        private List<string> _excludeFoldersList = new List<string> { "WebGLTemplates" };
        private bool _excludeFolders = true;
        private Vector2 scrollPositionExclude = Vector2.zero;
        private int _selectedElementExclude;
        private string _folderToAddExclude = string.Empty;
        private List<string> _includeFoldersList = new List<string>();
        private bool _includeFolders = false;
        private Vector2 scrollPositionInclude = Vector2.zero;
        private int _selectedElementInclude;
        private string _folderToAddInclude = string.Empty;
        private TextureImporterTypeFlags _textureImporterType = (TextureImporterTypeFlags)(-1);

        private bool _overrideGenerateMipMaps = true;
        private bool _overrideMaxSize = false;
        private bool _overrideResizeAlgorithm = false;
        private bool _overrideFormat = false;
        private bool _overrideCompression = true;
        private bool _overrideUseCrunchCompression = true;
        private bool _overrideCompressorQuality = true;
        #endregion

        #region TOOLTIPS
        private static string _resizeTexturesTolltip;
        private static readonly string _resizeTexturesTolltipRu = "Изменить размер текстуры, чтобы ширина и высота делились на 4 (нужно для работы Crunch Compression)";
        private static readonly string _resizeTexturesTolltipEn = "Resize the texture so that the width and height are divisible by 4 (needed for Crunch Compression to work)";
        private static string _skipIfReadWriteTolltip;
        private static readonly string _skipIfReadWriteTolltipRu = "Eсли у текстуры стоит галка Read/Write, ее размер меняться не будет (рекомендуется оставить опцию включенной)";
        private static readonly string _skipIfReadWriteTolltipEn = "If Read/Write is checked, the texture size will not change (it is recommended to leave this option enabled).";

        #endregion

        private Vector2 scrollPositionMain = Vector2.zero;
        private int _selectedTabNumber = 3;
        private string[] _tabNames = { "Audio", "Materials", "Models", "Textures" };
        private AssetOptimizerSettings _settings;
        private readonly float _widthOverride = 20f;
        private readonly float _widthLabel = 150f;
        private readonly float _widthField = 200f;
        private readonly float _widthButton = 250f;
        private readonly float _heightButton = 50f;
        private static readonly string _version = "Asset Optimizer v1.2";

        [MenuItem("Tools/Open Asset Optimizer")]
        private static void ShowWindow()
        {
             GetWindow(typeof(AssetOptimizerWindow), false, _version);
        }

        private void OnEnable()
        {
            if (Application.systemLanguage == SystemLanguage.Russian)
            {
                _resizeTexturesTolltip = _resizeTexturesTolltipRu;
                _skipIfReadWriteTolltip = _skipIfReadWriteTolltipRu;
            }
            else
            {
                _resizeTexturesTolltip = _resizeTexturesTolltipEn;
                _skipIfReadWriteTolltip = _skipIfReadWriteTolltipEn;
            }
        }

        private void OnGUI()
        {
            LoadSettingsFromSO();
            scrollPositionMain = EditorGUILayout.BeginScrollView(scrollPositionMain, GUILayout.Height(this.position.height));
            _selectedTabNumber = GUILayout.Toolbar(_selectedTabNumber, _tabNames);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Override", GUILayout.Width(_widthLabel));

            switch (_selectedTabNumber)
            {
                case 0: DrawAudio(); break;
                case 1: DrawMaterials(); break;
                case 2: DrawModels(); break;
                case 3: DrawTextures();  break;
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Optimize All In One Click!", GUILayout.Width(_widthButton), GUILayout.Height(_heightButton)))
            {
                Audio.OptimizeAudio();
                Materials.OptimizeMaterials();
                Models.OptimizeModels();
                Textures.OptimizeTextures();
            }

            EditorGUILayout.Space(20f);

            EditorGUILayout.LabelField(_version);
            EditorGUILayout.EndScrollView();
            SaveSettingsToSO();
        }

        private void DrawAudio()
        {
            EditorGUILayout.BeginHorizontal();
            _overrideForceToMono = GUILayout.Toggle(_overrideForceToMono, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Force To Mono", GUILayout.Width(_widthLabel));
            _forceToMono = GUILayout.Toggle(_forceToMono, "");
            EditorGUILayout.EndHorizontal();

            //if (_forceToMono)
            //{
            //    EditorGUILayout.BeginHorizontal();
            //    _overrideNormalize = GUILayout.Toggle(_overrideNormalize, "", GUILayout.Width(_widthOverride));
            //    EditorGUILayout.LabelField("", GUILayout.Width(10f));
            //    EditorGUILayout.LabelField("Normalize", GUILayout.Width(_widthLabel - 13f));
            //    _normalize = GUILayout.Toggle(_normalize, "");
            //    EditorGUILayout.EndHorizontal();
            //}

            EditorGUILayout.BeginHorizontal();
            _overrideLoadInBackground = GUILayout.Toggle(_overrideLoadInBackground, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Load In Background", GUILayout.Width(_widthLabel));
            _loadInBackground = GUILayout.Toggle(_loadInBackground, "");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _overrideAmbisonic = GUILayout.Toggle(_overrideAmbisonic, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Ambisonic", GUILayout.Width(_widthLabel));
            _ambisonic = GUILayout.Toggle(_ambisonic, "");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _overrideLoadType = GUILayout.Toggle(_overrideLoadType, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Load Type", GUILayout.Width(_widthLabel));
            _loadType = (AudioClipLoadType)EditorGUILayout.EnumPopup(_loadType, GUILayout.Width(_widthField));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _overridePreloadAudioData = GUILayout.Toggle(_overridePreloadAudioData, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Preload Audio Data", GUILayout.Width(_widthLabel));
            _preloadAudioData = GUILayout.Toggle(_preloadAudioData, "");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _overrideCompressionFormat = GUILayout.Toggle(_overrideCompressionFormat, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Compression Format", GUILayout.Width(_widthLabel));
            string[] names = { Enum.GetNames(typeof(AudioCompressionFormat))[0], Enum.GetNames(typeof(AudioCompressionFormat))[1], Enum.GetNames(typeof(AudioCompressionFormat))[2] };
            _compressionFormatInt = EditorGUILayout.Popup(_compressionFormatInt, names, GUILayout.Width(_widthField));
            _compressionFormat = (AudioCompressionFormat)_compressionFormatInt;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _overrideQuality = GUILayout.Toggle(_overrideQuality, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Quality", GUILayout.Width(_widthLabel));
            _quality = EditorGUILayout.Slider(_quality, 1f, 100f, GUILayout.Width(_widthField));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _overrideSampleRateSetting = GUILayout.Toggle(_overrideSampleRateSetting, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Sample Rate Setting", GUILayout.Width(_widthLabel));
            _sampleRateSetting = (AudioSampleRateSetting)EditorGUILayout.EnumPopup(_sampleRateSetting, GUILayout.Width(_widthField));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Audio extensions", GUILayout.Width(_widthLabel));
            _audioExtensions = (AudioExtensions)EditorGUILayout.EnumFlagsField(_audioExtensions, GUILayout.Width(_widthField));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Optimize Audio", GUILayout.Width(_widthButton)))
            {
                Audio.OptimizeAudio();
            }
        }

        private void DrawModels()
        {
            EditorGUILayout.BeginHorizontal();
            _overrideMeshCompression = GUILayout.Toggle(_overrideMeshCompression, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Mesh Compression", GUILayout.Width(_widthLabel));
            _meshCompression = (ModelImporterMeshCompression)EditorGUILayout.EnumPopup(_meshCompression, GUILayout.Width(_widthField));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Optimize Models", GUILayout.Width(_widthButton)))
            {
                Models.OptimizeModels();
            }
        }

        private void DrawMaterials()
        {
            EditorGUILayout.BeginHorizontal();
            _overrideGPUInstancing = GUILayout.Toggle(_overrideGPUInstancing, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Enable GPU Instancing", GUILayout.Width(_widthLabel));
            _enableGPUInstancing = GUILayout.Toggle(_enableGPUInstancing, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _changeShaders = GUILayout.Toggle(_changeShaders, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Change Shaders", GUILayout.Width(_widthLabel));
            EditorGUILayout.EndHorizontal();

            if (_changeShaders)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Old Shader", GUILayout.Width(_widthLabel));
                _oldShader = GUILayout.TextField(_oldShader, GUILayout.Width(_widthField));
                EditorGUILayout.EndHorizontal();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("New Shader", GUILayout.Width(_widthLabel));
                _newShader = GUILayout.TextField(_newShader, GUILayout.Width(_widthField));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Optimize Materials", GUILayout.Width(_widthButton)))
            {
                Materials.OptimizeMaterials();
            }
        }

        private void DrawTextures()
        {
            EditorGUILayout.BeginHorizontal();
            _overrideGenerateMipMaps = GUILayout.Toggle(_overrideGenerateMipMaps, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Generate MipMaps", GUILayout.Width(_widthLabel));
            _generateMipMaps = GUILayout.Toggle(_generateMipMaps, "");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _overrideMaxSize = GUILayout.Toggle(_overrideMaxSize, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Max Size", GUILayout.Width(_widthLabel));
            _maxSizeInt = EditorGUILayout.Popup(_maxSizeInt, _maxSizeOptions, GUILayout.Width(_widthField));
            _maxSize = (int)Mathf.Pow(2, 5 + _maxSizeInt);
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _overrideResizeAlgorithm = GUILayout.Toggle(_overrideResizeAlgorithm, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Resize Algorithm", GUILayout.Width(_widthLabel));
            _resizeAlgorithm = (TextureResizeAlgorithm)EditorGUILayout.EnumPopup(_resizeAlgorithm, GUILayout.Width(_widthField));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _overrideFormat = GUILayout.Toggle(_overrideFormat, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Format", GUILayout.Width(_widthLabel));
            _format = (TextureImporterFormat)EditorGUILayout.EnumPopup(_format, GUILayout.Width(_widthField));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _overrideCompression = GUILayout.Toggle(_overrideCompression, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Compression", GUILayout.Width(_widthLabel));
            _compressionInt = EditorGUILayout.Popup(_compressionInt, _compressionOptions, GUILayout.Width(_widthField));
            SetCompression();
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _overrideUseCrunchCompression = GUILayout.Toggle(_overrideUseCrunchCompression, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Use Crunch Compression", GUILayout.Width(_widthLabel));
            _useCrunchCompression = GUILayout.Toggle(_useCrunchCompression, "");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            _overrideCompressorQuality = GUILayout.Toggle(_overrideCompressorQuality, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Compressor Quality", GUILayout.Width(_widthLabel));
            _compressorQuality = EditorGUILayout.Slider(_compressorQuality, 1f, 100f, GUILayout.Width(_widthField));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            _disableOverrideForPlatforms = GUILayout.Toggle(_disableOverrideForPlatforms, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Disable override settings for platforms", GUILayout.Width(_widthLabel * 2));
            EditorGUILayout.EndHorizontal();

            if (_disableOverrideForPlatforms)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Platforms", GUILayout.Width(_widthLabel));
                _platforms = (Platforms)EditorGUILayout.EnumFlagsField(_platforms, GUILayout.Width(_widthField));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.BeginHorizontal();
            _resizeTextures = GUILayout.Toggle(_resizeTextures, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField(new GUIContent("Resize Textures (only for \".jpg\" and \".png\")", _resizeTexturesTolltip), GUILayout.Width(_widthLabel * 2));
            EditorGUILayout.EndHorizontal();

            if (_resizeTextures)
            {
                EditorGUILayout.BeginHorizontal();
                _skipIfReadWrite = GUILayout.Toggle(_skipIfReadWrite, "", GUILayout.Width(_widthOverride));
                EditorGUILayout.LabelField(new GUIContent("Skip if Read/Write", _skipIfReadWriteTolltip), GUILayout.Width(_widthLabel));
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Texture extensions", GUILayout.Width(_widthLabel));
            _textureExtensions = (TextureExtensions)EditorGUILayout.EnumFlagsField(_textureExtensions, GUILayout.Width(_widthField));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Filter By Type:", GUILayout.Width(_widthLabel));
            _textureImporterType = (TextureImporterTypeFlags)EditorGUILayout.EnumFlagsField(_textureImporterType, GUILayout.Width(_widthField));
            EditorGUILayout.EndHorizontal();

            #region EXCLUDE FOLDERS
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            _excludeFolders = GUILayout.Toggle(_excludeFolders, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Exclude folders:", GUILayout.Width(_widthLabel));
            EditorGUILayout.EndHorizontal();

            if (_excludeFolders)
            {
                scrollPositionExclude = EditorGUILayout.BeginScrollView(scrollPositionExclude, GUI.skin.box, GUILayout.Height(Mathf.Min(_excludeFoldersList.Count * 25, 100)));
                _selectedElementExclude = GUILayout.SelectionGrid(_selectedElementExclude, GetFolders(_excludeFoldersList).ToArray(), 1, GUILayout.Width(_widthField));
                EditorGUILayout.EndScrollView();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Folder to add:", GUILayout.Width(_widthField / 2));
                _folderToAddExclude = GUILayout.TextField(_folderToAddExclude, GUILayout.Width(_widthField));
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    if (_folderToAddExclude != string.Empty && _excludeFoldersList.Contains(_folderToAddExclude) == false)
                    {
                        _excludeFoldersList.Add(_folderToAddExclude);
                        _folderToAddExclude = string.Empty;
                    }
                }
                if (GUILayout.Button("-", GUILayout.Width(30)) && _excludeFoldersList.Count > 0)
                {
                    if (_selectedElementExclude < _excludeFoldersList.Count)
                        _excludeFoldersList.RemoveAt(_selectedElementExclude);
                }
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
            #endregion

            #region INCLUDE FOLDERS
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.BeginHorizontal();
            _includeFolders = GUILayout.Toggle(_includeFolders, "", GUILayout.Width(_widthOverride));
            EditorGUILayout.LabelField("Include only folders:", GUILayout.Width(_widthLabel));
            EditorGUILayout.EndHorizontal();

            if (_includeFolders)
            {
                scrollPositionInclude = EditorGUILayout.BeginScrollView(scrollPositionInclude, GUI.skin.box, GUILayout.Height(Mathf.Min(_includeFoldersList.Count * 25, 100)));
                _selectedElementInclude = GUILayout.SelectionGrid(_selectedElementInclude, GetFolders(_includeFoldersList).ToArray(), 1, GUILayout.Width(_widthField));
                EditorGUILayout.EndScrollView();

                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Folder to add:", GUILayout.Width(_widthField / 2));
                _folderToAddInclude = GUILayout.TextField(_folderToAddInclude, GUILayout.Width(_widthField));
                if (GUILayout.Button("+", GUILayout.Width(30)))
                {
                    if (_folderToAddInclude != string.Empty && _includeFoldersList.Contains(_folderToAddInclude) == false)
                    {
                        _includeFoldersList.Add(_folderToAddInclude);
                        _folderToAddInclude = string.Empty;
                    }
                }
                if (GUILayout.Button("-", GUILayout.Width(30)) && _includeFoldersList.Count > 0)
                {
                    if (_selectedElementInclude < _includeFoldersList.Count)
                        _includeFoldersList.RemoveAt(_selectedElementInclude);
                }
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndVertical();
            #endregion

            EditorGUILayout.Space();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            if (GUILayout.Button("Optimize Textures", GUILayout.Width(_widthButton)))
            {
                Textures.OptimizeTextures();
            }
        }

        private List<GUIContent> GetFolders(List<string> list)
        {
            List<GUIContent> content = new List<GUIContent>();
            foreach (var item in list)
            {
                content.Add(new GUIContent(item));
            }

            return content;
        }

        private void SetCompression()
        {
            if (_compressionInt == 0)
                _compression = TextureImporterCompression.Uncompressed;
            else if (_compressionInt == 1)
                _compression = TextureImporterCompression.CompressedLQ;
            else if (_compressionInt == 2)
                _compression = TextureImporterCompression.Compressed;
            else if (_compressionInt == 3)
                _compression = TextureImporterCompression.CompressedHQ;
        }

        private void LoadSettingsFromSO()
        {
            if (_settings == null)
                _settings = AssetOptimizerSettings.GetSettings();

            _forceToMono = _settings.ForceToMono;
            //_normalize = _settings.Normalize;
            _loadInBackground = _settings.LoadInBackground;
            _ambisonic = _settings.Ambisonic;
            _preloadAudioData = _settings.PreloadAudioData;
            _loadType = _settings.LoadType;
            _compressionFormat = _settings.CompressionFormat;
            _compressionFormatInt = _settings.CompressionFormatInt;
            _sampleRateSetting = _settings.SampleRateSetting;
            _quality = Mathf.Clamp(_settings.Quality * 100f, 1f, 100f);
            _audioExtensions = _settings.AudioExtensions;

            _overrideForceToMono = _settings.OverrideForceToMono;
            //_overrideNormalize = _settings.OverrideNormalize;
            _overrideLoadInBackground = _settings.OverrideLoadInBackground;
            _overrideAmbisonic = _settings.OverrideAmbisonic;
            _overridePreloadAudioData = _settings.OverridePreloadAudioData;
            _overrideLoadType = _settings.OverrideLoadType;
            _overrideCompressionFormat = _settings.OverrideCompressionFormat;
            _overrideSampleRateSetting = _settings.OverrideSampleRateSetting;
            _overrideQuality = _settings.OverrideQuality;
            //Materials
            _overrideGPUInstancing = _settings.OverrideGPUInstancing;
            _enableGPUInstancing = _settings.EnableGPUInstancing;
            _changeShaders = _settings.ChangeShaders;
            _oldShader = _settings.OldShader;
            _newShader = _settings.NewShader;

            _meshCompression = _settings.MeshCompression;
            _overrideMeshCompression = _settings.OverrideMeshCompression;

            _generateMipMaps = _settings.GenerateMipMaps;
            _maxSize = _settings.MaxSize;
            _maxSizeInt = _settings.MaxSizeInt;
            _resizeAlgorithm = _settings.ResizeAlgorithm;
            _format = _settings.Format;
            _compressionInt = _settings.CompressionInt;
            _compression = _settings.Compression;
            _useCrunchCompression = _settings.UseCrunchCompression;
            _compressorQuality = _settings.CompressorQuality;
            _disableOverrideForPlatforms = _settings.DisableOverrideForPlatforms;
            _platforms = _settings.Platforms;
            _resizeTextures = _settings.ResizeTextures;
            _skipIfReadWrite = _settings.SkipIfReadWrite;
            _textureExtensions = _settings.TextureExtensions;
            _excludeFoldersList = _settings.ExcludeFoldersList;
            _excludeFolders = _settings.ExcludeFolders;
            _includeFoldersList = _settings.IncludeFoldersList;
            _includeFolders = _settings.IncludeFolders;
            _textureImporterType = _settings.TextureImporterType;

            _overrideGenerateMipMaps = _settings.OverrideGenerateMipMaps;
            _overrideMaxSize = _settings.OverrideMaxSize;
            _overrideResizeAlgorithm = _settings.OverrideResizeAlgorithm;
            _overrideFormat = _settings.OverrideFormat;
            _overrideCompression = _settings.OverrideCompression;
            _overrideUseCrunchCompression = _settings.OverrideUseCrunchCompression;
            _overrideCompressorQuality = _settings.OverrideCompressorQuality;
        }

        private void SaveSettingsToSO()
        {
            _settings.ForceToMono = _forceToMono;
            //_settings.Normalize = _normalize;
            _settings.LoadInBackground = _loadInBackground;
            _settings.Ambisonic = _ambisonic;
            _settings.PreloadAudioData = _preloadAudioData;
            _settings.LoadType = _loadType;
            _settings.CompressionFormat = _compressionFormat;
            _settings.CompressionFormatInt = _compressionFormatInt;
            _settings.SampleRateSetting = _sampleRateSetting;
            _settings.Quality = Mathf.Clamp(_quality / 100f, 0.01f, 1f);
            _settings.AudioExtensions = _audioExtensions;

            _settings.OverrideForceToMono = _overrideForceToMono;
            //_settings.OverrideNormalize = _overrideNormalize;
            _settings.OverrideLoadInBackground = _overrideLoadInBackground;
            _settings.OverrideAmbisonic = _overrideAmbisonic;
            _settings.OverridePreloadAudioData = _overridePreloadAudioData;
            _settings.OverrideLoadType = _overrideLoadType;
            _settings.OverrideCompressionFormat = _overrideCompressionFormat;
            _settings.OverrideSampleRateSetting = _overrideSampleRateSetting;
            _settings.OverrideQuality = _overrideQuality;
            //Materials
            _settings.OverrideGPUInstancing = _overrideGPUInstancing;
            _settings.EnableGPUInstancing = _enableGPUInstancing;
            _settings.ChangeShaders = _changeShaders;
            _settings.OldShader = _oldShader;
            _settings.NewShader = _newShader;

            _settings.MeshCompression = _meshCompression;
            _settings.OverrideMeshCompression = _overrideMeshCompression;

            _settings.GenerateMipMaps = _generateMipMaps;
            _settings.MaxSize = _maxSize;
            _settings.MaxSizeInt = _maxSizeInt;
            _settings.ResizeAlgorithm = _resizeAlgorithm;
            _settings.Format = _format;
            _settings.Compression = _compression;
            _settings.CompressionInt = _compressionInt;
            _settings.UseCrunchCompression = _useCrunchCompression;
            _settings.CompressorQuality = _compressorQuality;
            _settings.DisableOverrideForPlatforms = _disableOverrideForPlatforms;
            _settings.Platforms = _platforms;
            _settings.ResizeTextures = _resizeTextures;
            _settings.SkipIfReadWrite = _skipIfReadWrite;
            _settings.TextureExtensions = _textureExtensions;
            _settings.ExcludeFoldersList = _excludeFoldersList;
            _settings.ExcludeFolders = _excludeFolders;
            _settings.IncludeFoldersList = _includeFoldersList;
            _settings.IncludeFolders = _includeFolders;
            _settings.TextureImporterType = _textureImporterType;

            _settings.OverrideGenerateMipMaps = _overrideGenerateMipMaps;
            _settings.OverrideMaxSize = _overrideMaxSize;
            _settings.OverrideResizeAlgorithm = _overrideResizeAlgorithm;
            _settings.OverrideFormat = _overrideFormat;
            _settings.OverrideCompression = _overrideCompression;
            _settings.OverrideUseCrunchCompression = _overrideUseCrunchCompression;
            _settings.OverrideCompressorQuality = _overrideCompressorQuality;
        }
    }
}
#endif