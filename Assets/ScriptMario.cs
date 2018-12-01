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
    public float GetVelocidade
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
    // Use this for initialization
    void Start () {
       
        spriteRenderer = GetComponent<SpriteRenderer>();
        rigidbody2D=GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        onFloor = false;
        pulando = false;
        dir = 0;
    }

	// Update is called once per frame
	void Update () {

        atualizaRayCasts();


        if (onFloor && pulando)
        {
            pulando = false;
            animator.SetBool("Jumping", false);
            blockLeft = false;
            blockRight = false;
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
            if (!pulando)
            {
                animator.SetBool("WalkingRight", true);
            }
          
            dir = 1;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && !esquerdaOcupada && transform.position.x - spriteRenderer.bounds.size.x / 2 > minX)
        {
            if (!pulando)
            {
                animator.SetBool("WalkingLeft", true);
            }
            
            dir = -1;
        }


        if (Input.GetKeyDown(KeyCode.Space) && onFloor)
        {
            rigidbody2D.AddForce(Vector3.up * inpulsoPulo);
            pulando = true;
            animator.SetBool("Jumping", true);


        }

       
        if (pulando&&!blockLeft&&!blockRight)
        {
            if (dir == 1)
            {
                blockLeft = true;
            }else if(dir==-1){
                blockRight = true;
            }
        }

        if (Input.GetKey(KeyCode.LeftArrow) && !esquerdaOcupada && transform.position.x - spriteRenderer.bounds.size.x / 2 > minX &&  !blockLeft)
        {
            transform.Translate(Vector3.left * velocidadeLateral * Time.deltaTime);
            andando = true;
        }
        if (Input.GetKey(KeyCode.RightArrow) && !direitaOcupada && !blockRight)
        {
            transform.Translate(Vector3.right * velocidadeLateral * Time.deltaTime);
            andando = true;
        }

        if (Input.GetKeyUp(KeyCode.LeftArrow)|| Input.GetKeyUp(KeyCode.RightArrow))
        {
            andando = false;
        }
        transform.rotation = Quaternion.identity;
    }

    public void atualizaRayCasts()
    {
        boxCollider2D.enabled = false;
        onFloor = Physics2D.Raycast(transform.position, Vector2.down, spriteRenderer.bounds.size.y / 2 + 0.03f);
        direitaOcupada = Physics2D.Raycast(transform.position, Vector2.right, spriteRenderer.bounds.size.x / 2 + 0.03f);
        esquerdaOcupada = Physics2D.Raycast(transform.position, Vector2.left, spriteRenderer.bounds.size.x / 2 + 0.03f);
        boxCollider2D.enabled = true;
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
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
       
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
       

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

}
