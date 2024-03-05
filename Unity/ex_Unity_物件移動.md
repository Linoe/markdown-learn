# Unity 物件移動

紀錄 Unity 物件移動方法

## UI GameObject

UI 物件移動透過 `RectTransform` 進行修改

RectTransform 取得
> GetComponent<RectTransform>();
> text.rectTransform;

RectTransform.height 計算
> (anchorMax.y – anchorMin.y) * Parent.rect.height + sizeDelta.y

+ `anchorMin`	錨點左下點 X Y (相對於父物件)
+ `anchorMax`	錨點右上點 X Y (相對於父物件)
+ `offsetMin`	當錨點非點時，長寬的左下點 X Y (Bottom Left)(相對於 anchorMin)
+ `offsetMax`	當錨點非點時，長寬的右上點 X Y (Right Top)(相對於 anchorMax)

+ `anchoredPosition` 錨點是點時，錨點的 posX posY posZ
+ `sizeDelta` 錨點是點時，相當於 width height (計算方式參考上面)
+ `pivot` 旋轉縮放等基準點 X Y (相對於父物件)

+ `position` 以 Canvas 的左下座標為基準?
+ `localPosition`	以父物件的中心座標為基準?

部分屬性必須依照方式修改，否則不會有變化
```C#
// anchoredPosition 更改方式
myRectTransform.anchoredPosition = new Vector2(100,100);

```
