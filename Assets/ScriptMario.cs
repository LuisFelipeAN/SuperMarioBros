using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptMario : MonoBehaviour {
    public float velocidade=3;
    public float maxInpulsoPulo=20;
    private float impulsoPulo;
    private Rigidbody2D rigidbody2D;
    private bool pulando;

    // Use this for initialization
    void Start () {
        rigidbody2D = GetComponent<Rigidbody2D>();
        pulando = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space)&&!pulando)
        {
            impulsoPulo += maxInpulsoPulo / 3;
            rigidbody2D.AddForce(Vector2.up * maxInpulsoPulo);
            pulando = true;
        }
        /*else
        {
            impulsoPulo = maxInpulsoPulo / 3;
            pulando = false;
            rigidbody2D.AddForce(Vector2.up * impulsoPulo);
        }*/
        if (Input.GetKey(KeyCode.RightArrow)){
            transform.Translate(Vector2.right * velocidade * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * velocidade * Time.deltaTime);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //pulando = false;
    }
}
