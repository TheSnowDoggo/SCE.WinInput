using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace SCE.WinInput
{
    internal class KeyConversion
    {
        #region Constants
        private const uint MAPVK_VK_TO_VSC = 0;
        private const uint MAPVK_VSC_TO_VK = 1;
        private const uint MAPVK_VK_TO_CHAR = 2;
        private const uint MAPVK_VSC_TO_VK_EX = 3;
        private const uint MAPVK_VK_TO_VSC_EX = 4;
        #endregion

        #region Imports
        [DllImport("user32.dll")]
        private static extern int ToUnicodeEx(uint wVirtKey, uint wScanCode, byte[] lpKeyState,
            [Out, MarshalAs(UnmanagedType.LPWStr)] StringBuilder pwszBuff, int cchBuff, uint wFlags, IntPtr dwhk);

        [DllImport("user32.dll")]
        private static extern IntPtr GetKeyboardLayout(uint threadId);

        [DllImport("user32.dll")]
        private static extern uint GetWindowThreadProcessId(IntPtr hwindow, out uint processId);

        [DllImport("user32.dll")]
        private static extern uint MapVirtualKeyA(uint uCode, uint uMapType);
        #endregion

        public static string KeyToString(uint vkCode)
        {
            var lpKeyState = CurrentKeyboardState();
            var hkl = CurrentKeyboardLayout();

            var strBuilder = new StringBuilder(256);

            ToUnicodeEx(vkCode, MapVirtualKeyA(vkCode, MAPVK_VK_TO_VSC), lpKeyState, strBuilder, 256, 0, hkl);
            return strBuilder.ToString();
        }

        public static char KeyToChar(uint vkCode)
        {
            string str = KeyToString(vkCode);
            return str.Length > 0 ? str[0] : '\0';
        }

        public static ConsoleKeyInfo KeyToConsoleKeyInfo(uint vkCode)
        {
            InputController.ModifierKeyStatus(out bool shift, out bool alt, out bool control);
            return new(KeyToChar(vkCode), (ConsoleKey)vkCode, shift, alt, control);
        }

        private static uint CurrentWindowThreadProcessId()
        {
            return GetWindowThreadProcessId(Process.GetCurrentProcess().MainWindowHandle, out _);
        }

        private static IntPtr CurrentKeyboardLayout()
        {
            IntPtr hkl = GetKeyboardLayout(CurrentWindowThreadProcessId());
            if (hkl == IntPtr.Zero)
                throw new Exception("Unsupported keyboard layout.");
            return hkl;
        }

        private static byte[] CurrentKeyboardState()
        {
            byte[] lpKeyState = new byte[256];
            InputController.ModifierKeyStatus(out bool shift, out bool alt, out bool control);
            if (shift)
                lpKeyState[(uint)Keys.ShiftKey] = 0x80;
            if (alt)
                lpKeyState[(uint)Keys.Menu] = 0x80;
            if (control)
                lpKeyState[(uint)Keys.ControlKey] = 0x80;
            if (Control.IsKeyLocked(Keys.CapsLock))
                lpKeyState[(uint)Keys.Capital] = 0x01;
            return lpKeyState;
        }
    }
}
