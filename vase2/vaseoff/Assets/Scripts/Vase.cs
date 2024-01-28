using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Vase : MonoBehaviour {

    static List<Collider2D>[] mojoVases = new List<Collider2D>[]
    {
        new List<Collider2D>(),
        new List<Collider2D>()
    };

    const int carriedLayer = 10;
    public float gravityOnTime;

    public float horizThrowYOffset = 2f;
    public float horizThrowXOffset = 1.6f;

    public bool startFatal;
    public Sprite[] spriteSeq;
    public float flashDelay;
    private int spriteIndex;

    bool thrown;
    Character thrower;
    bool isMojoVase;

    public void Carry(Character thrower)
    {
        var collider = GetComponent<Collider2D>();
        Physics2D.IgnoreCollision(collider, thrower.GetComponent<Collider2D>(), true);
        transform.parent = thrower.transform;
        transform.position = thrower.vaseAnchor.position;
        gameObject.layer = carriedLayer;
        if(thrower.Mojo > 0)
        {
            isMojoVase = true;
            StartCoroutine(Flash());
        }        
    }

    public void Throw(Character thrower, Vector2 vel)
    {
        thrown = true;
        this.thrower = thrower;
        
        var rigidbody = GetComponent<Rigidbody2D>();
        if(isMojoVase)
        {
            var collider = GetComponent<Collider2D>();
            for(int i = 0; i < mojoVases[thrower.playerIndex].Count; i++)
            {
                Physics2D.IgnoreCollision(mojoVases[thrower.playerIndex][i], collider, true);
            }
            mojoVases[thrower.playerIndex].Add(collider);            
        }
        transform.parent = null;        
        rigidbody.isKinematic = false;
        rigidbody.position = GetThrowOffset(thrower, vel);
        rigidbody.velocity = vel;
        gameObject.layer = 0; //default
        StartCoroutine(GravityOn());
    }

    IEnumerator Flash()
    {
        var renderer = GetComponent<SpriteRenderer>();
        while (true)
        {
            renderer.sprite = spriteSeq[spriteIndex];
            spriteIndex = (spriteIndex + 1) % spriteSeq.Length;
            yield return new WaitForSeconds(flashDelay);
        }
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if(!c.enabled)
        {
            return; //platform "collision"
        }
        if (thrown || startFatal)
        {
            var hitChar = c.collider.GetComponent<Character>();
            if (hitChar)
            {
                hitChar.Kill();
            }
            StartCoroutine(Death());
        }
    }

    IEnumerator GravityOn()
    {
        yield return new WaitForSeconds(gravityOnTime);
        GetComponent<Rigidbody2D>().gravityScale = 1;
    }

    //L/R throws have an offset
    Vector2 GetThrowOffset(Character thrower, Vector2 velocity)
    {        
        float dot = Vector2.Dot(velocity.normalized, Vector2.right);
        float yOffset = Mathf.Abs(dot) * horizThrowYOffset;
        float xOffset = dot * horizThrowXOffset;
        return (Vector2)transform.position + new Vector2(xOffset, yOffset);
    }

    IEnumerator Death()
    {
        if (thrower)
        {
            thrower.PlayVaseCrash();
        }
        GetComponent<Rigidbody2D>().isKinematic = true;
        var collider = GetComponent<Collider2D>();
        if (isMojoVase)
        {
            mojoVases[thrower.playerIndex].Remove(collider);            
        }
        Destroy(collider);
        var renderer = GetComponent<Renderer>();
        for (int i = 0; i < 4; i++)
        {
            renderer.enabled = !renderer.enabled;
            yield return new WaitForSeconds(0.16f);
        }
        Destroy(gameObject);
    }

    void OnDestroy()
    {
        if (isMojoVase && thrower != null)
        {
            var collider = GetComponent<Collider2D>();
            if (collider)
            {
                mojoVases[thrower.playerIndex].Remove(collider);
            }
        }
    }
}
