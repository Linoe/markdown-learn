# ex_Unity_Camera Rendering Path

`Camera` 中渲染相關 `Rendering Path` 選項 以下兩個

- `Forward`  前方渲染 使用於單一燈光或簡單面數物件 效率不好但硬體要求不高 如:白天平行光
- `Deferred` 延遲渲染 使用於複數燈光或複雜面數物件 效率好但硬體要求高 如:夜晚範圍燈光

<https://www.klab.com/jp/blog/tech/2021/unitydeferredrendering.html>

## Forward Shading 前方渲染

`Forward Shading` 畫面上所有物件渲染一遍

由於會將場上所有物件循環計算 效率等同 `Fragment` 與 `光源` 成正比
> 物件 → Vertex → Geometry → Fragment ┐<br>
> 物件 → Vertex → Geometry → Fragment ┼ → 最終結果<br>
> 物件 → Vertex → Geometry → Fragment ┘<br>
> (花費時間: `O(總面數 X 光線數)`)

改善效率可以透過下面
- 透過深度判斷前方模組 降低繪製的面數
- 預先計算靜態的光線或減少光源 降低光線計算需求
- 光線僅計算較少面數
- 調整的 Shader 計算(`URP` 可以使用 Forward+)
- 調整模組與光源位置 減少光線計算

## Deferred Shading 延遲渲染

`Deferred Shading` 延遲的光線的計算 最後在畫面合成

> 物件 → Vertex → Geometry → Fragment（不含光線）┬ 深度圖 ┐<br>
> 物件 → Vertex → Geometry → Fragment（不含光線）┼ 法線圖 ┼ 光線計算 → 最終結果<br>
> 物件 → Vertex → Geometry → Fragment（不含光線）┼ 顏色圖 ┘<br>
> (花費時間: `O(畫面解析度 X 光線數)`)

由於需要儲存面數資訊與光線計算後畫面 GPU 需要較高記憶體容量與頻寬需求 低規格不建議使用 


## 比較

`Forward Shading` 適合用於低規格靜態光線 缺點如下:
- 每個物件都是分開自己計算的
- 即使被遮住 仍然會計算光線
- 複數光線 每個都獨立計算一次

`Deferred Shading` 適合用於複數動態光線 缺點如下:
- 需要儲存資料用於最後計算 記憶體與頻寬需求較高
- 無法使用 MSAA (反鋸齒技術?)
- 對於影子的效率沒有影響