# Open-closed principle 開閉原則

類別應該能夠容易地被擴展,但同時也要能夠容易地被封閉

## 判斷方式
擴展開放：
- 如果增加新繼承類別，也無法達成既有功能，代表無法被擴展
- 如果必須用if-else來切割這個執行區塊，代表這個功能存在歧異流程，很難被擴展

修改封閉：
- 新需求或修改條件時，需要修改原本核心程式，代表違反 OCP

```C#
// 壞例子，如果需要修改 Circle 或 Rectangle 面積公式，必須修改 AreaCalculator 原本程式碼
public class AreaCalculator
{
    public float GetRectangleArea(Rectangle rectangle)
    {
        return rectangle.width * rectangle.height;
    }
    public float GetCircleArea(Circle circle)
    {
        return circle.radius * circle.radius * Mathf.PI;
    }
}
// 即使增加新的矩形，也沒辦法再 AreaCalculator 達成計算面積的功能
public class Rectangle
{
    public float width;
    public float height;
}
public class Circle
{
    public float radius;
}
```

```C#
// 好例子，即使增加新的類型，不需要去修改原本核心程式碼，也能達原本功能
public class AreaCalculator
{
    public float GetArea(Shape shape)
    {
        return shape.CalculateArea();
    }
}
// Rectangle 跟 Circle 繼承 Shape 通用於原本核心程式碼
public abstract class Shape
{
    public abstract float CalculateArea();
}
// 新增新的矩形也不需要去更動任何原本的程式碼
// 如果要修改面積公式，只需要在各自矩形修改，而非 AreaCalculator
public class Rectangle : Shape
{
    public float width;
    public float height;
    public override float CalculateArea()
    {
        return width * height;
    }
}
public class Circle : Shape
{
    public float radius;
    public override float CalculateArea()
    {
        return radius * radius * Mathf.PI;
    }
}
```

## 其他判斷

- 每次需要新的多邊形時,只需定義一個繼承自 Shape 的新類別即可。

- 每個子類都會覆寫 CalculateArea方法,以計算出正確的面積。

- 這個新設計讓除錯變得更加簡單。如果某個新形狀導致了錯誤,你不必再重新檢查 AreaCalculator 的代碼。

- 舊代碼保持不變,因此你只需檢查新代碼中的邏輯錯誤即可。