using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Character : MonoBehaviour {

    static List<Character> chars = new List<Character>();

    const int groundLayer = 8;
    const int charLayer = 9;
    const int charFallThroughLayer = 11;

    public int playerIndex;
    public float moveSpeed;
    public float airControl = 1;
    public float jumpSpeed;
    public float minJump;
    public float jumpAlpha;
    public Transform vaseAnchor;
    public float throwSpeed;
    public float vaseSpawnTime;
    public float vaseSpawnTimeNoVases;
    public TimerDisplay vaseTimerDisplay;
    public float mojoFromPowerup = 15;
    public float mojoDrain = 1;
    public float mojoDrainWhileThrowing = 3;
    public AudioSource throwSfx;
    public AudioSource crashSfx;
    public AudioSource powerSfx;
    public SpriteRenderer spriteRenderer;
    public Sprite armsUp;
    public Sprite armsDown;

    public float Mojo { get { return mojo;  } }
    public GameObject vasePrefab;
    public bool invulnerable;
    private float jumpBoost;
    private Vase carriedVase;
    Vector2 throwDir;
    private float vaseSpawnCredit;
    private float mojo;

    private new Rigidbody2D rigidbody;
    

    void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
        throwDir = Vector2.right * throwSpeed;
        chars.Add(this);
        GetVase();
	}

    void OnDestroy()
    {
        chars.Remove(this);
    }

    void GetVase()
    {
        if(!carriedVase)
        {
            var obj = Instantiate(vasePrefab);
            carriedVase = obj.GetComponent<Vase>();
            carriedVase.Carry(this);
        }
    }

    public void GetPowerup(Powerup pow)
    {
        Destroy(pow.gameObject);            
        mojo = mojoFromPowerup;
        if(carriedVase)
        {
            Destroy(carriedVase.gameObject);
        }
        GetVase();
        powerSfx.Play();
    }
	
	void Update () {                
        if (AnyAxis)
        {
            throwDir = GetThrowVel();
        }        
        if (carriedVase && (Winnput.AUp[playerIndex] || mojo > 0 && Winnput.A[playerIndex]))
        {
            if (mojo > 0)
            {
                carriedVase.Throw(this, Quaternion.AngleAxis(Random.Range(-22.5f, 22.5f), Vector3.forward) * throwDir);
            }
            else {
                carriedVase.Throw(this, throwDir);
            }
            carriedVase = null;
            throwSfx.Play();
        }
        if (mojo > 0)
        {
            if (!carriedVase)
            {
                mojo -= Time.deltaTime * mojoDrainWhileThrowing;
            }
            else
            {
                mojo -= Time.deltaTime * mojoDrain;
            }            
        } 
        if (!carriedVase)
        {
            float timer;
            if(mojo > 0)
            {
                timer = 0.05f;
            }
            else
            {
                timer = chars.TrueForAll(x => x.carriedVase == null) ? vaseSpawnTimeNoVases : vaseSpawnTime;
            }
            vaseSpawnCredit += Time.deltaTime / timer;
            if (vaseSpawnCredit >= 1)
            {
                vaseSpawnCredit -= 1;
                GetVase();
            }
        }        
        if(mojo > 0)
        {
            vaseTimerDisplay.Show(mojo / mojoFromPowerup);
        }
        else if(!carriedVase)
        {
            vaseTimerDisplay.Show(vaseSpawnCredit);
        }
        else
        {
            vaseTimerDisplay.Hide();
        }
        spriteRenderer.sprite = carriedVase ? armsUp : armsDown;
	}

    public void PlayVaseCrash()
    {
        if (!crashSfx.isPlaying || (crashSfx.time / crashSfx.clip.length) > Random.value)
        {
            crashSfx.Play();
        }
    }

    public bool Grounded
    {
        get
        {
            return updatesSinceGrounded <= 1;
        }
    }
    private int updatesSinceGrounded;

    public void Kill()
    {
        if(invulnerable)
        {
            return;
        }
        crashSfx.transform.SetParent(null);
        PlayVaseCrash();        
        Camera.main.GetComponent<KillCam>().ShowWinner(this);
        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if(rigidbody.velocity.y <= 0.1f && System.Array.Exists(col.contacts, x => Vector2.Dot(x.normal, Vector2.up) > 0.1f))
        {
            updatesSinceGrounded = 0;
        }
    }

    void FixedUpdate()
    {
        float horizSpeed = 0;
        if (Winnput.Left[playerIndex])
        {
            horizSpeed = -moveSpeed;
        }
        else if (Winnput.Right[playerIndex])
        {
            horizSpeed = moveSpeed;
        }
        if(!Grounded)
        {
            horizSpeed = Mathf.MoveTowards(rigidbody.velocity.x, horizSpeed, airControl * moveSpeed * 2);
        }
        rigidbody.velocity = new Vector2(horizSpeed, rigidbody.velocity.y);
        //platform fall-through
        bool fallThrough = Winnput.B[playerIndex] && Winnput.Down[playerIndex];
        gameObject.layer = fallThrough ? charFallThroughLayer : charLayer;
        //jump start
        if (Winnput.BDown[playerIndex] && !fallThrough)        
        {
            if (Grounded)
            {
                jumpBoost = jumpSpeed;
            }
        }
        //jump continue
        if(jumpBoost > 0)
        {
            if (Winnput.B[playerIndex] || jumpBoost > jumpSpeed - minJump)
            {
                float boost = Mathf.Clamp(jumpAlpha * jumpBoost * Time.deltaTime, 0, jumpBoost);
                jumpBoost -= boost;
                rigidbody.velocity = new Vector2(rigidbody.velocity.x, rigidbody.velocity.y + boost);
            }
            else
            {
                jumpBoost = 0;
            }
        }

        updatesSinceGrounded++;

    }

    private int LayerToMask(int layerIndex)
    {
        int mask = 0;
        for(int i = 0; i < 32; i++)
        {
            if (i == layerIndex)
            {
                mask |= 1 << i;
            }
        }        
        return mask;
    }   

    Vector2 GetThrowVel()
    {
        float vert = Winnput.Vertical[playerIndex];
        float horiz = Winnput.Horizontal[playerIndex];
        if(vert != 0 && horiz != 0)
        {
            vert *= 0.707f;
            horiz *= 0.707f;
        }
        return new Vector2(horiz, vert) * throwSpeed;
    }

    bool AnyAxis
    {
        get
        {
            return Winnput.Down[playerIndex] || Winnput.Up[playerIndex] || Winnput.Left[playerIndex] || Winnput.Right[playerIndex];
        }
    }
}
