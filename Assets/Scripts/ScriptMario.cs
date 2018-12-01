using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptMario : MonoBehaviour {
    public float velocidadeLateral=3;
    public float inpulsoPulo=7.5f;
    public float minX;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2D;
    private bool andando;
    private bool onFloor,direitaOcupada,esquerdaOcupada;
    private BoxCollider2D boxCollider2D;
    private Animator animator;
    private bool pulando;
    private int dir;
    private bool blockLeft = false, blockRight = false;
    private bool gameOver = false,win=false;
    private Vector2 leftFoot, rightFoot;
    public float GetVelocidadeHorizontal
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
    public float GetVelocidadeVertical
    {
        get
        {
            return rigidbody2D.velocity.y;
           
        }
    }
    // Use this for initialization
    void Start () {
       
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D=GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        onFloor = false;
        pulando = false;
        leftFoot = new Vector2();
        leftFoot = new Vector2();
        dir = 0;
    }

	// Update is called once per frame
	void Update () {
        if (!gameOver&&!win)
        {
            atualizaRayCasts();


            if (onFloor && pulando)
            {
                pulando = false;
                animator.SetBool("Jumping", false);
                blockLeft = false;
                blockRight = false;
            }
            if (Input.GetKeyDown(KeyCode.Space) && onFloor)
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

            if (Input.GetKeyDown(KeyCode.RightArrow) && !direitaOcupada)
            {
               
                dir = 1;
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) && !esquerdaOcupada && transform.position.x - spriteRenderer.bounds.size.x / 2 > minX)
            {
                
                dir = -1;
            }


           


            if (pulando && !blockLeft && !blockRight)
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

            if (Input.GetKey(KeyCode.LeftArrow) && !esquerdaOcupada && transform.position.x - spriteRenderer.bounds.size.x / 2 > minX && !blockLeft)
            {
                transform.Translate(Vector3.left * velocidadeLateral * Time.deltaTime);
                animator.SetBool("WalkingLeft", true);
                andando = true;
            }
            if (Input.GetKey(KeyCode.RightArrow) && !direitaOcupada && !blockRight)
            {
                transform.Translate(Vector3.right * velocidadeLateral * Time.deltaTime);
                animator.SetBool("WalkingRight", true);
                andando = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
            {
                andando = false;
            }
            
        }else if (win)
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
        if (animator.GetBool("WalkingLeft")&&esquerdaOcupada)
        {
            animator.SetBool("WalkingLeft", false);
        }
        else if (animator.GetBool("WalkingRight")&&direitaOcupada)
        {
            animator.SetBool("WalkingRight", false);
        }

 
        if (collision.collider.name.StartsWith("enemies_0"))
        {
            if (collision.collider.bounds.center.y > transform.position.y - spriteRenderer.bounds.size.y / 2)
            {
                animator.SetBool("Die", true);
                boxCollider2D.enabled = false;
                gameOver = true;
                rigidbody2D.velocity = new Vector3(0,0, 0);
                rigidbody2D.AddForce(Vector3.up * inpulsoPulo);
            }

        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transform.position = new Vector2(57.303f, -0.803f);
        animator.SetBool("Win", true);
        win = true;

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
    public void BandeiraOK()
    {
        animator.SetBool("BandeiraOK", true);
        animator.SetBool("Win", false);
    }

}
