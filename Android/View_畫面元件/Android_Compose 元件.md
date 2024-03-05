# Android_Compose 元件

Compose 目前 Android 推薦建立方式
Activity 為基底透過 Kotlin 程式碼直接撰寫畫面構成

## 引用庫

| 群組                 | 說明                                                                                                                                |
| :------------------- | :---------------------------------------------------------------------------------------------------------------------------------- |
| `compose.animation`  | 在 Jetpack Compose 應用程式中建構動畫                                                                                               |
| `compose.compiler`   | @Composable 函式                                                                                                                    |
| `compose.foundation` | 基礎構成元素編寫 Jetpack Compose 應用程式。建構出自己的設計系統元件。                                                               |
| `compose.material`   | Material Design 元件建構 Jetpack Compose UI。這是更高層級的 Compose 進入點，用意是確保提供的元件與 www.material.io 上所述元件相符。 |
| `compose.material3`  | Material Design 3 元件建構 Jetpack Compose UI。Material 3 具有新的主題設定和元件                                                    |
| `compose.runtime`    | Compose 程式設計模型和狀態管理的基本類別成員，以及 Compose Compiler 外掛程式指定的核心執行階段。                                    |
| `compose.ui`         | 裝置互動的 Compose UI 屬性成員類別，包括版面配置 繪圖 輸入                                                                              |

```js
dependencies {
    val compose_version = "1.5.0"
    // Crossfade Animatable fadeIn slideOut
    implementation("androidx.compose.animation:animation-core:$compose_version")
    implementation("androidx.compose.animation:animation:$compose_version")
    // Layout Canvas Image Lazy Shape Text
    implementation("androidx.compose.foundation:foundation:$compose_version")
    implementation("androidx.compose.foundation:foundation-layout:$compose_version")
    // AlertDialog Button Card Checkbox Slider Surface Text TextField TopAppBar
    implementation("androidx.compose.material:material:$compose_version")
    implementation("androidx.compose.material:material-icons-core:$compose_version")
    implementation("androidx.compose.material:material-icons-extended:$compose_version")
    // State、remember、mutableStateOf
    implementation("androidx.compose.runtime:runtime-livedata:$compose_version")
    implementation("androidx.compose.runtime:runtime-rxjava2:$compose_version")

    implementation("androidx.compose.ui:ui:$compose_version")
    implementation("androidx.compose.ui:ui-geometry:$compose_version")
    implementation("androidx.compose.ui:ui-graphics:$compose_version")
    implementation("androidx.compose.ui:ui-text:$compose_version")
    implementation("androidx.compose.ui:ui-util:$compose_version")
    implementation("androidx.compose.ui:ui-viewbinding:$compose_version")
    implementation("androidx.compose.ui:ui-tooling:$compose_version")
    // BackHandler
    implementation("androidx.activity:activity-compose:1.3.1")

    //Compose Constraintlayout
    implementation("androidx.constraintlayout:constraintlayout-compose:1.0.0")
}
```


## 畫面元件

字串按鈕圖形...等，畫面使用的元素

+ `IconButton` 圖示按鈕
+ `OutlinedButton` 邊框按鈕

## 風格屬性

布局中排版細部屬性

