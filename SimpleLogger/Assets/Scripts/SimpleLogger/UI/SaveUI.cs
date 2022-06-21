using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using SimpleLogger.IO;

namespace SimpleLogger.UI
{
    public class SaveUI : MonoBehaviour
    {
        [Tooltip("Create personal access token on GitHub and add the token string here. " +
            "Token can be created using this link https://github.com/settings/tokens")]
        [SerializeField] private string _personalAccessToken;

        [Header("UI")]
        [Space][SerializeField] private TMP_InputField _fileNameInput;
        [SerializeField] private TMP_InputField _descriptionInput;
        [SerializeField] private TextMeshProUGUI _resultText;
        [SerializeField] private Button _uploadButton;
        [SerializeField] private Button _saveLocallyButton;
        [SerializeField] private Button _closeButton;

        private ILogWriter _gistsLogWriter;
        private ILogWriter _localLogWriter;
        private Coroutine _waitingCoroutine;
        private string _result;
        private bool _responseReceived;

        private string FileName => _fileNameInput.text;
        private string Description => _descriptionInput.text;

        private void OnEnable()
        {
            if (string.IsNullOrEmpty(_personalAccessToken))
                ShowGistsErrorMessage();
            else
                _gistsLogWriter = new GistsWriter(_personalAccessToken.Trim());

            AddListeners();
        }

        private void OnDisable()
        {
            if (_waitingCoroutine != null)
                StopCoroutine(_waitingCoroutine);

            UpdateResultText(string.Empty);
            RemoveListeners();
        }

        private void AddListeners()
        {
            _uploadButton.onClick.AddListener(OnClickUpload);
            _saveLocallyButton.onClick.AddListener(OnClickSaveLocally);
            _closeButton.onClick.AddListener(OnClickClose);
        }

        private void RemoveListeners()
        {
            _uploadButton.onClick.RemoveListener(OnClickUpload);
            _saveLocallyButton.onClick.RemoveListener(OnClickSaveLocally);
            _closeButton.onClick.RemoveListener(OnClickClose);
        }

        private void OnClickUpload()
        {
            if (_gistsLogWriter == null)
            {
                UpdateResultText("Please assign access token to upload the logs.");
                ShowGistsErrorMessage();
                return;
            }

            if (!HasFileName()) return;

            StartWaitForResponseCoroutine();
            _gistsLogWriter.WriteLogs(LogManager.Instance.GetLogs(), FileName, Description, OnCompleteUpload);
            UpdateResultText("Uploading...");
        }

        private void OnClickSaveLocally()
        {
            if (!HasFileName()) return;

            StartWaitForResponseCoroutine();
            _localLogWriter = _localLogWriter ?? new LocalLogWriter();
            _localLogWriter.WriteLogs(LogManager.Instance.GetLogs(), FileName, Description, OnCompleteSave);
            UpdateResultText("Saving...");
        }

        private void StartWaitForResponseCoroutine()
        {
            if (_waitingCoroutine != null)
                StopCoroutine(_waitingCoroutine);
            _waitingCoroutine = StartCoroutine(WaitForResponse());
        }

        private bool HasFileName()
        {
            var hasFileName = !string.IsNullOrEmpty(FileName);
            if (!hasFileName)
                UpdateResultText("Please enter the file name.");

            return hasFileName;
        }

        private void OnClickClose()
        {
            gameObject.SetActive(false);

            _fileNameInput.text = string.Empty;
            _descriptionInput.text = string.Empty;
            _resultText.text = string.Empty;
        }

        private IEnumerator WaitForResponse()
        {

            yield return new WaitUntil(() => _responseReceived);

            UpdateUI();
            _responseReceived = false;
            _result = string.Empty;
        }

        private void OnCompleteUpload(string url, string error)
        {
            if (!string.IsNullOrEmpty(error))
                Debug.LogError($"Failed to post the logs: {error}");
            else
                this._result = url;
            _responseReceived = true;
        }

        private void OnCompleteSave(string path, string error)
        {
            if (!string.IsNullOrEmpty(path))
            {
                this._result = path;
                _responseReceived = true;
            }
            else
                UpdateResultText($"Problem saving file: {error}");
        }

        private void UpdateUI()
        {
            if (string.IsNullOrEmpty(_result))
            {
                UpdateResultText("Something went wrong when uploading the logs.");
                return;
            }

            GUIUtility.systemCopyBuffer = _result;
            UpdateResultText($"{_result} - Copied to clipboard");
        }

        private void UpdateResultText(string msg)
        {
            _resultText.text = msg;
        }

        private void ShowGistsErrorMessage()
        {
            Debug.LogError("Please assign personal access token in the inspector inorder to upload the logs. " +
                    "You can create access token here -> https://github.com/settings/tokens", this);
        }
    }
}