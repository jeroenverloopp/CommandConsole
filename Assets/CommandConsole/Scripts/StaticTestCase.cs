namespace CommandConsole
{
    public static class StaticTestCase
    {
        [Command("Static")]
        public static void HelloWorld(string input = "")
        {
            CommandManager.Instance.SendMessage($"Static :: Hello world {input}");
        }
        
        [Command("Static")]
        private static void Multiply(int input)
        {
            CommandManager.Instance.SendMessage($"Static :: {input}*{input} = {input * input}");
        }
        
        [Command("Static")]
        private static int Multiply(int inputA, int inputB)
        {
            CommandManager.Instance.SendMessage($"Static :: {inputA}*{inputB} = {inputA * inputB}");
            return inputA * inputB;
        }
    }
}