using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Mopsicus.InfiniteScroll;
using SimpleLogger.Data;
using System.Collections;

namespace SimpleLogger.UI
{
    public class LogUIManager : MonoBehaviour
    {
        [SerializeField] private GameObject _postGistsUIPanel;

        [Header("Infinite scroll settings")]
        [SerializeField] private InfiniteScroll _infiniteScroll;
        [SerializeField] private int _height;
        [SerializeField] private ScrollRect _scrollRect;

        [Header("Toggles")]
        [SerializeField] private Toggle _logToggle;
        [SerializeField] private Toggle _warningToggle;
        [SerializeField] private Toggle _errorToggle;

        [Header("Colors")]
        [SerializeField] private Color _logBackgroundColor;
        [SerializeField] private Color _warningBackgroundColor;
        [SerializeField] private Color _errorBackgroundColor;

        [Header("Button")]
        [SerializeField] private Button _refreshButton;
        [SerializeField] private Button _saveButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _clearLogsButton;
        [SerializeField] private Button _moveToBottomButton;
        [SerializeField] private Button _moveToTopButton;

        [Space][SerializeField] private TMP_InputField _searchInput;

        private IEnumerable<LogData> _logs;

        private void OnEnable()
        {
            AddListeners();
            StartCoroutine(InitLogsCoroutine());
        }

        private IEnumerator InitLogsCoroutine()
        {
            var eof = new WaitForEndOfFrame();
            yield return eof;
            yield return eof;

            InitLogs();
        }

        private void OnDisable()
        {
            RemoveListeners();
        }

        public void InitLogs(string searchString = "")
        {
            if (string.IsNullOrEmpty(searchString))
                searchString = _searchInput.text;

            _logs = LogManager.Instance.GetLogs(GetSelectedLogTypes(), searchString);
            var logCount = _logs.Count();
            _infiniteScroll.InitData(logCount);
        }

        #region Listeners

        private void AddListeners()
        {
            _infiniteScroll.OnHeight += OnHeight;
            _infiniteScroll.OnFill += OnFill;
            _searchInput.onValueChanged.AddListener(OnSearchInput);
            _refreshButton.onClick.AddListener(OnClickRefresh);
            _saveButton.onClick.AddListener(OnClickPostLogsToServer);
            _closeButton.onClick.AddListener(OnClickClose);
            _clearLogsButton.onClick.AddListener(OnClickClear);
            _moveToBottomButton.onClick.AddListener(OnClickMoveToBottom);
            _moveToTopButton.onClick.AddListener(OnClickMoveToTop);
        }

        private void RemoveListeners()
        {
            _infiniteScroll.OnHeight -= OnHeight;
            _infiniteScroll.OnFill -= OnFill;
            _searchInput.onValueChanged.RemoveListener(OnSearchInput);
            _refreshButton.onClick.RemoveListener(OnClickRefresh);
            _saveButton.onClick.RemoveListener(OnClickPostLogsToServer);
            _closeButton.onClick.RemoveListener(OnClickClose);
            _clearLogsButton.onClick.RemoveListener(OnClickClear);
            _moveToBottomButton.onClick.RemoveListener(OnClickMoveToBottom);
            _moveToTopButton.onClick.RemoveListener(OnClickMoveToTop);
        }

        private int OnHeight(int index)
        {
            return _height;
        }

        private void OnFill(int index, GameObject go)
        {
            var data = _logs.ElementAt(index);
            var logInfo = go.GetComponent<LogInfoUI>();
            var color = GetColorForLogType(data.logType);
            logInfo.UpdateUI(color, data.info, data.stackstrace);
        }

        #endregion

        private byte GetSelectedLogTypes()
        {
            var logs = (byte)0;
            if (_logToggle.isOn)
                logs |= (byte)UnityLogType.Log;
            if (_warningToggle.isOn)
                logs |= (byte)UnityLogType.Warning;
            if (_errorToggle.isOn)
                logs |= (byte)UnityLogType.Error;

            return logs;
        }

        private Color GetColorForLogType(UnityLogType type)
        {
            switch (type)
            {
                case UnityLogType.Log:
                    return _logBackgroundColor;
                case UnityLogType.Warning:
                    return _warningBackgroundColor;
                default:
                    return _errorBackgroundColor;
            }
        }

        #region UI Callbacks

        private void OnSearchInput(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                InitLogs();
                return;
            }

            InitLogs(input);
        }

        private void OnClickRefresh()
        {
            _searchInput.text = string.Empty;
            InitLogs();
        }

        private void OnClickPostLogsToServer()
        {
            _postGistsUIPanel.SetActive(true);
        }

        private void OnClickClose()
        {
            gameObject.SetActive(false);
        }

        private void OnClickClear()
        {
            LogManager.Instance.ClearLogs();
            InitLogs();
        }

        private void OnClickMoveToBottom()
        {
            StartCoroutine(StartScroll(0));
        }

        private void OnClickMoveToTop()
        {
            StartCoroutine(StartScroll(1));
        }

        private IEnumerator StartScroll(float val)
        {
            _scrollRect.verticalNormalizedPosition = val;
            yield return null;
        }

        #endregion
    }
}