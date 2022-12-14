using CommandConsole.Models;
using UnityEngine;
using UnityEngine.UI;

namespace CommandConsole.UI
{
    public class MessageList : MonoBehaviour
    {

        [SerializeField] private Message _messagePrefab;
        [SerializeField] private RectTransform _messageContainer;
        [SerializeField] private ScrollRect _scrollRect;
        
        private void Awake()
        {
            CommandManager.Instance.OnMessageSend += OnMessageSend;
        }

        private void OnDestroy()
        {
            CommandManager.Instance.OnMessageSend += OnMessageSend;
        }

        private void OnMessageSend(Models.Message message)
        {
            if (_messagePrefab == null)
            {
                Debug.LogError($"MessageList :: _messagePrefab is null");
                return;
            }
            if (_messageContainer == null)
            {
                Debug.LogError($"MessageList :: _messageContainer is null");
                return;
            }

            Message messageComponent = Instantiate(_messagePrefab, _messageContainer);
            messageComponent.Set(message);
            SnapToLastMessage();
        }
        
        private void SnapToLastMessage()
        {
            _messageContainer.anchoredPosition = new Vector2(_messageContainer.anchoredPosition.x, 0);
        }
    }
}