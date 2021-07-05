using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    private static string path = Application.persistentDataPath + "player.cglWhorigami";
    public static void SavePlayer(int gameStage, Vector3 position, Vector3 rotation){
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(gameStage, position, rotation);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadPlayer(){
        if(File.Exists(path)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        }
        else {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void Deletesave(){
        if(File.Exists(path)) File.Delete(path);
        else Debug.LogError("Save file not found in " + path);
    }
}
