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
    private Managers.TimerManager _tm;
    // Start is called before the first frame update
    void Start()
    {
        _gm = GameManager.Instance;
        _tm = _gm.Get<Managers.TimerManager>();

        _currentTimeText.text = "Current Time: " + _tm.CurrentTime.ToString();

        if (!_tm.BestTimeFollowsCurrent)
            _bestTimeText.text = "Best Time: " + _tm.BestTime.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        TimeSpan time = TimeSpan.FromSeconds(_gm.Get<Managers.TimerManager>().CurrentTime);

        _currentTimeText.text = "Current Time: " + time.ToString(@"mm\:ss\:fff");

        // If the Level has not yet been completed, sets the BestTime value to be the same as CurrentTime
        if (_tm.BestTimeFollowsCurrent)
            _bestTimeText.text = "Best Time: " + time.ToString(@"mm\:ss\:fff");
    }
}
