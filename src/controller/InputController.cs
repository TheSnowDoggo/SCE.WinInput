namespace SCE.WinInput
{
    public static class InputController
    {
        private static readonly LowLevelKeyboardHook _lowLevelKeyboardHook = new();

        private static readonly Thread _thread = new(Run);

        private static readonly KeyMap _keysMap = new();

        public static bool OnlyReceiveFocused { get; set; } = false;

        #region OnKey
        public static Action<UISKeyInfo>? OnKeyEvent { get; set; }
        #endregion

        public static void Start()
        {
            if (!_thread.IsAlive)
                _thread.Start();
        }

        public static void Stop()
        {
            _lowLevelKeyboardHook.Unhook();
            Application.Exit();
        }

        #region KeyStatus
        public static bool IsKeyDown(Keys keys)
        {
            return _keysMap.IsKeyDown(keys);
        }

        public static bool IsKeyDown(ConsoleKey consoleKey)
        {
            return IsKeyDown((Keys)consoleKey);
        }

        public static bool IsModifierKeyDown(Keys modifierKey)
        {
            return (Control.ModifierKeys & modifierKey) == modifierKey;
        }

        public static bool IsShiftPressed()
        {
            return IsModifierKeyDown(Keys.Shift);
        }

        public static bool IsAltPressed()
        {
            return IsModifierKeyDown(Keys.Alt);
        }

        public static bool IsControlPressed()
        {
            return IsModifierKeyDown(Keys.Control);
        }

        public static void ModifierKeyStatus(out bool shift, out bool alt, out bool control)
        {
            shift = IsShiftPressed();
            alt = IsAltPressed();
            control = IsControlPressed();
        }
        #endregion

        #region Link
        public static void Link(InputHandler inputHandler)
        {
            OnKeyEvent += inputHandler.QueueKey;
        }

        public static void Delink(InputHandler inputHandler)
        {
            OnKeyEvent -= inputHandler.QueueKey;
        }
        #endregion

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

            if ((keyState == 1 && !IsKeyDown(keys)) || (keyState == 0 && IsKeyDown(keys)))
            {
                _keysMap[keys] = keyState == 1;

                OnKeyEvent?.Invoke(new UISKeyInfo(keyInfo, (InputType)keyState));
            }

            if (keyState == 1)
                OnKeyEvent?.Invoke(new UISKeyInfo(keyInfo, InputType.InputStream));
        }
    }
}
