# Android_View material3 元件

`material3` 中包含的元件

https://developer.android.com/reference/kotlin/androidx/compose/material3/package-summary

## 引用庫

`material3` 本身就包含 `foundation` 不需要再引用
```js
dependencies {
    implementation(platform("androidx.compose:compose-bom:2023.03.00"))
    implementation("androidx.compose.material3:material3")
}
```

## Dialog 視窗
### AlertDialog

`AlertDialog` 自帶 標題 文本 確認/取消按鈕 布局的對話框
```js
/*
onDismissRequest - 單擊外部取消對話框時的監聽
confirmButton - 確認的按鈕布局
modifier - 修飾符
dismissButton - 取消的按鈕布局
icon - 圖標布局
title - 標題布局
text - 文本布局
shape - 對話框的容器的形狀
containerColor - 對話框背景的顏色
iconContentColor - 圖標的內容顏色
titleContentColor - 標題的內容顏色
textContentColor - 文本的內容顏色
tonalElevation - 倒影 colorscheme.surface?
properties - 進一步配置對話框?
*/
AlertDialog(
    onDismissRequest = {},
    confirmButton = TextButton{},
    modifier = Modifier,
    dismissButton = TextButton{},
    icon = Icon{},
    title = Text{},
    text = Text{},
    shape = AlertDialogDefaults.shape,
    containerColor = Color,
    iconContentColor = Color,
    titleContentColor = Color,
    textContentColor = Color,
    tonalElevation = 10.dp,
    properties = DialogProperties()
)
```

## Layout 布局
### Scaffold

`Scaffold` 已配置標準 APP 導覽霸布局
```js
/*
modifier - 修飾符
topBar - 屏幕的頂部應用程序欄 通常 SmallTopAppBar
bottomBar - 屏幕的底欄 通常 NavigationBar
snackbarHost - 如果使用 Snackbars 搭配使用的 SnackBarhost
floatingActionButton - 屏幕的主要動作按鈕 通常 FloatingActionButton
floatingActionButtonPosition - FAB 在屏幕上的位置 參考 FabPosition
containerColor - 背景的顏色?
contentColor - 內容的顏色?
contentWindowInsets - 內容的 paddingValues ?
content - 傳入 PaddingValues 通常傳入內容 contentPadding
*/
Scaffold (
  modifier =,
  topBar = {CenterAlignedTopAppBar(...)},
  bottomBar = {},
  snackbarHost = {},
  floatingActionButton = {},
  floatingActionButtonPosition =,
  containerColor =,
  contentColor =,
  contentWindowInsets =
){ sit ->
    Column(
        contentPadding = sit
    ){
      ...
    }
}
```

### TopAppBar

`TopAppBar` 頂部導覽霸
```js
/*
title - 標題布局
modifier - 修飾符
navigationIcon - 導航圖標 通常 IconButton IconToggleButton
actions - 欄末尾顯示的動作 通常 Iconbuttons 佈局 Row
windowInsets - 應用程序欄將尊重的窗口插圖?
colors - 不同狀態下應用程序欄的顏色?
scrollBehavior - Topappbarscrollbehavior具有各種偏移值，該頂部應用程序將應用於設置其高度和顏色?
*/
TopAppBar(
    title = {Text()},
    modifier = modifier,
    navigationIcon = {IconButton()},
    actions = {
      Iconbuttons()
      Iconbuttons()
    },
    windowInsets =,
    colors =,
    scrollBehavior = null
)
```

還有其他預設好功能的元件
- `MediumTopAppBar`
- `LargeTopAppBar`
- `CenterAlignedTopAppBar` 頂部應用程序欄
  - `TopAppBarScrollBehavior` 頂部應用程序欄 選單內容表現方式
- `BottomAppBar`

## Box 容器
### Surface

`Surface` 色塊空間 主用修改背景陰影與邊框
```js
/**  
 * shape - 形狀 例如圓角或銳角
 * color - 背景顏色
 * contentColor - 前景顏色
 * tonalElevation - 陰影效果
 * shadowElevation - 陰影效果
 * border - 邊框粗細
 */
Surface(
    modifier = Modifier.padding(8.dp),
    shape = CircleShape,
    color = Color.Black,
    contentColor = Color.Blue,
    tonalElevation = 2.dp,
    shadowElevation = 2.dp,
    border = BorderStroke(2.dp, Color.Red)
) {
    MyContent()
}
```

