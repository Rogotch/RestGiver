using UnityEngine;
using System;
using System.IO;
using UnityEditor.Overlays;

public class FileDataHandler
{
    private string dataDirPath  = "";
    private string dataFileName = "";

    public FileDataHandler(string newDataDirPath, string newDataFileName)
    {
        this.dataDirPath  = newDataDirPath;
        this.dataFileName = newDataFileName;
    }

    private string GetFilePath()
    {
        return Path.Combine(dataDirPath, dataFileName);
    }

    public GameData Load()
    {
        string fullPath = GetFilePath();
        GameData loadedData = null;
        if (File.Exists(fullPath))
        {
            string savedData = File.ReadAllText(fullPath);

            loadedData = JsonUtility.FromJson<GameData>(savedData);
        }
        return loadedData;
    }
    
    public void Save(GameData data)
    {
        string fullPath = GetFilePath();
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            string dataToStore = JsonUtility.ToJson(data, true);

            File.WriteAllText(fullPath, dataToStore);
        }
        catch (Exception e)
        {
            Debug.LogError("Error while trying save data to file: " + fullPath + "\n" + e);
        }
    }
}
