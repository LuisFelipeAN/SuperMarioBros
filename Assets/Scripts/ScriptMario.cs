using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptMario : MonoBehaviour {
    public float velocidadeLateral=3;//velocidade de deslocamento lateral
    public float inpulsoPulo=7.5f;//impulso do pulo

    public float minX;//x minimo estipulado pela camera dessa forma o player nao pode voltar uma vez que ja avancou no mapa

    //componentes do objeto
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2D;
    private BoxCollider2D boxCollider2D;
    private Animator animator;

    private bool pulando;//se o jogador esta pulando
    private int dir;//ultima direcao que foi acionada
    private bool blockLeft = false, blockRight = false;//durante um pulo a primeira direcao de deslocamento acionada é a predominante dessa forma bloqueia a alteracao de direcao durante o pulo

    private bool gameOver = false,win=false;
    private Vector2 leftFoot, rightFoot;//centro dos rays casts um no pe esquerdo e outro no pe direito do personagem
    private bool andando;//se o personagem esta andando
    private bool onFloor, direitaOcupada, esquerdaOcupada;//utilizados pelos raycasts 

    public float GetVelocidadeHorizontal//se o personagem estiver andando retorna a velocidade leteral senao retorna 0 para a camera ficar parada caso o jogador esteja parado no mei da mesma
    {
        get {

            if (andando) {
                return velocidadeLateral;
            }
            else
            {
                return 0;
            }
        }
    }
    public float GetVelocidadeVertical//para saber se o personagem esta caindo
    {
        get
        {
            return rigidbody2D.velocity.y;
           
        }
    }
    // Use this for initialization
    void Start () {
       // pega os componentes do objeto
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D=GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        //inicializa algumas variaveis
        onFloor = false;
        pulando = false;
        leftFoot = new Vector2();
        leftFoot = new Vector2();
        dir = 0;
    }

	// Update is called once per frame
	void Update () {
        if (!gameOver&&!win)//enquanto em jogo
        {
            atualizaRayCasts();//verifica se tem alguma coisa a direita, esquerda, abaixo


            if (onFloor && pulando)//se tem alguma coisa abaixo e esta pulando termina a animcao do pulo
            {
                pulando = false;
                animator.SetBool("Jumping", false);
                blockLeft = false;
                blockRight = false;
            }

            if (Input.GetKeyDown(KeyCode.Space) && onFloor)//se jogador esta no chão ele pode pular
            {
                rigidbody2D.AddForce(Vector3.up * inpulsoPulo);
                pulando = true;
                animator.SetBool("Jumping", true);
            }

            if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                animator.SetBool("WalkingRight", false);
                dir = 0;
            }
            if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                animator.SetBool("WalkingLeft", false);
                dir = 0;
            }


            if (Input.GetKeyDown(KeyCode.RightArrow) && !direitaOcupada)//se a direnta ta livre atualiza a direcao
            {
                dir = 1;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && !esquerdaOcupada && transform.position.x - spriteRenderer.bounds.size.x / 2 > minX)//se a esquerda ta livre e ele nao esta passando o limte esquedo definido pela camera entao aturaliza a direcao
            {
                dir = -1;
            }


           


            if (pulando && !blockLeft && !blockRight)//se ele ta pulando faz com que a primeira direcao precionada depois do pulo seja a predominante
            {
                if (dir == 1)
                {
                    blockLeft = true;
                }
                else if (dir == -1)
                {
                    blockRight = true;
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow) && !esquerdaOcupada && transform.position.x - spriteRenderer.bounds.size.x / 2 > minX && !blockLeft)//se a esquerda ta livre e ele nao esta passando o limte esquedo definido pela camera e nao tem bloqueio entao movimenta para esquerda
            {
                transform.Translate(Vector3.left * velocidadeLateral * Time.deltaTime);
                animator.SetBool("WalkingLeft", true);
                andando = true;
            }
            if (Input.GetKey(KeyCode.RightArrow) && !direitaOcupada && !blockRight)//se nao tem nada na direita e nao tem bloqueio entao desloca para direita
            {
                transform.Translate(Vector3.right * velocidadeLateral * Time.deltaTime);
                animator.SetBool("WalkingRight", true);
                andando = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))//se levantar alguma das teclas de movimentacao o personagem nao vai estar mais andando
            {
                andando = false;
            }
            
        }else if (win)//funcao que desloca o personagem da bandeira ate o castelo no final da fase
        {
            if (animator.GetBool("WalkToTheCastle")&&transform.position.x<59.2)
            {
                transform.Translate(Vector3.right * velocidadeLateral * Time.deltaTime);

            }else if(animator.GetBool("WalkToTheCastle") && transform.position.x > 59.2)
            {
                spriteRenderer.enabled = false;
            }
        }

        transform.rotation = Quaternion.identity;

    }

    //fincao que lanca 4 ray casts um em cada sentido (baixo pe esquerdo, baixo pe direito,direita,esquerda)
    public void atualizaRayCasts()
    {
        if (boxCollider2D.enabled)
        {
            boxCollider2D.enabled = false;
            leftFoot.x = transform.position.x - spriteRenderer.bounds.size.x / 2;
            leftFoot.y = transform.position.y;

            rightFoot.x= transform.position.x + spriteRenderer.bounds.size.x / 2;
            rightFoot.y= transform.position.y;

            if (Physics2D.Raycast(leftFoot, Vector2.down, spriteRenderer.bounds.size.y / 2 + 0.03f)||
                Physics2D.Raycast(rightFoot, Vector2.down, spriteRenderer.bounds.size.y / 2 + 0.03f))
            {
                onFloor =true ;
            }
            else
            {
                onFloor = false;
            }
          
            direitaOcupada = Physics2D.Raycast(transform.position, Vector2.right, spriteRenderer.bounds.size.x / 2 + 0.03f);
            esquerdaOcupada = Physics2D.Raycast(transform.position, Vector2.left, spriteRenderer.bounds.size.x / 2 + 0.03f);
            boxCollider2D.enabled = true;
        }
        else
        {
            leftFoot.x = transform.position.x - spriteRenderer.bounds.size.x / 2;
            leftFoot.y = transform.position.y;

            rightFoot.x = transform.position.x + spriteRenderer.bounds.size.x / 2;
            rightFoot.y = transform.position.y;

            if (Physics2D.Raycast(leftFoot, Vector2.down, spriteRenderer.bounds.size.y / 2 + 0.03f) ||
                Physics2D.Raycast(rightFoot, Vector2.down, spriteRenderer.bounds.size.y / 2 + 0.03f))
            {
                onFloor = true;
            }
            else
            {
                onFloor = false;
            }

            direitaOcupada = Physics2D.Raycast(transform.position, Vector2.right, spriteRenderer.bounds.size.x / 2 + 0.03f);
            esquerdaOcupada = Physics2D.Raycast(transform.position, Vector2.left, spriteRenderer.bounds.size.x / 2 + 0.03f);
        }
           
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        atualizaRayCasts();
        if (animator.GetBool("WalkingLeft")&&esquerdaOcupada)//se colidiu com alguma coisa quando indo para a esquerda para a animacao
        {
            animator.SetBool("WalkingLeft", false);
        }
        else if (animator.GetBool("WalkingRight")&&direitaOcupada)//se colidiu com alguma coisa quando indo para a direita para a animacao
        {
            animator.SetBool("WalkingRight", false);
        }

 
        if (collision.collider.name.StartsWith("enemies_0"))//se colidiu com um inimigo
        {
            if (collision.collider.bounds.center.y > transform.position.y - spriteRenderer.bounds.size.y / 2)//verifca a altura se estiver acima nao morre caso abaixo morre
            {
                animator.SetBool("Die", true);
                boxCollider2D.enabled = false;
                gameOver = true;
                rigidbody2D.velocity = new Vector3(0,0, 0);//a velocidade e sempre zerada para o caso do jogador morrer pulando
                rigidbody2D.AddForce(Vector3.up * inpulsoPulo);//impulso final depois da morte
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)//se colidiu com a banteira
    {
        win = true;//avisa que ganhou
        transform.position = new Vector2(57.303f, -0.803f);//coloca o jogador na base dela
        animator.SetBool("Win", true);//ativa a animacao que ganhou
        

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    public void BandeiraOK()//funcao serve para avisar o jogador que a baideira ja foi descida
    {
        animator.SetBool("BandeiraOK", true);
        animator.SetBool("Win", false);
    }

}