`Surface` 本身作為以下功能使用
- `Surface` - 一般容器
- `ClickableSurface` - Button 元件
  - onClick: () -> Unit,
  - enabled: Boolean = true,
- `SelectableSurface` - SelectButton 元件 
  - selected: Boolean,
  - onClick: () -> Unit,
  - enabled: Boolean = true,
- `ToggleableSurface` - ToggleButton 元件
  - checked: Boolean,
  - onCheckedChange: (Boolean) -> Unit,
  - enabled: Boolean = true,

`Surface` 主體 `CompositionLocalProvider` `Box` 組合的元件
`Box` 中得知拆解 modifier.surface() 功能使用
```js
// source
@Composable
@NonRestartableComposable
Surface(...){
    val absoluteElevation = LocalAbsoluteTonalElevation.current + tonalElevation
    CompositionLocalProvider(
        LocalContentColor provides contentColor,
        LocalAbsoluteTonalElevation provides absoluteElevation
    ) {
        Box(
            modifier = modifier
                .surface(
                    shape = shape,
                    backgroundColor = surfaceColorAtElevation(
                        color = color,
                        elevation = absoluteElevation
                    ),
                    border = border,
                    shadowElevation = shadowElevation
                )
                .semantics(mergeDescendants = false) {}
                .pointerInput(Unit) {},
            propagateMinConstraints = true
        ) {
            content()
        }
    }
}
```

### Card

`Card` 帶有陰影的容器
```js
/*
modifier - 修飾符
shape - 容器的形狀 邊框（邊框不是空）和陰影（使用高程時）
colors - 不同狀態的顏色
elevation - 不同狀態下的高程 控制陰影的大小
border - 容器的邊界
*/
Card(
  modifier = modifier,
  border = BorderStroke(4.dp, Color.Black)
  ) {
    Image(...)
    Text(...)
}
```

`Card` 本體 `Surface` 與 `Column` 組合
```js
@Composable
fun Card(
    modifier: Modifier = Modifier,
    shape: Shape = CardDefaults.shape,
    colors: CardColors = CardDefaults.cardColors(),
    elevation: CardElevation = CardDefaults.cardElevation(),
    border: BorderStroke? = null,
    content: @Composable ColumnScope.() -> Unit
) {
    Surface(
        modifier = modifier,
        shape = shape,
        color = colors.containerColor(enabled = true).value,
        contentColor = colors.contentColor(enabled = true).value,
        tonalElevation = elevation.tonalElevation(enabled = true, interactionSource = null).value,
        shadowElevation = elevation.shadowElevation(enabled = true, interactionSource = null).value,
        border = border,
    ) {
        Column(content = content)
    }
}
```


## View 元件
### Button

`Button` 可以建立點擊後有回饋的按鈕
```js
/**
 * onClick - 單擊按鈕時監聽
 * modifier - 修飾符
 * enabled - 啟用狀態 false時不回應用戶輸入 不會有視覺上回饋
 * shape - 按鈕容器的形狀 影響邊框（邊框不為空）和陰影（使用高程時）
 * colors - 按鈕在不同狀態下顏色
 * elevation - 按鈕在不同狀態下的高程
 * border - 容器的邊界粗細
 * contentPadding - 容器與子物件的間距
 * interactionSource - 自訂用以觀察不同狀態下的實例?
 */
Button(
    onClick = { result = (1..6).random() },
    modifier = Modifier,
    enabled = true,
    shape = ButtonDefaults.shape,
    colors = ButtonDefaults.buttonColors(Color.Rad),
    elevation = ButtonDefaults.buttonElevation(),
    border = null,
    contentPadding = ButtonDefaults.ContentPadding,
    interactionSource = remember { MutableInteractionSource() },
) {
    Text("roll")
}
```

### RadioButton

