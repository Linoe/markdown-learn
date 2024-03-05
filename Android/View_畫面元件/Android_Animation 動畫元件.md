# Android_Animation 動畫元件

## 類別

資源存取
`AnimatedVectorDrawable` 向量圖片資源

使用在組合元件
`AnimatedVisibility` 動畫出現/消失 animateFloatAsState 與 Modifier.alpha()
`AnimatedContent`
`Crossfade`
`Pager`
`Modifier.animateContentSize()` 修飾符 動畫尺寸
`TextMotion.Animated` 文本

單一參數
`by animateFloatAsState`
`by animateColorAsState`
`by animateIntOffsetAsState`
`by animateDpAsState`

持續單一參數
`by infiniteTransition.animateFloat`
`by infiniteTransition.animateColor`

低階 API
`AnimationState` 動畫狀態
`Animatable` 動畫

導覽進出動畫
```js
NavHost(){
  composable(
    enterTransition 
    exitTransition 
  )
}
```

## 基礎

`AnimatedVisibility` 預設淡進淡出動畫元件 令元件長寬動畫補值縮減達到進出效果
  `visible` 設定顯示 true 顯現 false 消失
```js
// 用於外部控制動畫
var visible by remember{ mutableStateOf(true) }
// AnimatedVisibility 不是排版元件 仍需要靠其他元件排版
Column {
    ... // 用於控制 visible 元件 
    AnimatedVisibility(visible = visible) {
        /* 動畫元件 */
    }
}
```

完整範例
```js
var visible by remember{ mutableStateOf(true) }
Column {
    Button(onClick = { visible = !visible }) {
        Text(text = if (visible) "true" else "false")
    }
    AnimatedVisibility(visible = visible) {
        Text(
            text = "Animate",
            modifier = Modifier.background(Color.Red))
    }
}
```

### AnimatedVisibility

`AnimatedVisibility` 內部畫面元件的進出的動畫
```js
/*
visible - 內容可見 控制動畫
modifier - 修飾符
enter - 出現動畫
exit - 消失動畫
content - @Composable AnimatedVisibilityScope.() -> Unit
*/
var v by remember{mutableStateOf(true)}
AnimatedVisibility(
  visible = v,
  modifier = Modifier,
  enter = fadeIn() + expandVertically(),
  exit = fadeOut() + shrinkVertically(),
  label = "AnimatedVisibility"
){ Text(...) }
```

`AnimatedVisibility` 自訂動畫
  `enter` 控制進入動畫
  `exit` 控制退出動畫
```js
val density = LocalDensity.current // 計算手機畫面用
AnimatedVisibility(
    visible = true,
    enter = slideInVertically {
        with(density) { -40.dp.roundToPx() } // 內部回傳值 此處為起始座標
    },
    exit = slideOutVertically() + fadeOut() // 使用 + 連結
) {
    Text(...)
}
```

`AnimatedVisibility` 使用狀態值控制動畫
`MutableTransitionState` 控制當前與目標的值
  `currentState` 當前值
  `targetState` 目標值
  `isIdle` 是否等待 true 沒有動作 false 正在動畫
```js
val visibleState = remember { MutableTransitionState(false) }
// .apply { targetState = true } // 設置為 此設置進入後開始動畫 通常一次性使用

Column {
    Button(onClick = { visibleState.targetState = !visibleState.currentState }) { // 修改 targetState 做為控制動畫
        Text( // 用於顯示當前動畫狀態
            text = when {
                visibleState.isIdle && visibleState.currentState -> "Visible"
                !visibleState.isIdle && visibleState.currentState -> "Disappearing"
                visibleState.isIdle && !visibleState.currentState -> "Invisible"
                else -> "Appearing"
            }
        )
    }
    AnimatedVisibility(
        visibleState = visibleState
    ) {
        Text(
            text = "Animate",
            modifier = Modifier.background(Color.Red))
    }
}
```

