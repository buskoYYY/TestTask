#if UNITY_EDITOR

namespace WMAO
{
    [System.Flags]
    public enum TextureImporterTypeFlags
    {
        Default = 1 << 0,
        NormalMap = 1 << 1,
        GUI = 1 << 2,
        Sprite = 1 << 3,
        Cursor = 1 << 4,
        Cookie = 1 << 5,
        Lightmap = 1 << 6,
        DirectionalLightmap = 1 << 7,
        Shadowmask = 1 << 8,
        SingleChannel = 1 << 9,
    }
}
#endif
