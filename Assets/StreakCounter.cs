using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StreakCounter : MonoBehaviour
{
    TMPro.TextMeshProUGUI text;

    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = $"Current Streak: {GameController.instance.streak}\r\nNumber of Skips Used: {GameController.instance.skips}";
    }
}