`Modifier.animateEnterExit()` 內部元件動畫 與外部 `AnimatedVisibility` 連動
```js
AnimatedVisibility(
    visible = vis,
    enter = fadeIn(),
    exit = fadeOut()
) {
    Box(
        Modifier
            .animateEnterExit(
                // 與 AnimatedVisibility 連動效果
                enter = slideInVertically(),
                exit = slideOutVertically()
            )
            .fillMaxSize()
            .background(Color.DarkGray)
    ) {}
}
```

`AnimatedVisibility` 內部數值取得
`AnimatedVisibilityScope` 是 `AnimatedVisibility{}` lambda 區域 可以直接訪問內部數值
  `transition.animateColor()` 依據動畫狀態返回顏色

```js
AnimatedVisibility(
    visible = vis,
) { // AnimatedVisibilityScope 範圍
    // transition 為 AnimatedVisibilityScope 內部值
    val background by transition.animateColor(label = "color") { state -> // EnterExitState enum 狀態
        if (state == EnterExitState.Visible) Color.Blue else Color.Red
    }
    Box(modifier = Modifier.size(128.dp).background(background))
}
```

### AnimatedContent
`AnimatedContent` 依據給予值 執行動畫 相當於 `AnimatedVisibility` + `MutableTransitionState`
  `targetState` 作為狀態的值
  `transitionSpec` 回傳值作為動畫方式
  `{targetCount ->}` 傳入動畫值

```js
var count by remember { mutableStateOf(0) }

Column {
    Button(onClick = { count++ }) {
        Text("Add")
    }
    Button(onClick = { count-- }) {
        Text("Dec")
    }
    AnimatedContent(
        targetState = count,
        transitionSpec = { // 回傳值作為動畫
            if (targetState > initialState) { // 比較狀態值 此處增加
                slideInVertically { height -> height } + fadeIn() with
                        slideOutVertically { height -> -height } + fadeOut()
            } else { // 減少
                slideInVertically { height -> -height } + fadeIn() with
                        slideOutVertically { height -> height } + fadeOut()
            }.using(
                //禁用剪輯，因為褪色的滑入/out應該
                //按範圍顯示。
                SizeTransform(clip = false)
            )
        }, label = "text"
    ) { targetCount ->
        Text(text = "$targetCount",
            modifier = Modifier.padding(24.dp))
    }
}
```

`AnimatedContent` 動畫連結方式 
  `with` 結合 `EnterTransition` 與 `ExitTransition` 產生 `ContentTransform`
  `using` 內容加入至 `ContentTransform`
```js
AnimatedContent(
        transitionSpec = { // 回傳值作為動畫
            fadeIn() with
                fadeOut() using // 合併後 ContentTransform
                  SizeTransform()
        }
    )
```

動畫伸縮長寬效果
```js
AnimatedContent(
        transitionSpec = {
            fadeIn(animationSpec = tween(150, 150)) with
                fadeOut(animationSpec = tween(150)) using
                SizeTransform { initialSize, targetSize ->
                    if (targetState) {
                        keyframes {
                            // 水平 
                            IntSize(targetSize.width, initialSize.height) at 150
                            durationMillis = 300
                        }
                    } else {
                        keyframes {
                            // 垂直
                            IntSize(initialSize.width, targetSize.height) at 150
                            durationMillis = 300
                        }
                    }
                }
        }
    ) { targetExpanded ->
        Box(modifier = Modifier
            .size(if(targetExpanded)328.dp else 128.dp)
            .background(if(targetExpanded)Color.Red else Color.Blue))
    }
```

### Crossfade
`Crossfade` 兩個元件過度動畫
  `targetState` 作為狀態的值
  `animationSpec` 補值的方式
  `{ it ->}` 傳入狀態的值
```js
var currentPage by remember { mutableStateOf(true) }

Crossfade(
  targetState = currentPage
) { state ->
    when (state) {
        true -> Text("Page A")
        false -> Text("Page B")
    }
}
```

