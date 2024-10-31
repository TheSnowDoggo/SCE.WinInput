namespace SCEInputSystem
{
    using System.Windows.Forms;

    internal class TestClass
    {
        private int horizontalInput;

        private int verticalInput;

        private string current = "";

        private InputController inputController = InputController.Instance;

        private ConsoleColor lastColor;

        public TestClass()
        {
            inputController.OnPressedKeyModify += TestClass_OnPressedKeyModify;
        }

        public void Update()
        {
            if (true)
            {
                string write = $"x:{horizontalInput} | y:{verticalInput}\n";

                KeyGroup pressedKey = inputController.GetPressedKeys();

                for (int i = 0; i < pressedKey.Keys; i++)
                {
                    Keys key = pressedKey[i];
                    write += $"{key},";
                }

                if (write != current)
                {
                    Console.Clear();

                    Console.WriteLine(write);

                    current = write;
                }
            }
        }

        public void TestClass_OnPressedKeyModify(object? sender, InputControllerEventArgs e)
        {
            Keys key = e.Key;

            if (key == Keys.A || key == Keys.D)
            {
                int newHorizontalInput = 0;

                if (inputController.KeyPressed(Keys.A))
                {
                    newHorizontalInput--;
                }
                if (inputController.KeyPressed(Keys.D))
                {
                    newHorizontalInput++;
                }

                horizontalInput = newHorizontalInput;
            }

            if (key == Keys.S || key == Keys.W)
            {
                int newVerticalInput = 0;

                if (inputController.KeyPressed(Keys.S))
                {
                    newVerticalInput--;
                }
                if (inputController.KeyPressed(Keys.W))
                {
                    newVerticalInput++;
                }

                verticalInput = newVerticalInput;
            }
        }
    }
}
