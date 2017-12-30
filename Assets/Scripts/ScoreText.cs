using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreText : MonoBehaviour {
    Text text;

    // Use this for initialization
    void Awake () {
        text = GetComponent<Text> ();
    }

    int Score;

    void Start () {
        SetScoreText ();
    }

    void Update () {
        if (Score != ScoreManager.Score) {
            SetScoreText ();

            Score = ScoreManager.Score;
        }

    }

    public void SetScoreText () {
        text.text = ScoreManager.Score.ToString ();
    }
}