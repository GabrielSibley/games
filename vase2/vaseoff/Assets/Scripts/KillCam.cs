using UnityEngine;
using System.Collections;

public class KillCam : MonoBehaviour {

    public float closeUpCameraSize = 5;
    public Character graham, tom;

    public void ShowWinner(Character loser)
    {
        StartCoroutine(ShowWinnerCoroutine(loser));
    }

	private IEnumerator ShowWinnerCoroutine(Character loser)
    {
        Character winner = loser == graham ? tom : graham;
        winner.invulnerable = true;
        yield return new WaitForSeconds(0.2f);
        float startCamSize = Camera.main.orthographicSize;
        Vector3 startCamPos = Camera.main.transform.position;
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            Camera.main.orthographicSize = Mathf.Lerp(startCamSize, closeUpCameraSize, t);
            Vector3 lerp = Vector3.Lerp(startCamPos, winner.transform.position, t);
            lerp.z = Camera.main.transform.position.z;
            Camera.main.transform.position = lerp;
            yield return 0;
        }
        for(float t = 0; t < 3; t += Time.deltaTime)
        {
            Vector3 lerp = winner.transform.position;
            lerp.z = Camera.main.transform.position.z;
            Camera.main.transform.position = lerp;
            yield return 0;
        }
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