+ `Modifier` 元件自身布局方式
  + `padding()` 間隔，詳細設定屬性 `start` `top` `end` `bottom`
  + `fillMaxWidth()` 如果外層有寬度則填充到最大
  + `background` 背景，圖片或顏色
  + `align()` 對齊，使用 `Alignment`
  + `clickable{}` 點擊監聽
  + `wrapContentWidth` 如果寬度大於元件則擺放位置，使用 `Alignment`
  + `aspectRatio` 比例分配空間?
  + `clip` 元件縮放方式
  + `animateContentSize` 大小變化時的動畫效果
    + `animationSpec` 自動動畫，內容可[參考](https://developer.android.com/jetpack/compose/animation?hl=zh-tw#animationspec) 
+ `style` 風格，元件有各自 `Style` 以下預設風格
  + `MaterialTheme.typography.displaySmall`


## Material3 材質

一種實驗中材質包，可能隨版本更新變動，使用以下修飾
> `@OptIn(ExperimentalMaterial3Api::class)` 方法前修飾
> `@file:OptIn(ExperimentalMaterial3Api::class)` 檔案修飾第一行
> `@file:Suppress("UnstableApiUsage")` build.gradle.kts 修飾第一行

- `Scaffold` 布局框架
  - `topBar` 至頂應用欄空間
  - `bottomBar` 底部應用欄空間
  - `floatingActionButton` 螢幕右下角的浮懸按鈕
- `CenterAlignedTopAppBar` 居中對齊頂部應用欄


## Shape 邊框形狀
```js
shape = shapes.large
```

## Color 顏色

## Colors 狀態顏色

## Elevation 倒影

## Modifier 修飾
```js
Modifier
  .fillMaxWidth()
  .wrapContentHeight()
  .padding(mediumPadding)
  .padding(horizontal = 10.dp, vertical = 4.dp)
  .verticalScroll(rememberScrollState())
  .clip(shapes.medium)
  .background(colorScheme.surfaceTint)
  .align(alignment = Alignment.End),

  .background(Color.Red)
  .animateContentSize()
  .height(400.dp)
  .clickable(
      interactionSource = remember { MutableInteractionSource() },
      indication = null
  ) {
      expanded = !expanded
  }
```

## style 風格

## KeyboardOptions 虛擬鍵盤
```js
keyboardOptions = KeyboardOptions.Default.copy(
    imeAction = ImeAction.Done
)
```
## KeyboardActions 虛擬鍵盤監聽
```js
keyboardActions = KeyboardActions(
    onDone = { onKeyboardDone() }
)
```
## Arrangement 排列
```js
verticalArrangement = Arrangement.spacedBy(mediumPadding),
horizontalAlignment = Alignment.CenterHorizontally,
```
## Alignment 對齊
```js
verticalArrangement = Arrangement.spacedBy(mediumPadding),
horizontalAlignment = Alignment.CenterHorizontally,
```
## 應用

Modifier 作為最外層參數溝通，不使用在內部
```js
MyView(modifier = Modifier.padding(4dp))

fun MyView(
  modifier : Modifier = Modifier
){
  Column(
    modifier = modifier // 使用外部參數
  ){
    Row(modifier = Modifier.padding(4dp)) // 內部自訂
  }
}
```

按鈕控制開關顯示
```js
var expanded by remember { mutableStateOf(false) }
Column{
  Button(
    expanded = expanded,
    onClick = { expanded = !expanded },
  )
  if (expanded) {/*元件*/}
}
```

圖示控制
```js
var expanded by remember { mutableStateOf(false) }
Icon(
  imageVector = if (expanded) Icons.Filled.ExpandLess else Icons.Filled.ExpandMore,
)
```

modifier 參數必須要傳入最外層，內層可以不用
```js
@Composable
fun MyTopAppBar(modifier: Modifier = Modifier) {
    CenterAlignedTopAppBar(
        title = {
            Row(
                verticalAlignment = Alignment.CenterVertically
            ) {
                Image(
                    modifier = Modifier
                        .size(dimensionResource(R.dimen.image_size))
                        .padding(dimensionResource(R.dimen.padding_small)),
                    painter = painterResource(R.drawable.ic_woof_logo),
                    contentDescription = null
                )
            }
        },
        modifier = modifier
    )
}
```

依序建立元件
```js
enum class MyColors(val color: Color) {
    Red(Color.Red), Green(Color.Green), Blue(Color.Blue)
}
Row {
    MyColors.values().forEach { myColors ->
        Button(
            onClick = { currentColor = myColors },
            Modifier.weight(1f, true)
                .height(48.dp)
                .background(myColors.color),
            colors = ButtonDefaults.buttonColors(myColors.color)
        ) {
            Text(myColors.name)
        }
    }
}
```