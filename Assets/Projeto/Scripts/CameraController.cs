using UnityEngine;

public class CameraController : MonoBehaviour
{
    //SCRIPT RESPONS�VEL POR REALIZAR A MOVIMENTA��O DA C�MERA DE ACORDO COM A MOVIMENTA��O DO PLAYER

    public float offsetX = 10f; //distancia do eixo X entre o jogador e o centro da c�mera
    public float smooth = 0.1f; //suaviza��o na velocidade de movimenta��o da c�mera

    //Coordenadas do limite da c�mera para cada lado.
    //S�o definidas ao carregar o script no objeto pela interface da Unity.
    public float limitedUp;
    public float limitedDown;
    public float limitedLeft;
    public float limitedRight;

    //Armazena informa��es do posicionamento do player
    private Transform player;
    private float playerX;
    private float playerY;

    void Start() //Rotina executada uma �nica vez, quando a aplica��o � carregada
    {
        //procura dentro de todos os objetos o script PlayerController
        player = FindObjectOfType<PlayerController>().transform; 
    }

    void FixedUpdate() //Rotina executada a cada frame, seguindo uma taxa de quadros fixa
    {
        if(player != null)
        {
            //salva as coordenadas do player, para saber onde posicionar a c�mera, de forma que o player fique no centro
            playerX = Mathf.Clamp(player.position.x + offsetX, limitedLeft, limitedRight);
            playerY = Mathf.Clamp(player.position.y, limitedDown, limitedUp);

            //faz a posi��o da camera se deslocar at� a posi��o salva do player, utilizando o valor de smooth para suavizar o movimento
            transform.position = Vector3.Lerp(transform.position, new Vector3(playerX, playerY, transform.position.z), smooth);
        }
    }
}
