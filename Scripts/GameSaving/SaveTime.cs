using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveTime
{
    public static void SaveTimes(Timer timer)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = SetFilePath();
        //Get old times
        PlayerTime oldData = LoadTimes();
        FileStream stream = new FileStream(path, FileMode.Create);

        //Compare old times with new time
        PlayerTime data = new PlayerTime(oldData, timer);

        //Write times
        formatter.Serialize(stream, data);
        stream.Close();
        Debug.Log("Saved file to " + path);
    }

    public static PlayerTime LoadTimes()
    {
        string path = SetFilePath();
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerTime data = formatter.Deserialize(stream) as PlayerTime;

            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    private static string SetFilePath()
    {
        return Application.persistentDataPath + "/BestTimes.timer";
    }

    public static void ResetPlayerDataFile()
    {
        if(LoadTimes() != null)
        {
            File.Delete(SetFilePath());
        }
    }
}
