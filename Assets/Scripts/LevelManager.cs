using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelManager : MonoBehaviour
{
    private string sceneName;
    private void Update()
    {
        sceneName = SceneManager.GetActiveScene().name;
    }
    public void NextLevel()
    {
        SceneManager.LoadScene(2);
    }
    public void Retry()
    {
        if(sceneName == "Level1") { SceneManager.LoadScene(1); }
        if(sceneName == "Level2") { SceneManager.LoadScene(2); }
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Play()
    {
        SceneManager.LoadScene(1);
    }
    public void Exit()
    {
        Application.Quit();
    }
    public void Support()
    {
        string url = "https://www.youtube.com/channel/UCGPS3t2FfqaljxiaNs9YaSA";
        Application.OpenURL(url);
    }
}
