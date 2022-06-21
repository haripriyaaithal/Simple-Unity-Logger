using System.Collections.Generic;
using System.Linq;
using UnityEditor;

public static class SimpleLoggerMenu
{
    private const string ENABLE_PATH = "Tools/SimpleLogger/Enable";
    private const string DISABLE_PATH = "Tools/SimpleLogger/Disable";
    private const string DEFINE_SYMBOL = "USE_SIMPLE_LOGGER";

    [MenuItem(ENABLE_PATH)]
    public static void EnableLogger()
    {
        AddSymbol();
    }

    [MenuItem(ENABLE_PATH, true)]
    private static bool EnableValidator()
    {
        return !IsScriptingSymbolAdded();
    }

    [MenuItem(DISABLE_PATH)]
    public static void DisableLogger()
    {
        RemoveSymbol();
    }

    [MenuItem(DISABLE_PATH, true)]
    private static bool DisableValidator()
    {
        return IsScriptingSymbolAdded();
    }

    private static bool IsScriptingSymbolAdded()
    {
        return GetDefines().Contains(DEFINE_SYMBOL);
    }

    private static List<string> GetDefines()
    {
        var definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        return definesString.Split(';').ToList();
    }

    private static void AddSymbol()
    {

        var allDefines = GetDefines();
        allDefines.Add(DEFINE_SYMBOL);
        SaveDefines(allDefines);
    }

    private static void RemoveSymbol()
    {
        var allDefines = GetDefines();
        allDefines.Remove(DEFINE_SYMBOL);
        SaveDefines(allDefines);
    }

    private static void SaveDefines(List<string> defines)
    {
        PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup,
                    string.Join(";", defines.ToArray()));
    }
}
