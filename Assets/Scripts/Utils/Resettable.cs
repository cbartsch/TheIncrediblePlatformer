using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resettable {

    public interface IResettable
    {
        void Reset(bool resetLevelPosition);
    }

    public static void ResetAll(Level level, bool resetLevelPosition = false)
    {
        if (!level)
        {
            return;
        }
        foreach (var c in level.GetComponentsInChildren<IResettable>())
        {
            c.Reset(resetLevelPosition);
        }
    }
}
