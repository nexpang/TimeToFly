using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class SaveManager : MonoBehaviour
{
    //教臂沛====================
    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }
    static SaveManager _instance;
    public static SaveManager Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "GameData";
                _instance = _container.AddComponent(typeof(SaveManager)) as SaveManager;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }
    // =================================================

    public string GameDataFileName = ".json";

    private string filePath = "";

    public SaveData _gameData;
    public SaveData gameData
    {
        get
        {
            if (_gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }
    private void Awake()
    {
        filePath = string.Concat(Application.persistentDataPath, GameDataFileName);
        LoadGameData();
    }

    public void LoadGameData()
    {
        if (File.Exists(filePath))
        {
            string code = File.ReadAllText(filePath);

            byte[] bytes = System.Convert.FromBase64String(code);
            string FromJsonData = System.Text.Encoding.UTF8.GetString(bytes);
            _gameData = JsonUtility.FromJson<SaveData>(FromJsonData);
        }
        else
        {
            Debug.Log("货肺款 颇老 积己");
            _gameData = new SaveData();
        }
    }

    [ContextMenu("历厘")]
    public void SaveGameData()
    {
        string ToJsonData = JsonUtility.ToJson(gameData, true);
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(ToJsonData);
        string code = System.Convert.ToBase64String(bytes);

        File.WriteAllText(filePath, code);
    }

    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}
