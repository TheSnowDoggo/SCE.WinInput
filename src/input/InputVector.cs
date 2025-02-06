using SCE.WinInput;

namespace SCE
{
    public class InputVector : InputBase
    {
        public InputVector(ConsoleKey positiveKey, ConsoleKey negativeKey)
        {
            PositiveKey = positiveKey;
            NegativeKey = negativeKey;
        }

        public ConsoleKey PositiveKey { get; set; }

        public ConsoleKey NegativeKey { get; set; }

        public int Vector { get; private set; }

        public Action<int>? OnModify { get; set; }

        #region Presets
        public static InputVector PresetWS() => new(ConsoleKey.W, ConsoleKey.S);

        public static InputVector PresetDA() => new(ConsoleKey.D, ConsoleKey.A);

        public static InputVector PresetUpDown() => new(ConsoleKey.UpArrow, ConsoleKey.DownArrow);

        public static InputVector PresetRightLeft() => new(ConsoleKey.RightArrow, ConsoleKey.LeftArrow);
        #endregion

        public override void LoadKeyInfo(UISKeyInfo uisKeyInfo)
        {
            if (uisKeyInfo.InputMode is InputType.InputStream)
                return;
            var key = uisKeyInfo.KeyInfo.Key;
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
