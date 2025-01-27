using SCE.WinInput;

namespace SCE
{
    public class InputMoveVector : InputBase
    {
        private readonly InputVector _verticalVector;

        private readonly InputVector _horizontalVector;

        public InputMoveVector(ConsoleKey upKey, ConsoleKey downKey, ConsoleKey rightKey, ConsoleKey leftKey)
        {
            _verticalVector = new(upKey, downKey) { OnModify = OnModify };
            _horizontalVector = new(rightKey, leftKey) { OnModify = OnModify };
        }

        #region Presets
        public static InputMoveVector PresetWASD() => new(ConsoleKey.W, ConsoleKey.S, ConsoleKey.D, ConsoleKey.A);

        public static InputMoveVector PresetUpDownLeftRight() => new(ConsoleKey.UpArrow, ConsoleKey.DownArrow, ConsoleKey.RightArrow, ConsoleKey.LeftArrow);
        #endregion

        #region Keys
        public ConsoleKey UpKey
        {
            get => _verticalVector.PositiveKey;
            set => _verticalVector.PositiveKey = value;
        }

        public ConsoleKey DownKey
        {
            get => _verticalVector.NegativeKey;
            set => _verticalVector.NegativeKey = value;
        }

        public ConsoleKey RightKey
        {
            get => _horizontalVector.PositiveKey;
            set => _horizontalVector.PositiveKey = value;
        }

        public ConsoleKey LeftKey
        {
            get => _horizontalVector.NegativeKey;
            set => _horizontalVector.NegativeKey = value;
        }
        #endregion

        public Vector2Int RawMoveVector { get; private set; }

        public Vector2 NormalizedMoveVector { get; private set; }

        public Action<Vector2Int>? OnModifyRaw { get; set; }

       public Action<Vector2>? OnModifyNormalized { get; set; }
 
        public override void LoadKeyInfo(UISKeyInfo uisKeyInfo)
        {
            if (uisKeyInfo.InputMode is InputType.InputStream)
                return;
            _verticalVector.LoadKeyInfo(uisKeyInfo);
            _horizontalVector.LoadKeyInfo(uisKeyInfo);
        }

        private void OnModify(int _)
        {
            RawMoveVector = new(_horizontalVector.Vector, _verticalVector.Vector);
            var vec2 = RawMoveVector.ToVector2();
            NormalizedMoveVector = vec2.Magnitude != 0 ? vec2.Normalize() : Vector2.Zero;

            OnModifyRaw?.Invoke(RawMoveVector);
            OnModifyNormalized?.Invoke(NormalizedMoveVector);
        }
    }
}
