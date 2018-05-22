using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;

public static class SaveLoad {

    //public static List<SaveData> savedData = new List<SaveData>();
    public static SaveData savedData = new SaveData();

    public static void Save()
    {
        /*Debug.Log(Application.persistentDataPath);
        SaveData.current = new SaveData();
        SaveData.current.coins = TestPlayer.Instance.coins;
        SaveData.current.upgrades = TestPlayer.Instance.upgrades;*/

        //savedData.Add(SaveData.current);
        savedData = SaveData.current;
        BinaryFormatter bf = new BinaryFormatter();
        /*FileStream file = File.Create(Path.Combine(
            Application.persistentDataPath,
            "/savedData.gd"));*/
        FileStream file = File.Create(Application.persistentDataPath + "/savedData.gd");
        bf.Serialize(file, SaveLoad.savedData);
        file.Close();

        TestPlayer.Instance.InitializeUpgrades();
    }

    public static SaveData Load()
    {
        if (File.Exists(Application.persistentDataPath + "/savedData.gd"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            /*FileStream file = File.Open(
                Path.Combine(
                    Application.persistentDataPath,
                    "/savedData.gd"), 
                FileMode.Open);*/
            FileStream file = File.Open(Application.persistentDataPath + "/savedData.gd", FileMode.Open);
            //SaveLoad.savedData = (List<SaveData>)bf.Deserialize(file);
            SaveLoad.savedData = (SaveData)bf.Deserialize(file);
            SaveData.current = savedData;
            file.Close();
        }
        else
        {
            SaveData.current = new SaveData();
            savedData = SaveData.current;
        }

        TestPlayer.Instance.upgrades = savedData.upgrades;

        return savedData;
    }
}
