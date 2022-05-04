using UnityEngine;

public class IAPlatform : MonoBehaviour
{
    //SCRIPT RESPONSÁVEL POR CONTROLAR A INTELIGÊNCIA ARTIFICIAL DAS PLATAFORMAS (MOVIMENTAÇÃO AUTOMÁTICA)

    public Transform platform; //salva o transform da plataforma
    public Transform pointA; //salva o ponto A da movimentação
    public Transform pointB; //salva o ponto B da movimentação
    public float speed; //salva a velocidade da movimentação
    private Transform target; //salva a posição de destino da plataforma

    void Start() //Rotina executada uma única vez, quando a aplicação é carregada
    {
        target = pointB; //inicia a movimentação da plataforma, indo da posição inicial para o ponto B
    }

    void Update() //Rotina executada a cada frame
    {
        MovePlatform();
    }


    void MovePlatform() //método responsável por movimentar a plataforma
    {
                                                //posição atual,  posição destino,  velocidade * normalizador de tempo
        platform.position = Vector3.MoveTowards(platform.position, target.position, speed * Time.deltaTime);

        //se a plataforma chegou no destino e o destino é o ponto A, altera o destino para o ponto B
        if (platform.position == target.position && target.position == pointA.position)
        {
            target = pointB;
        }

        //se a plataforma chegou no destino e o destino é o ponto B, altera o destino para o ponto A
        if (platform.position == target.position && target.position == pointB.position)
        {
            target = pointA;
        }
    }
}
