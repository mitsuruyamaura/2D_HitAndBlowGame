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
    
    [SerializeField] private Text[] txtSelectNumbers;  // 回答した番号の表示
    
    // OnNumberButtonClickAsObservableプロパティは、numberButtons配列の各ボタンに対してOnClickAsObservable()を購読し、クリックされたボタンの要素番号をストリームに流すIObservable<int>を作成している
    // numberButtonList.Select()は、各ボタンとそのインデックスを引数として、ボタンがクリックされたときにインデックスを流すIObservable<int>を返す
    // これにより、numberButtons配列の各ボタンが自分のインデックスを記録し、クリックされたときにそれをストリームに流すことができる
    // また、最後にあるMerge()は、numberButtonList.Select()によって生成された複数のIObservable<int>ストリームを1つのIObservable<int>ストリームに統合している
    // これにより、OnNumberButtonClickAsObservableプロパティは、どのボタンがクリックされたかに関係なく、クリックされたボタンのインデックスを流す単一のIObservable<int>ストリームとして扱うことができる
    public IObservable<int> OnNumberButtonClickAsObservable => numberButtonList.Select((button, index) => button.OnClickAsObservable().Select(_ => index)).Merge();
    
    [SerializeField] private Button callButton;
    [SerializeField] private Button deleteButton;
        
    public Button CallButton => callButton;  // プロパティ
    public Button DeleteButton => deleteButton;  // プロパティ
    
    // OnClickAsObservable()の購読処理を施したボタンのプロパティ
    public IObservable<Unit> OnCallButtonClickAsObservable => callButton.OnClickAsObservable();
    public IObservable<Unit> OnDeleteButtonClickAsObservable => deleteButton.OnClickAsObservable();
    
    [SerializeField] private Text txtExplanation;
    
    
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
    public void SwitchAllButtons(bool isSwitch) 
    {
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
        for (int i = 0; i < txtSelectNumbers.Length; i++) {
            txtSelectNumbers[i].text = i < inputNumberList.Count ? inputNumberList[i].ToString() : "";
        }
    }
    
    /// <summary>
    /// 入力した回答の画面表示更新
    /// </summary>
    /// <param name="ansCount"></param>
    /// <param name="inputNumbers"></param>
    /// <param name="hit"></param>
    /// <param name="blow"></param>
    public void UpdateExplanation(int ansCount, ReactiveCollection<int> inputNumbers, int hit, int blow) {
        
        // string.Join を使うことで、第2引数の配列か List の要素を１つずつ取り出し、第1引数の文字を間に加える
        // カンマを指定すればカンマ区切りの文字列になり、今回のように空白を入れれば要素同士がつながる
        var inputNumbersStr = string.Join("", inputNumbers);
        
        // 文字列補完
        var result = $"回答 {ansCount}回目：{inputNumbersStr}： {hit} HIT {blow} BLOW ";
        
        Debug.Log(txtExplanation);
        
        // StringBuiler クラスをインスタンスし、コンストラクタに txtExplanation.text を渡して初期化
        var stringBuilder = new StringBuilder(txtExplanation.text);
        
        // AppendLine メソッドを使い、result をstringBuilder の最後の行に加える
        // 明示的な改行命令がなくても、自動的に改行した上で最後の行に追加される
        stringBuilder.AppendLine(result);
        
        // 文字列に変換して画面表示を更新
        // StringBuilder を使用する理由は、文字列の連結操作が繰り返される場合に、パフォーマンスが向上するため
        // 文字列はイミュータブル（不変）なので、+= を使って文字列を連結するたびに新しい文字列が生成される
        // これが多くの連結操作で行われると、パフォーマンスが低下することがある
        // StringBuilder を使うことで、この問題を回避し、効率的に文字列の連結を行うことができる
        txtExplanation.text = stringBuilder.ToString();
    }
}