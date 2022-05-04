using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    //SCRIPT RESPONSÁVEL POR CONTROLAR AS GENERALIDADES DO GAME COMO:
    // - controlar o Score
    // - armazenar prefabs (objetos pré-fabricados) para serem instanciados pelos outros Scripts

    public GameObject imgScore; //armazena o icone que fica ao lado do score
    public Text txtScore; //exibe o total de pontos na tela
    public Text txtPoints; //exibe os pontos conquistados ou perdidos a cada colisão com um inimigo
 
    public Text txtLevel; //armazena o level atual
    public GameObject endGamePanel; 
    public Text txtLevelCompleted;

    public GameObject gameOverPanel; //painel com os elementos do game over
    public Button btnRestart; //reinicia o jogo
    public Button btnMainMenu; //volta ao menu
    public Text txtScoreGameOver; //exibe a score de fim de jogo

    //objeto pré-fabricado (animações para serem instanciadas)
    public GameObject explosionPrefab; //efeito visual para quando um inimigo é eliminado

    public AudioSource fxGame; //musica tema do jogo
    public AudioClip fxJump; //som do pulo
    public AudioClip fxWrongEnemy; //som de inimigo errado
    public AudioClip fxCorrectEnemy; //som de inimigo correto
    public AudioClip fxGameOver; //som de game over

    public GameObject player; //salva o objeto player

    private BackToMenuController _BackToMenuController; //referencia o script que permite voltar ao menu principal
    private PlayerController _PlayerController; //referencia o script que permite controlar o player

    public GameObject quitPanel; //painel que solicita confirmação para sair do jogo
    public Button btnYesQuit; //volta para o menu principal
    public Button btnNoQuit; //continua no jogo

    private void Start() //Rotina executada uma única vez, quando a aplicação é carregada
    {
        //carrega o score acumulado dos níveis concluídos e mostra na tela
        txtScore.text = GlobalVariables.GetScore().ToString();

        txtLevel.text = "Level " + SceneManager.GetActiveScene().buildIndex; //mostra qual o level atual no canto da tela

        //pega uma referência do script que controla as ações do player
        _PlayerController = FindObjectOfType<PlayerController>(); 

        //pega uma referência do script que controla o retorno ao menu principal
        _BackToMenuController = GameObject.Find("MainMenu").GetComponent<BackToMenuController>(); 

        //define qual método o clique de cada botão irá executar
        btnRestart.onClick.AddListener(RestartGame);
        btnMainMenu.onClick.AddListener(_BackToMenuController.BackToMenu);
        btnYesQuit.onClick.AddListener(_BackToMenuController.BackToMenu);
        btnNoQuit.onClick.AddListener(HideQuitPanel);
    }

    private void Update() //Rotina executada a cada frame
    {
        Invoke("CheckGameOver", 6f); // verifica se ocorreu game over com um delay de 6 seg
        ShowQuitPanel(); //verifica se o player clicou em ESC, caso sim, mostra a tela de confirmação para sair do jogo
    }

    public void Score(int amountPoints) //contabiliza os pontos da partida
    {
        GlobalVariables.SetScore(GlobalVariables.GetScore() + amountPoints);
        txtScore.text = GlobalVariables.GetScore().ToString();
        txtScoreGameOver.text = "score: " + GlobalVariables.GetScore().ToString();
        ShowPoints(amountPoints);
    }

    public void ShowPoints(int points) //exibe os pontos da colisão atual
    {
        if(points > 0)                      //na cor verde se o player fez a escolha certa (soma 100 pontos)
        {
            txtPoints.color = Color.green;
            fxGame.PlayOneShot(fxCorrectEnemy);
        }
        else
        {                                   //na cor vermelha se o player fez a escolha errada (desconta 50 pontos)
            txtPoints.color = Color.red;
            fxGame.PlayOneShot(fxWrongEnemy);
        }
        txtPoints.text = points.ToString(); //atribui o valor ao objeto de texto na tela

        StartCoroutine("HidePoints"); //executa a rotina que oculta o texto dos pontos da colisão atual após 1 segundo
    }

    IEnumerator HidePoints() //oculta o texto dos pontos da colisão atual após 1 segundo
    {
        yield return new WaitForSeconds(1f);
        txtPoints.text = "";

    }

    public void CheckGameOver() //verifica se ocorreu game over
    {
        //se o player cair fora do terreno ou o score ficar negativo, ocorre game over
        if (player.transform.position.y < -15 || GlobalVariables.GetScore() < 0) 
        {
            gameOverPanel.SetActive(true); //se houve game over, exibe o respectivo painel

            //oculta score do topo da tela quando aparece o game over
            imgScore.SetActive(false);
            txtScore.gameObject.SetActive(false);
           
            fxGame.PlayOneShot(fxGameOver, 0.2f);//toca o efeito sonoro de game over
            FreezePlayer(); //congela a movimentação o player

            //se o painel de game over está visivel, ao clicar em ESC volta para o menu principal
            if (gameOverPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            {
                _BackToMenuController.BackToMenu();
            }

            //se o painel de game over está visivel, ao clicar em ENTER, reinicia o level
            else if (gameOverPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
            {
                RestartGame();
            }
        }
    }

    void ShowQuitPanel()
    {
        if (!endGamePanel.activeSelf)
        {
            //se o painel para sair do jogo está inativo, ele abre ao pressionar ESC
            if (!quitPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            {
                FreezePlayer();
                quitPanel.SetActive(true);
            }
            //se o painel para sair do jogo está ativo, ele fecha ao pressionar ESC
            else if (quitPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            {
                UnfreezePlayer();
                quitPanel.SetActive(true);
                //HideQuitPanel();
            }
            //se o painel para sair do jogo está ativo, volta ao menu ao pressionar ENTER
            else if (quitPanel.activeSelf && Input.GetKeyDown(KeyCode.Return))
            {
                _BackToMenuController.BackToMenu();
            }
        }
    }

    void FreezePlayer() //congela a movimentação do player
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        //congela a movimentação do corpo rígido do player
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        //desabilita o script que controla as ações do player
        _PlayerController.enabled = false;                 
    }

    void UnfreezePlayer()
    {
        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        //ativa a movimentação do corpo rígido do player
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        //habilita o script que controla as ações do player
        _PlayerController.enabled = true;             
    }

    void HideQuitPanel() //oculta o painel de sair do jogo e habilita a movimentação do player novamente
    {
        UnfreezePlayer();
        quitPanel.SetActive(false);
    }

    private void RestartGame() //recarrega a cena ativa
    {
        GlobalVariables.SetScore(0);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ShowCongratulations(int sceneIndex) //exibe mensagem para o player ao concluir o level
    {
        FreezePlayer(); //congela o player ao chegar no fim da cena
        endGamePanel.SetActive(true);
        if (sceneIndex == 0)
        {
            txtLevelCompleted.text = "Parabéns! Você completou todos os níveis \n\n\n" +
                "Jogo desenvolvido por:\n\n" +
                "Kassuelo Moreira Okaszeski\n" +
                "Maicon Gian Lemanski Schmidt\n" +
                "Vicente Woitchumas Kryszczun";
        }
        else
        {
            txtLevelCompleted.text = "Parabéns! Você completou o " + txtLevel.text.ToString();
        }
        StartCoroutine("NextScene", sceneIndex); //chama o método que carrega a proxima cena
    }

    IEnumerator NextScene(int sceneIndex) //carrega a cena, cujo index foi recebido como argumento
    {
        if(sceneIndex == 0)
        {
            yield return new WaitForSeconds(5f);
            GlobalVariables.SetScore(0); //zera o score se retornar ao menu principal
        }
        else
        {
            yield return new WaitForSeconds(3f);
        }
        SceneManager.LoadScene(sceneIndex);
        txtLevelCompleted.text = "";
    }
}



