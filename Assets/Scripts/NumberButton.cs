using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class NumberButton : MonoBehaviour
{
    [SerializeField] private Text txtNumber;
    public Text TxtNumber => txtNumber;

    [SerializeField] private Button btnNumber;
    public Button BtnNumber => btnNumber;

    // Button の購読処理のプロパティ(ボタンの購読処理のみを作成しておく。Subscribe する内容はこのクラスでは設定しない)
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
        
        //Debug.Log(this.GetInstanceID());
    }
}
