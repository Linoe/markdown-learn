
目前並不存在擴充成員
因此擴充方法 被視為聚合類型 非強結合

以下方法只適用在 新增類別與元類別進行操作時
不更動元類別方式 進行操作方法增加
相對缺點 難以找到該類別額外增加方法 單一檔案訊息完整性不夠
```C#
//元類別
public class CoroutineContext{}
//新增類別
public class Job{}
//擴充方法
public static class JobExtension
{
    public static void doJob(this CoroutineContext context, Job job){...}
}
```