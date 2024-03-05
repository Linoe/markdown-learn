# ex_Kotlin_Volatile Atomic

Volatile 與 Atomic 都是用來讓所有線程可見變數 修飾方法
紀錄使用觀念與使用方式

|      名稱      |     使用     | 阻塞線程 | 效能  |              說明               |
| :------------: | :----------: | :------: | :---: | :-----------------------------: |
|   `Volatile`   |  變數修飾詞  |    X     |  高   |    變數修飾 保證所有線程可見    |
|    `Atomic`    | 變數類別封裝 |    X     |  中   |    變數包裝 CAS 比較內存變化    |
| `synchronized` | 資源方法鎖定 |    O     |  低   | 指定變數存取 阻塞其他存取的線程 |


## 內存一致性問題

java 內存模組(JMM) 可以避免硬體或系統存取差異 這也是實現多平台效果之一
JMM 定義線程與主內存之間關係
1. 共享變數儲存在 主內存 共享變數指 會被多線程存取得變數
2. 線程存在一個 私有內存 
3. 線程只能對 私有內存 進行存取 不能對 主內存 存取
4. 私有內存 對 主內存 拷貝 線程使用中的變數副本

以上的規則 當多線程對同一個變數 進行大量存取時
可能會出現 存取到尚未被更改到的變數值

此現象被稱為 內存一致性問題

## 名詞解釋

原子性(Atomic)
單一或多操作時 所有執行過程不會被任何因素中斷 或所有不執行
> 帳戶轉帳 A 帳戶 向 B 帳戶 轉入 1000 出現以下兩個動作
> A 帳戶 -1000
> B 帳戶 +1000
> 具備原子性 保證 A/B 帳戶的操作不會中斷 資金總和正常
> 不具備原子性 A 帳戶 操作中斷 B 帳戶 操作正常 資金總和不正常

可見性
當多線程存取同一變數時 變數被修改時 所有線程立刻看到修改的值
> int i = 0;
> i = 10; //線程1
> j = i; //線程2
> 具備可見性 線程1修改 i = 10 時 線程2執行 j = i 其讀取到 i 以被修改為10
> 不具備可見性 線程1修改 i = 10 時 線程2執行 j = i 其讀取到 i 尚未被修改為10

有序性
程式執行執行順序按照代碼順序執行
雖然編譯時為效率優化代碼 但仍會保證結果一致
> int i = 0;
> boolean flag = false;
> i = 1;       //代碼1  
> flag = true; //代碼2
> 具備有序性 代碼1 先執行 i = 1; 代碼2 後執行 flag = true; 符合當初撰寫
> 不具備有序性 代碼2 先執行 flag = true; 代碼1 後執行 i = 1; 不府和撰寫順序

## Volatile

`volatile` Java 輕量的共享變數(`synchronized` 為重量) 具有以下特性
1. 保證可見性 (被更改後數值所有線程立即可見)
2. 保證有序性 (不可被編譯器重新排序)
```js
//java
volatile static int i = 0;
//kotlin
@Volatile var i: Int = 0
```

`volatile` 不保證 原子性 所有當初出現多線程操作時會出現 內存一致性問題
```js
//java
public static volatile int count = 0;
for(int i=0;i<10000;i++){ // for 迴圈多次建立線程
    new Thread(){
        public void run(){
            count++;
        }
    }.start();
}
//kotlin
@Volatile var count = 0
repeat(10000) { // repeat 多次建立協程
    launch {
        count++
    }
}
```

## Atomic

`Atomic` 類別包 保證數據增減操作 對變數進行封裝 以下特性
1. 類別封裝 (非基本變數)
2. 保證原子性
```js
//java
static AtomicInteger count = new AtomicInteger(0);
//kotlin
val count = AtomicInteger()
```
所有 `Atomic` 類別會有隱性 `volatile`
操作如下
```js
//java
static AtomicInteger count = new AtomicInteger(0);
for(int i=0;i<10000;i++){ // for 迴圈多次建立線程
    new Thread(){
        public void run(){
            count.incrementAndGet();
        }
    }.start();
}
//kotlin
val count = AtomicInteger()
repeat(10000) { // repeat 多次建立協程
    launch {
        count.incrementAndGet()
    }
}
```
原子性 實現原理為 Compare And Swap(CAS)
比較內存地址上變數值是否符合預期上的數值
在此不詳細描述


## synchronized

`synchronized` Java 重量的共享變數 代價效能較差 以下特性
1. 保證原子性 (阻塞線程對變數存取的範圍)
2. 保證可見性 (被更改後數值所有線程立即可見)

`synchronized` 特定區塊內執行期間 阻塞其他存取該變數的線程
```js
//kotlin
fun testWaitThread1() = synchronized(lock) {
    lock.wait()
    println("Print second")
}

fun testWaitThread2() = synchronized(lock) {
    println("Print first")
    lock.notify()
}
//java
synchronized(this) {
  this.count++;
}
public synchronized void increment() {
  this.count++;
}
```

## ReentrantLock

`ReentrantLock` Java 可繼承的 synchronized
`Lock()` 作為存取直到 `unlock()`
有其他線程存取會被阻塞? 經常用在 try 區塊
```js
private final ReentrantLock lock = new ReentrantLock();

public void m() {
  lock.lock();  // block until condition holds
  try {
    // ... method body
  } finally {
    lock.unlock();
  }
}
```