namespace SCEInputSystem
{
    using System;

    using static LowLevelKeyboardHook;

    internal sealed class InputController
    {
        public LowLevelKeyboardHook lowLevelKeyboardHook;

        public event EventHandler<InputControllerEventArgs>? OnKeyEvent;

        public event EventHandler<InputControllerEventArgs>? OnPressedKeyModify;

        private static readonly Lazy<InputController> lazy = new(() => new());

        private readonly Thread thread;

        private readonly KeyGroup pressedKeys = new();

        private InputController()
        {
            lowLevelKeyboardHook = new();

            thread = new(Run);
        }

        public static InputController Instance { get => lazy.Value; }

        public Action<Keys>? OnKeyPress { get; set; }
        public Action<Keys>? OnKeyRelease { get; set; }

        /// <summary>
        /// Starts checking for inputs on a new thread.
        /// </summary>
        public void Start()
        {
            thread.Start();
        }

        /// <summary>
        /// Stops checking for inputs.
        /// </summary>
        public void Stop()
        {
            lowLevelKeyboardHook.Unhook();

            Application.Exit();
        }

        /// <summary>
        /// Determines whether the specified key is currently being pressed down.
        /// </summary>
        /// <param name="key">The key to check.</param>
        /// <returns><see langword="true"/> if the <paramref name="key"/> is being pressed down; otherwise, <see langword="false"/>.</returns>
        public bool KeyPressed(Keys key)
        {
            return pressedKeys.HasKey(key);
        }

        public KeyGroup GetPressedKeys()
        {
            return pressedKeys;
        }

        private void Run()
        {
            lowLevelKeyboardHook.Hook();

            lowLevelKeyboardHook.OnKeyEvent += InputController_OnKeyEvent;

            Application.Run();
        }

        private void InputController_OnKeyEvent(object? sender, LowLevelKeyboardHookEventArgs e)
        {
            Keys key = (Keys)e.KBDLLHOOKSTRUCT.VkCode;

            int keyState = e.MessageType is MessageType.KeyDown or MessageType.SysKeyDown ? 1 : 0;

            InputControllerEventArgs eventArgs = new(keyState, key);

            if ((keyState == 1 && pressedKeys.Add(key)) || (keyState == 0 && pressedKeys.Remove(key)))
            {
                OnPressedKeyModify?.Invoke(this, eventArgs);
            }

            OnKeyEvent?.Invoke(this, eventArgs);

            if (keyState == 1)
            {
                OnKeyPress?.Invoke(key);
            }
            else
            {
                OnKeyRelease?.Invoke(key);
            }
        }
    }
}
