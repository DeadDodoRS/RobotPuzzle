using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MBSingleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    public static T Instance()
    {
        if (_instance == null)
            _instance = FindObjectOfType<T>();

        return _instance;
    }
}
