using System;
using UnityEngine;
using UniRx;
using System.Linq;
using UniRx.Triggers;

public class NumberGamePresenter : MonoBehaviour
{
    [SerializeField] private NumberGameView view;
    
    private NumberGameModel model;
    public NumberGameModel NumberGameModel => model;
    
    [SerializeField] private int maxChallengeCount = 10;

    private Transform canvasTran;
    private CompositeDisposable disposableModels = new();

    private GameLogic gameLogic;


    void Start() {
        // デバッグ用
        InitializeGame(disposableModels, canvasTran);

        // デバッグ用
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Space))
            .Subscribe(_ =>
            {
                // 正解の表示
                Debug.Log($"正解 : {model.CorrectNumbersString}");
                
                // デバッグ用の乱数取得
                model.RandomInputNumbers();

                // ロジックの動作確認
                (int hit, int blow) result = gameLogic.CheckHitAndBlow(model.InputNumberList);

                Debug.Log(result.hit);
                Debug.Log(result.blow);
            });
        
        
        Debug.Log(model.InputNumberList.Count);
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="disposables"></param>
    /// <param name="canvasTran"></param>
    public void InitializeGame(CompositeDisposable disposables, Transform canvasTran) {

        // Model のインスタンス生成。ここで最大試行回数を設定
        model = new NumberGameModel(maxChallengeCount);

        // デバッグ用の乱数取得
        model.RandomInputNumbers();
        
        // GameLogic のインスタンス作成
        gameLogic = new GameLogic(model.CorrectNumbers);
        
        // TODO View の初期化

        this.canvasTran = canvasTran;

        view.GenerateNumberButtons();
        
        

        // 各数字ボタンのクリックイベントを購読。結合してあるので、個別に購読しなくてよい
        view.OnNumberButtonClickAsObservable
            // TODO 選択されている数が3以下 かつ すでに選択されている数字ではない
            .Where(number => model.InputNumberList.Count < 3 && !model.InputNumberList.Contains(number))

            // 連続クリック防止(重複は防止済)
            .ThrottleFirst(TimeSpan.FromSeconds(0.25f))
            .Subscribe(number =>
            {
                // 入力値として保持し、画面更新
                model.AddInputNumber(number);

                // 画面表示更新
                view.UpdateInputDisplay(model.InputNumberList);

                // 押された数字のボタンのみを無効にする
                view.DisableNumberButton(number);

                Debug.Log(number);
            })
            .AddTo(disposables);



        // Call ボタンのクリックイベントを購読
        // 上の処理で Call ボタンのオン・オフ切り替えをしているため、ここでは Subscribe のみでよい
        view.OnCallButtonClickAsObservable
            .Subscribe(_ => ProcessInputNumbers())
            .AddTo(disposables);

        // List を購読させることで ReactiveProperty(InputNumbersCount 変数) を１つ削除できる
        // Call ボタンの有効状態(on/off)を入力された数字の数に応じて変更
        // ReactiveCommandの実行可否状態をButton.interactableプロパティにバインドして
        // ReactiveCommandの状態に応じてボタンが有効化・無効化される
        // ReactiveCommandの状態は Select の条件の評価により、true か false になる
        model.InputNumberList.ObserveCountChanged()
            .Select(count => count == 3) // 評価後、true になったらストリームを int から bool に変換
            .ToReactiveCommand() // bool型のストリームをReactiveCommandに変換
            .BindTo(view.CallButton) // その後BindToを使ってViewクラスのCallButtonプロパティに紐づけ
            .AddTo(disposableModels);
        
        // ReactiveCollection クリア時の処理が必要な場合には追加
        model.InputNumberList.ObserveReset()
            .Subscribe(_ =>
            {
                view.UpdateInputDisplay(model.InputNumberList);
                view.SwitchAllButtons(true);
                Debug.Log("Clear");
            })
            .AddTo(disposableModels);

        // Delete ボタンのオンオフ切り替え
        model.InputNumberList.ObserveCountChanged()
            .Select(count => count > 0)
            .ToReactiveCommand()
            .BindTo(view.DeleteButton)
            .AddTo(disposableModels);
        
        // 削除ボタンのクリックイベントを購読
        view.OnDeleteButtonClickAsObservable
            .Where(_ => model.InputNumberList.Count > 0)
            .Subscribe(_ =>
            {
                // List の最後の要素を取り出す(Linq)
                int deletedNumber = model.InputNumberList.Last();
                //int deletedNumber = model.InputNumberList.LastOrDefault();  // こちらの方が安全
                
                model.RemoveLastInputNumber();
        
                // 削除したボタンを有効にする
                view.EnableNumberButton(deletedNumber);
        
                view.UpdateInputDisplay(model.InputNumberList);
                Debug.Log(deletedNumber);
            })
            .AddTo(disposables);

        // GameState の購読
        model.CurrentNumberGameState
            .Where(state => state == NumberGameState.Win || state == NumberGameState.Lose)
            .Subscribe(state =>
            {
                // TODO ゲームの勝敗を反映してリザルト表示
                
                Debug.Log("GameState : " + state);
            })
            .AddTo(disposableModels);
        
        // TODO チャレンジ回数の購読
        
        model.ClearInputNumbers();
    }
    
    /// <summary>
    /// 入力番号の評価処理
    /// チャレンジ回数の確認
    /// </summary>
    private void ProcessInputNumbers() {
    
        (int hit, int blow) result;
        model.IncrementAnsCount();
        result = gameLogic.CheckHitAndBlow(model.InputNumberList);
    
        // 3HIT検出した場合
        if (result.hit == 3) {
            // ゲームクリア。解除成功メッセージを表示
            model.CurrentNumberGameState.Value = NumberGameState.Win;
    
            return;
        }
    
        // チャレンジ回数に達した場合
        else if (model.AnsCount.Value >= model.MaxCount) {
            // ゲーム失敗。解除失敗メッセージを表示
            model.CurrentNumberGameState.Value = NumberGameState.Lose;
    
            return;
        }
    

        
        // 不正解の場合
        //StartCoroutine(ShowInputDetailCoroutine(result.hit, result.blow));
        
        // 入力された数値をクリアし、表示を更新する
        model.ClearInputNumbers();
    }
    
    // TODO 不正解の場合、数値の判定結果を通知する Detail の生成
    
    // TODO 正解の場合、リザルトの生成
    
}