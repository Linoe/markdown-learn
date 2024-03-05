# Unity_Render 名詞

`Render` 渲染 3D 2D 物件計算後輸出到畫面的過程 功能細分成以下
- `Renderer` 渲染器 負責處理渲染的模組 包含數個 `Shader`
- `Shader` 著色器 負責處理渲染過程中圖像著色的模組 例如:依據光源計算圖像明暗效果
- `Post-Processing` 後處理 指渲染完成後再進行二次畫面渲染 例如: 色調 顏色過濾

`Unity` 自己名詞解釋:
- `PBM`(Physically Based Materials) 基於物理材質 指預設材質
- `PBR`(Physically Based Rendering) 基於物理渲染 指預設渲染
- `Materials` 材質 定義 3D 2D 物件渲染資訊 例如:紋理 色調 著色器
- `Shaders` 著色器 定義計算光照與材質 渲染的像素的顏色
- `Textures` 紋理 基本是點陣圖圖像 用於物件的表面的顏色與光照計算用 例如:反射率 粗糙度

## Renderer 渲染器

`Renderer` 現代渲染流程將 3D 物件輸出到畫面的流程 步驟如下
> Vertex Shader → Geometry Shader → Fragment Shader → 最後結果

過程解釋如下:
- `Vertex Shader` 針對每個物件的每個頂點執行一次的著色器
- `Fragment Shader` 針對每個物件佔據畫面的每個像素執行一次的著色器

通常 `Fragment` 計算會多於 `Vertex` (依據製作與計算而定)
