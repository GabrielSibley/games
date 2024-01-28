using UnityEngine;
using System.Collections;

public class TimerDisplay : MonoBehaviour {

    public Transform progressObj;

	public void Show(float progress)
    {
        gameObject.SetActive(true);
        progressObj.localScale = new Vector3(progress, 1, 1);
        progressObj.localPosition = new Vector3((1-progress) * -0.066f, 0, 0);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
