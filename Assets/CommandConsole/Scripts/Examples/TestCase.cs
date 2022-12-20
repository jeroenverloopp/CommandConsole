namespace CommandConsole.Examples
{
    public class TestCase
    {
        [Command]
        public void HelloWorld()
        {
            CommandManager.Instance.SendMessage($"Hello world!");
        }

        [Command]
        public void HelloWorld(string input)
        {
            CommandManager.Instance.SendMessage($"Hello world {input}");
        }
        
        [Command]
        private void Multiply(int input)
        {
            CommandManager.Instance.SendMessage($"{input}*{input} = {input * input}");
        }
        
        [Command]
        private int Multiply(int inputA, int inputB)
        {
            CommandManager.Instance.SendMessage($"{inputA}*{inputB} = {inputA * inputB}");
            return inputA * inputB;
        }
    }
}