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
    private bool direitaOcupada, esquerdaOcupada,onFloor;
    private float baseY;
    // Use this for initialization
    void Start () {
        velocidadeAtual = velocidadeX;
        spriteRenderer = GetComponent<SpriteRenderer>();
       
        boxCollider2D = GetComponent<BoxCollider2D>();
        baseY = transform.position.y;
        atualizaRaio();
    }
	private void atualizaRaio()
    {
        minX = transform.position.x - spriteRenderer.bounds.size.x / 2 - raio;
        maxX = transform.position.x + spriteRenderer.bounds.size.x / 2 + raio;
    }
	// Update is called once per frame
	void Update () {
        transform.Translate(velocidadeAtual * Time.deltaTime, 0, 0);
        if (transform.position.x - spriteRenderer.size.x / 2 < minX)
        {
            velocidadeAtual = -velocidadeAtual;
        }else if(transform.position.x + spriteRenderer.size.x / 2 > maxX){
            velocidadeAtual = -velocidadeAtual;
        }
        transform.rotation = Quaternion.identity;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        if (obj.tag != "Player")
        {
            atualizaRayCasts();
            if (direitaOcupada||esquerdaOcupada)
            {
                velocidadeAtual = -velocidadeAtual;
            }
            if (transform.position.y < baseY && onFloor)
            {
                atualizaRaio();
                baseY = transform.position.y;
            }
            if (obj.tag == "Enemy")
            {
                atualizaRaio();
            }
        }
    }
    public void atualizaRayCasts()
    {
        boxCollider2D.enabled = false;
        onFloor = Physics2D.Raycast(transform.position, Vector2.down, spriteRenderer.bounds.size.y / 2 + 0.03f);
        direitaOcupada = Physics2D.Raycast(transform.position, Vector2.right, spriteRenderer.bounds.size.x / 2 + 0.06f);
        esquerdaOcupada = Physics2D.Raycast(transform.position, Vector2.left, spriteRenderer.bounds.size.x / 2 + 0.06f);
        boxCollider2D.enabled = true;
    }

}
