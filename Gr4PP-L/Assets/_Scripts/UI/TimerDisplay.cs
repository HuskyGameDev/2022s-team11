using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    [SerializeField]
    private Text _currentTimeText;
    [SerializeField]
    private Text _bestTimeText;

    private GameManager _gm;
    // Start is called before the first frame update
    void Start()
    {
        _gm = GameManager.Instance;

        _currentTimeText.text = "Current Time: " + _gm.timerManager.CurrentTime.ToString();
        _bestTimeText.text = "Best Time: " + _gm.timerManager.BestTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(_gm.timerManager.CurrentTime);

        _currentTimeText.text = "Current Time: " + time.ToString(@"mm\:ss\:fff");

        // If the Level has not yet been completed, sets the BestTime value to be the same as CurrentTime
        if (_gm.timerManager.BestTime > _gm.timerManager.CurrentTime)
            _bestTimeText.text = "Best Time: " + time.ToString(@"mm\:ss\:fff");
    }
}
