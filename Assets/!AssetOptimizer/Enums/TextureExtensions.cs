#if UNITY_EDITOR

namespace WMAO
{
    [System.Flags]
    public enum TextureExtensions
    {
        PNG = 1 << 0,
        JPG = 1 << 1,
        TIF = 1 << 2,
        TGA = 1 << 3,
        PSD = 1 << 4,
        EXR = 1 << 5,
        BMP = 1 << 6,
        PSB = 1 << 7,
        SpriteAtlas = 1 << 8,
        JPEG = 1 << 9,
    }
}
#endif
