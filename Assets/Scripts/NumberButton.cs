using UnityEngine;
using UnityEngine.UI;

public class NumberButton : MonoBehaviour
{
    [SerializeField] private Text txtNumber;

    private int number;
    public int Number => number;

    /// <summary>
    /// /数字ボタンの初期化
    /// </summary>
    /// <param name="num"></param>
    public void SetUpNumberButton(int num) {
        number = num;

        txtNumber.text = number.ToString();
    }
}