### Modifier
`Modifier.animateContentSize` 修飾符元件大小動畫
```js
var expanded by remember { mutableStateOf(false) }
Button(
    onClick = {expanded = !expanded},
    modifier = Modifier
        .animateContentSize()
        .size(if (expanded) 100.dp else 50.dp)
) {
}
```

## 進階

### animateFloatAsState

`animateFloatAsState` 可以針對一個特定數值進行補值變化
```js
// 範例
var enabled by remember { mutableStateOf(true) }

val alpha: Float by animateFloatAsState(if (enabled) 1f else 0.5f)
Box(
    Modifier.fillMaxSize()
        .graphicsLayer(alpha = alpha)
        .background(Color.Red)
)
```

`animateFloatAsState` 特定參數對動畫細部設定
  `targetValue` 控制變化值
  `animationSpec` 數值補值方式
    `dampingRatio` 彈力 前後彈跳度
    `stiffness` 剛力 漸變速度
    `easing` 補值方程式
```js

val alpha: Float by animateFloatAsState(
    targetValue = if (enabled) 1f else 0.5f,
    
    // spring 曲線
    animationSpec = spring(
        dampingRatio = Spring.DampingRatioHighBouncy,
        stiffness = Spring.StiffnessMedium
    )

    // tween 直線
    animationSpec = tween(
        durationMillis = 300,
        delayMillis = 50,
        easing = LinearOutSlowInEasing
    )
    
    //自訂補值方程式函數
    animationSpec = tween(
        durationMillis = 300,
        easing = Easing { fraction -> fraction * fraction }
    )

    //keyframes 關鍵偵
    animationSpec = keyframes {
        durationMillis = 375
        0.0f at 0 with LinearOutSlowInEasing // for 0-15 ms
        0.2f at 15 with FastOutLinearInEasing // for 15-75 ms
        0.4f at 75 // ms
        0.4f at 225 // ms
    }

    //repeatable 重複次數
    animationSpec = repeatable(
        iterations = 3,
        animation = tween(durationMillis = 300),
        repeatMode = RepeatMode.Reverse
    )

    //infiniteRepeatable 持續動畫
    animationSpec = infiniteRepeatable(
        animation = tween(durationMillis = 300),
        repeatMode = RepeatMode.Reverse
    )

    //snap 結束動畫
    animationSpec = snap(delayMillis = 50)
)
```

### rememberInfiniteTransition

`rememberInfiniteTransition` 建立一個持續在起始與目標值反覆的動畫
  `initialValue` 起始
  `targetValue` 目標
  `animationSpec` 動畫補值

```js
val infiniteTransition = rememberInfiniteTransition()
// 針對數值設定
val color by infiniteTransition.animateColor(
    initialValue = Color.Red,
    targetValue = Color.Green,
    animationSpec = infiniteRepeatable(
        animation = tween(1000, easing = LinearEasing),
        repeatMode = RepeatMode.Reverse
    )
)

Box(Modifier.fillMaxSize().background(color))
```

## 其他
## Animation
Animation 手動控制動畫的時間 最低 API
  TargetBasedAnimation 控制動畫播放時間
  ```js
  val anim = remember {
    TargetBasedAnimation(
        animationSpec = tween(200),
        typeConverter = Float.VectorConverter,
        initialValue = 200f,
        targetValue = 1000f
    )
  }
  var playTime by remember { mutableStateOf(0L) }

  LaunchedEffect(anim) {
      val startTime = withFrameNanos { it }

      do {
          playTime = withFrameNanos { it } - startTime
          val animationValue = anim.getValueFromNanos(playTime)
      } while (someCustomCondition())
  }
  ```
  DecayAnimation 提供的設定的起始條件來計算它時間

## LaunchedEffect

LaunchedEffect 組合項目建立僅在指定鍵值的持續時間內的範圍
```js
// Start out gray and animate to green/red based on `ok`
val color = remember { Animatable(Color.Gray) }
LaunchedEffect(ok) {
    color.animateTo(if (ok) Color.Green else Color.Red)
}
Box(Modifier.fillMaxSize().background(color.value))
```

