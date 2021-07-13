using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data
{
    private static bool loadSave;

    private static OptionData options;
    public static bool LoadSave{
        get { return loadSave; }
        set { loadSave = value; }
    }

    public static OptionData Options{
        get { return options; }
        set { options = value; }
    }
}
