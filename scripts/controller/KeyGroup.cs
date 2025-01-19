namespace SCEInputSystem
{
    public readonly struct KeyGroup
    {
        private readonly List<Keys> _list;

        public KeyGroup()
        {
            _list = new();
        }

        public Keys this[int index] { get => _list[index]; }

        /// <summary>
        /// Gets the number of keys in the key group.
        /// </summary>
        public int Keys { get => _list.Count; }

        /// <summary>
        /// Adds a new original key to the key group.
        /// </summary>
        /// <param name="key">The key to try add.</param>
        /// <returns><see langword="true"/> if the given <paramref name="key"/> is original and is succesfully added; otherwise, <see langword="false"/>.</returns>
        public bool Add(Keys key)
        {
            bool unique = !HasKey(key);
            if (unique)
                _list.Add(key);
            return unique;
        }

        public bool Remove(Keys key)
        {
            return _list.Remove(key);
        }

        public bool HasKey(Keys key)
        {
            return _list.Contains(key);
        }

        public int IndexOf(Keys key)
        {
            return _list.IndexOf(key);
        }
    }
}
