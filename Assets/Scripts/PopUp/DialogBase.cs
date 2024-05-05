using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using DG.Tweening;
using UniRx;

public class DialogBase : MonoBehaviour, IDialogClose
{
    protected UnityAction onCloseAction;
    protected Canvas dialogCanvas;
    protected float fadeDuration = 1.0f;
    
    protected UnityAction<ItemData> onCloseActionItemData;
    protected List<ButtonBase> buttonList = new();
    protected int itemCount = 5;  // デバッグ用。本来は所持アイテムの数を参照する
    
#pragma warning disable 0649
    [SerializeField] protected CanvasGroup canvasGroupDialog;
    [SerializeField] protected Button btnClose;
    [SerializeField] protected Ease fadeEase = Ease.InQuart;
    [SerializeField] protected ButtonBase buttonPrefab;
    [SerializeField] protected Transform buttonSetTran;
#pragma warning restore 0649

    public IObservable<ItemData> OnItemButtonClickAsObservable => buttonList
        .Select(buttonBase => buttonBase.OnButtonClickAsObservable.Select(_ => buttonBase.ItemData))
        .Merge();

    protected ItemData chooseItemData;

    /// <summary>
    /// Dialog を使いまわす場合にはインスタンス後に最初に１回だけ実行する
    /// 引数で外部クラスの処理を受け取ることで依存関係を持たなくて済む
    /// </summary>
    /// <param name="closeAction"></param>
    public virtual void SetUp(UnityAction closeAction) {
        
        if(!transform.parent.TryGetComponent(out dialogCanvas))
        {
            Debug.Log("dialogCanvas 取得出来ません");
            return;
        }
        
        // 外部クラスの処理を保持しておく
        onCloseAction = closeAction;
        
        // ダイアログを閉じるボタンに処理を登録①
        //btnClose.onClick.AddListener(() => Hide());

        // ダイアログを閉じるボタンに処理を登録②
        btnClose.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => Hide())
            .AddTo(this);
        
        // TODO 他にも１回だけ設定したいものを追加
        
    }
    
    /// <summary>
    /// オーバーロード機能を利用
    /// Dialog を使いまわす場合にはインスタンス後に最初に１回だけ実行する
    /// 引数で外部クラスの処理を受け取ることで依存関係を持たなくて済む
    /// </summary>
    /// <param name="closeActionItem"></param>
    public virtual void SetUp(UnityAction<ItemData> closeActionItem) {
        
        if(!transform.parent.TryGetComponent(out dialogCanvas))
        {
            Debug.Log("dialogCanvas 取得出来ません");
            return;
        }
        
        // 外部クラスの処理を保持しておく
        onCloseActionItemData = closeActionItem;
        
        // ダイアログを閉じるボタンに処理を登録①
        //btnClose.onClick.AddListener(() => Hide());

        // ダイアログを閉じるボタンに処理を登録②
        btnClose.OnClickAsObservable()
            .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => Hide())
            .AddTo(this);
        
        // TODO 他にも１回だけ設定したいものを追加
        
    }
    
    /// <summary>
    /// 外部クラスから実行
    /// ダイアログを表示
    /// </summary>
    public virtual void Show() {
        
        dialogCanvas.enabled = true;
        canvasGroupDialog.alpha = 0;
        
        // 表示準備や再設定がある場合
        OnEnterOpen();
        
        // ダイアログを表示するアニメ開始
        PlayOpenDialog();
    }

    /// <summary>
    /// ダイアログ内の表示準備や再設定がある場合に利用する
    /// </summary>
    protected virtual void OnEnterOpen() {
        
        // TODO 再設定の処理が必要な場合に処理を追加
        
        // 表示中のボタンの数と、新しく表示するボタンの数が変わっていないときも一緒にチェックして return させる 
        if(buttonList.Count > 0) return;

        for (int i = 0; i < itemCount;i++) {
            ButtonBase button = Instantiate(buttonPrefab, buttonSetTran, false);
            button.SetUpButton(new(i));
            buttonList.Add(button);
        }

        // 各ボタンの挙動をまとめて制御
        OnItemButtonClickAsObservable
            .ThrottleFirst(TimeSpan.FromSeconds(1.0f))
            .Subscribe(itemData => chooseItemData = itemData)
            .AddTo(this);
    }

    /// <summary>
    /// ダイアログを表示するアニメ
    /// </summary>
    protected virtual void PlayOpenDialog() {
        canvasGroupDialog
            .DOFade(1.0f, fadeDuration)
            .SetEase(fadeEase)
            .SetLink(gameObject)
            .OnComplete(() => OnExitOpen());
    }

    /// <summary>
    /// ダイアログ表示後に実行しておく処理
    /// </summary>
    protected virtual void OnExitOpen() {
        // TODO 開いたときの挙動
        
    }

    /// <summary>
    /// ボタンから実行
    /// ダイアログを閉じる
    /// </summary>
    /// <returns></returns>
    public virtual bool Hide() {

        // 非表示にする前にやっておく処理
        OnEnterClose();
            
        // ダイアログを非表示にするアニメ開始
        PlayCloseDialog();
        
        return true;
    }

    /// <summary>
    /// 非表示にする前にやっておく処理
    /// </summary>
    protected virtual void OnEnterClose() {
        // 外部クラスで登録した外部の処理を実行
        //onCloseAction?.Invoke();
        
    }
    
    /// <summary>
    /// ダイアログを非表示にするアニメ
    /// </summary>
    protected virtual void PlayCloseDialog() {
        canvasGroupDialog
            .DOFade(0, fadeDuration)
            .SetEase(fadeEase)
            .SetLink(gameObject)
            .OnComplete(() => OnExitClose(chooseItemData));
    }

    /// <summary>
    /// ダイアログ非表示後に実行しておく処理
    /// </summary>
    protected virtual void OnExitClose() {
        dialogCanvas.enabled = false;
        
        // TODO 他にもあれば追加
        
        // 外部クラスで登録した外部の処理を実行
        onCloseAction?.Invoke();
        
        //onCloseActionItemData?.Invoke(chooseItemData);
        chooseItemData = null;
        
        //onCloseActionItemData?.Invoke(new(1));
    }


    protected virtual void OnExitClose(ItemData itemData) {
        
        onCloseActionItemData?.Invoke(itemData);
        chooseItemData = null;
    }
}
