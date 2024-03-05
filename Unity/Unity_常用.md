# Unity 常用

紀錄常使用的類別

## 材質

+ `MaterialPropertyBlock` - 調用屬性的參數
	```C#
	MaterialPropertyBlock mpb = new MaterialPropertyBlock();
	mpb.SetColor(colorPropertyName, newColor);
	GetComponent<MeshRenderer>().SetPropertyBlock(mpb);
	```
+ `Color` - 顏色，float 型態 rgba
+ `Gradient` - 色彩漸層，提供給材質顏色使用
	+ Evaluate(float time) - 依據參數，回傳 `Color`


## 物理

+ `Physics` - 物理計算方法，包含區塊距離接觸判定
	+ CircleCast(...) - 圓形區塊
	+ SphereCast(...) - 方塊區塊
	+ Raycast(...) - 直線

+ `Rigidbody` - 剛體的物理計算?
	+ MovePosition(Vector3 position) - 計算移動到該點?
	+ MoveRotation(Vector3 position) - 計算旋轉到該點?

+ `LayerMask` - 圖層遮罩結構，主要作為參數傳遞當作遮罩判定

## 計算

+ `Mathf`
	+ Clamp(float value, float min, float max) - 數值如果超出 min/max ，回傳 min/max
	+ MoveTowards(float current, float target, float maxDelta) - 與 Mathf.Lerp 相同，但不會超出 target，maxDelta -值會倒退
	+ Lerp(float a, float b, float t) - 回傳 a 到 b 的線性差值，t 為 [0, 1], 0 = a , 1 = b
	+ Min(float a, float b) - 如果 a < b 回傳 b

+ `Vector3` - 空間座標的儲存結構
	+ Distance(Vector3 a, Vector3 b) - 回傳 A 到 B 的直線距離

+ `Random`
	+ insideUnitCircle - 回傳一個半徑 1 內的 Vector2 點

## UI

+ `Graphic Raycaster` - 檢測是否被擊中?通常與 Canvas 一起

+ `Sprite Mask` - 對 Sprite 進行遮罩效果?

## 除錯

+ 指定運行
```C#
//https://docs.unity3d.com/cn/2018.4/Manual/PlatformDependentCompilation.html
#if UNITY_EDITOR
//只會在編輯模式中運行
#endif
```

## 資源

官方教學
<https://learn.unity.com/>