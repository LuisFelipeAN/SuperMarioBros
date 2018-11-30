using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptMario : MonoBehaviour {
    public float velocidadeLateral=3;
    public float inpulsoPulo=20;
    public float peso=1;

    private float yBase;
    private float velocidadePulo;
    private bool pulando;

    // Use this for initialization
    void Start () {
        pulando = false;
        yBase = transform.position.y;
    }

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Space)&&!pulando)
        {
            velocidadePulo=inpulsoPulo;
            pulando = true;
        }

        if (Input.GetKey(KeyCode.RightArrow)){
            transform.Translate(Vector2.right * velocidadeLateral * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(Vector2.left * velocidadeLateral * Time.deltaTime);
        }
			  if (pulando) {
  				transform.position += transform.up * velocidadePulo * Time.deltaTime;

  				velocidadePulo -= peso * Time.deltaTime;

  				float yAux = transform.position.y - yBase;

  				// se essa altura é zero (cuidado com erros numéricos)
  				if (yAux < 0.0001f) {
  					// posiciona o personagem sobre o suporte, mantendo sua coordenada x, e finaliza o pulo
  					transform.position = new Vector2 (transform.position.x, yBase);
  					pulando = false;
  				}
    }

    private void OnTriggerEnter2D(Collision2D collision)
    {
      GameObject obj = collision.gameObject;

      // se esse objeto é uma plataforma
      if (obj.tag == "Plataforma") {
        // verifica se o Kirby está em movimento descendente (isso impede a colisão com a plataforma de ser ativada durante a subida)
        if (velocidadePulo < 0) {
          // muda a altura de suporte do personagem para o topo da plataforma. O campo 'bounds' retorna os limites da geometria de colisão, que possui
          // os pontos min e max. O max indica a coordenada do canto superior direito da geometria, que é exatamente onde o Kirby deve estar. Porém,
          // o transform.position.y do Kirby representa o seu centro de massa, e não a sua base. Portanto, aplicamos um deslocamento extra igual a
          // altura/2 da geometria de colisão. Esse valor é obtido pelo campo extents, que guarda as meias-dimensões da caixa envolvente.
          yBase = collision.collider.bounds.max.y + collision.collider.bounds.extents.y;
        }
      // caso contrário, é um inimigo
      } else {
        // verifica se o Kirby está acima do inimigo (o col representa a geometria de colisão do inimigo, sendo max seu canto superior direito)
        if (transform.position.y > collision.collider.bounds.max.y)
          // destrói o inimigo
          Destroy (obj);
        else
          // senão, destrói o Kirby
          Destroy (gameObject);
      }
    }
}
