# Unity 物件建立

紀錄 Unity 物件生成/建立方法

## GameObject

Unity 不能直接 new 以包裝好的物件(Text，Button)<br>
只能從 GameObject 一個個 AddComponent 加入自己所擁有的組件


```C#
static Text CreateText(Transform parent)
{
  var go = new GameObject();
  go.transform.parent = parent;
  var text = go.AddComponent<Text>();
  return text;
}
```


## Prefab

Unity 將物件預製在資源庫，直接參考複製

```C#
//編輯處連結資源庫的 Prefab
public Text text;
//實例化後相當於複製一份
Text t = Instantiate(text, transform, transform);
```