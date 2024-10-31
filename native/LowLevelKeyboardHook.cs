﻿namespace SCEInputSystem
{
    using System;
    using System.Diagnostics;
    using System.Runtime.InteropServices;

    internal class LowLevelKeyboardHook
    {
        #region Constants
        public const int HC_ACTION = 0;

        public const int WH_KEYBOARD_LL = 13;

        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        #endregion

        public event EventHandler<LowLevelKeyboardHookEventArgs>? OnKeyEvent;

        private readonly LowLevelKeyboardProc proc;

        private IntPtr hookID = IntPtr.Zero;

        public LowLevelKeyboardHook()
        {
            proc = HookCallback;
        }

        public enum MessageType
        {
            KeyDown = WM_KEYDOWN,
            KeyUp = WM_KEYUP,
            SysKeyUp = WM_SYSKEYUP,
            SysKeyDown = WM_SYSKEYDOWN,
        };

        public void Hook()
        {
            hookID = SetHook(proc);
        }

        public bool Unhook()
        {
            return UnhookWindowsHookEx(hookID);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode == HC_ACTION)
            {
                MessageType messageType = (MessageType)wParam;

                KBDLLHOOKSTRUCT kbd = KBDLLHOOKSTRUCT.GetStructFromLParam(lParam);

                LowLevelKeyboardHookEventArgs eventArgs = new(messageType, kbd);

                OnKeyEvent?.Invoke(this, eventArgs);
            }
            return CallNextHookEx(hookID, nCode, wParam, lParam);
        }

        public delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        #region Imports
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool UnhookWindowsHookEx(IntPtr hHook);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr CallNextHookEx(IntPtr hHook, int code, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern IntPtr GetModuleHandle(string lpModuleName);
        #endregion
    }
}
