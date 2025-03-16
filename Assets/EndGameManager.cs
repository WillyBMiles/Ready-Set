using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public TextMeshProUGUI success;
    public TextMeshProUGUI failureHostage;
    public TextMeshProUGUI failureEnemy;
    public TextMeshProUGUI failureOperator;
    public GameObject button;

    enum TypeOfFailure
    {
        Hostage,
        Enemy, 
        Operator
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    bool over = false;
    int start = 10;
    // Update is called once per frame
    void Update()
    {
        start--;

        if (over || start > 0)
            return;

        bool aliveOperator = false;
        bool aliveEnemy = false;
        foreach (Character c in TimeController.instance.allCharacters)
        {
            if (c.hostage)
            {
                if (c.S_body.enabled)
                {
                    Failure(TypeOfFailure.Hostage);
                    return;
                }
            }
            if (c.playerTeam)
            {
                if (!c.S_body.enabled)
                    aliveOperator = true;
            }
            if (!c.playerTeam && !c.hostage && !c.S_body.enabled)
            {
                aliveEnemy = true;
            }
        }
        if (!aliveOperator)
        {
            Failure(TypeOfFailure.Operator);
        }
        if (aliveEnemy && TimeController.instance.ended > 2f)
        {
            Failure(TypeOfFailure.Enemy);
        }
        if (!aliveEnemy && TimeController.instance.ended > 0f)
        {
            Success();
        }
    }

    public void Success()
    {
        success.gameObject.SetActive(true);
        GameController.instance.FinishLevel();
        success.text += GameController.instance.streak;
        button.SetActive(true);
        over = true;

    }

     void Failure(TypeOfFailure t)
    {
        button.SetActive(true);
        TextMeshProUGUI text = t switch
        {
            TypeOfFailure.Enemy => failureEnemy,
            TypeOfFailure.Hostage => failureHostage,
            TypeOfFailure.Operator => failureOperator,
            _ => throw new System.NotImplementedException(),
        };
        text.gameObject.SetActive(true);
        text.text += GameController.instance.streak;
        GameController.instance.FailLevel();
        over = true;
    }
    

    public void Continue()
    {
        GameController.instance.StartLevel();
    }

    public void Skip()
    {
        GameController.instance.SkipLevel();
    }
}
