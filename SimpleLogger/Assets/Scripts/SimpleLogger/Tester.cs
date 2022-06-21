using UnityEngine;

public class Tester : MonoBehaviour
{
    [SerializeField] private uint _maxLogs;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        Invoke("Log", 5f);
    }

    private void Log()
    {
        for (var index = 0; index < _maxLogs; index++)
        {
            Debug.Log($"Log: {index}");
            Debug.LogWarning($"Warning: {index}");
            Debug.LogError($"Error: {index}");
        }
    }
}