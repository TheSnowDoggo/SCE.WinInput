namespace SCE.WinInput
{
    internal class LowLevelKeyboardHookEventArgs : EventArgs
    {
        public LowLevelKeyboardHookEventArgs(LowLevelKeyboardHook.MessageType messageType, KBDLLHOOKSTRUCT kbd)
        {
            MessageType = messageType;
            KBDLLHOOKSTRUCT = kbd;
        }

        public LowLevelKeyboardHook.MessageType MessageType { get; private set; }

        public KBDLLHOOKSTRUCT KBDLLHOOKSTRUCT { get; private set; }
    }
}
