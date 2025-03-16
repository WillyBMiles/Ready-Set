using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialController : MonoBehaviour
{
    int current;
    List<string> TutorialTexts = new List<string>()
    {
        "Welcome to Ready Set, a tactical game where you plan out just 3 seconds of action.",
        "Use WASD to move the camera around",
        "At the bottom is the timeline, it shows a preview of the current time. Press Q and E to step through the timeline.",
        "The green characters are your operatives, you can give them commands to tell them what to do throughout the 3 seconds. At least one must survive.",
        "The red characters are hostiles, you must eliminate all of them by the end of the 3 seconds.",
        "The blue characters are hostages, if even one falls you fail your mission.",
        "To give one of your operatives a command, left click them. Then you can click somewhere on the ground to command them to move there. (Hold shift to command them to walk.)",
        "Remember to step through the timeline with Q and E to see where the operative will be at any given time. Right click to remove the last move command.",
        "When at a particular point in the timeline, press P to command the operative to pause, meaning they won't move for a moment.",
        "To command an operative to use their weapon press O while hovering your mouse over where you want them to shoot. Operatives are more accurate while walking or standing still.",
        "To try out your plan press Enter. In this tutorial world you can retry as much as you like but in the actual game you'll only get one chance when you hit enter!",
        "Good luck! See how many missions you can succeed in a row! Press Escape to return to the menu when you're ready!"

    };
    TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        current = Mathf.Clamp(current, 0, TutorialTexts.Count-1);
        text.text = TutorialTexts[current];
    }

    public void NextText()
    {
        current++;
    }
    public void PreviousText()
    {
        current--;
    }
}
