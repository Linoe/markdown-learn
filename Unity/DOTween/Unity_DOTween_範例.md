# DOTween 範例

紀錄使用到的範例

## 文本變換

```C#
// 直接更換文字
text.DOText("text", 2);
// 接續在後面
relativeText.DOText(" - relativeText", 2).SetRelative();
// 文字會有拉霸變動的叫果
scrambledText.DOText("scrambledText", 2, true, ScrambleMode.All);
```

## 變換柱列

```C#
//Sequence 使用方式
Sequence mySequence = DOTween.Sequence();
mySequence.Append(transform.DOMoveX(45, 1))
  .Append(transform.DORotate(new Vector3(0,180,0), 1))
  .PrependInterval(1)
  .Insert(0, transform.DOScale(new Vector3(3,3,3), mySequence.Duration()));
```

加入延遲 PrependInterval() AppendInterval()
```C#
//預處理延遲，等同下面先加入延遲
mySequence.Append(transform.DOMoveX(45, 1))
  .PrependInterval(1);

//加入延遲
mySequence.AppendInterval(1);
  .Append(transform.DOMoveX(45, 1));
```

## 自訂變換

用來自訂型態變換

使用 DOTween.To()，第一個參數使用自訂的類別<br>
後面按照一般 DOTween.To() 作法

```C#
//將給定屬性從當前值更改為給定的屬性
// getter: 補間返回值?
// setter: 補間回傳值?
// to: 結束值
// duration: 持續時間
static DOTween.To(getter, setter, to, float duration)

// Vector3 範例
DOTween.To(()=> myVector, x=> myVector = x, new Vector3(3,4,8), 1);
// Float 範例
DOTween.To(()=> myFloat, x=> myFloat = x, 52, 1);
```
```C#
//從給定的啟動到給定端值的虛擬屬性，並實現了一個允許使用外部方法或lambda使用該值的設置器。
// setter: 設定器用二元值執行的操作。
// startValue: 啟動值開始的值。
// endValue: 端值要達到的價值。
// duration: 持續時間
DOTween.To(setter, float startValue, float endValue, float duration)

// 方法
DOTween.To(MyMethod, 0, 12, 0.5f);
// lambda
DOTween.To(x => someProperty = x, 0, 12, 0.5f);
```

```C#
//使用自定義插件的二元屬性或字段到給定值
// 參數:
//   plugin: 自訂插件，繼承 ABSTweenPlugin<T1, T2, TPlugOptions>
//   getter: 對該領域或屬性的收集器為tween。
//   setter: 字段或屬性的設定器
//   endValue: 結束值
//   duration: 持續時間
DOTween.To(ABSTweenPlugin<T1, T2, TPlugOptions> plugin, DOGetter<T1> getter, DOSetter<T1> setter, T2 endValue, float duration)
//常規通用方式的區別是簡單
//必須將插件傳遞以用作附加的第一個參數
DOTween.To(customRangePlugin,
  () => customRange,
  x => customRange = x,
  new CustomRange(20, 100), 4);
```
```C#
TweenerCore<T1, T2, TPlugOptions> t;

t.getter();
t.setter(t.startValue);

t.endValue = t.startValue + t.changeValue;
t.changeValue = t.endValue - t.startValue;
```

```C#

//生命週期
// ConvertToStartValue: 如果傳進來的值不能計算在此進行轉換
// SetChangeValue: 計算總變換值，設定 t.changeValue
// EvaluateAndApply: 補間變換呼叫，大多參數用來給 EaseManager.Evaluate() 使用
//   一般會使用 EaseManager.Evaluate() 計算補間量，結果傳遞給 setter()，如下
//   EaseManager.Evaluate(t, elapsed, duration, t.easeOvershootOrAmplitude, t.easePeriod);
// Reset: 起始值和狀態重置時呼叫，重新設定 


public abstract class ABSTweenPlugin<T1, T2, TPlugOptions> : ITweenPlugin where TPlugOptions : struct, IPlugOptions
{
  protected ABSTweenPlugin();

  // 用來轉換 T1 > T2 變換?
  public abstract T2 ConvertToStartValue(TweenerCore<T1, T2, TPlugOptions> t, T1 value);
  // 根據給定時間和輕鬆計算值
  public abstract void EvaluateAndApply(TPlugOptions options, Tween t, bool isRelative, DOGetter<T1> getter, DOSetter<T1> setter, float elapsed, T2 startValue, T2 changeValue, float duration, bool usingInversePosition, int newCompletedSteps, UpdateNotice updateNotice);
  // 將常規持續時間轉換為基於速度的持續時間
  public abstract float GetSpeedBasedDuration(TPlugOptions options, float unitsXSecond, T2 changeValue);
  // 重置?
  public abstract void Reset(TweenerCore<T1, T2, TPlugOptions> t);
  // 設置Tween的變化值
  public abstract void SetChangeValue(TweenerCore<T1, T2, TPlugOptions> t);
  // 設置Tween的From值
  public abstract void SetFrom(TweenerCore<T1, T2, TPlugOptions> t, bool isRelative);
  // 設置Tween的From值，如果特殊值?
  public abstract void SetFrom(TweenerCore<T1, T2, TPlugOptions> t, T2 fromValue, bool setImmediately, bool isRelative);
  // 設置Tween的Relative值的結束值?
  public abstract void SetRelativeEndValue(TweenerCore<T1, T2, TPlugOptions> t);
}
```

## 僅使用數值變換


```C#
// easeType: 曲線類型
// customEase: ?
// time: 經過時間
// duration: 總時間期間
// overshootOrAmplitude: 提供給 EvaluateAndApply() 使用
// period: 提供給 EvaluateAndApply() 使用

Evaluate(Ease easeType, 
EaseFunction customEase, 
float time, 
float duration, 
float overshootOrAmplitude, 
float period);
```
```C#
//計算經過時間
float passedTime = nowTime - startTime;
//只使用曲線計算
float v = EaseManager.Evaluate(Ease.Linear, null, passedTime, _scaleDuration, 0, 0);
//轉換成數值
float scaleValue = Mathf.Lerp(_scale, _toScale, v);
//設定給物件
_dropText.transform.localScale = new Vector3(scaleValue, scaleValue, scaleValue);
```

