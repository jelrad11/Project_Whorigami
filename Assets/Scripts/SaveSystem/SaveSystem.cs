using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem
{
    private static string path = Application.persistentDataPath + "player.cglWhorigami";
    private static string optPath = Application.persistentDataPath + "options.cglWhorigami";

    public static void SavePlayer(int gameStage, Vector3 position, Vector3 rotation, string cameraState, bool canTransformLong, bool canTransformShort, bool canFly, bool canTurn){
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(gameStage, position, rotation, cameraState, canTransformLong, canTransformShort, canFly, canTurn);

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
            //Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
    public static void Deletesave(){
        if(File.Exists(path)) File.Delete(path);
        //else Debug.LogError("Save file not found in " + path);
    }


    public static void SaveOptions(float subtitles, float audio){
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(optPath, FileMode.Create);

        OptionData data = new OptionData(subtitles, audio);

        formatter.Serialize(stream, data);
        stream.Close();
    }
    public static OptionData LoadOptions(){
        if(File.Exists(optPath)){
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(optPath, FileMode.Open);

            OptionData data = formatter.Deserialize(stream) as OptionData;
            stream.Close();

            return data;
        }
        else {
            Debug.LogError("Option file not found in " + optPath);
            return null;
        }
    }
}
