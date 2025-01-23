using SCE.WinInput;

namespace SCE
{
    public class InputVector : IKeyInfoModifyReceiver
    {
        public InputVector(ConsoleKey positiveKey, ConsoleKey negativeKey, bool autoload = false)
        {
            PositiveKey = positiveKey;
            NegativeKey = negativeKey;
            if (autoload)
                InputController.LoadKIMR(this);
        }

        public bool IsActive { get; set; } = true;

        public ConsoleKey PositiveKey { get; set; }

        public ConsoleKey NegativeKey { get; set; }

        public int Vector { get; private set; }

        public Action<int>? OnModify { get; set; }

        #region Presets
        public static InputVector PresetWS(bool autoLoad = false) => new(ConsoleKey.W, ConsoleKey.S, autoLoad);

        public static InputVector PresetDA(bool autoLoad = false) => new(ConsoleKey.D, ConsoleKey.A, autoLoad);

        public static InputVector PresetUpDown(bool autoLoad = false) => new(ConsoleKey.UpArrow, ConsoleKey.DownArrow, autoLoad);

        public static InputVector PresetRightLeft(bool autoLoad = false) => new(ConsoleKey.RightArrow, ConsoleKey.LeftArrow, autoLoad);
        #endregion

        public void KeyInfoModify(ConsoleKeyInfo keyInfo, int keyState)
        {
            if (!IsActive)
                return;
            var key = keyInfo.Key;
            if (key == PositiveKey || key == NegativeKey)
            {
                Vector = 0;
                if (InputController.IsKeyDown(PositiveKey))
                    Vector++;
                if (InputController.IsKeyDown(NegativeKey))
                    Vector--;
                OnModify?.Invoke(Vector);
            }
        }
    }
}
