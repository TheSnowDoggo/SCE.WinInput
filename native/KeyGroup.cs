namespace SCEInputSystem
{
    internal readonly struct KeyGroup
    {
        private readonly List<Keys> keyList;

        public KeyGroup()
        {
            keyList = new();
        }

        public Keys this[int index] { get => keyList[index]; }

        /// <summary>
        /// Gets the number of keys in the key group.
        /// </summary>
        public int Keys { get => keyList.Count; }

        /// <summary>
        /// Adds a new original key to the key group.
        /// </summary>
        /// <param name="key">The key to try add.</param>
        /// <returns><see langword="true"/> if the given <paramref name="key"/> is original and is succesfully added; otherwise, <see langword="false"/>.</returns>
        public bool Add(Keys key)
        {
            bool newKey = !HasKey(key);

            if (newKey)
            {
                keyList.Add(key);
            }

            return newKey;
        }

        public bool Remove(Keys key)
        {
            return keyList.Remove(key);
        }

        public bool HasKey(Keys key)
        {
            return keyList.Contains(key);
        }

        public int IndexOf(Keys key)
        {
            return keyList.IndexOf(key);
        }
    }
}
