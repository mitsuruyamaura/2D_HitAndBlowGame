using System.Collections.Generic;
using UniRx;

public class GameLogic
{
    // コンストラクタで初期化する場合には readonly をつけられる(ついていても代入できる)
    // 通常のメソッドの場合には代入できないので外す必要がある
    private readonly List<int> correctNumbers;

    /// <summary>
    /// 毎回インスタンスを作成する場合にはコンストラクタで初期化する
    /// </summary>
    /// <param name="correctNumbers"></param>
    public GameLogic(List<int> correctNumbers) {
        this.correctNumbers = correctNumbers;
    }
    
    // １つのインスタンスをずっと使う場合にはこちらの処理を入れてコンストラクタの代わりに初期化させる
    // public void InitialGameLogic(List<int> correctNumbers) {
    //     this.correctNumbers = correctNumbers;
    // }

    /// <summary>
    /// Hit と Blow の評価
    /// ValueTuple の場合には戻り値に変数名をつけられる
    /// </summary>
    /// <param name="inputNumbers"></param>
    /// <returns></returns>
    public (int hit, int blow) CheckHitAndBlow(ReactiveCollection<int> inputNumbers) {
        int hit = 0;
        int blow = 0;

        for (int i = 0; i < inputNumbers.Count; i++) {
            if (inputNumbers[i] == correctNumbers[i]) {
                hit++;
            } else if (correctNumbers.Contains(inputNumbers[i])) {
                blow++;
            }
        }

        return (hit, blow);
    }
}