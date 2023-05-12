using System;
using System.Collections.Generic; // IObservable を使うために必要
using System.Linq;
using System.Text;  // StringBuilder を使うために必要
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class NumberGameView : MonoBehaviour
{
    [SerializeField] private NumberButton numberButtonPrefab;
    [SerializeField] private Transform numberButtonTran;
    private int buttonCount = 10;

    [SerializeField] private List<Button> numberButtonList = new();
    

    void Start() {
        GenerateNumberButtons();    
    }

    /// <summary>
    /// 数字ボタンの生成と設定
    /// </summary>
    public void GenerateNumberButtons() {
        for (int i = 0; i < buttonCount; i++) {
            int index = i;
            NumberButton numberButton = Instantiate(numberButtonPrefab, numberButtonTran, false);
            numberButton.SetUpNumberButton(index);
            numberButtonList.Add(numberButton.BtnNumber);
        }
    }
}