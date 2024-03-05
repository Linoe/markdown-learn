# MonoBehaviou

Unity 直接掛載物件上的基類

## 方法

+ OnValidate() - 腳本被掛載時與 Inspector 中的任何值被修改時會被呼叫
+ OnMouseDown() - 當有掛載  Collider 或 GUIElement 時，點擊後觸發
+ OnMouseDrag() - 當有掛載  Collider 或 GUIElement 時，點擊中拖曳後觸發
+ OnMouseUp() - 當有掛載  Collider 或 GUIElement 時，點擊放開後觸發


+ `gameObject` - 遊戲物件，用來搜尋或設置基礎
	+ hideFlags - 設置是否可被看見， `HideFlags` 進行設置

+ `transform` - 儲存物件的空間座標
	+ TransformPoint(Vector3 position) - 取得物件的面向，參數使用 `Vector3`.right

+ GetComponent()
+ GetComponentsInChildren()