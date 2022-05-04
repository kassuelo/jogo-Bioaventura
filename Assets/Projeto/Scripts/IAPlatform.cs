using UnityEngine;

public class IAPlatform : MonoBehaviour
{
    //SCRIPT RESPONS�VEL POR CONTROLAR A INTELIG�NCIA ARTIFICIAL DAS PLATAFORMAS (MOVIMENTA��O AUTOM�TICA)

    public Transform platform; //salva o transform da plataforma
    public Transform pointA; //salva o ponto A da movimenta��o
    public Transform pointB; //salva o ponto B da movimenta��o
    public float speed; //salva a velocidade da movimenta��o
    private Transform target; //salva a posi��o de destino da plataforma

    void Start() //Rotina executada uma �nica vez, quando a aplica��o � carregada
    {
        target = pointB; //inicia a movimenta��o da plataforma, indo da posi��o inicial para o ponto B
    }

    void Update() //Rotina executada a cada frame
    {
        MovePlatform();
    }


    void MovePlatform() //m�todo respons�vel por movimentar a plataforma
    {
                                                //posi��o atual,  posi��o destino,  velocidade * normalizador de tempo
        platform.position = Vector3.MoveTowards(platform.position, target.position, speed * Time.deltaTime);

        //se a plataforma chegou no destino e o destino � o ponto A, altera o destino para o ponto B
        if (platform.position == target.position && target.position == pointA.position)
        {
            target = pointB;
        }

        //se a plataforma chegou no destino e o destino � o ponto B, altera o destino para o ponto A
        if (platform.position == target.position && target.position == pointB.position)
        {
            target = pointA;
        }
    }
}
