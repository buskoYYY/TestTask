#if UNITY_EDITOR

namespace WMAO
{
    [System.Flags]
    public enum AudioExtensions
    {
        MP3 = 1 << 0,
        OGG = 1 << 1,
        WAV = 1 << 2,
    }
}
#endif
