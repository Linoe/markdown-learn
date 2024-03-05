# Unity_Render pipeline 渲染管道

Unity 專門為渲染製作一套步驟 順序如下:
1. `Culling` 僅渲染需要的對象 (例如:相機的可見對象)
2. `Rendering` ：光源投射物件屬性儲存到畫面的緩衝區中
3. `Post-processing` 在緩衝區上處理特效 (例如:色彩分級 光暈 景深) 產生結果輸出到畫面影格

Unity 渲染管道種類有以下:
- `BRP` (Built-in Render Pipeline) 內建渲染管道 固定的自訂功能
- `SRP` (Scriptable Render Pipeline) 可編程渲染管線 建立自己自訂渲染管道 Unity 有提供里以下預制
  - `URP` (Universal Render Pipeline) 通用渲染管道 專注於性能與跨各種平台
  - `HDRP` (High Definition Render Pipeline) 高清渲染管道 專注於高端圖形渲染 注意 HDRP 不相容於 URP
> `LWRP` (Lightweight Render Pipeline) 輕量級渲染管線 `URP` 的早期版本 目前已被取代

Unity 選擇渲染管線
<https://docs.unity3d.com/Manual/choose-a-render-pipeline.html>

## 設置渲染管道資源

Unity 編輯平台選項中設置

`BRP` 設定方式 刪除所有渲染管道資源
1. 刪除 Quality
   > Select Edit > Project Settings > Quality.
2. 刪除 Graphics
   > Select Edit > Project Settings > Graphics.

`URP` `HDRP` `SRP` 設定方式 使用對應渲染管道資源
1. 設置 Quality
   > Select Edit > Project Settings > Quality.
2. (可選)設置 Graphics
   > Select Edit > Project Settings > Graphics.

可以透過程式動態修改渲染管道資源 [參考](https://docs.unity3d.com/2022.3/Documentation/Manual/srp-setting-render-pipeline-asset.html)

## BRP 內建渲染管道

Unity 內建渲染管道是舊的渲染管道 因此不能用於編程

- `Graphics tiers` 圖形層等級
- `Rendering paths in the Built-in Render Pipeline` 渲染路徑
  - `Forward rendering path` 前方渲染路徑
  - `Deferred Shading rendering path` 遞延陰影渲染路徑
  - `Vertex Lit Rendering Path` 頂點發光渲染路徑
- `Rendering order in the Built-in Render Pipeline` 渲染順序
- `Extending the Built-in Render Pipeline with CommandBuffers` 命令擴展
- `Hardware requirements for the Built-in Render Pipeline` 硬件要求
- `Example shaders for the Built-in Render Pipeline` 示例著色器
  - `Custom shader fundamentals` 自定義著色器基礎
  - `Visualizing vertex data` 可視化頂點數據

<https://docs.unity3d.com/2022.3/Documentation/Manual/built-in-render-pipeline.html>

## URP 通用渲染管道

`URP` Universal Render Pipleline 通用渲染管道 特色如下:
- 性能較高 適用於低規格
- 適合 2D 遊戲 具有光照和陰影

<https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@16.0/manual/index.html>

## HDRP 高清渲染管道

`HDRP` High Definition Render Pipleline 高清管線流程 特色如下:
- 渲染高真實度的圖形和建築
- 提供高級光照功能
- 具有延遲渲染(`URP` 預計增加)

`HDRP` 光照特色的效果
- AO（環境光遮蔽）
- 自動曝光（模擬人眼適應不同光線條件的能力）
- 屏幕空間反射（模擬基於屏幕上可見物體的反射）

由於更多 shader 編輯功能 相對需要更大量時間與龐大的團隊制作

## URP HDRP 比較

|       | 效能  | 跨平台 | 畫質  | 硬體需求 | SRP 需求 |
| :---: | :---: | :----: | :---: | :------: | :------: |
| HDRP  |   L   |   L    |   H   |    H     |    H     |
|  URP  |   H   |   H    |   L   |    L     |    L     |

