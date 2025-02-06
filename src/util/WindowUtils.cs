using System.Runtime.InteropServices;

namespace SCE.WinInput
{
    internal class WindowUtils
    {
        // thanks to https://stackoverflow.com/questions/7162834/determine-if-current-application-is-activated-has-focus

        #region Imports
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int GetWindowThreadProcessId(IntPtr handle, out int pid);
        #endregion

        public static bool ApplicationInFocus()
        {
            IntPtr fHandle = GetForegroundWindow();
            if (fHandle == IntPtr.Zero)
                return false;

            GetWindowThreadProcessId(fHandle, out int fId);

            return Environment.ProcessId == fId;
        }
    }
}
