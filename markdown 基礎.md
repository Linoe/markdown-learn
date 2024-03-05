主標題\===
===

副標題\---
---

# 標題\<h1>(Ctrl-Shift-[ or ])

## 標題\<h2>

### 標題\<h3>

#### 標題\<h4>

##### 標題\<h5>

###### 標題\<h6>

<br>

# 一般

## 字體

*斜体(Ctrl-I)*
<br>
**粗体(Ctrl-B)**
<br>
***粗斜体***
<br>
_斜体2_
<br>
__粗体2__
<br>
___粗斜体2___
<br>
~~刪除(Alt-S)~~
<br>
\\ 跳脫字元

## 標記文字

`string` \`字串\`
<br>
``float`` \``浮點\``

## 斷行

標籤斷行\<br><br>
兩個\&nbsp;\&nbsp;空格  
<p>獨立<br>字串</p>

## 橫條

---

\---  
\***

## 列表

+ \+ \- \* 清單

1. 序列
    1. 子序列
    2. 子序列
2. 序列

+ 巢狀
  + 巢狀
    + 巢狀

## 區塊

> \>第一層
>> \>>第二層
>>> \>>>第三層

## 格子對齊(Ctrl-Shift-F)

| 欄位1  |  欄位2 |  欄位3   |
| :----- | -----: | :------: |
| \:置左 | 置右\: | \:置中\: |

<br>

# 高級

## 程式區塊

```csharp
void hello()
{
  console.log("Hello World!");
}
```

```json
{
  "firstName": "John",
  "lastName": "Smith",
  "age": 25
}
```

    四格縮排
    <html>
      <head>
      </head>
    </html>

## HTML標籤

<span style="color: red; ">紅色</span>
<br>
<a href="about.html">超連結<a>

    <span style="color: red; ">紅色</span>
    <br>
    <a href="about.html">超連結<a>

## HTML-style

<red>Red</red>
<br>
<blue>blue</blue>

<style>
red { color: red }
blue { color: blue }
</style>

    <red>Red</red>
    <br>
    <blue>blue</blue>

    <style>
    red { color: red }
    blue { color: blue }
    </style>

## 連結

[標題](#列表)  
[跨文件標題](./other.md#some-header)  
[跟目錄](/path/to/other#some-header)  
[網址](https://notepm.jp)  
<https://www.markdownguide.org>  

    [標題](#列表)  
    [跨文件標題](./other.md#some-header)  
    [跟目錄](/path/to/other#some-header)  
    [網址](https://notepm.jp)  
    <https://www.markdownguide.org>  

## 標籤

[Google][1]  
[Yahoo][2]  
[MSN][3]

[1]: http://google.com/        "顯示Google"
[2]: http://search.yahoo.com/  "顯示Yahoo"
[3]: http://search.msn.com/    "顯示MSN"


    [Google][1]  
    [Yahoo][2]  
    [MSN][3]

    [1]: http://google.com/        "顯示Google"
    [2]: http://search.yahoo.com/  "顯示Yahoo"
    [3]: http://search.msn.com/    "顯示MSN"

## 圖片連結

![圖片](https://notepm.jp/assets/img/apple-touch-icon-120x120.png)
![img]

[img]: https://notepm.jp/assets/img/apple-touch-icon-120x120.png

    ![圖片](https://notepm.jp/assets/img/apple-touch-icon-120x120.png)
    ![img]

    [img]: https://notepm.jp/assets/img/apple-touch-icon-120x120.png

<br>

# 非通用

## Emoji

😎
:joy:

## OX列表(Markdown All in One)

+ [ ] Unchecked

+ [x] Checked

## 數學(Markdown+Math)

$$
C^2
$$
