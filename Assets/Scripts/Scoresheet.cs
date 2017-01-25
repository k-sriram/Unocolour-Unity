using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Scoresheet : MonoBehaviour {


    public Text score;
    public Text buttonText;

    public void Button()
    {
        gameObject.SetActive(false);
    }

    public void ShowScore(int score, bool final)
    {
        gameObject.SetActive(true);
        this.score.text = score.ToString();
        buttonText.text = final ? "Main Menu" : "Continue";
    }
        



}
