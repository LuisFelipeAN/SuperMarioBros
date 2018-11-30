using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptMario : MonoBehaviour {
    public float velocidadeLateral=3;
    public float inpulsoPulo=7.5f;

    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody2D;
    private bool andando;
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
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            rigidbody2D.AddForce(Vector3.up * inpulsoPulo);
        }

        if (Input.GetKey(KeyCode.RightArrow)){
          transform.Translate(Vector3.right * velocidadeLateral * Time.deltaTime);
          andando = true;


        }
        if (Input.GetKey(KeyCode.LeftArrow))
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
