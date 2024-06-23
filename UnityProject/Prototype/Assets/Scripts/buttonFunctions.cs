using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    [SerializeField] GameObject quitGame;
    public Animator animator;
    public float transition = 1f;

	void Start()
	{
		if (Application.platform == RuntimePlatform.WebGLPlayer)
		{
			quitGame.gameObject.SetActive(false);
		}
	}

	public void PlayGame()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }
    public void resume()
    {
        gameManager.instance.stateUnpause();
    }

    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnpause();
    }

    IEnumerator LoadLevel(int level)
    {
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(transition);
        SceneManager.LoadScene(level);
    }

    public void respawnPlayer()
    {
        gameManager.instance.playerScript.spawnPlayer();
        gameManager.instance.stateUnpause();
    }
    
    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
