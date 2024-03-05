# EventTrigger

Unity 事件觸發器 Event 衍伸類，掛載可以直接觸發調用<br>
`EventTrigger` 同時實現以下所有介面

+ `IPointerClickHandler` 滑鼠事件
+ `IPointerDownHandler` 滑鼠按壓
+ `IPointerEnterHandler` 滑鼠進入
+ `IPointerExitHandler` 滑鼠離開
+ `IPointerUpHandler` 滑鼠放開

## 滑鼠相關

以下是需要掛載 Collider

```csharp
//IPointerClickHandler
public override void OnPointerClick(PointerEventData eventData){}
//IPointerDownHandler
public override void OnPointerDown(PointerEventData eventData){}
//IPointerEnterHandler
public override void OnPointerEnter(PointerEventData eventData){}
//IPointerExitHandler
public override void OnPointerExit(PointerEventData eventData){}
//IPointerUpHandler
public override void OnPointerUp(PointerEventData eventData){}
```

`PointerEventData` 包含當下滑鼠的狀態，按鍵，時間等<br>

```C#
//PointerEventData
if (eventData.button == PointerEventData.InputButton.Right) {
    Debug.Log ("Right Mouse Button Clicked on: " + name);
}
//https://docs.unity3d.com/2018.2/Documentation/ScriptReference/EventSystems.PointerEventData.html
```

## EventTrigger

+ OnBeginDrag(PointerEventData data)
+ 
+ OnCancel(BaseEventData data)

+ OnDeselect(BaseEventData data)

+ OnDrag(PointerEventData data)

+ OnDrop(PointerEventData data)
+ 
+ OnEndDrag(PointerEventData data)

+ OnInitializePotentialDrag(PointerEventData data)

+ OnMove(AxisEventData data)

+ OnScroll(PointerEventData data)

+ OnSelect(BaseEventData data)

+ OnSubmit(BaseEventData data)

+ OnUpdateSelected(BaseEventData data)
