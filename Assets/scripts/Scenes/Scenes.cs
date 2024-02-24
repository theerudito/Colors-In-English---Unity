using UnityEngine;
using UnityEngine.SceneManagement;


public class Scenes : MonoBehaviour
{


    public void NextScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene((index + 1) % SceneManager.sceneCountInBuildSettings);
    }

    public void BackScene()
    {
        int index = SceneManager.GetActiveScene().buildIndex;

        int indexBack = (index - 1 + SceneManager.sceneCountInBuildSettings) % SceneManager.sceneCountInBuildSettings;

        SceneManager.LoadScene(indexBack);
    }

}
