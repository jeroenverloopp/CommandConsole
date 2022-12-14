using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CommandConsole.UI
{
    public class Console : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _commandLine;
        [SerializeField] private Button _submitButton;


        private void Awake()
        {
            if (_commandLine == null)
            {
                Debug.LogWarning("Console :: _commandLine not set.");
            }
            else
            {
                _commandLine.onSubmit.RemoveAllListeners();
                _commandLine.onSubmit.AddListener(SendCommand);
                if (_submitButton == null)
                {
                    Debug.LogWarning("Console :: _submitButton not set.");
                }
                else
                {
                    _submitButton.onClick.RemoveAllListeners();
                    _submitButton.onClick.AddListener(()=>{SendCommand(_commandLine.text);});
                }
            }
        }

        private void OnDestroy()
        {
            if (_commandLine != null)
            {
                _commandLine.onSubmit.RemoveAllListeners();
            }

            if (_submitButton != null)
            {
                _submitButton.onClick.RemoveAllListeners();
            }
        }

        private void SendCommand(string command)
        {
            CommandManager.Instance.RunCommand(_commandLine.text);
            _commandLine.text = string.Empty;
        }
    }
}