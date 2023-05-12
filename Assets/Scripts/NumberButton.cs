using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class NumberButton : MonoBehaviour
{
    [SerializeField] private Text txtNumber;

    [SerializeField] private Button btnNumber;
    public Button BtnNumber => btnNumber;

    // Button の購読処理のプロパティ
    public IObservable<Unit> OnButtonClickAsObservable => btnNumber.OnClickAsObservable();

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
