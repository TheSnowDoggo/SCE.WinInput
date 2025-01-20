using SCE;

namespace SCEInputSystem
{
    public static class InputController
    {
        private static readonly LowLevelKeyboardHook _lowLevelKeyboardHook = new();

        private static readonly Thread _thread = new(Run);

        private static readonly KeyGroup _pressedKeys = new();

        public static bool OnlyReceiveFocused { get; set; } = false;

        public static Action<ConsoleKeyInfo>? OnKeyInfoDown { get; set; }

        public static Action<ConsoleKeyInfo>? OnKeyInfoUp { get; set; }

        public static Action<ConsoleKeyInfo, int>? OnKeyInfoModify { get; set; }

        public static Action<Keys>? OnKeysDown { get; set; }

        public static Action<Keys>? OnKeysUp { get; set; }

        public static Action<Keys, int>? OnKeysModify { get; set; }

        /// <summary>
        /// Starts checking for inputs on a new thread.
        /// </summary>
        public static void Start()
        {
            if (!_thread.IsAlive)
                _thread.Start();
        }

        /// <summary>
        /// Stops checking for inputs.
        /// </summary>
        public static void Stop()
        {
            _lowLevelKeyboardHook.Unhook();

            Application.Exit();
        }

        public static bool IsKeyDown(Keys key)
        {
            return _pressedKeys.HasKey(key);
        }

        public static bool IsKeyDown(ConsoleKey consoleKey)
        {
            return IsKeyDown((Keys)consoleKey);
        }

        public static KeyGroup GetPressedKeys()
        {
            return _pressedKeys;
        }

        public static bool IsModifierKeyDown(Keys modifierKey)
        {
            return (Control.ModifierKeys & modifierKey) == modifierKey;
        }

        public static bool IsShiftPressed() => IsModifierKeyDown(Keys.Shift);

        public static bool IsAltPressed() => IsModifierKeyDown(Keys.Alt);

        public static bool IsControlPressed() => IsModifierKeyDown(Keys.Control);

        public static void ModifierKeyStatus(out bool shift, out bool alt, out bool control)
        {
            shift = IsShiftPressed();
            alt = IsAltPressed();
            control = IsControlPressed();
        }

        public static void Link(InputHandler inputHandler)
        {
            OnKeyInfoDown += inputHandler.QueueKey;
        }

        public static void Delink(InputHandler inputHandler)
        {
            OnKeyInfoDown -= inputHandler.QueueKey;
        }

        private static void Run()
        {
            _lowLevelKeyboardHook.Hook();

            _lowLevelKeyboardHook.OnKeyEvent += InputController_OnKeyEvent;

            Application.Run();
        }

        private static void InputController_OnKeyEvent(object? sender, LowLevelKeyboardHookEventArgs e)
        {
            if (OnlyReceiveFocused && !WindowUtils.ApplicationInFocus())
                return;

            uint vkCode = e.KBDLLHOOKSTRUCT.VkCode;

            Keys keys = (Keys)vkCode;
            ConsoleKeyInfo keyInfo = KeyConversion.KeyToConsoleKeyInfo(vkCode);

            int keyState = e.MessageType is LowLevelKeyboardHook.MessageType.KeyDown or LowLevelKeyboardHook.MessageType.SysKeyDown ? 1 : 0;

            if (keyState == 1)
            {
                _pressedKeys.Add(keys);
                OnKeyInfoDown?.Invoke(keyInfo);
                OnKeysDown?.Invoke(keys);
            }
            else
            {
                _pressedKeys.Remove(keys);
                OnKeyInfoUp?.Invoke(keyInfo);
                OnKeysUp?.Invoke(keys);
            }

            OnKeyInfoModify?.Invoke(keyInfo, keyState);
            OnKeysModify?.Invoke(keys, keyState);
        }
    }
}
