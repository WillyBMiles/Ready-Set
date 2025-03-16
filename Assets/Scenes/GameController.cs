using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    public int streak;
    public int skips;


    public List<string> possibleLevels = new List<string>();

    public string startLevel = "StartScene";
    public string TutorialScene = "Tutorial";
    public const string STREAK_PREF = "STREAK";
    public const string SKIP_PREF = "SKIP";
    // Start is called before the first frame update
    void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
            
        instance = this;
        if (PlayerPrefs.HasKey(STREAK_PREF))
        {
            streak = PlayerPrefs.GetInt(STREAK_PREF);

        }
        if (PlayerPrefs.HasKey(SKIP_PREF))
        {
            skips = PlayerPrefs.GetInt(SKIP_PREF);
        }
        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        //IF THERE ARE NO ENEMIES LEFT, AT LEAST ONE PLAYER, AND NO HOSTAGES DEAD YOU WIN!
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(startLevel);
        }
    }

    public void StartLevel()
    {
        SceneManager.LoadScene(possibleLevels[Random.Range(0,possibleLevels.Count)]);
    }

    public void FinishLevel()
    {
        streak++;
        PlayerPrefs.SetInt(STREAK_PREF, streak);
    }

    public void SkipLevel()
    {
        skips++;
        PlayerPrefs.SetInt(SKIP_PREF, skips);
        StartLevel();
    }

    public void FailLevel()
    {
        streak = 0;
        skips = 0;
        PlayerPrefs.SetInt(STREAK_PREF, streak);
        PlayerPrefs.SetInt(SKIP_PREF, skips);
    }

}
