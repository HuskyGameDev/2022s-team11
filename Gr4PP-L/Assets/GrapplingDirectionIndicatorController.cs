using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingDirectionIndicatorController : MonoBehaviour
{
    private Vector2 _currPos;
    private Vector2 _input;
    private readonly Vector2 UPPER_RIGHT = new Vector2(1,1);
    // Start is called before the first frame update
    void Start()
    {
        _currPos = Vector2.zero;
        transform.localPosition = _currPos;
    }

    // Update is called once per frame
    void Update()
    {   
        _input = GameManager.Instance.DirectionalInput;

        if (_input.Equals(Vector2.zero)) {
            _currPos = UPPER_RIGHT;
        } else if (!_input.Equals(_currPos)) {
            _currPos = _input;
        }

        transform.localPosition = _currPos;
    }
}
