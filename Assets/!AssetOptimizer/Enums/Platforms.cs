#if UNITY_EDITOR

namespace WMAO
{
    [System.Flags]
    public enum Platforms
    {
        Windows = 1 << 0,
        DedicatedServer = 1 << 1,
        Android = 1 << 2,
        iPhone = 1 << 3,
        WebGL = 1 << 4,
    }
}
#endif
