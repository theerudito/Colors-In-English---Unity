using UnityEngine;

public class LocalStorage
{
    public static void SaveData(string key, string value) => PlayerPrefs.SetString(key, value);
    public static string LoadData(string key) => PlayerPrefs.GetString(key);
    public static bool LoadKey(string key) => PlayerPrefs.HasKey(key);
    public static void Delete(string key) => PlayerPrefs.DeleteKey(key);
}
