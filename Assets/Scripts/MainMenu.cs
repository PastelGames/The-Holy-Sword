using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public GameObject titleText;
    public GameObject playButton;
    public GameObject playBotToggle;
    GameObject god;

    // Start is called before the first frame update
    void Start()
    {
        LeanTween.scale(titleText, Vector2.one, .75f).setOnComplete(() => {
            LeanTween.scale(playButton, Vector2.one * 1.75f, .15f);
            LeanTween.scale(playBotToggle, Vector2.one * .56f, .18f);
        });

        playButton.GetComponent<Button>().onClick.AddListener(() =>
        {
            GameObject.Find("SceneSwapper").GetComponent<SceneSwapper>().LoadScene(2);
        });
        god = GameObject.Find("God");
    }

    // Update is called once per frame
    void Update()
    {
        
        god.GetComponent<God>().playAgainstBot = playBotToggle.GetComponent<Toggle>().isOn;
        
    }
}
