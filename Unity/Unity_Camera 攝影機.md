# Unity_Camera 攝影機

## 渲染順序 Rendering Order

不考慮渲染的 深度機制的判斷 (ZTest Always) 場景物件的深度 (ZWrite Off) 即沒有 z-buffering 機制
物件渲染順序如下
1. Camera depth - 數字越大越晚渲染
2. Material type -  不透明物件 (opaque) 再透明物件 (transparent) 根據 material render queue 決定 不透明 < 2500 < 透明
3. Sorting layer - 順位越大越晚渲染 在 Tag Manager 編輯
4. Order in layer - 數字越大越晚渲染
5. Material render queue - 數字越大越晚渲染 2000 不透明物件 (Opaque)、2450半透明物件 (AlphaTest)、3000透明物件 (Transparent) 預設值分別 2000 透明物件會關閉 ZWrite
6. Camera order algorithm - 程式中調整
  - camera.opaqueSortMode - 非透明物件排序演算法
  - camera.transparencySortMode - 透明物件排序演算法 

<https://dev.twsiyuan.com/2018/05/unity-rendering-order.html>

### Canvas

Canvas 依據使用模式決定渲染順序
- `Screen Space - Overlay`
  1. Sort Order - 多個相同模式時 數字越大越晚渲染
- `Screen Space - Camera`
  1. Order in layer - 多個相同模式時 數字越大越晚渲染
- `World Space`
  1. Sorting layer - 順位越大越晚渲染 在 Tag Manager 編輯
  2. Order in layer - 數字越大越晚渲染

UI 在 Canvas 下的順序如下
1. Material render queue (非 Screen Space - Overlay 時) - 依據材質球 (material) 的 render queue 參數
2. Transform order - 依照 Transform 階層關係，採 Pre-order 方式排序

鏡頭不穿牆
<https://www.youtube.com/watch?v=CZ9SzK5jeWo&t=208s>