using System;
using System.Collections;
using UnityEngine;
using UniRx;
using System.Linq;

public class NumberGamePresenter : MonoBehaviour
{
    [SerializeField] private NumberGameView view;

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
    
        // TODO View の初期化
        
        this.canvasTran = canvasTran;
    
        view.GenerateNumberButtons();
    
        // 各数字ボタンのクリックイベントを購読。結合してあるので、個別に購読しなくてよい
        view.OnNumberButtonClickAsObservable
            // TODO 選択されている数が3以下 かつ すでに選択されている数字ではない
            //.Where(number => model.InputNumberList.Count < 3 && !model.InputNumberList.Contains(number))
            .Subscribe(number =>
            {
                // TODO 入力値として保持し、画面更新
                
                // TODO 画面表示更新
                
    
                // 押された数字のボタンのみを無効にする
                view.DisableNumberButton(number);
    
                Debug.Log(number);
            })
            .AddTo(disposables);
    
        // TODO 削除ボタンのクリックイベントを購読

    
        // TODO Call ボタンのクリックイベントを購読
        // 上の処理で Call ボタンのオン・オフ切り替えをしているため、ここでは Subscribe のみでよい
        
    }
}