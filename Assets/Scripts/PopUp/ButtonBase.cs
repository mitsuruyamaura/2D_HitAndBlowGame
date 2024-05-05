using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


public class ButtonBase : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] protected Button btnSubmit;
    [SerializeField] protected Image imgButton;
#pragma warning restore 0649
    
    //public Button BtnSubmit => btnSubmit;
    
    protected ItemData itemData;
    public ItemData ItemData => itemData;

    protected IObservable<Unit> onButtonClickAsObservable;
    public IObservable<Unit> OnButtonClickAsObservable => onButtonClickAsObservable;
    

    public void SetUpButton(ItemData itemData) {
        onButtonClickAsObservable = btnSubmit.OnClickAsObservable();
        
        this.itemData = itemData;
        
        // TODO 画像設定、文字設定など
        
    }
}
