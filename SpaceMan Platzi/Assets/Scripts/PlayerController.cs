using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public const float JUMP_FORCE = 8f;
    private Rigidbody2D rigidBody;
    public LayerMask groundMask; //Creamos una variable LayerMask que referiense a la capa del suelo
    private Animator anim;
    private const string STATE_ALIVE = "isAlive";
    private const string STATE_ON_THE_GROUND = "isOnTheGround";
    public float runningSpeed = 2.0f;
    private SpriteRenderer spriteRender;
    Vector3 startPosition;
    [SerializeField] //A pesar de que sea una variable privado con esto la podemos ver en el editor de unity
    private int healthPoints, manaPoints;
    public const int INITIAL_HEALTH = 100, INITIAL_MANA = 15,
                    MAX_HEALTH = 200, MAX_MANA = 30,
                    MIN_HEALTH = 0, MIN_MANA = 0;

    public const int SUPERJUMP_COST = 15;
    public const float SUPERJUMP_FORCE = 1.5F;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
        healthPoints = 50;
    }
    // Start is called before the first frame update
    void Start()
    {
        startPosition = this.transform.position;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(this.transform.position, Vector2.down * 1.5f, Color.green);
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
            {
                Jump(false);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {
                Jump(true);
            }
        }
        anim.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());
        if (healthPoints <= 0)
        {
            Die();
        }
    }

    void Jump(bool superJump)
    {
        float jumpForceFactor = JUMP_FORCE;
        if (superJump && manaPoints>=SUPERJUMP_COST)
        {
            manaPoints -= SUPERJUMP_COST;
            jumpForceFactor *= SUPERJUMP_FORCE;
        }
        if (IsTouchingTheGround())
        {
            rigidBody.AddForce(Vector2.up * jumpForceFactor, ForceMode2D.Impulse);
            GetComponent<AudioSource>().Play();
        }
            
    }
    private void FixedUpdate()
    {
        if (GameManager.sharedInstance.currentGameState == GameState.inGame)
        {
            if (rigidBody.velocity.x < runningSpeed)
            {
                if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
                {
                    rigidBody.velocity = new Vector2(runningSpeed, rigidBody.velocity.y);
                    spriteRender.flipX = false;
                }
                if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
                {
                    rigidBody.velocity = new Vector2(-runningSpeed, rigidBody.velocity.y);
                    spriteRender.flipX = true;
                }
            }
            
        }
    }


    //Nos devuelve un booleano para saber  si el personaje esta tocando o no el suelo
    bool IsTouchingTheGround()
    {
        if (Physics2D.Raycast(this.transform.position, Vector2.down,1.5f, groundMask)) //origen direccion distancia maxima y layerMask
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void Die()
    {
        this.anim.SetBool(STATE_ALIVE, false);
        GameManager.sharedInstance.GameOver();
    }

    public void StartGame()
    {
        anim.SetBool(STATE_ALIVE, true);
        anim.SetBool(STATE_ON_THE_GROUND, true);
        Invoke("RestartPosition", 0.4f);
        healthPoints = INITIAL_HEALTH;
        manaPoints = INITIAL_MANA;
    }

    void RestartPosition()
    {
        this.transform.position = startPosition;
        rigidBody.velocity = Vector2.zero;
        GameObject mainCamera = GameObject.Find("Main Camera");
        mainCamera.GetComponent<CameraFollow>().ResetCameraPosition();
    }

    public void CollectHealth(int points)
    {
        this.healthPoints += points;
        if (healthPoints >= MAX_HEALTH)
        {
            healthPoints = MAX_HEALTH;
        }
    }
    public void CollectMana(int points)
    {
        this.manaPoints += points;
        if (manaPoints >= MAX_MANA)
        {
            manaPoints = MAX_MANA;
        }
    }

    public int GetHealth()
    {
        return healthPoints;
    }
    public int GetMana()
    {
        return manaPoints;
    }
}
