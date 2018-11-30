using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour {

	// variáveis públicas para serem alteradas no Inspector
	public float speed;				// velocidade de movimentacao do Kirby em x
	public float jumpImpulse;		// impulso aplicado no salto
	public float weight;			// peso do personagem (simulacao da forca peso, massa * aceleracao da gravidade)

	// componentes úteis 
	private Animator animator;			
	private SpriteRenderer sprite;

	// outras variáveis
	private float ground;			// coordenada y do chão
	private float baseHeight;		// coordenada y do suporte do personagem (pode ser o chão ou pode ser uma plataforma)
	private bool jumping = false;	// variável booleana para indicar se o personagem está pulando
	private float jumpSpeed = 0;	// velocidade instantânea em y

	// Use this for initialization
	void Start () {
		// pega os componentes
		animator = GetComponent<Animator>();
		sprite = GetComponent<SpriteRenderer> ();

		// define a posicão inicial do Kirby como sendo o chão (está assumindo que o Kirby sempre comeca no chão)
		ground = transform.position.y;

		// o suporte do personagem inicialmente é o chão
		baseHeight = ground;
	}
	
	// Update is called once per frame
	void Update () {
		// se fim de jogo
		if (animator.GetBool("Ending")) {
			// aplica uma translacão associada à animacão do personagem voando na nuvem
			Vector2 goAway = new Vector2 (-1.0f, 0.2f);
			transform.Translate (goAway.normalized * speed * Time.deltaTime);
			sprite.flipX = true;
		} else {
			// captura teclas de movimentacão e translada de acordo em x, ativando ou desativando a animacão de movimento
			if (Input.GetKey (KeyCode.RightArrow)) {
				animator.SetBool ("Moving", true);
				sprite.flipX = false;
				Vector3 dx = transform.right * speed * Time.deltaTime;
				transform.position += dx;
			} else if (Input.GetKey (KeyCode.LeftArrow)) {
				animator.SetBool ("Moving", true);
				sprite.flipX = true;
				Vector3 dx = transform.right * speed * Time.deltaTime;
				transform.position -= dx;
			} else {
				animator.SetBool ("Moving", false);
			}

			// se o jogador quer pular e o personagem está "aterrado"
			if (Input.GetKeyDown (KeyCode.Space) && !jumping) {
				// seta a velocidade inicial igual ao impulso e ativa a animacão de pulo
				jumpSpeed = jumpImpulse;
				jumping = true;
				animator.SetBool ("Jumping", jumping);
			}

			// se está no meio do movimento de pulo
			if (jumping) {
				// translada em y em funcão da velocidade vertical
				transform.position += transform.up * jumpSpeed * Time.deltaTime;

				// decrementa a velocidade em y em funcão da gravidade
				jumpSpeed -= weight * Time.deltaTime;

				// verifica a altura do personagem em relacão ao seu suporte
				float height = transform.position.y - baseHeight;

				// se essa altura é zero (cuidado com erros numéricos)
				if (height < 0.0001f) {
					// posiciona o personagem sobre o suporte, mantendo sua coordenada x, e finaliza o pulo
					transform.position = new Vector2 (transform.position.x, baseHeight);
					jumping = false;
					animator.SetBool ("Jumping", jumping);
				}
			}
		}
	}

	/// <summary>
	/// Este evento será disparado sempre que o Kirby colidir com um inimigo ou com uma plataforma
	/// </summary>
	/// <param name="col">objeto que guarda os dados de colisão</param>
	void OnCollisionEnter2D(Collision2D col) {
		// obtém o objeto envolvido na colisão
		GameObject obj = col.gameObject;

		// se esse objeto é uma plataforma
		if (obj.tag == "Platform") {
			// verifica se o Kirby está em movimento descendente (isso impede a colisão com a plataforma de ser ativada durante a subida)
			if (jumpSpeed < 0) {
				// muda a altura de suporte do personagem para o topo da plataforma. O campo 'bounds' retorna os limites da geometria de colisão, que possui
				// os pontos min e max. O max indica a coordenada do canto superior direito da geometria, que é exatamente onde o Kirby deve estar. Porém,
				// o transform.position.y do Kirby representa o seu centro de massa, e não a sua base. Portanto, aplicamos um deslocamento extra igual a 
				// altura/2 da geometria de colisão. Esse valor é obtido pelo campo extents, que guarda as meias-dimensões da caixa envolvente.
				baseHeight = col.collider.bounds.max.y + col.collider.bounds.extents.y;
			}
		// caso contrário, é um inimigo
		} else {
			// verifica se o Kirby está acima do inimigo (o col representa a geometria de colisão do inimigo, sendo max seu canto superior direito)
			if (transform.position.y > col.collider.bounds.max.y)
				// destrói o inimigo
				Destroy (obj);
			else
				// senão, destrói o Kirby
				Destroy (gameObject);
		}
	}

	/// <summary>
	/// Evento disparado quando o Kirby deixar de colidir com um objeto (no nosso caso, o interesse é só nas plataformas)
	/// </summary>
	/// <param name="col">Col.</param>
	void OnCollisionExit2D(Collision2D col) {
		GameObject obj = col.gameObject;

		// verifica se o objeto é uma plataforma (não queremos tratar a saída da colisão com inimigos)
		if (obj.tag == "Platform") {
			// como o personagem perdeu o suporte, a altura base volta a ser o chão
			baseHeight = ground;

			// ativa o modo de pulo (mesmo que o movimento seja, na verdade, de queda livre)
			jumping = true;
		}
	}

	/// <summary>
	/// Evento disparado no contato com a estrela (definido como trigger)
	/// </summary>
	/// <param name="col">Col.</param>
	void OnTriggerEnter2D(Collider2D col) {
		// destrói a estrela e ativa a animacão de fim de jogo
		Destroy (col.gameObject);
		animator.SetBool ("Moving", false);
		animator.SetBool ("Jumping", false);
		animator.SetBool ("Ending", true);
	}
}
