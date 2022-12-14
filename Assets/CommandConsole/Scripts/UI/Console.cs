using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CommandConsole.UI
{
    public class Console : MonoBehaviour
    {
        [SerializeField] private TMP_InputField _commandLine;
        [SerializeField] private Button _submitButton;

        private List<string> _previousCommands = new List<string>();
        private int _previousCommandIndex = -1;
        private bool _commandLineSelected = false;
        
        private void Awake()
        {
            if (_commandLine == null)
            {
                Debug.LogWarning("Console :: _commandLine not set.");
            }
            else
            {
                _commandLine.onSubmit.RemoveAllListeners();
                _commandLine.onSelect.RemoveAllListeners();
                _commandLine.onDeselect.RemoveAllListeners();
                
                _commandLine.onSubmit.AddListener(SendCommand);
                _commandLine.onSelect.AddListener(OnCommandLineSelected);
                _commandLine.onDeselect.AddListener(OnCommandLineDeselected);
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
        
        private void Update()
        {
            if (_commandLineSelected)
            {
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    if (_previousCommands.Count == 0)
                    {
                        return;
                    }

                    SetPreviousCommandIndexUp();
                    SetPreviousCommand();
                }
                else if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    if (_previousCommands.Count == 0)
                    {
                        return;
                    }

                    SetPreviousCommandIndexDown();
                    SetPreviousCommand();
                }
            }
        }

        private void OnCommandLineSelected(string t)
        {
            _commandLineSelected = true;
        }
        
        private void OnCommandLineDeselected(string t)
        {
            _commandLineSelected = false;
        }

        private void OnDestroy()
        {
            if (_commandLine != null)
            {
                _commandLine.onSubmit.RemoveAllListeners();
                _commandLine.onSelect.RemoveAllListeners();
                _commandLine.onDeselect.RemoveAllListeners();
            }

            if (_submitButton != null)
            {
                _submitButton.onClick.RemoveAllListeners();
            }
        }

        private void SendCommand(string command)
        {
            if (string.IsNullOrEmpty(command))
            {
                return;
            }
            AddPreviousCommand(_commandLine.text);
            CommandManager.Instance.RunCommand(_commandLine.text);
            _commandLine.text = string.Empty;
            _commandLine.ActivateInputField();
            _commandLine.caretPosition = 0;
            _commandLine.MoveTextEnd(false);
        }

        private void AddPreviousCommand(string command)
        {
            _previousCommandIndex = -1;
            if (_previousCommands.Count > 0 && _previousCommands[_previousCommands.Count - 1] == command)
            {
                return;
            }
            _previousCommands.Add(_commandLine.text);
        }

        private void SetPreviousCommandIndexUp()
        {
            if (_previousCommandIndex == -1)
            {
                _previousCommandIndex = _previousCommands.Count - 1;
            }
            else if(_previousCommandIndex > 0)
            {
                _previousCommandIndex--;
            }
        }
        
        private void SetPreviousCommandIndexDown()
        {
            if (_previousCommandIndex == -1)
            {
                return;
            }
            if(_previousCommandIndex < _previousCommands.Count-1)
            {
                _previousCommandIndex++;
            }
        }

        private void SetPreviousCommand()
        {
            _commandLine.text = _previousCommands[_previousCommandIndex];
            _commandLine.MoveTextEnd(false);
        }
    }
}