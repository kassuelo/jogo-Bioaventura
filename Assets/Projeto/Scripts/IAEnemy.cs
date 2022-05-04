using UnityEngine;

public class IAEnemy : MonoBehaviour
{
    //SCRIPT RESPONS�VEL POR CONTROLAR A INTELIG�NCIA ARTIFICIAL DOS INIMIGOS (MOVIMENTA��O AUTOM�TICA)

    public Transform enemy; //salva o transform do inimigo
    private SpriteRenderer enemySprite; //salva a imagem do inimigo
    public Transform pointA; //salva o ponto A da movimenta��o
    public Transform pointB; //salva o ponto B da movimenta��o
    public float speed; //salva a velocidade da movimenta��o
    public bool isRight; //salva a dire��o que o inimigo est� olhando
    private Transform target; //salva a posi��o de destino do inimigo


    void Start() //Rotina executada uma �nica vez, quando a aplica��o � carregada
    {
        //carrega o gerenciador da imagem do inimigo para poder alterar o lado que ele olha
        enemySprite = enemy.gameObject.GetComponent<SpriteRenderer>();

        //inicia a movimenta��o da plataforma, indo da posi��o inicial para o ponto B
        target = pointB;
    }

    void Update() //Rotina executada a cada frame
    {
        MoveEnemy();
    }

    void Flip(Transform source, Transform target)
    {
        //se o inimigo deve ir para a direita e ele est� virado para a esquerda
        // ou
        //se o inimigo deve ir para a esquerda e ele est� virado para a direita
        //ent�o a imagem (sprite) � invertida
        if ((source.position.x < target.position.x && !isRight) || (source.position.x > target.position.x && isRight))
        {
            isRight = !isRight;
            enemySprite.flipX = !enemySprite.flipX; //inverte o eixo x para girar a imagem
        }
        
    }

    void MoveEnemy() //m�todo respons�vel por movimentar o inimigo
    {
        if (enemy != null) //testa se o inimigo ja foi eliminado
        {
            Flip(enemy, target); //gira o inimigo se necess�rio
                                                //posi��o atual,  posi��o destino,  velocidade * normalizador de tempo
            enemy.position = Vector3.MoveTowards(enemy.position, target.position, speed* Time.deltaTime);

            //se a plataforma chegou no destino e o destino � o ponto A, altera o destino para o ponto B
            if (enemy.position == target.position && target.position == pointA.position)
            {
                target = pointB;
            }

            //se a plataforma chegou no destino e o destino � o ponto B, altera o destino para o ponto A
            if (enemy.position == target.position && target.position == pointB.position)
            {
                target = pointA;
            }


        }
    }
}
