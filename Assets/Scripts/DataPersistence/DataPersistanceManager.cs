using UnityEngine;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using System;

public class DataPersistanceManager : MonoBehaviour
{
    [Header("File storage config")]
    [SerializeField] private string fileName;
    private GameData gameData;
    private List<IDataPersistance> dataPersistanceObjects;
    private FileDataHandler dataHandler;
    public static DataPersistanceManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("More than one DataPersistenceManager");
            
        }
        Instance = this;
    }

    private void Start()
    {
        this.dataPersistanceObjects = FindAllDataPersistanceObjects();
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        LoadGame();
    }

    private List<IDataPersistance> FindAllDataPersistanceObjects()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None).OfType<IDataPersistance>();
        return dataPersistanceObjects.ToList();
    }

    private void NewGame()
    {
        this.gameData = new GameData();
    }

    private void LoadGame()
    {
        this.gameData = dataHandler.Load();
        //load game from file
        if (this.gameData == null)
        {
            NewGame();
        }
        //push loaded data to all other scripts
        foreach (IDataPersistance obj in dataPersistanceObjects)
        {
            obj.LoadData(gameData);
        }
    }

    private void SaveGame()
    {
        this.gameData = new GameData();
        foreach (IDataPersistance obj in dataPersistanceObjects)
        {
            obj.SaveData(ref gameData);
        }
       dataHandler.Save(gameData);
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
