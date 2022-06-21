using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SimpleLogger.UI
{
    public class LogDetailsUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Image _topPanelBg;
        [SerializeField] private ScrollRect _scrollRect;

        [Header("Buttons")]
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _copyButton;

        private void OnEnable()
        {
            AddListeners();
        }

        private void OnDisable()
        {
            RemoveListeners();
            ResetScrollPosition();
        }

        private void AddListeners()
        {
            _closeButton.onClick.AddListener(OnClickClose);
            _copyButton.onClick.AddListener(OnClickCopy);
        }

        private void RemoveListeners()
        {
            _closeButton.onClick.AddListener(OnClickClose);
            _copyButton.onClick.AddListener(OnClickCopy);
        }

        private void ResetScrollPosition()
        {
            _scrollRect.verticalNormalizedPosition = 1f;
        }

        private void OnClickClose()
        {
            gameObject.SetActive(false);
        }

        private void OnClickCopy()
        {
            GUIUtility.systemCopyBuffer = _text.text;
        }

        public void ShowUI(string log, string stacktrace, Color color)
        {
            _topPanelBg.color = color;
            _text.text = $"{log}\n\n----------------------------------------------------------------\n\n{stacktrace}";
            gameObject.SetActive(true);
        }
    }
}