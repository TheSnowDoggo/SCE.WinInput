namespace SCEInputSystem
{
    using System.Diagnostics;
    using System.Windows.Forms;

    internal static class MainClass
    {
        private static InputController inputController = InputController.Instance;

        private readonly static Stopwatch frameTimer = new();

        internal static void Main()
        {
            inputController.Start();

            TestClass test = new();

            OnUpdate += test.Update;

            frameTimer.Start();
            while (true)
            {
                OnUpdate?.Invoke();

                frameTimer.Restart();
            }
        }

        public static Action? OnUpdate { get; set; }

        public static double DeltaTime { get => frameTimer.Elapsed.TotalMilliseconds / 1000; }
    }
}