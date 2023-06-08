using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class NumberInputPopupController : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Text[] txtInputNumbers;

    [SerializeField] private Text txtResult;


    /// <summary>
    /// 今回の入力結果の表示
    /// </summary>
    /// <param name="numbers"></param>
    /// <param name="hit"></param>
    /// <param name="blow"></param>
    public void ShowInputNumbers(int[] numbers, int hit, int blow) {

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 0;

        for (int i = 0; i < txtInputNumbers.Length; i++) {
            txtInputNumbers[i].text = numbers[i].ToString();
            Debug.Log(i + "番目 : " + numbers[i]);
        }

        txtResult.text = hit + " HIT " + blow + " BLOW";
        Debug.Log("ShowInputNumbers : " + txtResult.text);

        canvasGroup.DOFade(1.0f, 0.5f);
    }

    /// <summary>
    /// 非表示
    /// </summary>
    public void HidePopUp() {
        canvasGroup.DOFade(0.0f, 0.5f);
            
        canvasGroup.blocksRaycasts = false;
            
        //umberGameGenerator.InActivatePlacementNumberGameDetailPopUp();
    }
}
