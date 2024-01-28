using UnityEngine;
using System.Collections;

public class Slowdown : MonoBehaviour {

    static int count;
    static int minForSlow = 16;

	void OnEnable()
    {
        count++;
        UpdateTime();
    }

    void OnDisable()
    {
        count--;
        UpdateTime();
    }

    void UpdateTime()
    {
        Time.timeScale = count <= minForSlow ? 1 : (float)minForSlow / (float)count;        
    }
}
