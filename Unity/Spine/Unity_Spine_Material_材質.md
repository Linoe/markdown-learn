# Unity Spine Material

紀錄 Unity 中 Spine 預設材質可以調整的參數

## 常用 Shader

+ `Spine/Outline/OutlineOnly-ZWrite` - 會依據 SkeletonAnimation/Advanced/Z Spacing 設定， Z 軸分離圖層製造外圍邊框
+ `Spine/Special/Skeleton Grayscale` - 將所有顏色轉為灰階
+ `Spine/Sprite/Vertex Lit` - 網格受到光照顏色影響?

## 常見參數

+ `Stencil`
	+ Stencil Comparison - 默認 Always
	+ Stencil Reference
+ `Outline` - 啟用後，帶有外圍邊框
	+ Outline Width - 粗細，xp
	+ Outline Color - 顏色