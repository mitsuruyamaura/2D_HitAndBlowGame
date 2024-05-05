using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleClass
{
    public int Value;
}

public class ExampleReferenceTypesScript : MonoBehaviour
{
    // 値型
    int a;  // 初期値は 0
    float b;  // 初期値は 0.0
    bool c;  // 初期値は false

    // 参照型
    SimpleClass p;  // 参照型の初期値は null
    string s;  // stringも参照型で、初期値は null
    
    void Start()
    {
        SimpleClass var1 = new SimpleClass();  // SimpleClass型は参照型です。var1は新たに作られたSimpleClassのインスタンスを参照しています。
        var1.Value = 10;  // var1が参照するインスタンスのValueフィールドに10を設定します。

        SimpleClass var2 = var1;  // var2にvar1の参照をコピーします。この時点では、var1とvar2は同じインスタンスを参照しています。

        var2.Value = 20;  // var2が参照するインスタンスのValueフィールドの値を20に変更します。

        Debug.Log(var1.Value);  // Console ビューに 20 と出力されます 
        Debug.Log(var2.Value);  // Console ビューに 20 と出力されます 
        
        


        Debug.Log(a);  // Console ビューに 0 と出力されます 
        Debug.Log(b);  // Console ビューに 0 と出力されます 
        Debug.Log(c);  // Console ビューに false と出力されます 
        Debug.Log(p);  // Console ビューに null と出力されます 
        Debug.Log(s);  // Console ビューに null と出力されます 
    }
}
