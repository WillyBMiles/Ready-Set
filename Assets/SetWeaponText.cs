using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetWeaponText : MonoBehaviour
{
    TextMeshProUGUI text;
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponentInChildren<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (TestMovement.currentSelection == null)
        {
            image.enabled = false;
            text.enabled = false;
        }else
        {
            image.enabled = true;
            text.enabled = true;
            text.text = TestMovement.currentSelection.GetComponent<Character>().weapon.description;
        }
    }
}
