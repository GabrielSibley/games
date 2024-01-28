using UnityEngine;
using System.Collections;

public class SuddenDeath : MonoBehaviour {

    public float suddenDeathTime = 120;
    public float powerupInterval = 10;
    public float powerupChance = 0.35f;
    public bool isSuddenDeath;
    public float safeZoneShrinkSpeed = 1;
    public float vasesPerSecondPerUnit = 3;

    public Vase vasePrefab;
    public GameObject powerupPrefab;

    private float levelWidth;
    private float safeZone;
    private float vases;

    void Start()
    {
        levelWidth = Camera.main.orthographicSize * Camera.main.aspect;
        safeZone = levelWidth;
        StartCoroutine(SpawnPowerups());
    }

	// Update is called once per frame
	void Update () {
        suddenDeathTime -= Time.deltaTime;
        if(suddenDeathTime <= 0)
        {
            isSuddenDeath = true;
        }
        if(isSuddenDeath)
        {
            safeZone = Mathf.Max(0, safeZone - safeZoneShrinkSpeed * Time.deltaTime);
            vases += Time.deltaTime * (levelWidth - safeZone) * 2 * vasesPerSecondPerUnit;
        }
        while(vases > 1)
        {
            vases -= 1;
            float spawnX = Random.Range(safeZone, levelWidth) * Mathf.Sign(Random.value - 0.5f);
            var vase = (Instantiate(vasePrefab, transform.position + new Vector3(spawnX, 0, 0), Quaternion.identity) as Vase).gameObject;
            Destroy(vase, 3);
        }
	}

    IEnumerator SpawnPowerups()
    {
        while(!isSuddenDeath)
        {
            yield return new WaitForSeconds(powerupInterval);
            if(Random.value < powerupChance)
            {
                float spawnX = (Random.value + Random.value - 1) * safeZone;
                Instantiate(powerupPrefab, transform.position + new Vector3(spawnX, 0, 0), Quaternion.identity);                
            }
        }
    }
}
