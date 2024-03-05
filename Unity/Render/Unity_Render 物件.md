# Unity_Render 物件

Unity 渲染相關的物件 記錄在此

## 3D 光源種類

`環境光` 整體亮度
`間接光` 反射亮度的二次亮度
`光源` 特定方向光照
`著色器材質` 用於計算光源在物件的呈像

## Unity 燈光物件

`定向光` 整體直線光源 例:太陽光
`燈光` 特定範圍光源 例:燈泡
`聚光燈` 直線範圍光照 例:手電筒
`烘焙` ?

## Unity Material 材質資源

`Material` 資源內建立 同時必須選擇一個 `Shader` 決定圖像呈現方式
`Shader` 改變時 其關聯的所有 `Material` 都會改變
`Mesh Renderer` 設定 3D 物件使用的 `Material`

## Shader Material 關係

`Shader` 物件光照的圖像呈現的方式
`Material` 管理 `Shader` 參數與 `Textures` 資源連接

## 3D Textures 紋理貼圖

`Textures` 依據法線貼合 3D 物件上 透過 `Material` 與 `Shader` 決定圖像呈現

- 紋理貼圖 物件的基礎圖像
- 漫反射貼圖
- 高光貼圖
- 金屬貼圖
- 平滑貼圖
- AO貼圖
- 法線貼圖
- 凹凸貼圖 光照環境下 低面數呈現凹凸
- 高度貼圖

## Unity Skybox

`Skybox` 無限遠方背景圖像

## Unity Frame Debugger

`Frame Debugger` 用來檢查每一禎使用的記憶體
> Windows > Frame Debugger