組合啟動動畫
val alphaAnimation = remember {  Animatable(0f) }
LaunchedEffect(Unit) {
    alphaAnimation.animateTo(1f)
}

串聯動畫
val alphaAnimation = remember { Animatable(0f) }
val yAnimation = remember { Animatable(0f) }

LaunchedEffect("animationKey") {
    alphaAnimation.animateTo(1f)
    yAnimation.animateTo(100f)
    yAnimation.animateTo(500f, animationSpec = tween(100))
}

並發動畫
val alphaAnimation = remember { Animatable(0f) }
val yAnimation = remember { Animatable(0f) }

LaunchedEffect("animationKey") {
    launch {
        alphaAnimation.animateTo(1f)
    }
    launch {
        yAnimation.animateTo(100f)
    }
}

## AnimatedImageVector

向量動畫圖片
```js
@Composable
fun AnimatedVectorDrawable() {
    val image = AnimatedImageVector.animatedVectorResource(R.drawable.ic_hourglass_animated)
    var atEnd by remember { mutableStateOf(false) }
    Image(
        painter = rememberAnimatedVectorPainter(image, atEnd),
        contentDescription = "Timer",
        modifier = Modifier.clickable {
            atEnd = !atEnd
        },
        contentScale = ContentScale.Crop
    )
}
```

## Modifier.pointerInput

自訂動畫手勢
```js
@Composable
fun Gesture() {
    val offset = remember { Animatable(Offset(0f, 0f), Offset.VectorConverter) }
    Box(
        modifier = Modifier
            .fillMaxSize()
            .pointerInput(Unit) {
                coroutineScope {
                    while (true) {
                        // Detect a tap event and obtain its position.
                        awaitPointerEventScope {
                            val position = awaitFirstDown().position

                            launch {
                                // Animate to the tap position.
                                offset.animateTo(position)
                            }
                        }
                    }
                }
            }
    ) {
        Circle(modifier = Modifier.offset { offset.value.toIntOffset() })
    }
}

private fun Offset.toIntOffset() = IntOffset(x.roundToInt(), y.roundToInt())
```

```js
fun Modifier.swipeToDismiss(
    onDismissed: () -> Unit
): Modifier = composed {
    val offsetX = remember { Animatable(0f) }
    pointerInput(Unit) {
        // Used to calculate fling decay.
        val decay = splineBasedDecay<Float>(this)
        // Use suspend functions for touch events and the Animatable.
        coroutineScope {
            while (true) {
                val velocityTracker = VelocityTracker()
                // Stop any ongoing animation.
                offsetX.stop()
                awaitPointerEventScope {
                    // Detect a touch down event.
                    val pointerId = awaitFirstDown().id

                    horizontalDrag(pointerId) { change ->
                        // Update the animation value with touch events.
                        launch {
                            offsetX.snapTo(
                                offsetX.value + change.positionChange().x
                            )
                        }
                        velocityTracker.addPosition(
                            change.uptimeMillis,
                            change.position
                        )
                    }
                }
                // No longer receiving touch events. Prepare the animation.
                val velocity = velocityTracker.calculateVelocity().x
                val targetOffsetX = decay.calculateTargetValue(
                    offsetX.value,
                    velocity
                )
                // The animation stops when it reaches the bounds.
                offsetX.updateBounds(
                    lowerBound = -size.width.toFloat(),
                    upperBound = size.width.toFloat()
                )
                launch {
                    if (targetOffsetX.absoluteValue <= size.width) {
                        // Not enough velocity; Slide back.
                        offsetX.animateTo(
                            targetValue = 0f,
                            initialVelocity = velocity
                        )
                    } else {
                        // The element was swiped away.
                        offsetX.animateDecay(velocity, decay)
                        onDismissed()
                    }
                }
            }
        }
    }
        .offset { IntOffset(offsetX.value.roundToInt(), 0) }
}
```

