namespace SCEInputSystem
{
    using System.Windows.Forms;

    internal class InputControllerEventArgs : EventArgs
    {
        public InputControllerEventArgs(int keyState, Keys key)
        {
            if (keyState != 0 && keyState != 1)
            {
                throw new ArgumentException("Keystate must be either 0 or 1.");
            }

            KeyState = keyState;
            Key = key;
        }

        public int KeyState { get; private set; }

        public Keys Key { get; private set; }
    }
}
