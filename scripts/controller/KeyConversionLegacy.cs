namespace SCEInputSystem
{
    public static class KeyConversionLegacy
    {
        public struct ShiftPair
        {
            public ShiftPair(char normal, char shift)
            {
                Normal = normal;
                Shift = shift;
            }

            public char Normal;

            public char Shift;
        }

        private static Dictionary<Keys, ShiftPair> UKShiftDictionary = new()
        {
            { Keys.Oem8, new('`', '¬') },
            { Keys.D1, new('1', '!') },
            { Keys.D2, new('2', '"') },
            { Keys.D3, new('3', '£') },
            { Keys.D4, new('4', '$') },
            { Keys.D5, new('5', '%') },
            { Keys.D6, new('6', '^') },
            { Keys.D7, new('7', '&') },
            { Keys.D8, new('8', '*') },
            { Keys.D9, new('9', '(') },
            { Keys.D0, new('0', ')') },
            { Keys.A, new('a', 'A') },
            { Keys.B, new('b', 'B') },
            { Keys.C, new('c', 'C') },
            { Keys.D, new('d', 'D') },
            { Keys.E, new('e', 'E') },
            { Keys.F, new('f', 'F') },
            { Keys.G, new('g', 'G') },
            { Keys.H, new('h', 'H') },
            { Keys.I, new('i', 'I') },
            { Keys.J, new('j', 'J') },
            { Keys.K, new('k', 'K') },
            { Keys.L, new('l', 'L') },
            { Keys.M, new('m', 'M') },
            { Keys.N, new('n', 'N') },
            { Keys.O, new('o', 'O') },
            { Keys.P, new('p', 'P') },
            { Keys.Q, new('q', 'Q') },
            { Keys.R, new('r', 'R') },
            { Keys.S, new('s', 'S') },
            { Keys.T, new('t', 'T') },
            { Keys.U, new('u', 'U') },
            { Keys.V, new('v', 'V') },
            { Keys.W, new('w', 'W') },
            { Keys.X, new('x', 'X') },
            { Keys.Y, new('y', 'Y') },
            { Keys.Z, new('z', 'Z') },
            { Keys.OemMinus, new('-', '_') },
            { Keys.Oemplus, new('=', '+') },
            { Keys.OemOpenBrackets, new('[', '{') },
            { Keys.Oem6, new(']', '}') },
            { Keys.Oem1, new(';', ':') },
            { Keys.Oemtilde, new('\'', '@') },
            { Keys.Oemcomma, new(',', '<') },
            { Keys.OemPeriod, new('.', '>') },
            { Keys.OemQuestion, new('/', '?') },
            { Keys.Oem7, new('\\', '|') },
        };

        private static Dictionary<Keys, char> UKStandaloneDictionary = new()
        {
            { Keys.Space, ' ' },
            { Keys.Divide, '/' },
            { Keys.Multiply, '*'},
            { Keys.Subtract, '-' },
            { Keys.Add, '+' },
            { Keys.Decimal, '.' },
            { Keys.NumPad0, '0' },
            { Keys.NumPad1, '1' },
            { Keys.NumPad2, '2' },
            { Keys.NumPad3, '3' },
            { Keys.NumPad4, '4' },
            { Keys.NumPad5, '5' },
            { Keys.NumPad6, '6' },
            { Keys.NumPad7, '7' },
            { Keys.NumPad8, '8' },
            { Keys.NumPad9, '9' },
        };

        public static char KeysToCharUK(Keys keys, bool shift)
        {
            if (UKShiftDictionary.ContainsKey(keys))
                return shift ? UKShiftDictionary[keys].Shift : UKShiftDictionary[keys].Normal;
            if (UKStandaloneDictionary.ContainsKey(keys))
                return UKStandaloneDictionary[keys];
            return '\x00';
        }
    }
}
