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
        onFloor = false;
    }

	// Update is called once per frame
	void Update () {

        atualizaRayCasts();

        if (Input.GetKeyDown(KeyCode.Space)&& onFloor)
        {
            rigidbody2D.AddForce(Vector3.up * inpulsoPulo);
        }

        if (Input.GetKey(KeyCode.RightArrow)&& !direitaOcupada)
        {
          transform.Translate(Vector3.right * velocidadeLateral * Time.deltaTime);
          andando = true;


        }
        if (Input.GetKey(KeyCode.LeftArrow)&& !esquerdaOcupada&&transform.position.x-spriteRenderer.bounds.size.x/2>minX)
        {
            transform.Translate(Vector3.left * velocidadeLateral * Time.deltaTime);
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
