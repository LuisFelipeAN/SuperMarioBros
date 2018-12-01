using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDesceBandeira : MonoBehaviour {
    public GameObject bandeira;
    public float tempo;
    public float altura;
    private float  dt;
    private float dv;
    private bool podeDescer;
    private bool ok;
	// Use this for initialization
	void Start () {
        podeDescer = false;

        dv = altura / tempo;
        dt = 0;
        ok = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (podeDescer)
        {
            if (dt < tempo)
            {
                bandeira.transform.Translate(0, -dv * Time.deltaTime, 0);
                dt += Time.deltaTime;
            }
            else if(!ok)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<ScriptMario>().BandeiraOK();
                ok = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        podeDescer = true;
    }
}
