using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptDesceBandeira : MonoBehaviour {
    public GameObject bandeira;
    public float tempo;//tempo para descer a bandeira
    public float altura;//altura que a bandeira ira derscer
    private float  dt;//acumulador de tempo
    private float dv;//velocidade de deslocamento da bandeira
    private bool podeDescer;//controla o momento de iniciar a descida da bandeira
    private bool ok;//controle para avisar o player somente uma vez
	// Use this for initialization
	void Start () {
        podeDescer = false;

        dv = altura / tempo;//calcula a velocidade que a bandeira ira descer
        dt = 0;
        ok = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (podeDescer)
        {
            if (dt < tempo)
            {
                bandeira.transform.Translate(0, -dv * Time.deltaTime, 0);//enquanto o tempo for menor que o tempo total desce a bandeira
                dt += Time.deltaTime;
            }
            else if(!ok)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.GetComponent<ScriptMario>().BandeiraOK();//avisa o player que a bandeira desceu
                ok = true;//seta ok para true para garantir que o player nao sera avisado novamente
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)//player entrou em contato com a bandeira
    {
        podeDescer = true;
    }
}
