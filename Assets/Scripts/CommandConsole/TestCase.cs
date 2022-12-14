using UnityEngine;

namespace CommandConsole
{
    public class TestCase
    {
        [Command]
        public static void HelloWorld(string input = "")
        {
            Debug.Log($"Hello World {input}");
        }
        
        [Command]
        private static void Multiply(int input)
        {
            Debug.Log($"{input}*{input} = {input*input}");
        }
        
        [Command]
        public void PublicTestMethod()
        {
            Debug.Log("This is: PublicTestMethod");
        }
        
        [Command]
        private void PrivateTestMethod()
        {
            Debug.Log("This is: PrivateTestMethod");
        }
    }
}