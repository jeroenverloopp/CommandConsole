using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace CommandConsole.UI
{
    public class ConsoleSizeDragger : MonoBehaviour
    {
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _console;

        public void DragHandler(BaseEventData data)
        {
            PointerEventData pointerData = (PointerEventData)data;
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)_canvas.transform,
                pointerData.position,
                _canvas.worldCamera,
                out position
            );

            Vector2 canvasPoint =_canvas.transform.TransformPoint(position);
            canvasPoint.x = Mathf.Max(300, canvasPoint.x);
            canvasPoint.y = Mathf.Max(45, canvasPoint.y);

            transform.position = canvasPoint;
            _console.sizeDelta = canvasPoint - Vector2.one*15;
        }
    }
}