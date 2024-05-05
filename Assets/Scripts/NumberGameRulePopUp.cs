using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NumberGameRulePopUp : MonoBehaviour
{
    [SerializeField]
    private CanvasGroup canvasGroup;

    [SerializeField]
    private Button btnDel;
        
        
        
    /// <summary>
    /// 数あてゲームの設定
    /// </summary>
    /// <param name="omikujiGenerator"></param>
    public void SetUpNumberGameRulePopUp()
    {
        // ポップアップを一度見えない状態にする
        canvasGroup.alpha = 0;

        // 各ボタンにメソッドを登録
        btnDel.onClick.AddListener(OnClickClose);
    }

    /// <summary>
    /// 配置を止めるボタンを押した際の処理
    /// </summary>
    private void OnClickClose()
    {
        // ポップアップの非表示
        HidePopUp();
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    private void HidePopUp()
    {
        // ポップアップの非表示
        canvasGroup.DOFade(0, 0.5f).SetEase(Ease.Linear);

        canvasGroup.blocksRaycasts = false;
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public void ShowPopUp() {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.DOFade(1.0f, 0.5f);
    }
}