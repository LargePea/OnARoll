using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadGame
{
    public static bool ReloadFromMenu = true;
    private static string _gameName = "LDFirstLevelPass";
    public static void ReloadGame(bool reloadFromMenu)
    {
        ReloadFromMenu = reloadFromMenu;
        SceneManager.LoadScene(_gameName);
    }
}
