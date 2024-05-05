using UnityEngine;
using UniRx;
using System;

public class NumberGameHandler : MonoBehaviour
{
    [SerializeField] private PopUpNumberGame numberGamePopUpPrefab;
    private PopUpNumberGame numberGamePopUp;

    private NumberGamePresenter presenter;
    private Transform canvasTran;

    private IDisposable gameStateSubscription;
    private CompositeDisposable disposables = new();


    /// <summary>
    /// GameManager から呼ばれる
    /// </summary>
    /// <param name="canvasTran"></param>
    /// <param name="playerManager"></param>
    public void GenerateNumberGamePopUp(Transform canvasTran) {
        numberGamePopUp = Instantiate(numberGamePopUpPrefab, canvasTran, false);
        numberGamePopUp.gameObject.SetActive(false);

        this.canvasTran = canvasTran;

        // 未使用部分
        // if (numberGamePopUp.TryGetComponent(out presenter)) {
        //     // ゲーム開始
        //     presenter.InitializeGame(disposables, canvasTran);
        //     Debug.Log("Initialize 終了");
        // }
    }
    
    ///// ↓ Presenter と View のコメントアウトを戻してから復活させる部分
    
    // /// <summary>
    // /// ボタンから呼ばれる
    // /// </summary>
    // public void ActivateNumberGamePopUp() {
    //     
    //     // 数当てゲームのポップアップの表示
    //     numberGamePopUp.onShowPopUp += OnShowPopUpHandler;
    //     numberGamePopUp.gameObject.SetActive(true);
    //     numberGamePopUp.ShowPopUp();
    // }

    // /// <summary>
    // /// ShowPopUp() が呼ばれたときの処理
    // /// </summary>
    // private void OnShowPopUpHandler() {
    //     if (numberGamePopUp.TryGetComponent(out presenter)) {
    //         presenter.ExecuteGame();
    //         // ゲーム開始
    //         // 新しくインスタンスしたものを参照渡しすることで同じ処理の購読が出来、再度、停止もできる
    //         //   → 受け渡し元で新しくインスタンスを作成していれば普通の値渡しでも問題ない
    //         presenter.InitializeGame(disposables, canvasTran);
    //         Debug.Log("Initialize 終了");
    //     }
    //     
    //     // CurrentNumberGameState プロパティの購読処理
    //     gameStateSubscription = presenter.NumberGameModel.CurrentNumberGameState
    //         .Where(gameState => gameState == NumberGameState.GameUp)
    //         .Subscribe(_ =>
    //         {
    //             // ポップアップの非表示
    //             numberGamePopUp.HidePopUp();
    //
    //             // ゲーム終了処理
    //             ExitNumberGame();
    //         });
    // }
    
    // /// <summary>
    // /// ゲーム終了処理
    // /// </summary>
    // private void ExitNumberGame()
    // {
    //     // 削除しておくことで、次回ポップアップが開かれたときに OnShowPopUpHandler が複数回実行されることを防ぐ
    //     numberGamePopUp.onShowPopUp -= OnShowPopUpHandler;
    //     
    //     // OnShowPopUpHandler 内の購読処理の停止
    //     gameStateSubscription?.Dispose();
    //     
    //     // CompositeDisposable の Dispose は一度だけしか実行できない
    //     // そのため、新たな IDisposable オブジェクトを追加しても、それらのオブジェクトは自動的に破棄される(破棄の処理が動いているため)
    //     disposables.Dispose();
    //     
    //     // 登録した購読処理は、Dispose してしまうとずっと購読停止され続けるので、新しくインスタンスを作り直す
    //     // よって CompositeDisposable は使いまわせない。
    //     disposables = new CompositeDisposable();
    // }
    
    
    ///// ここまで
    

    private void OnDestroy() {
        // OnShowPopUpHandler 内の購読処理の停止
        gameStateSubscription?.Dispose();
        
        // Presenter 内の View 関係の購読処理の停止
        disposables.Dispose();
    }
}