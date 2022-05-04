using UnityEngine;

public class IAEnemy : MonoBehaviour
{
    //SCRIPT RESPONSÁVEL POR CONTROLAR A INTELIGÊNCIA ARTIFICIAL DOS INIMIGOS (MOVIMENTAÇÃO AUTOMÁTICA)

    public Transform enemy; //salva o transform do inimigo
    private SpriteRenderer enemySprite; //salva a imagem do inimigo
    public Transform pointA; //salva o ponto A da movimentação
    public Transform pointB; //salva o ponto B da movimentação
    public float speed; //salva a velocidade da movimentação
    public bool isRight; //salva a direção que o inimigo está olhando
    private Transform target; //salva a posição de destino do inimigo


    void Start() //Rotina executada uma única vez, quando a aplicação é carregada
    {
        //carrega o gerenciador da imagem do inimigo para poder alterar o lado que ele olha
        enemySprite = enemy.gameObject.GetComponent<SpriteRenderer>();

        //inicia a movimentação da plataforma, indo da posição inicial para o ponto B
        target = pointB;
    }

    void Update() //Rotina executada a cada frame
    {
        MoveEnemy();
    }

    void Flip(Transform source, Transform target)
    {
        //se o inimigo deve ir para a direita e ele está virado para a esquerda
        // ou
        //se o inimigo deve ir para a esquerda e ele está virado para a direita
        //então a imagem (sprite) é invertida
        if ((source.position.x < target.position.x && !isRight) || (source.position.x > target.position.x && isRight))
        {
            isRight = !isRight;
            enemySprite.flipX = !enemySprite.flipX; //inverte o eixo x para girar a imagem
        }
        
    }

    void MoveEnemy() //método responsável por movimentar o inimigo
    {
        if (enemy != null) //testa se o inimigo ja foi eliminado
        {
            Flip(enemy, target); //gira o inimigo se necessário
                                                //posição atual,  posição destino,  velocidade * normalizador de tempo
            enemy.position = Vector3.MoveTowards(enemy.position, target.position, speed* Time.deltaTime);

            //se a plataforma chegou no destino e o destino é o ponto A, altera o destino para o ponto B
            if (enemy.position == target.position && target.position == pointA.position)
            {
                target = pointB;
            }

            //se a plataforma chegou no destino e o destino é o ponto B, altera o destino para o ponto A
            if (enemy.position == target.position && target.position == pointB.position)
            {
                target = pointA;
            }


        }
    }
}
