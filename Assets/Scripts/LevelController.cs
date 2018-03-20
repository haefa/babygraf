using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    public static int Level=1;

    public void Set(int level)
    {
        Level = level;
    }

    public int Get()
    {
        return Level;
    }
}