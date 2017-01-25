using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

	public string mainMenuName;
	public string gameName;

	static SceneChanger _instance;

	public static SceneChanger instance { get { return _instance; } }


	void Start()
	{
		if (_instance != null)
		{
			Destroy(gameObject);
			return;
		}
		_instance = this;
		DontDestroyOnLoad(gameObject);
	}

	public void LoadMainMenu()
	{
		SceneManager.LoadScene(mainMenuName);
	}

	public void LoadGame()
	{
		SceneManager.LoadScene(gameName);
	}
    public void Quit()
    {
        Application.Quit();
    }
}
