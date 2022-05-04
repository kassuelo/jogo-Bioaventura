using UnityEngine;

public class CameraController : MonoBehaviour
{
    //SCRIPT RESPONSÁVEL POR REALIZAR A MOVIMENTAÇÂO DA CÂMERA DE ACORDO COM A MOVIMENTAÇÃO DO PLAYER

    public float offsetX = 10f; //distancia do eixo X entre o jogador e o centro da câmera
    public float smooth = 0.1f; //suavização na velocidade de movimentação da câmera

    //Coordenadas do limite da câmera para cada lado.
    //São definidas ao carregar o script no objeto pela interface da Unity.
    public float limitedUp;
    public float limitedDown;
    public float limitedLeft;
    public float limitedRight;

    //Armazena informações do posicionamento do player
    private Transform player;
    private float playerX;
    private float playerY;

    void Start() //Rotina executada uma única vez, quando a aplicação é carregada
    {
        //procura dentro de todos os objetos o script PlayerController
        player = FindObjectOfType<PlayerController>().transform; 
    }

    void FixedUpdate() //Rotina executada a cada frame, seguindo uma taxa de quadros fixa
    {
        if(player != null)
        {
            //salva as coordenadas do player, para saber onde posicionar a câmera, de forma que o player fique no centro
            playerX = Mathf.Clamp(player.position.x + offsetX, limitedLeft, limitedRight);
            playerY = Mathf.Clamp(player.position.y, limitedDown, limitedUp);

            //faz a posição da camera se deslocar até a posição salva do player, utilizando o valor de smooth para suavizar o movimento
            transform.position = Vector3.Lerp(transform.position, new Vector3(playerX, playerY, transform.position.z), smooth);
        }
    }
}
