using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LogoSceneManager : MonoBehaviour
{
    private int _index;

    private void Start()
    {
        _index = SceneManager.GetActiveScene().buildIndex;
        StartCoroutine(LoadGame());
    }

    private IEnumerator LoadGame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(++_index);
    }
}