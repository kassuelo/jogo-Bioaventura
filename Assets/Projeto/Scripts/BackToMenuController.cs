using UnityEngine;
using UnityEngine.SceneManagement;


public class BackToMenuController : MonoBehaviour
{
    public void BackToMenu()
    {
        //ao retornar para o menu reseta o score
        GlobalVariables.SetScore(0);

        //carrega a cena do menu principal
        SceneManager.LoadScene(0);
    }
}