## AnimationVector1D
AnimationVector1D 數值型態轉化
```js
val IntToVector: TwoWayConverter<Int, AnimationVector1D> =
    TwoWayConverter({ AnimationVector1D(it.toFloat()) }, { it.value.toInt() })
/*
Color.VectorConverter
Dp.VectorConverter
Offset.VectorConverter
Int.VectorConverter
Float.VectorConverter
IntSize.VectorConverter
*/
// 範例 顏色
data class MySize(val width: Dp, val height: Dp){}

@Composable
fun MyAnimation(targetSize: MySize) {
    val animSize: MySize by animateValueAsState(
        targetSize,
        TwoWayConverter(
            convertToVector = { size: MySize ->
                // Extract a float value from each of the `Dp` fields.
                AnimationVector2D(size.width.value, size.height.value)
            },
            convertFromVector = { vector: AnimationVector2D ->
                MySize(vector.v1.dp, vector.v2.dp)
            }
        )
    )
}
```

## updateTransition
`updateTransition` 可建立及記住 `Transition` 的執行個體，並更新其狀態
單一值變動
```js
enum class BoxState {
    Collapsed,
    Expanded
}

var currentState by remember { mutableStateOf(BoxState.Collapsed) }
val transition = updateTransition(currentState, label = "box state")

val rect by transition.animateRect(label = "rectangle") { state ->
    when (state) {
        BoxState.Collapsed -> Rect(0f, 0f, 100f, 100f)
        BoxState.Expanded -> Rect(100f, 100f, 300f, 300f)
    }
}
val borderWidth by transition.animateDp(label = "border width") { state ->
    when (state) {
        BoxState.Collapsed -> 1.dp
        BoxState.Expanded -> 0.dp
    }
}

val color by transition.animateColor(
    transitionSpec = {
        when {
            BoxState.Expanded isTransitioningTo BoxState.Collapsed ->
                spring(stiffness = 50f)
            else ->
                tween(durationMillis = 500)
        }
    }, label = "color"
) { state ->
    when (state) {
        BoxState.Collapsed -> MaterialTheme.colorScheme.primary
        BoxState.Expanded -> MaterialTheme.colorScheme.background
    }
}

enum class DialerState { DialerMinimized, NumberPad }

@Composable
fun DialerButton(isVisibleTransition: Transition<Boolean>) {
    // `isVisibleTransition` spares the need for the content to know
    // about other DialerStates. Instead, the content can focus on
    // animating the state change between visible and not visible.
}

@Composable
fun NumberPad(isVisibleTransition: Transition<Boolean>) {
    // `isVisibleTransition` spares the need for the content to know
    // about other DialerStates. Instead, the content can focus on
    // animating the state change between visible and not visible.
}

@Composable
fun Dialer(dialerState: DialerState) {
    val transition = updateTransition(dialerState, label = "dialer state")
    Box {
        // Creates separate child transitions of Boolean type for NumberPad
        // and DialerButton for any content animation between visible and
        // not visible
        NumberPad(
            transition.createChildTransition {
                it == DialerState.NumberPad
            }
        )
        DialerButton(
            transition.createChildTransition {
                it == DialerState.DialerMinimized
            }
        )
    }
}
```

