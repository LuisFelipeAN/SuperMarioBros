using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptiEnemy : MonoBehaviour {
    public float velocidadeX=-0.5f;//velocidade x do inimigo
    public float raio=1.5f;//raio de movimentacao do inimigo

    private float velocidadeAtual;//velocidade atual do inimigo
    private float minX,maxX;//x min e max de movimentacao dentro do raio

    //componentes do inimigo
    private SpriteRenderer spriteRenderer;
    private BoxCollider2D boxCollider2D;
    private Rigidbody2D rigidbody2D;
    private Animator animator;

    private bool direitaOcupada, esquerdaOcupada,onFloor;//variaveis utilizadas pelos 4 raycasts 
    private float baseY;//y base aonde o inimigo estava
    private bool die;//se o inimigo morreu

    GameObject player;//palyer necessario para ver se matou o inimigo


    // Use this for initialization
    void Start () {
        velocidadeAtual = velocidadeX;//inicia a velocidade atual com a definida

        //pega os components do objeto
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();

        baseY = transform.position.y;//pega o y base onde o inimigo se encontra

        atualizaRaio();//atualiza o raio de movimentacao

        player = GameObject.FindGameObjectWithTag("Player");//encontra o player
        die = false;
    }

    //define um novo raio de movimentacao e chamada ao inicar e toda vez que um inimigo cai da plataforma
	private void atualizaRaio()
    {
        minX = transform.position.x - spriteRenderer.bounds.size.x / 2 - raio;
        maxX = transform.position.x + spriteRenderer.bounds.size.x / 2 + raio;
    }

	// Update is called once per frame
	void Update () {
        if (!die)
        {
            if (direitaOcupada || esquerdaOcupada)//se tem alguma coisa do lado inverte a velocidade
            {
                velocidadeAtual = -velocidadeAtual;
            }

            transform.Translate(velocidadeAtual * Time.deltaTime, 0, 0);//desloca o nimigo

            if (transform.position.x - spriteRenderer.size.x / 2 < minX)//verifica se chegou ao limite do raio a esquerda se sim inverte
            {
                velocidadeAtual = -velocidadeAtual;
            }
            else if (transform.position.x + spriteRenderer.size.x / 2 > maxX)//verifica se chegou ao limite do raio a direita se sim inverte
            {
                velocidadeAtual = -velocidadeAtual;
            }
        }
        transform.rotation = Quaternion.identity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject obj = collision.gameObject;
        atualizaRayCasts();
        if (obj.tag != "Player")//se colidiu com algo que nao é o player
        {
           
            if (direitaOcupada||esquerdaOcupada)//se for do lado inverte velocidade
            {
                velocidadeAtual = -velocidadeAtual;
            }
            if (transform.position.y < baseY && onFloor)//se for em baixo este inimigo caiu atualiza o raio dele e o y base
            {
                atualizaRaio();
                baseY = transform.position.y;
            }
        }
        else
        {

            if (player.transform.position.y>transform.position.y+spriteRenderer.bounds.size.y/2&& player.GetComponent<ScriptMario>().GetVelocidadeVertical < 0)//se o jogador estiver em cima 
            {
               
                transform.Translate(0, -spriteRenderer.bounds.size.y * 0.25f, 0);//desce o inimigo um pouco
                rigidbody2D.bodyType = RigidbodyType2D.Kinematic;//muda o tipo dele para kinematic para nao cair
                boxCollider2D.enabled = false;//desabilita o collider
                animator.SetBool("Die", true);//chama a animcao do inimigo morrendo
                die = true;
            }
           
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
       
    }

    //fincao que lanca 3 ray casts um em cada sentido (baixo,direita,esquerda)
    public void atualizaRayCasts()
    {
        if (boxCollider2D.enabled)
        {
            boxCollider2D.enabled = false;
            onFloor = Physics2D.Raycast(transform.position, Vector2.down, spriteRenderer.bounds.size.y / 2 + 0.03f);
            direitaOcupada = Physics2D.Raycast(transform.position, Vector2.right, spriteRenderer.bounds.size.x / 2 + 0.03f);
            esquerdaOcupada = Physics2D.Raycast(transform.position, Vector2.left, spriteRenderer.bounds.size.x / 2 + 0.03f);
            boxCollider2D.enabled = true;
        }
        else
        {
            onFloor = Physics2D.Raycast(transform.position, Vector2.down, spriteRenderer.bounds.size.y / 2 + 0.03f);
            direitaOcupada = Physics2D.Raycast(transform.position, Vector2.right, spriteRenderer.bounds.size.x / 2 + 0.03f);
            esquerdaOcupada = Physics2D.Raycast(transform.position, Vector2.left, spriteRenderer.bounds.size.x / 2 + 0.03f);
        }
    }

}