`RadioButton` 帶有開關的按鈕
```js
/*
selected - 是否選擇此按鈕
onClick - 點擊監聽
modifier - 修飾符
enabled - 啟用狀態
colors - 不同狀態下的顏色
interactionSource - 不同狀態下此廣播按鈕的外觀/行為?
*/

RadioButton(
  selected = true,
  onClick = {},
  modifier = Modifier
)
```

### IconButton

`IconButton` 配置圖片布局的按鈕
```js
/*
onClick - 單擊時監聽
modifier - 修飾符
enabled - 啟用狀態
colors - 不同狀態下此圖標按鈕的顏色
interactionSource - 以觀察交互並自定義不同狀態下此圖標按鈕的外觀 /行為
*/
IconButton(
    onClick = {...},
    modifier = Modifier
) {
    Icon(...)
}
```

### Text

`Text` 放置一段文字
```js
/*
text - 文字
modifier - 修飾符
color - 顏色的文本
fontSize - 字形大小
fontStyle - 字體變體
fontWeight - 字體厚度
fontFamily - 字體系列
letterSpacing - 每個字母之間空間
textDecoration - 常按內容 可作為偵錯用
textAlign - 段落對齊
lineHeight - 段落高度
overflow - 視覺溢出應如何處理?
softWrap - 文本是否應在軟線路中斷?
maxLines - 文本最大行數 超出捨棄
onTextLayout - 計算新的文本佈局時執行的回調?
style - 文本的樣式配置 例如顏色 字體 線高 
*/
Text(
  text = stringResource(id = R.string.roll)
  modifier = Modifier,
  color = Color.Red,
  maxLines = 3,
)
```

### TextField

`TextField` 文字輸入框
```js
/*
value -文本
onValueChange - 更新文本監聽
modifier - 修飾符
enabled - 啟用 false時用戶禁用
readOnly - 文本可編輯
textStyle - 文本的樣式
label - 未輸入時 顯示內容元件
placeholder - 文本為空時 顯示的可選佔位符?
leadingIcon - 文本字段左側 圖標內容元件
trailingIcon - 文本字段左側 圖標內容元件
supportingText - 支援文本 要顯示在文本字段下方
isError - 文本是否存在錯誤。 為true則將以錯誤顏色
visualTransformation - 轉換輸入值的視覺表示形式?
keyboardOptions - 軟件鍵盤選項
keyboardActions - 當輸入服務發出IME操作時
singleLine - 單行模式 啟用時 MaxLines 將被忽略
maxLines - 最大行數
interactionSource - 可以創建和傳遞自己的記憶實例 以觀察相互作用並自定義不同狀態中此文本字段的行為?
shape - 文本字段的容器的形狀
colors - 文本字段在不同狀態下使用的顏色
*/
TextField(
  value = stringResource(myString),
  onValueChange = {},
  label = {Text(stringResource(label))},
  leadingIcon = { Icon(painter = painterResource(id = leadingIcon), null) },
  keyboardOptions = KeyboardOptions.Default,
  keyboardActions = KeyboardActions.Default
  singleLine = true,
  modifier = modifier
)
```

### OutlinedTextField

`OutlinedTextField` 帶有邊框的文字輸入框
```js
/*
value - 顯示的輸入文本
onValueChange - 輸入文本時觸發的監聽
*/
OutlinedTextField(
    value = "String",
    onValueChange = { it -> }
)
```

### Switch

`Switch` 開關按鈕
```js
/*
checked - 是否開關
onCheckedChange - 開關時監聽
modifier - 修飾符
thumbContent - 將在拇指內繪製的內容?
enabled - 啟用狀態
colors - 開關的顏色
interactionSource - 定義不同狀態下此開關的外觀?
*/
Switch(
  checked = roundUp,
  onCheckedChange = onRoundUpChanged,
  modifier = Modifier.fillMaxWidth().wrapContentWidth(Alignment.End)
)
```

### LinearProgressIndicator

`LinearProgressIndicator` 橫進度條
```js
/*
progress - 進度 範圍[0-1]
modifier - 修飾符
color - 進度的顏色
trackColor - 軌道的顏色
*/
LinearProgressIndicator(
    progress = progressFactor,
    modifier = Modifier
        .fillMaxWidth()
        .height(16.dp)
        .clip(RoundedCornerShape(4.dp))
)
```
