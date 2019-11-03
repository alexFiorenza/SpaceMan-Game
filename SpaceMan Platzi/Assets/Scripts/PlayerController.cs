using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float jumpforce = 6f;
    private Rigidbody2D rigidBody;
    public LayerMask groundMask; //Creamos una variable LayerMask que referiense a la capa del suelo
    private Animator anim;
    private const string STATE_ALIVE = "isAlive";
    private const string STATE_ON_THE_GROUND = "isOnTheGround";
    public float runningSpeed = 2.0f;
    private SpriteRenderer spriteRender;
    Vector3 startPosition;


    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
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
                Jump();
            }
        }
        
        anim.SetBool(STATE_ON_THE_GROUND, IsTouchingTheGround());
    }

    void Jump()
    {
        if (IsTouchingTheGround())
        {
            rigidBody.AddForce(Vector2.up * jumpforce, ForceMode2D.Impulse);    
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
    }

    public void StartGame()
    {
        anim.SetBool(STATE_ALIVE, true);
        anim.SetBool(STATE_ON_THE_GROUND, true);
        Invoke("RestartPosition", 0.4f);   
    }

    void RestartPosition()
    {
        this.transform.position = startPosition;
        rigidBody.velocity = Vector2.zero;
    }
}
