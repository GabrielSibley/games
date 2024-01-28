using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

    new public SpriteRenderer renderer;
    public Sprite[] spriteSeq;
    public float flashDelay;
    public float startVel = -5;
    private int spriteIndex;

	// Use this for initialization
	IEnumerator Start() {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, startVel);
        while (true)
        {
            
            renderer.sprite = spriteSeq[spriteIndex];
            spriteIndex = (spriteIndex + 1) % spriteSeq.Length;
            yield return new WaitForSeconds(flashDelay);
            //edge of world die
            if(transform.position.y < -60)
            {
                Destroy(gameObject);
            }
        }
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        var character = other.GetComponent<Character>();
        if(character)
        {
            character.GetPowerup(this);
        }
    }
}
