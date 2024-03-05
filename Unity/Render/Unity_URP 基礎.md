# Unity_URP 基礎
`URP` (Universal Render Pipeline) 通用渲染管道 專注於性能與跨各種平台 此紀錄使用方式

## URP 安裝方式

以下安裝方式
- 建立專案時選擇 `URP 模板`
- 現有專案中安裝 `Universal RP`
  > Window > Package Manager > 安裝 `Universal RP`

## URP 專案套用渲染

當前專案中套用 `URP` 渲染 步驟如下
1. 資源中創建 URP 渲染 (產生 渲染器 設定檔 兩個檔案)
   > Asset > Create > Rendering > URP > 選擇 `Pipeline Asset (Forward Renderer)`
2. 專案更改顏色空間
   > Edit > Project Settings > Other Settings > 選擇 `Linear`
3. 專案套用 `URP` 渲染
   > Edit > Project Settings > Graphics > 選擇 `MyURPAsset`

## URP` 材質建立

`Unity` 創建材質方法一樣 步驟如下
1. 創建材質資源
   > Asset > Create > 選擇 `Material`
2. 材質資訊中 `Shader` 選擇 `URP` 相關

`Unity` 提供原生材質 變更改為 `URP` 材質 步驟如下
1. Edit 選項中選擇更改的材質(出現確認選項)
   > Edit > Rendering Pipeline > Universal Render Pipeline > 選擇以下
   > - `Upgrade Project Materials to UniversalRP Materials` 所有專案材質
   > - `Upgrade Selected Materials to UniversalRP Materials` 當前選擇材質

## 進階
### URP 渲染資源設置

`Asset` 建立的 `URP` 資源 相關功能如下
- `General` 紋理與渲染器資源
  - `Renderer list` 連結渲染器
  - `Depth Texture` 深度紋理 相機自定義著色器輸出?
  - `Opaque Texture` 不透明紋理 相機自定義著色器輸出?
  - `Terrain Holes` 紋理凹凸? 渲染凹凸的程度?
- `Quality` 光照與 3D 邊緣
  - `HDR`  允許使用各種照明 後處理效果（例如 Bloom） 相對成本越高
  - `Anti Aliasing` 抗鋸齒 3D 物件平滑邊緣
  - `Render Scale` 渲染比例 縮放目標解析度 越高畫質越好 成本越高
- `Lighting` 光照 主光與子光對於陰影設定
  - `Main Light` 主光 如:陽光 (`Per Pixel`像素/`Disabled`停用)
    - `Cast Shadows` 投射陰影 如果已經烘焙了照明且不需要陰影 請關閉此選項
    - `Shadow Resolution` 陰影解析度 越高陰影畫質越好 成本越高
  - `Additional Lights` 子光 如:燈光
    - `Per Object Limit` 每個物件限制 允許有多少子光運行
    - `Cast Shadows` 投射陰影 是否渲染子光陰影
    - `Shadow Resolution` 陰影解析度 越高陰影畫質越好 成本越高
- `Shadows` 陰影 相機渲染陰影設定
  - `Distance` 陰影距離 相機渲染物件陰影的距離 超過將不會渲染
  - `Cascades` 陰影階級 選擇渲染的級數 級數越高 成本越高
  - `Depth Bias`/`Normal Bias` 深度偏差/正常偏差 物件上的條帶或陰影偽影?
  - `Soft Shadows` 柔邊陰影 啟用渲染造成成本
- `Post-processing` 後處理 指渲染後對畫面像素處理
  - `Feature Set` 功能集 `Integrated` `Post Processing V2` (V2 將被棄用) ?
  - `Grading Mode` 分級模式 `High Dynamic Range` `Low Dynamic Range` 指顏色範圍 越高越鮮豔
  - `LUT Size `LUT 大小 查找的紋理的大小 越高越精確 但會增加記憶體
- `Advanced` 高級選項 分析或其他
  - `SRP Batcher`  SRP 批次 批次共用相同著色器的不同材質 將加速 CPU 渲染且不影響 GPU 效能?
  - `Dynamic Batching` 動態批次 批次使用相同材質的小型動態物件? 如果目標平台不支援 GPU 實例 則需啟用
  - `Mixed Lighting` 混合照明 動態光影與烘焙照明結合 如果燈光物件的 `Mode` 設定 `Mixed` 則需啟用
  - `Debug Level` 偵錯等級 渲染管道產生的偵錯資訊 可以在 FrameDebugger 查看
    - `Disabled` 停用
    - `Profiling` 分析
  - `Shader Variant Log` 產生 Shader Variants 和 Shader Stripping 的資訊
    - `Only Universal` 將記錄所有 URP 著色器的信息
    - `All` 將記錄構建中所有著色器的信息。 

### URP 預設著色器

`Unity` 預設用於 `URP` 的 `Shader` 相關功能如下
- `2D` 2D 專案專用著色器。 
- `Autodesk Interactive` 通用於 Autodesk 即時更新的著色器
- `Nature` 快速生成自然風景的樹
- `Particles` 粒子著色器
- `Terrain` 地形著色器
- `Baked Lit` 烘焙光照 用於僅需要透過光照貼圖和光探針進行烘焙光照 不使用基於物理著色 沒有即時光照 相對效率高
- `Lit` 光照 用於渲染真實物理材質 (如: 石頭 木材 玻璃 塑膠 金屬) 光線具有水平和反射正確反應 (如: 白天 晚上） 相對效率低
- `Simple Lit` 簡易光照 相當於輕量 `Lit` 相對畫面較不真實
- `Unlit` 不發光著色器 可以選擇對全域照明進行取樣  用於取代 Unity 的庫存未光照著色器?