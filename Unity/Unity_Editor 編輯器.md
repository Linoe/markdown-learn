# Unity Editor 編輯器

Unity 編輯器中出現功能<br>
包括選單，視窗，設定等

https://blog.csdn.net/WenHuiJun_/article/details/108975211

## Inspector Script

`Inspector` 對應選擇的物件顯示特定的內容<br>
紀錄針對 `.cs` 的編輯方法

以下標準寫法
```C#
using UnityEditor;

[CustomEditor(typeof(MyClass))]
public class MyClassEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //基底 OnInspectorGUI()，預設方式增加
        base.DrawDefaultInspector();
        
        //取得編輯的對象
        MyClass myclass = target as MyClass;

        //執行修改
        serializedObject.ApplyModifiedProperties();
    }
}
```

針對特殊形況
```C#
//如果對象是泛型
[CustomEditor(typeof(MyClass<>))]
```