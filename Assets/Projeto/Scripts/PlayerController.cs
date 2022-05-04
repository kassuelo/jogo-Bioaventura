using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    //SCRIPT RESPONSÁVEL POR TODAS AS INTERAÇÕES DO PLAYER COM O CENÁRIO E COM OS INIMIGOS

    private GameController _GameController; //referencia o script que controla as generalidades do jogo

    private Animator playerAnimator; //objeto que controla as animações do player
    private Rigidbody2D playerRigidbody2D; // corpo rígido -> componente do player que simula a gravidade

    public Transform groundCheck; //objeto que detecta, pelo posicionamento, se o player esta pisando o chão
    public bool isGround = false; //representa se o player está pisando no chão

    public float speed; //armazena a velocidade em que o player se desloca

    public float touchRun = 0.0f; //controla a movimentação do player com toque na tela ou tecla pressionada

    public bool facingRight = true; //controla a direção que o personagem olha, inicia o jogo olhando para a direita

    //Pulo
    public bool jump = false; //inicialmente o player não está pulando
    public int numberJumps = 0; //contador de pulos consecutivos
    public int maxJumps = 2; //número máximo de pulos consecutivos
    public float jumpForce; //força do pulo

    void Start() //Rotina executada uma única vez, quando a aplicação é carregada
    {
        playerAnimator = GetComponent<Animator>(); //armazena o objeto que controla as animações do player
        playerRigidbody2D = GetComponent<Rigidbody2D>(); //armazena o objeto que controla o corpo rígido do player
        _GameController = FindObjectOfType<GameController>(); //referencia o script que controla as generalidades do jogo
    }

    void Update() //função executada repetidamente, a cada quadro, em intervalos que variam
    {
        //se o player estiver encostando em uma layer chamada Ground, define isGrounded = true, para alterar a animação
        isGround = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        playerAnimator.SetBool("IsGrounded", isGround);

        //captura os valores de entrada do eixo Horizontal (setas direita e esquerda)
        touchRun = Input.GetAxisRaw("Horizontal"); 

        SetMovements(); //altera as animações do player

        //se uma das teclas (Space ou W ou up) definidas para a ação Jump for pressionada, altera seu valor para true
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) 
        {
            jump = true;
        }
    }

    private void FixedUpdate() //função executada repetidamente, a cada quadro da taxa de quadro fixada
    {
        MovePlayer(touchRun); //movimenta o player de acordo com a direção recebida na entrada
        Jump(); //verifica se o player deve pular
    }

    void MovePlayer(float movimentoH) //responsável por movimentar o player horizontalmente, recebendo a direção como argumento
    {
        //move o player para a direção recebida em movimentoH, multiplica pela velocidade, para definir a velocidade do movimento
        // e mantem a velocidade do eixo Y padrão, pois movimenta apenas no eixo X
        playerRigidbody2D.velocity = new Vector2(movimentoH * speed, playerRigidbody2D.velocity.y);

        //se o player estiver olhando pro lado oposto à movimentação que deve realizar, sua imagem é invertida
        if ((movimentoH < 0 && facingRight) || (movimentoH > 0 && !facingRight))
        {
            Flip();
        }
    }

    void Flip() //inverte a imagem do player
    {
        facingRight = !facingRight; //inverte o valor da variavel que representa a direção que o player está olhando

        //define uma nova escala para o player, aplicando invertendo o valor do eixo X e mantendo os valores existentes nos eixos Y e Z
        transform.gameObject.GetComponent<SpriteRenderer>().flipX = !transform.gameObject.GetComponent<SpriteRenderer>().flipX;
    }

    void SetMovements() //altera as animações do player
    {
        //na aba animator da unity existe o parametro Run, que será atribuido true se 
        //o personagem estiver se movimentando no eixo X e estiver no chão
        // quando o atributo Run é true, a animação é alterada para exibir o player correndo
        playerAnimator.SetBool("Run", (playerRigidbody2D.velocity.x != 0) && isGround);

        //se o player não esta no chão, altera a animação para pulando
        playerAnimator.SetBool("Jump", !isGround); 
    }

    void Jump()
    {
        if (jump) //se o botão de pulo for pressionado, então ...
        {
            if (isGround) //se o player estiver em contato com o chão define o total de pulos consecutivos para 0
            {
                numberJumps = 0;
            }

            //se o player estiver em contato com o chão e não realizou o máximo de pulos consecutivos, realiza mais um
            if (isGround || numberJumps < maxJumps)
            {
                if (numberJumps == 1)
                {
                    //impulsiona o player para cima, com a força reduzida, se for o segundo pulo consecutivo
                    playerRigidbody2D.AddForce(new Vector2(0f, jumpForce / 1.8f));
                }
                else {
                    //se for o primeiro pulo, impulsiona o player para cima com a força padrão, definida na variável jumpForce
                    playerRigidbody2D.AddForce(new Vector2(0f, jumpForce));
                }
                isGround = false; //após iniciar o pulo, é definido que o player não está  mais no chão
                numberJumps++; //incrementa o total de pulos consecutivos
                _GameController.fxGame.PlayOneShot(_GameController.fxJump); //executa áudio do pulo
            }
           jump = false; //após realizar o pulo, reseta a variável de tecla pressionada
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) //Detecta todas as triggers (áreas de colisão com gatilhos para executar ações)
    {
        //pega o corpo rígido do player para adicionar impulso ao pular em um inimigo
        Rigidbody2D rb = GetComponentInParent<Rigidbody2D>();

        //Se não for uma estrela, adiciona impulso após pular em cima do inimigo
        //Se for uma estrela, só irá coletar sem adicionar impulso
        if(collision.gameObject.tag != "Star")
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(new Vector2(0, 700));
        }


        switch (collision.gameObject.tag) //verifica a tag do objeto colidido
        {
            case "WrongChoice": //se for a escolha errada, desconta 50 pontos do score
                KillEnemy(-50, collision.gameObject);
                break;

            case "CorrectChoice": //se for a escolha correta, soma 100 pontos ao score
                KillEnemy(100, collision.gameObject);
                break;
            case "Star": //se for uma estrela, mostra um acerto e soma 10 pontos ao score
                KillEnemy(10, collision.gameObject);
                break;
            case "FinishLevel1": //se for o final do level 1, exibe felicitações e redireciona para o level 2
                rb.velocity = new Vector2(0, 0);
                _GameController.ShowCongratulations(2);
                Destroy(collision.gameObject);
                break;
            case "FinishLevel2": //se for o final do level 1, exibe felicitações e redireciona para o level 3
                rb.velocity = new Vector2(0, 0);
                _GameController.ShowCongratulations(3);
                Destroy(collision.gameObject);
                break;
            case "Finish": //se concluir o level final(level 3), exibe felicitações e redireciona para o menu principal
                rb.velocity = new Vector2(0, 0);
                _GameController.ShowCongratulations(0);
                Destroy(collision.gameObject);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        switch (collision.gameObject.tag) //verifica a tag do objeto colidido
        {
            case "Platform": //quando pular na plataforma o player vai movimentar junto com ela
                this.transform.parent = collision.transform;
                break;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        switch (collision.gameObject.tag) //verifica a tag do objeto colidido
        {
            case "Platform":
                //desvincula a movimentação do player da movimentação da plataforma, ao perder o contato com ela
                this.transform.parent = null;
                break;
        }
    }

    private void KillEnemy(int points, GameObject enemy) //recebe a pontuação que deve ser contabilizada e o inimigo
    {
        Transform enemyTransform = enemy.transform; //salva as cordenadas do inimigo atingido

        //cria uma nova posição acima do inimigo para exibir a pontuação de erro ou acerto
        Vector3 pointPosition = new Vector3(enemyTransform.position.x, enemyTransform.position.y + 2f, enemyTransform.position.z);

        //atribui essa posição à campo de texto que está na tela
        _GameController.txtPoints.transform.position = pointPosition;

        Destroy(enemy); //elimina o inimigo

        //cria uma animação de explosão no lugar onde estava o inimigo, e 0.5 seg após exibida, a animação é destruida
        GameObject explosion = Instantiate(_GameController.explosionPrefab, enemyTransform.position, enemyTransform.localRotation);
        Destroy(explosion, 0.5f);

        _GameController.Score(points); //contabiliza a pontuação após colidir com o inimigo
    }
}
