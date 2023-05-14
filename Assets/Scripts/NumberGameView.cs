using System;
using System.Collections.Generic; // IObservable を使うために必要
using System.Linq;
using System.Text;  // StringBuilder を使うために必要
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class NumberGameView : MonoBehaviour
{
    [SerializeField] private NumberButton numberButtonPrefab;
    [SerializeField] private Transform numberButtonTran;
    private int buttonCount = 10;

    [SerializeField] private List<Button> numberButtonList = new();
    [SerializeField] private List<Text> numberTextList = new();
    
    // OnNumberButtonClickAsObservableプロパティは、numberButtons配列の各ボタンに対してOnClickAsObservable()を購読し、クリックされたボタンの要素番号をストリームに流すIObservable<int>を作成している
    // numberButtonList.Select()は、各ボタンとそのインデックスを引数として、ボタンがクリックされたときにインデックスを流すIObservable<int>を返す
    // これにより、numberButtons配列の各ボタンが自分のインデックスを記録し、クリックされたときにそれをストリームに流すことができる
    // また、最後にあるMerge()は、numberButtonList.Select()によって生成された複数のIObservable<int>ストリームを1つのIObservable<int>ストリームに統合している
    // これにより、OnNumberButtonClickAsObservableプロパティは、どのボタンがクリックされたかに関係なく、クリックされたボタンのインデックスを流す単一のIObservable<int>ストリームとして扱うことができる
    public IObservable<int> OnNumberButtonClickAsObservable => numberButtonList.Select((button, index) => button.OnClickAsObservable().Select(_ => index)).Merge();
    
    
    void Start() {
        // デバッグ用
        //GenerateNumberButtons();    
    }

    /// <summary>
    /// 数字ボタンの生成と設定
    /// </summary>
    public void GenerateNumberButtons() {
        for (int i = 0; i < buttonCount; i++) {
            int index = i;
            NumberButton numberButton = Instantiate(numberButtonPrefab, numberButtonTran, false);
            numberButton.SetUpNumberButton(index);
            numberButtonList.Add(numberButton.BtnNumber);
            numberTextList.Add(numberButton.TxtNumber);
        }
    }
    
    /// <summary>
    /// ボタンを非活性化
    /// </summary>
    /// <param name="number"></param>
    public void DisableNumberButton(int number)
    {
        numberButtonList[number].interactable = false;
    }

    /// <summary>
    /// ボタンを活性化
    /// </summary>
    /// <param name="number"></param>
    public void EnableNumberButton(int number)
    {
        numberButtonList[number].interactable = true;
    }
    
    /// <summary>
    /// 全数字ボタンの状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchAllButtons(bool isSwitch) {

        for (int i = 0; i < numberButtonList.Count; i++) {
            numberButtonList[i].interactable = isSwitch;
        }
    }
    
    /// <summary>
    /// 入力した数字を画面に表示
    /// </summary>
    /// <param name="inputNumberList"></param>
    public void UpdateInputDisplay(ReactiveCollection<int> inputNumberList) {
        //Debug.Log(inputNumberList.Count);
        for (int i = 0; i < numberTextList.Count; i++) {
            numberTextList[i].text = i < inputNumberList.Count ? inputNumberList[i].ToString() : "";
        }
    }
}