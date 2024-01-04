using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SavingSystem
{
    public static void Save()
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "revolutionProgress.src");
        FileStream stream = new FileStream(path, FileMode.Create);

        SaveData sd = new SaveData();

        formatter.Serialize(stream, sd);
        stream.Close();
    }

    public static SaveData Load()
    {
        string path = Path.Combine(Application.persistentDataPath, "revolutionProgress.src");
        if (File.Exists(path)) 
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveData sd = formatter.Deserialize(stream) as SaveData;
            stream.Close();

            return sd;
        }
        else 
        {
            Debug.Log("Save file not found/existing");
            return null;
        }
    }
}
