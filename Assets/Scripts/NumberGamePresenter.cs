using System;
using System.Collections;
using UnityEngine;
using UniRx;
using System.Linq;

public class NumberGamePresenter : MonoBehaviour
{
    [SerializeField] private NumberGameView view;
    
    private NumberGameModel model;
    public NumberGameModel NumberGameModel => model;
    
    [SerializeField] private int maxChallengeCount = 10;

    private Transform canvasTran;
    private CompositeDisposable disposableModels = new();


    void Start() {
        // デバッグ用
        InitializeGame(disposableModels, canvasTran);    
    }

    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="disposables"></param>
    /// <param name="canvasTran"></param>
    public void InitializeGame(CompositeDisposable disposables, Transform canvasTran) {

        // Model のインスタンス生成。ここで最大試行回数を設定
        model = new NumberGameModel(maxChallengeCount);

        // TODO GameLogic のインスタンス作成

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

                // TODO 画面表示更新
                view.UpdateInputDisplay(model.InputNumberList);

                // 押された数字のボタンのみを無効にする
                view.DisableNumberButton(number);

                Debug.Log(number);
            })
            .AddTo(disposables);

        // TODO 削除ボタンのクリックイベントを購読


        // TODO Call ボタンのクリックイベントを購読
        // 上の処理で Call ボタンのオン・オフ切り替えをしているため、ここでは Subscribe のみでよい

        // List を購読させることで ReactiveProperty(InputNumbersCount 変数) を１つ削除できる
        // Call ボタンの有効状態(on/off)を入力された数字の数に応じて変更
        // ReactiveCommandの実行可否状態をButton.interactableプロパティにバインドして
        // ReactiveCommandの状態に応じてボタンが有効化・無効化される
        // ReactiveCommandの状態は Select の条件の評価により、true か false になる
        model.InputNumberList.ObserveCountChanged()
            .Select(count => count == 3) // 評価後、true になったらストリームを int から bool に変換
            .ToReactiveCommand() // bool型のストリームをReactiveCommandに変換
            //.BindTo(view.CallButton) // その後BindToを使ってViewクラスのCallButtonプロパティに紐づけ
            .AddTo(disposableModels);

        // GameState の購読
        model.CurrentNumberGameState
            .Where(state => state == NumberGameState.Win || state == NumberGameState.Lose)
            .Subscribe(state =>
            {
                // TODO ゲームの勝敗を反映してリザルト表示
                
                Debug.Log("GameState : " + state);
            })
            .AddTo(disposableModels);
        
    }
}