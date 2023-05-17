using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

[Serializable]
public class NumberGameModel
{
    public ReactiveCollection<int> InputNumberList = new(); 

    public List<int> CorrectNumbers { get; private set; }
    
    public ReactiveProperty<int> AnsCount = new ();
    public int MaxCount { get; private set; }
    
    public ReactiveProperty<NumberGameState> CurrentNumberGameState { get; } = new ();
    
    
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="maxCount"></param>
    public NumberGameModel(int maxCount) {
        
        // TODO ゲームステートの初期化
        CurrentNumberGameState.Value = NumberGameState.Play;
        
        // TODO 正解の数字の設定
        
        // 回答数の初期化
        AnsCount.Value = 0;
        
        // 最大回答数の設定
        MaxCount = maxCount;
    }
    
    // TODO 数あてゲームの正解を作る

    /// <summary>
    /// ReactiveCollection に追加。
    /// Presenter で購読し、Call ボタンのオンオフを監視
    /// </summary>
    /// <param name="number"></param>
    public void AddInputNumber(int number) {
        // 数字が追加されたら、入力された数字の数を更新
        InputNumberList.Add(number);
    }

    /// <summary>
    /// ReactiveCollection から削除。
    /// Presenter で購読し、Call ボタンのオンオフを監視
    /// </summary>
    public void RemoveLastInputNumber() {
        // 最後に登録された番号を削除
        InputNumberList.RemoveAt(InputNumberList.Count - 1);
    }

    public void IncrementAnsCount() {
        AnsCount.Value++;

        UnityEngine.Debug.Log(AnsCount.Value);
    }
}