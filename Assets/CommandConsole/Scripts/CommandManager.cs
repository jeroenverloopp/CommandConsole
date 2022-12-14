using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CommandConsole.Models;

namespace CommandConsole
{
    public class CommandManager
    {
        public static CommandManager Instance
        {
            get { return _instance ??= new CommandManager(); }
        }
        private static CommandManager _instance;

        public List<Message> Messages
        {
            get
            {
                if (_messages == null)
                {
                    return new List<Message>();
                }

                return _messages;
            }
        }

        public Action<Message> OnMessageSend;

        private Dictionary<string, List<MethodInfo>> _commandMethodDictionary;
        private List<Message> _messages;


        private CommandManager()
        {
            if (_instance != null)
            {
                throw new Exception("Can't initialize a Singleton. This should happen automatically");
            }
            AddCommands();
        }

        public bool RunCommand(string command)
        {
            string[] commandList = command.Split(' ');
            object[] parameters = CreateParameters(commandList);
            int parameterCount = parameters.Length;
            string methodName = commandList[0];

            if (_commandMethodDictionary.ContainsKey(methodName) == false)
            {
                SendMessage($"Command \"{command}\" does not exist", MessageType.Warning);
                return false;
            }

            List<MethodInfo> availableMethods = _commandMethodDictionary[methodName];
            if (availableMethods.Count <= parameterCount || availableMethods[parameterCount] == null)
            {
                List<int> availableIndexes = new List<int>();
                for (int i = 0 ; i < availableMethods.Count ; i++)
                {
                    if (availableMethods[i] == null)
                    {
                        continue;
                    }
                    availableIndexes.Add(i);
                    
                }
                SendMessage($"Command \"{command}\" should be called with {string.Join(" or ", availableIndexes)} parameters. Not with \"{parameterCount}\"", MessageType.Warning);
                return false;
            }

            MethodInfo method = availableMethods[parameterCount];
            if (method == null || method.DeclaringType == null)
            {
                SendMessage($"Command \"{command}\" is invalid because there is no Method attached", MessageType.Error);
                return false;
            }
            object target = method.IsStatic ? null : Activator.CreateInstance(method.DeclaringType);
            try
            {
                method.Invoke(target, parameters);
            }
            catch (Exception exception)
            {
                SendMessage($"Command \"{command}\" :: {exception.Message}", MessageType.Error);
                return false;
            }

            return true;
        }

        public void SendMessage(string text)
        {
            _messages ??= new List<Message>();
            Message message = new Message { Text = text, Type = MessageType.Normal };
            _messages.Add(message);
            OnMessageSend?.Invoke(message);
        }
        
        public void SendMessage(string text, MessageType type)
        {
            _messages ??= new List<Message>();
            Message message = new Message { Text = text, Type = type };
            _messages.Add(message);
            OnMessageSend?.Invoke(message);
        }
        
        private void AddCommands()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var commandMethods = assembly.GetTypes()
                .SelectMany(t => t.GetMethods( BindingFlags.Static|BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic))
                .Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                .ToArray();
            
            _commandMethodDictionary = new Dictionary<string, List<MethodInfo>>();
            foreach (MethodInfo method in commandMethods)
            {
                var commandAttribute = method.CustomAttributes.First(a => a.AttributeType == typeof(CommandAttribute));
                string prefix = commandAttribute.ConstructorArguments.First().Value as string;
                
                int parameterCount = method.GetParameters().Length;
                string methodName = prefix != null ? $"{prefix}.{method.Name}" : method.Name;
                if (_commandMethodDictionary.ContainsKey(methodName) == false)
                {
                    _commandMethodDictionary[methodName] = new List<MethodInfo>();
                }

                while (_commandMethodDictionary[methodName].Count <= parameterCount)
                {
                    _commandMethodDictionary[methodName].Add(null);
                }

                _commandMethodDictionary[methodName][parameterCount] = method;
            }
        }

        private object[] CreateParameters(string[] commandList)
        {
            List<object> parameters = new List<object>();
            if (commandList.Length > 1)
            {
                for (int i = 1; i < commandList.Length; i++)
                {
                    string param = commandList[i];
                    int num;
                    if (int.TryParse(param, out num))
                    {
                        parameters.Add(num);
                        continue;
                    }
                    parameters.Add(param);
                }
            }

            return parameters.ToArray();
        }
        

    }
}