## @Stable

`@Stable` 傳回始終相同 該值無法變更 提高編譯器效能

```js
@Stable
val fontScale: Float
@Stable
fun Dp.toPx(): Float = value * density
```