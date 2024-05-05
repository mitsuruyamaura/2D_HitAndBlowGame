using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class PopUpNumberGame : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    
    public UnityAction onShowPopUp;
    
    
    /// <summary>
    /// 数当てゲームポップアップの表示
    /// </summary>
    public void ShowPopUp() {
        canvasGroup.blocksRaycasts = true;
           
        // ポップアップの表示
        canvasGroup.DOFade(1.0f, 0.5f).SetEase(Ease.Linear);

        onShowPopUp?.Invoke();
    }
    
    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    public void HidePopUp() {
            
        // ポップアップの非表示
        canvasGroup.DOFade(0f, 0.5f).SetEase(Ease.Linear);

        canvasGroup.blocksRaycasts = false;
    }
}