using CommandConsole.Models;
using TMPro;
using UnityEngine;

namespace CommandConsole.UI
{
    public class Message : MonoBehaviour
    {
        [SerializeField] private Color _normalColor , _warningColor, _errorColor;
        [SerializeField] private TMP_InputField _textInput;

        public void Set(Models.Message message)
        {
            switch (message.Type)
            {
                case MessageType.Normal:
                    _textInput.textComponent.color = _normalColor;
                    break;
                case MessageType.Warning:
                    _textInput.textComponent.color = _warningColor;
                    break;
                case MessageType.Error:
                    _textInput.textComponent.color = _errorColor;
                    break;
            }
            _textInput.text = $"- {message.Text}";
        }
    }
}