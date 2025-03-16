using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreen : MonoBehaviour
{

    public string TutorialScene = "Tutorial";

    public void StartTutorial()
    {
        SceneManager.LoadScene(GameController.instance.TutorialScene);
    }

    public void StartGame()
    {
        GameController.instance.StartLevel();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
