using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int gameStage; //-1 = not started; 0 = paper; 1 = bird; 2 = frog;
    public float[] position;
    public float[] rotation;

    public PlayerData(){
        gameStage = -1;

        position = new float[3];
        position[0] = 0f;
        position[1] = 0f;
        position[2] = 0f;

        rotation = new float[3];
        rotation[0] = 0f;
        rotation[1] = 0f;
        rotation[2] = 0f;
    }
    public PlayerData(int pGameStage, Vector3 pPosition, Vector3 pRotation){
        gameStage = pGameStage;

        position = new float[3];
        position[0] = pPosition[0];
        position[1] = pPosition[1];;
        position[2] = pPosition[2];;

        rotation = new float[3];
        rotation[0] = pRotation[0];
        rotation[1] = pRotation[1];
        rotation[2] = pRotation[2];
    }
}
