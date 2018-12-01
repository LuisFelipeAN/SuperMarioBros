using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraController : MonoBehaviour {

	private GameObject player;					// jogador a ser seguido pela câmera principal
	private ScriptMario playerScript;		// script do jogador que possui a variável corresponde à velocidade
	private Object backgroundController = null; // controlador do fundo para o efeito de paralaxe (pode variar dependendo da forma como implementamos a paralaxe, por isso definido como uma instância de Object, que é superclasse de todas as demais)

	// Variáveis públicas para o Inspector
	public float cameraSpeed;					// velocidade da câmera (usada somente quando a câmera precisa se mover mais rapidamente do que o jogador)
	public float backgroundSpeed;				// velocidade de movimentacão do fundo (deve ser diferente da velocidade da câmera principal para criar a sensacão de profundidade)
    private Vector3 leftBorderCam;

	// Use this for initialization
	void Start () {
        // Pega o objeto controlado pelo jogador, que deve estar definido com a tag "Player" (poderia ter sido passado via Inspector também)
        player = GameObject.FindGameObjectWithTag ("Player");

		// Pega o script do jogador através do componente (o ideal seria criar uma classe separada para o jogador que fosse acessada tanto pelo CharacterScript quanto pelo CameraController)
		playerScript = player.GetComponent<ScriptMario> ();

		
		// Neste caso, a câmera traseira fica fixa e a textura aplicada sobre o objeto é deslocada com padrão de repeticão ativado (o Quad que contém a textura foi marcado com a tag "Background")
		backgroundController = GameObject.FindGameObjectWithTag ("Background").GetComponent<MeshRenderer> ();

        leftBorderCam = new Vector3(0, 0, 0);
        playerScript.minX = Camera.main.ScreenToWorldPoint(leftBorderCam).x;
    }
	
	// Update is called once per frame
	void Update () {
		// Obtém a posicão do jogador em coordenadas de tela 
		Vector3 screenPos = Camera.main.WorldToScreenPoint (player.transform.position);
        Vector3 dx = Vector3.right *  Time.deltaTime;

        //movimenta a camera e o backgroud somente se o jogador pasar a metade da tela
        if (screenPos.x >= Screen.width /2)
        {
            Camera.main.transform.position += dx * playerScript.GetVelocidadeHorizontal;
            if (playerScript.GetVelocidadeHorizontal > 0)
            {
                Renderer backRenderer = backgroundController as Renderer;
                backRenderer.material.mainTextureOffset += new Vector2(dx.x * backgroundSpeed, 0);
            }
            playerScript.minX = Camera.main.ScreenToWorldPoint(leftBorderCam).x;
        }
    }
}
