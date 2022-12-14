using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace CommandConsole
{
    public class CommandManager
    {
        public static CommandManager Instance
        {
            get { return _instance ??= new CommandManager(); }
        }
        
        private static CommandManager _instance;

        private Dictionary<string, MethodInfo> _commandMethodDictionary;


        private CommandManager()
        {
            if (_instance != null)
            {
                throw new Exception("Can't initialize a Singleton. This should happen automatically");
            }
            AddCommands();
        }

        private void AddCommands()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var commandMethods = assembly.GetTypes()
                .SelectMany(t => t.GetMethods( BindingFlags.Static|BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic))
                .Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                .ToArray();
            
            _commandMethodDictionary = new Dictionary<string, MethodInfo>();
            foreach (MethodInfo method in commandMethods)
            {
                _commandMethodDictionary[method.Name] = method;
            }
        }

        public void RunCommand(string command)
        {
            string[] commandList = command.Split(' ');
            string methodName = commandList[0];

            if (_commandMethodDictionary.ContainsKey(methodName))
            {
                MethodInfo method = _commandMethodDictionary[methodName];
                if (method == null || method.DeclaringType == null)
                {
                    Debug.Log($"Command \"{methodName}\" has not method attached");
                }
                
                object target = method.IsStatic ? null : Activator.CreateInstance(method.DeclaringType);
                _commandMethodDictionary[methodName].Invoke(target, CreateParameters(commandList));
            }
            else
            {
                Debug.Log($"Command \"{command}\" does not exist");
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