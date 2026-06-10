using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void GoToStartGame()
    {
        SceneManager.LoadScene("StartGame");
    }
}