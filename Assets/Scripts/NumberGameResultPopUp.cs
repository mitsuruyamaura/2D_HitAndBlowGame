using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NumberGameResultPopUp : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Image imgUnLock;

    [SerializeField] private Image imgLocked;
  
        
    /// <summary>
    /// リザルト表示
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <returns></returns>
    public IEnumerator ShowPopUp(bool isSuccess) {
        canvasGroup.alpha = 0;
        gameObject.SetActive(true);
            
        if (isSuccess) {
            Debug.Log("解除成功");
            imgUnLock.gameObject.SetActive(true);
            imgLocked.gameObject.SetActive(false);
        }
        else {
            Debug.Log("解除失敗");

            imgUnLock.gameObject.SetActive(false);
            imgLocked.gameObject.SetActive(true);
        }

        // アニメーションが終了するまで待機
        yield return canvasGroup.DOFade(1.0f, 0.5f).WaitForCompletion();
        yield return new WaitForSeconds(3.0f);

        HidePopUp();
    }


    public void HidePopUp() {
        canvasGroup.DOFade(0f, 0.5f).SetEase(Ease.Linear)
            .OnComplete(() => Destroy(gameObject));
    }
}
