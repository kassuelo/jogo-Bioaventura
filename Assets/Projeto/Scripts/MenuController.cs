using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    //SCRIPT RESPONS�VEL POR GERENCIAR O MENU INICIAL DO JOGO

    //armazenam os bot�es do menu
    public Button btnStart;
    public Button btnHowToPlay;
    public Button btnBackMenu;
    public Button btnQuit;

    public GameObject panelHowToPlay; //armazena o painel com as instru��es de como jogar

    void Start() //Rotina executada uma �nica vez, quando a aplica��o � carregada
    {
        //Define qual m�todo o clique de cada bot�o ir� executar
        btnStart.onClick.AddListener(GameStart); 
        btnHowToPlay.onClick.AddListener(ShowHowToPlayPanel);
        btnBackMenu.onClick.AddListener(HideHowToPlayPanel);
        btnQuit.onClick.AddListener(QuitGame);
    }

    void Update() //Rotina executada a cada frame
    {
        CheckKeyDown();
    }

    void CheckKeyDown() //m�todo pra permitir chamar os m�todos dos bot�es por meio de teclas
    {
        if (Input.GetKeyDown(KeyCode.Return)) //Tecla Enter = Start
        {
            GameStart();
        }

        if (Input.GetKeyDown(KeyCode.H)) //Tecla H = Como jogar
        {
            ShowHowToPlayPanel();
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !panelHowToPlay.activeSelf) //Tecla Esc = Quit game
        {
            QuitGame();
        }else if (Input.GetKeyDown(KeyCode.Escape) && panelHowToPlay.activeSelf)
        {
            HideHowToPlayPanel();
        }


    }

    void GameStart() //carrega a primeira fase do jogo
    {
         SceneManager.LoadScene(1);
    }

    void ShowHowToPlayPanel() //exibe o painel com as instru��e de como jogar
    {
        panelHowToPlay.SetActive(true);
    }

    void HideHowToPlayPanel() //oculta o painel com as instru��e de como jogar
    {
        panelHowToPlay.SetActive(false);
    }

    void QuitGame() // finaliza o jogo
    {
        Application.Quit();
    }
}

