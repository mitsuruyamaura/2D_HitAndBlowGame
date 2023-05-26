using System;
using System.Collections.Generic;
using System.Linq;
using UniRx;

[Serializable]
public class NumberGameModel
{
    public ReactiveCollection<int> InputNumberList = new(); 
    
    public ReactiveProperty<int> AnsCount = new ();
    public int MaxCount { get; private set; }
    
    public ReactiveProperty<NumberGameState> CurrentNumberGameState { get; } = new ();
    
    public List<int> CorrectNumbers { get; private set; }
    
    
    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="maxCount"></param>
    public NumberGameModel(int maxCount) {
        
        // ゲームステートの初期化
        CurrentNumberGameState.Value = NumberGameState.Play;
        
        // 正解の数字の設定
        CorrectNumbers = GenerateCorrectNumbers();
        
        // 回答数の初期化
        AnsCount.Value = 0;
        
        // 最大回答数の設定
        MaxCount = maxCount;
    }
    
    /// <summary>
    /// 数あてゲームの正解を作る
    /// </summary>
    private List<int> GenerateCorrectNumbers() {
        
        // 初期値の情報を元に、新しい List 作成
        List<int> availableNumbers = new (){ 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            
        // // 正解用の数字格納用の配列の初期化
        // int[] correctNumbers = new int[3];
        //
        // // ランダムな値を３つ取得。Remove することで重複する数字を選択しないようにする
        // for (int i = 0; i < correctNumbers.Length; i++) {
        //     
        //     //int randomIndex = UnityEngine.Random.Range(0, availableNumbers.Count);
        //     // System の Random クラスの場合、int 型の乱数は Next メソッドで作成
        //     int randomIndex = new Random().Next(0, availableNumbers.Count);
        //     correctNumbers[i] = availableNumbers[randomIndex];
        //     availableNumbers.RemoveAt(randomIndex);
        // }

        // 配列で作成していた処理を List かつ UniRx で作成
        // Random はここで１つだけインスタンスする。OrderBy の中で new すると毎回インスタンスされて効率が悪いため
        var random = new Random();
        
        // OrderBy を利用して、ランダムに取得された値を取得した順番に並べ、Take で先頭の３つを取り出す
        var correctNumbers = availableNumbers.OrderBy(x => random.Next()).Take(3).ToList();
        
        Console.WriteLine($"正解 : { string.Join(", ", correctNumbers)}");
        UnityEngine.Debug.Log($"正解 : { string.Join(", ", correctNumbers)}");
        
        return correctNumbers;
    }

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

    // TODO ゲームのリセット機能
    
}