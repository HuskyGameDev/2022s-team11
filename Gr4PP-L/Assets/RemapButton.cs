using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RemapButton : MonoBehaviour
{

    private Managers.InputManager _im;
    [SerializeField]
    private string _axisName;
    [SerializeField]
    private TextMeshProUGUI _btnTxt;

    private static Color _unclickedClr = new Color(0.65f, 0.65f, 0.65f);
    private static Color _clickedClr = new Color(0.65f, 0.80f, 0.65f);
    // Start is called before the first frame update
    void Start()
    {
        _im = GameManager.Instance.Get<Managers.InputManager>();
        _btnTxt.text = _im.GetAxisName(_axisName);
    }

    public void Clicked()
    {
        _im.StartRebind(_axisName, FinishRebind);
        GetComponent<Image>().color = _clickedClr;
    }

    private void FinishRebind()
    {
        _btnTxt.text = _im.GetAxisName(_axisName);
        GetComponent<Image>().color = _unclickedClr;
    }
}
