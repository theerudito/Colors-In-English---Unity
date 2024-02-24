using System.IO;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public static string GetPath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName);
    }
}
