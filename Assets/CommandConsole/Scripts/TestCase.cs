namespace CommandConsole
{
    public class TestCase
    {
        [Command]
        public static void HelloWorld(string input = "")
        {
            CommandManager.Instance.SendMessage($"Hello world {input}");
        }
        
        [Command]
        private static void Multiply(int input)
        {
            CommandManager.Instance.SendMessage($"{input}*{input} = {input * input}");
        }
        
        [Command]
        private static int Multiply(int inputA, int inputB)
        {
            CommandManager.Instance.SendMessage($"{inputA}*{inputB} = {inputA * inputB}");
            return inputA * inputB;
        }
    }
}