using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    private static bool loadSave;

    private static float abilityAddTimer;
    private static OptionData options;
    public static bool LoadSave{
        get { return loadSave; }
        set { loadSave = value; }
    }

    public static OptionData Options{
        get { return options; }
        set { options = value; }
    }

    public static float AbilityAddTimer{
        get {return abilityAddTimer;}
        set {abilityAddTimer = value;}
    }

    private void Update()
    {
        if(abilityAddTimer > 0f) abilityAddTimer -= Time.deltaTime;
    }
}
