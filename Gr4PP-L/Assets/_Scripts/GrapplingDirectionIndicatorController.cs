using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingDirectionIndicatorController : MonoBehaviour
{
    private Vector2 _currPos;
    private Quaternion _currRot;
    private Vector2 _input;
    private readonly Vector2 UPPER_RIGHT = new Vector2(1, 1);
    private Movement.PlayerController _player = null;
    private SpriteRenderer _sr;
    private float _scale = 0.25f;
    // Start is called before the first frame update
    void Start()
    {
        this.gameObject.SetActive(PlayerPrefs.GetInt("ShowReticle") == 1);
        _currPos = Vector2.zero;
        _currRot = new Quaternion(0, 0, 0, 0);
        transform.localPosition = _currPos;
        transform.rotation = _currRot;
        _sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        if (_player == null)
        {
            _player = GameManager.Instance.FindPlayer();
            return;
        }

        _input = GameManager.Instance.DirectionalInput;

        if (_input.Equals(Vector2.zero))
        {
            _currPos = UPPER_RIGHT * _scale;
            _currRot.eulerAngles = new Vector3(0, 0, 45);
        }
        else if (!_input.Equals(_currPos * _scale))
        {
            _currPos = _input * _scale;
            _currRot.eulerAngles = _input.y < 0 ? new Vector3(0, 0, 360 - Vector2.Angle(_input, Vector2.right)) : new Vector3(0, 0, Vector2.Angle(_input, Vector2.right));
        }

        _sr.color = _player.CanGrapple ? Color.white : Color.black;
        transform.localPosition = _currPos;
        transform.rotation = _currRot;
    }
}
