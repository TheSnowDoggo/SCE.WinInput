namespace SCEInputSystem
{
    using System.Runtime.InteropServices;

    public struct KBDLLHOOKSTRUCT
    {
        public const uint LLKHF_EXTENDED = 0x01;
        public const uint LLKHF_INJECTED = 0x10;
        public const uint LLKHF_ALTDOWN = 0x20;
        public const uint LLKHF_UP = 0x80;

        public static KBDLLHOOKSTRUCT GetStructFromLParam(IntPtr lParam)
        {
            object? obj = Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

            return (KBDLLHOOKSTRUCT)(obj ?? throw new NullReferenceException("Resulting object is null."));
        }

        public enum Flag : uint
        {
            LLKHFExtended = LLKHF_EXTENDED,
            LLKHFInjected = LLKHF_INJECTED,
            LLKHFAltdown = LLKHF_ALTDOWN,
            LLKHFUp = LLKHF_UP,
        }

        public uint VkCode;

        public uint ScanCode;

        public Flag Flags;

        public uint Time;

        public ulong DwExtraInfo;
    }
}
