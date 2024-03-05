# Android_View foundation 元件

`foundation` 中包含的元件

## 引用庫

如果使用 `material3` 本身就包含 `foundation` 不需要再引用
```js
dependencies {
    implementation(platform("androidx.compose:compose-bom:2023.03.00"))
    implementation("androidx.compose.foundation:foundation")
    implementation("androidx.compose.foundation:foundation-layout")
}
```

## Layout 排版

基礎排版

+ `LazyColumn` 垂直捲動清單，內部使用 `item` `items` 元件
  + `item` 放置一格元件空間
  + `items` 讀取 List 遍歷元素創建元件空間
+ `Card` 會在元件外圍呈現倒影，使元件像卡片

### Column

`Column` ↓ 直向依序排版
```js
/**
 * modifier - 修飾符
 * verticalArrangement - 子元件的垂直佈置
 * horizontalAlignment - 子元件的水平對齊
 */ 
Column (
    modifier = Modifier,
    verticalArrangement = Arrangement.Center,
    horizontalAlignment = Alignment.CenterHorizontally
){
    MyView1()
    MyView2()
}
```

`Column` 主體 `Layout` 自訂 `columnMeasurePolicy` 成員
```js
@Composable
inline fun Column(
    modifier: Modifier = Modifier,
    verticalArrangement: Arrangement.Vertical = Arrangement.Top,
    horizontalAlignment: Alignment.Horizontal = Alignment.Start,
    content: @Composable ColumnScope.() -> Unit
) {
  val measurePolicy = columnMeasurePolicy(verticalArrangement, horizontalAlignment)
    Layout(
        content = { ColumnScopeInstance.content() },
        measurePolicy = measurePolicy,
        modifier = modifier
    )
}
```

### Row

`Row` → 橫向依序排版
```js
/**
 * modifier - 修飾符
 * horizontalArrangement - 子元件的水平對齊
 * verticalAlignment - 子元件的垂直佈置
 */ 
Row (
    modifier = Modifier,
    horizontalArrangement = Arrangement.Center,
    verticalAlignment = Alignment.CenterVertically){
}{
    MyView1()
    MyView2()
}
```

`Row` 等同 `Layout` 自訂 `rowMeasurePolicy` 成員
```js
@Composable
inline fun Row(
    modifier: Modifier = Modifier,
    horizontalArrangement: Arrangement.Horizontal = Arrangement.Start,
    verticalAlignment: Alignment.Vertical = Alignment.Top,
    content: @Composable RowScope.() -> Unit
) {
    val measurePolicy = rowMeasurePolicy(horizontalArrangement, verticalAlignment)
    Layout(
        content = { RowScopeInstance.content() },
        measurePolicy = measurePolicy,
        modifier = modifier
    )
}
```

### LazyColumn

`LazyColumn` 帶有 `List` 與 `Column` 功能可延伸的布局

```js
/*
modifier - 修飾符。
state - 用於控製或觀察列表狀態的狀態對象?
contentPadding - 整個內容的填充
reverseLayout - 扭轉滾動和佈局的方向
verticalArrangement - 子元件垂直安排
horizontalAlignment - 子元件水平對齊
flingBehavior - 邏輯描述了飛行行為?
userScrollEnabled - 允許用戶滾動
content - 子元件使用 items(List) 添加歷遍項目
*/
LazyColumn(
  modifier = modifier
  ){
    items(affirmationList)
    {affirmation ->
        AffirmationCard(affirmation, modifier)
    }
}
```
`LazyColumn` 使用 `LazyList` 透過 `LazyLayout` 完成

## View 元件
### Image
`Image` 放置一張圖片
```js
/*
painter - 畫布
ContentDescription - 常按內容 可作為偵錯用
modifier - 修飾符 用於調整佈局或繪製內容（背景）
alignment - 對齊
contentScale - 縮放尺寸
alpha - 透明
colorFilter - 渲染?
*/
Image(
    painter = painterResource(id = imageResource),
    contentDescription = result.toString(),
    modifier = Modifier,
    alignment = Alignment.Center,
    contentScale = ContentScale.Fit,
    alpha = 0.0f,
    colorFilter = null
)
```

`Image` 本身透過 `modifier` 內部功能完成
```js
// source
@Composable
fun Image(
    painter: Painter,
    contentDescription: String?,
    modifier: Modifier = Modifier,
    alignment: Alignment = Alignment.Center,
    contentScale: ContentScale = ContentScale.Fit,
    alpha: Float = DefaultAlpha,
    colorFilter: ColorFilter? = null
) {
    val semantics = if (contentDescription != null) {
        Modifier.semantics {
            this.contentDescription = contentDescription
            this.role = Role.Image
        }
    } else {
        Modifier
    }

    Layout(
        {},
        modifier.then(semantics).clipToBounds().paint(
            painter,
            alignment = alignment,
            contentScale = contentScale,
            alpha = alpha,
            colorFilter = colorFilter
        )
    ) { _, constraints ->
        layout(constraints.minWidth, constraints.minHeight) {}
    }
}
```

### Spacer

`Spacer` 元件間配置空的空間
```js
/**
 * modifier - 修飾符 透過內部 height width
 */ 
Spacer(
    modifier = Modifier.height(16.dp)
)
```

`Spacer` 本體 `Layout` 自訂 `MeasurePolicy` 只處理 width height
```js
@Composable
@NonRestartableComposable
fun Spacer(modifier: Modifier) {
    Layout({}, measurePolicy = SpacerMeasurePolicy, modifier = modifier)
}

private object SpacerMeasurePolicy : MeasurePolicy {

    override fun MeasureScope.measure(
        measurables: List<Measurable>,
        constraints: Constraints
    ): MeasureResult {
        return with(constraints) {
            val width = if (hasFixedWidth) maxWidth else 0
            val height = if (hasFixedHeight) maxHeight else 0
            layout(width, height) {}
        }
    }
}

```

## 成員屬性
### shape
`AbsoluteCutCornerShape` - 描述矩形帶有切角的形狀
`AbsoluteRoundedCornerShape` - 描述矩形帶有圓角的形狀
`CornerBasedShape` - 由四個角落定義的形狀的基類
`CutCornerShape` - 描述矩形帶有切角的形狀
`GenericShape` - 通過將提供的構建器應用於路徑上來創建形狀
`RoundedCornerShape` - 描述矩形帶有圓角的形狀

```js
Shapes(
    small = RoundedCornerShape(50.dp),
    medium = RoundedCornerShape(bottomStart = 16.dp, topEnd = 16.dp)
)
```

## 框架布局

已經做好對應的框架位置，只需要放入需要的元素即可使用

+ `AlertDialog` 跳出對話視窗
  + `onDismissRequest{}` 關閉時間聽?
  + `title{}` 標題區塊，通常 `Text`
  + `text{}` 內文區塊，通常 `Text`
  + `dismissButton{}` 取消按鈕區塊，通常 `TextButton`
  + `confirmButton{}` 確定按鈕區塊，通常 `TextButton`
+ `OutlinedTextField` 輸入框 + 敘述字串
  + `onValueChange{}` 數值發生變化時監聽
  +  `label{}` 敘述區塊，通常 `Text`
  +  `keyboardOptions` 設定鍵盤種類，使用 `KeyboardOptions`
  +  `keyboardActions ` 輸入完成監聽，使用 `KeyboardActions`