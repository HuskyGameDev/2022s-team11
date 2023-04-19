using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

public class BestTimeDisplayer : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro _introText;

    [SerializeField]
    private TextMeshPro _1Text;

    [SerializeField]
    private TextMeshPro _2Text;

    [SerializeField]
    private TextMeshPro _3Text;

    [SerializeField]
    private TextMeshPro _4Text;

    private GameManager _gm;
    private Managers.TimerManager _tm;


    // Start is called before the first frame update
    void Start()
    {
        _gm = GameManager.Instance;
        _tm = _gm.Get<Managers.TimerManager>();

        

        float introTime = PlayerPrefs.GetFloat("Tutorial", 0);
        if (introTime > 0) {
            _introText.text = TimeSpan.FromSeconds(introTime).ToString(@"mm\:ss\:fff");
        } else {
            _introText.text = "--:--:---";
        }

        float oneTime = PlayerPrefs.GetFloat("River Scene", 0);
        if (oneTime > 0) {
            _1Text.text = TimeSpan.FromSeconds(oneTime).ToString(@"mm\:ss\:fff");
        } else {
            _1Text.text = "--:--:---";
        }

        float twoTime = PlayerPrefs.GetFloat("Noah Beginner", 0);
        if (twoTime > 0) {
            _2Text.text = TimeSpan.FromSeconds(twoTime).ToString(@"mm\:ss\:fff");
        } else {
            _2Text.text = "--:--:---";
        }

        float threeTime = PlayerPrefs.GetFloat("Ethan-Level", 0);
        if (threeTime > 0) {
            _3Text.text = TimeSpan.FromSeconds(threeTime).ToString(@"mm\:ss\:fff");
        } else {
            _3Text.text = "--:--:---";
        }

        float fourTime = PlayerPrefs.GetFloat("NoahMidLevel", 0);
        if (fourTime > 0) {
            _4Text.text = TimeSpan.FromSeconds(fourTime).ToString(@"mm\:ss\:fff");
        } else {
            _4Text.text = "--:--:---";
        }


    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