AnimatedVisibility 和 AnimatedContent 使用 updateTransition
```js
var selected by remember { mutableStateOf(false) }
// Animates changes when `selected` is changed.
val transition = updateTransition(selected, label = "selected state")
val borderColor by transition.animateColor(label = "border color") { isSelected ->
    if (isSelected) Color.Magenta else Color.White
}
val elevation by transition.animateDp(label = "elevation") { isSelected ->
    if (isSelected) 10.dp else 2.dp
}
Surface(
    onClick = { selected = !selected },
    shape = RoundedCornerShape(8.dp),
    border = BorderStroke(2.dp, borderColor),
    elevation = elevation
) {
    Column(modifier = Modifier.fillMaxWidth().padding(16.dp)) {
        Text(text = "Hello, world!")
        // AnimatedVisibility as a part of the transition.
        transition.AnimatedVisibility(
            visible = { targetSelected -> targetSelected },
            enter = expandVertically(),
            exit = shrinkVertically()
        ) {
            Text(text = "It is fine today.")
        }
        // AnimatedContent as a part of the transition.
        transition.AnimatedContent { targetState ->
            if (targetState) {
                Text(text = "Selected")
            } else {
                Icon(imageVector = Icons.Default.Phone, contentDescription = "Phone")
            }
        }
    }
}

//封裝
enum class BoxState { Collapsed, Expanded }

@Composable
fun AnimatingBox(boxState: BoxState) {
    val transitionData = updateTransitionData(boxState)
    // UI tree
    Box(
        modifier = Modifier
            .background(transitionData.color)
            .size(transitionData.size)
    )
}

// Holds the animation values.
private class TransitionData(
    color: State<Color>,
    size: State<Dp>
) {
    val color by color
    val size by size
}

// Create a Transition and return its animation values.
@Composable
private fun updateTransitionData(boxState: BoxState): TransitionData {
    val transition = updateTransition(boxState, label = "box state")
    val color = transition.animateColor(label = "color") { state ->
        when (state) {
            BoxState.Collapsed -> Color.Gray
            BoxState.Expanded -> Color.Red
        }
    }
    val size = transition.animateDp(label = "size") { state ->
        when (state) {
            BoxState.Collapsed -> 64.dp
            BoxState.Expanded -> 128.dp
        }
    }
    return remember(transition) { TransitionData(color, size) }
}
```


自訂動畫元件封裝
```js
enum class BoxState { Collapsed, Expanded }

@Composable
fun AnimatingBox(boxState: BoxState) {
    val transitionData = updateTransitionData(boxState)
    // UI tree
    Box(
        modifier = Modifier
            .background(transitionData.color)
            .size(transitionData.size)
    )
}

// Holds the animation values.
private class TransitionData(
    color: State<Color>,
    size: State<Dp>
) {
    val color by color
    val size by size
}

// Create a Transition and return its animation values.
@Composable
private fun updateTransitionData(boxState: BoxState): TransitionData {
    val transition = updateTransition(boxState, label = "box state")
    val color = transition.animateColor(label = "color") { state ->
        when (state) {
            BoxState.Collapsed -> Color.Gray
            BoxState.Expanded -> Color.Red
        }
    }
    val size = transition.animateDp(label = "size") { state ->
        when (state) {
            BoxState.Collapsed -> 64.dp
            BoxState.Expanded -> 128.dp
        }
    }
    return remember(transition) { TransitionData(color, size) }
}
```

## 組合元件
### Crossfade

`Crossfade` 指定的數值變化時 內部畫面元件過度變化
`Crossfade{}` 過度動畫元件
  `targetState` 指定變數
  `animationSpec` 過度動畫
  `selectedColor ->` 變化後的數值
```js
enum class MyColors(val color: Color) {
    Red(Color.Red), Green(Color.Green), Blue(Color.Blue)
}

@Composable
fun CrossfadeDemo() {
    var currentColor by remember { mutableStateOf(MyColors.Red) }
    Column {
        Row {
            MyColors.values().forEach { myColors ->
                Button(
                        onClick = { currentColor = myColors },
                        Modifier.weight(1f, true)
                                .height(48.dp)
                                .background(myColors.color),
                                colors = ButtonDefaults.buttonColors(backgroundColor = myColors.color)
                ) {
                    Text(myColors.name)
                }
            }
        }
        Crossfade(
          targetState = currentColor, 
          animationSpec = tween(3000)
        ) { selectedColor ->
            Box(
              modifier = Modifier
                  .fillMaxSize()
                  .background(selectedColor.color))
        }
    }
}
```



### tween
`tween` 動畫時間的插值類別
```js
/*
durationMillis - 持續時間(毫秒)
delayMillis - 延遲時間(毫秒)
easing - 插值曲線
*/
fadeIn(animationSpec = tween(
    durationMillis = 150,
    delayMillis = 150,
    easing: Easing = FastOutSlowInEasing
    ))
```