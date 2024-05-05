using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

[System.Serializable]
public class ItemData {
    public int id;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="no"></param>
    public ItemData(int no) {
        id = no;
    }
}

public class DialogTest : MonoBehaviour
{
#pragma warning disable 0649
    [SerializeField] private DialogBase dialogPrefab;
    [SerializeField] private Transform dialogTran;
#pragma warning restore 0649
    
    private DialogBase dialogInstance;  // 生成済のダイアログを代入して保持しておく


    void Start() {
        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.A))
            .ThrottleFirst(System.TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ => OpenDialog());

        this.UpdateAsObservable()
            .Where(_ => Input.GetKeyDown(KeyCode.Escape))
            .Where(_ => dialogInstance)
            .ThrottleFirst(System.TimeSpan.FromSeconds(1.0f))
            .Subscribe(_ =>
            {
                if (dialogInstance.Hide()) {
                    Debug.Log($"Android 端末の BackKey が押されたので、ダイアログを閉じます {dialogInstance.name}");
                }
            });
    }

    /// <summary>
    /// ダイアログ表示
    /// </summary>
    private void OpenDialog() {
        
        // ダイアログ生成済の場合には再表示する
        if (dialogInstance) {
            dialogInstance.Show();
            return;
        }
        
        // ダイアログが生成されていない場合には、生成、初期設定、表示
        dialogInstance = Instantiate(dialogPrefab, dialogTran, false);
        //dialogInstance.SetUp(() => CloseDialogAction());
        dialogInstance.SetUp(itemData => CloseDialogAction(itemData));  // 引数付の場合
        dialogInstance.Show();
    }

    /// <summary>
    /// ダイアログが非表示になったときに実行されるコールバック
    /// </summary>
    private void CloseDialogAction() {
        Debug.Log("ダイアログが非表示になりました");
    }

    /// <summary>
    /// ダイアログが非表示になったときに実行されるコールバック
    /// 引数付
    /// </summary>
    /// <param name="chooseItemData"></param>
    private void CloseDialogAction(ItemData chooseItemData) {
        if (chooseItemData == null) {
            Debug.Log("選択された情報はありません");
            return;
        }
        Debug.Log($"Dialog 内で選択された情報が届きました : {chooseItemData.id}");
        
        // 上記の処理の参考演算子
        Debug.Log(chooseItemData != null ? $"Dialog 内で選択された情報が届きました : {chooseItemData.id}" : "選択された情報はありません");
    }
}