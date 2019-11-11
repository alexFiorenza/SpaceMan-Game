using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float runningSpeed = 1.5f;

    Rigidbody2D r_body;
    public int enemyDamage = 10;
    public bool facingRight=false;
    private Vector3 startPosition;

    private void Awake()
    {
        r_body = GetComponent<Rigidbody2D>();
        startPosition = this.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        float currentRunningSpeed = runningSpeed;

        if (facingRight)
        {
            //Looking right
            currentRunningSpeed = runningSpeed;
            this.transform.eulerAngles = new Vector3(0, 180, 0);
        }
        else
        {
            //Looking left
            currentRunningSpeed = -runningSpeed;
            this.transform.eulerAngles = Vector3.zero;
        }

        if (GameManager.sharedInstance.currentGameState == GameState.inGame)  //Si estamos en el juego se movera en  la direccion correspondiente
        {
            r_body.velocity = new Vector2(currentRunningSpeed, r_body.velocity.y);
        }
        else if (GameManager.sharedInstance.currentGameState == GameState.gameOver)
        {
            r_body.velocity = Vector3.zero;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Coin")
        {
            return;
        }
        if (collision.tag == "Player")
        {
            collision.gameObject.GetComponent<PlayerController>().CollectHealth(-enemyDamage);
            return;
        }
        //Si llegamos aqui, no chocamos ni con monedas,ni con players
        facingRight = !facingRight;
    }
}
