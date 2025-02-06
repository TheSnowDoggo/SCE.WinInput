namespace SCE.WinInput
{
    public readonly struct KeyMap
    {
        private readonly bool[] _keys = new bool[256];

        public KeyMap()
        {
        }

        public bool this[Keys keys]
        {
            get => _keys[(int)keys];
            set => _keys[(int)keys] = value;
        }

        public void Add(Keys keys)
        {
            this[keys] = true;
        }

        public void Remove(Keys keys)
        {
            this[keys] = false;
        }

        public bool IsKeyDown(Keys keys)
        {
            return this[keys];
        }
    }
}
