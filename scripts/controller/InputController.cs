namespace SCEInputSystem
{
    public static class InputController
    {
        private static readonly LowLevelKeyboardHook _lowLevelKeyboardHook = new();

        private static readonly Thread _thread = new(Run);

        private static readonly KeyGroup _pressedKeys = new();

        public static bool OnlyReceiveFocused { get; set; } = false;

        public static Action<ConsoleKeyInfo>? OnKeyDown { get; set; }

        public static Action<ConsoleKeyInfo>? OnKeyUp { get; set; }

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

        /// <summary>
        /// Determines whether the specified key is currently being pressed down.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns><see langword="true"/> if the <paramref name="key"/> is being pressed down; otherwise, <see langword="false"/>.</returns>
        public static bool IsKeyDown(Keys key)
        {
            return _pressedKeys.HasKey(key);
        }

        public static KeyGroup GetPressedKeys()
        {
            return _pressedKeys;
        }

        public static bool IsModifierKeyDown(Keys modifierKey)
        {
            return (Control.ModifierKeys & modifierKey) == modifierKey;
        }

        public static void ModifierKeyStatus(out bool shift, out bool alt, out bool control)
        {
            shift = IsModifierKeyDown(Keys.Shift);
            alt = IsModifierKeyDown(Keys.Alt);
            control = IsModifierKeyDown(Keys.Control);
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
                OnKeyDown?.Invoke(keyInfo);
            }
            else
            {
                _pressedKeys.Remove(keys);
                OnKeyUp?.Invoke(keyInfo);
            }
        }
    }
}
