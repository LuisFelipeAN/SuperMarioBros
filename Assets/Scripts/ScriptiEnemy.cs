using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptiEnemy : MonoBehaviour {
    public float velocidadeX=-0.5f;
    public float raio=2f;
    private float velocidadeAtual;
    private float minX,maxX;
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private bool direitaOcupada, esquerdaOcupada,onFloor,aboveHead;
    private float baseY;
    private bool inverteu;
    private Rigidbody2D rigidbody2D;
    private Animator animator;
    private bool die;
    GameObject player;


    // Use this for initialization
    void Start () {
        velocidadeAtual = velocidadeX;
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        baseY = transform.position.y;
        atualizaRaio();
        player = GameObject.FindGameObjectWithTag("Player");
        die = false;
    }
	private void atualizaRaio()
    {
        minX = transform.position.x - spriteRenderer.bounds.size.x / 2 - raio;
        maxX = transform.position.x + spriteRenderer.bounds.size.x / 2 + raio;
    }
	// Update is called once per frame
	void Update () {
        if (!die)
        {
            
            if (direitaOcupada || esquerdaOcupada)
            {
                velocidadeAtual = -velocidadeAtual;
                inverteu = true;
            }

            transform.Translate(velocidadeAtual * Time.deltaTime, 0, 0);
            if (transform.position.x - spriteRenderer.size.x / 2 < minX)
            {
                inverteu = true;
                velocidadeAtual = -velocidadeAtual;
            }
            else if (transform.position.x + spriteRenderer.size.x / 2 > maxX)
            {
                velocidadeAtual = -velocidadeAtual;
                inverteu = true;
            }
            if (transform.position.x - spriteRenderer.size.x / 2 > minX && transform.position.x + spriteRenderer.size.x / 2 < maxX && inverteu)
            {
                inverteu = false;
            }
        }
        transform.rotation = Quaternion.identity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        atualizaRayCasts();
        if (obj.tag != "Player")
        {
           
            if (direitaOcupada||esquerdaOcupada)
            {
                velocidadeAtual = -velocidadeAtual;
                inverteu = true;
            }
            if (transform.position.y < baseY && onFloor)
            {
                atualizaRaio();
                baseY = transform.position.y;
            }
        }
        else
        {

            if (player.transform.position.y>transform.position.y+spriteRenderer.bounds.size.y/2&& player.GetComponent<ScriptMario>().GetVelocidadeVertical < 0)
            {
               
                transform.Translate(0, -spriteRenderer.bounds.size.y * 0.25f, 0);
                boxCollider2D.enabled = false;
                rigidbody2D.bodyType = RigidbodyType2D.Kinematic;
                animator.SetBool("Die", true);
                die = true;
            }
           
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
       
    }

    public void atualizaRayCasts()
    {
        if (boxCollider2D.enabled)
        {
            boxCollider2D.enabled = false;
            onFloor = Physics2D.Raycast(transform.position, Vector2.down, spriteRenderer.bounds.size.y / 2 + 0.03f);
            direitaOcupada = Physics2D.Raycast(transform.position, Vector2.right, spriteRenderer.bounds.size.x / 2 + 0.03f);
            esquerdaOcupada = Physics2D.Raycast(transform.position, Vector2.left, spriteRenderer.bounds.size.x / 2 + 0.03f);
            aboveHead = Physics2D.Raycast(transform.position, Vector2.up, spriteRenderer.bounds.size.y / 2 + 0.03f);
            boxCollider2D.enabled = true;
        }
        else
        {
            aboveHead = Physics2D.Raycast(transform.position, Vector2.up, spriteRenderer.bounds.size.y / 2 + 0.03f);
            onFloor = Physics2D.Raycast(transform.position, Vector2.down, spriteRenderer.bounds.size.y / 2 + 0.03f);
            direitaOcupada = Physics2D.Raycast(transform.position, Vector2.right, spriteRenderer.bounds.size.x / 2 + 0.03f);
            esquerdaOcupada = Physics2D.Raycast(transform.position, Vector2.left, spriteRenderer.bounds.size.x / 2 + 0.03f);
        }
    }

}
