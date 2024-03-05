# DOTween 基礎

紀錄使用到的步驟

## 安裝

unity asset store 下載執行庫<br>
https://assetstore.unity.com/packages/tools/animation/dotween-hotween-v2-27676

官網下載示範<br>
http://dotween.demigiant.com/index.php

## 使用方式

DOTween 直接擴展方法在類別上

+ DOTween - 直接控制 Unity 上 Static DOTween 物件
  + PlayAll() - 所有 Pause 中的變換開始
  + PauseAll() - 所有變換中的 Pause
  + KillAll() - 所有變換中的強制結束，包含 SetAutoKill(false)
  + RestartAll() - 所有變換中的重新開始
  + To() - 自訂一個變換的數值

+ Sequence - 儲存一列 TweenerCore 用於組合變換
  + Append() - 加入一個 TweenerCore，變換順序依加入的順序執行
  + Insert() - 插入一個 TweenerCore，依據參數插入在指定的時間播放
  + Join() - 加入一個 TweenerCore，會加入在當前變換結束後
  + PrependInterval() - 預處理延遲，所有柱列前優先處理延遲
  + AppendInterval() - 加入延遲，在當前柱列後加入一個延遲
  + SetLoops() - 設定循環，從 Sequence 列表順序循環播放

+ Tweener - 控制變換中其動畫的補間，如果 Tween 已被移除則出錯
  + ChangeEndValue() - 變更變換中的目標數值
  + Restart() - 重新啟用變換動畫

+ TweenerCore - 大部分方法都匯回傳此
  + From() - 從指定的數值變換回來
  + SetRelative() - 從當前數值移動到相對數值
  + SetAutoKill() - 設置當完成後自動移除 DOTween (效能相關)
  + SetLoops() - 在當前數值與數值間循環
  + TogglePause() - 切換變換的暫停/開始
  + Pause() - 變換暫停
  + SetEase() - 變換的曲線設置
  + SetOptions() - 變換選項，依據使用的物件相關對應的參數
  + SetLookAt() - 路徑用來指定面向

+ TweenerCore.callbacks - TweenerCore 的監聽
  + OnStepComplete() - 如果會反覆執行，則每次重新開始時呼叫

+ Transform
  + DOMove() - 移動到指定位置
  + DOPath() - 指定的路徑上移動

+ Material
  + DOColor() - 變換到指定的顏色
  + DOOffset() - 材質的偏移

+ Image
  + DOFade() - 透明值漸層變換
  + DOFillAmount - fillAmount 變換指定
  + DOColor() - color 變換指定

+ Text
  + DOText() - text 變換指定

+ Slider
  + DOValue() - value 變換指定

## 方法詳細

```C#
//變換 Text 文本給指定值
//  endValue: 文本
//  duration: 時間
//  richtextenabled: 動畫中標籤正確顯示文本?
//  scramblemode: 設置爭奪模式?
//  scramblechars: 用於爭奪模式中的字符?
DOText(string endValue, float duration,
  bool richTextEnabled = true,
  ScrambleMode scrambleMode = ScrambleMode.None,
  string scrambleChars = null)

//爭奪模式類型
public enum ScrambleMode
{
  None = 0, //不使用
  All = 1, //A-Z + a-z + 0-9
  Uppercase = 2, //A-Z
  Lowercase = 3, //a-z
  Numerals = 4, //0-9
  Custom = 5 //自訂
}
```

```C#
//指定的點轉換成路徑
//   path: 通道經過的路點
//   duration: 補間的持續時間
//   pathType: 路徑的類型
//   pathMode: 路徑模式
//   resolution: 路徑分辨率，默認 10，效能相關
//   gizmoColor: 路徑顏色，只會顯示在編輯器
DOPath(Vector3[] path, float duration,
  PathType pathType = PathType.Linear,
  PathMode pathMode = PathMode.Full3D,
  int resolution = 10,
  Color? gizmoColor = null);

//Path tweens 相關設定
//   closePath: 關閉路徑，開頭與結束點連接
//   lockPosition: 鎖定特定軸向
//   lockRotation: 鎖定特定旋轉軸
SetOptions(bool closePath,
  AxisConstraint lockPosition = AxisConstraint.None,
  AxisConstraint lockRotation = AxisConstraint.None)

//Path tweens 設定面相的方向
//   lookAhead: 混和比例(0 to 1)
//   forwardDirection: 指定的前方(默認 null 路徑前方)
//   up: 面相方向(默認 Vector3.up)
SetLookAt(float lookAhead,
  Vector3? forwardDirection = null,
  Vector3? up = null);

//DOPath 路徑模式
public enum PathType
{
  Linear = 0, //線性，由每個路點之間的直段組成
  CatmullRom = 1, //彎曲，Catmull-Rom 曲線
  CubicBezier = 2 //實驗中，彎曲路徑，Bezier 曲線
}
```

```C#
//設置變換的曲線
//  customEase: 使用自訂的 Ease ?
//  animCurve: 使用 Unity 的曲線
SetEase(Ease ease)
SetEase(Ease ease, EaseFunction customEase)
SetEase(Ease ease, AnimationCurve animCurve)

//搭配 Ease.Flash 用的?
SetEase(Ease ease, float amplitude, float period)
//搭配 Ease.Flash Ease.Back 用的?
SetEase(Ease ease, float overshoot)
```

```C#
//設置循環模式
//  loops: 循環次數，-1為無限
//  loopType: 循環的方式
SetLoops(int loops, LoopType loopType)
public enum LoopType
{
  Restart = 0, //完成一輪後重新
  Yoyo = 1, //週期性的前後
  Incremental = 2 // Restart 相同，但如果結束時數值不足捕間時間，則自動扣除掉多餘的時間，有限制條件?
}
```

## 參考

http://dotween.demigiant.com/documentation.php

https://easings.net/