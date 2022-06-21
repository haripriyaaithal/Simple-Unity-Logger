using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace SimpleLogger.UI
{
    public class LogInfoUI : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private LogDetailsUI _logDetailsUI;
        private string _stacktrace;

        public void UpdateUI(Color color, string text, string stacktrace)
        {
            _backgroundImage.color = color;
            _text.text = text;
            _stacktrace = stacktrace;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _logDetailsUI.ShowUI(_text.text, _stacktrace, _backgroundImage.color);
        }
    }
}