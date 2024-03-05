# Unity UnityEvent

Unity 用來處理事件註冊，自訂用的類別

## UnityEvent 與 Delegate 關係

`UnityEvent` 為經過包裝的 `Delegate`

```C#
// UnityEvent
public class UnityEvent : UnityEventBase
{
  public void AddListener(UnityAction call);
  public void Invoke();
  public void RemoveListener(UnityAction call);
}
// UnityAction 
public delegate void UnityAction();
```

## UnityEvent 使用方法

當作變數來使用<br>
```C#
// 宣告
public UnityEvent event;
// 被呼叫方法
void OnCallBack();
//註冊
event.AddListener(OnCallBack);
//呼叫
event.Invoke();
```

帶有參數的回傳
```C#
// 宣告
public UnityEvent<string> event;
// 被呼叫方法
void OnCallBack(string s);
//註冊
event.AddListener(OnCallBack);
//呼叫
event.Invoke("hollw");
```

## UnityAction 使用方法

當作 `delegate` 使用

```C#
// 宣告
public static UnityAction OnLogin = new delegate {};
// 被呼叫方法
void OnCallBack();
//註冊
OnLogin += OnCallBack;
//呼叫
OnLogin();
```