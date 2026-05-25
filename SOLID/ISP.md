# Interface segregation principle 介面隔離原則

不應該強迫任何客戶端去依賴它不需要使用的方法。

## 判斷方式
被迫依賴：
- 假如這個介面出現過多的行為功能，可能代表介面邊界過度
- 假如這個介面異動時，發生不相關的類別也被迫修改，代表介面邊界錯誤

不需要的實作：
- 繼承介面時，卻出現這個類別根本不需要實作的功能，代表介面違反 ISP

```C#
// 壞例子，通用 ITarget 介面，其中包含受到傷害、爆炸以及觸發效果的等方法
public interface ITarget
{
    void TakeDamage(int amount);
    void Explode();
    void TriggerEffect();
}
// 但卻被迫實作 ITarget 介面中定義的所有方法。
public class ExplodableTarget : ITarget
{
    public void TakeDamage(int amount) { }
    public void Explode() { }
    public void TriggerEffect() { }
}
public class DamageableTarget : ITarget
{
    public void TakeDamage(int amount) { }
    public void Explode() { }
    public void TriggerEffect() { }
}
```

```C#
// 好例子，將行為介面邊界確實
public interface IMovable
{
    float MoveSpeed { get; set; }
    float Acceleration { get; set; }

    void GoForward();
    void Reverse();
    void TurnLeft();
    void TurnRight();
}

public interface IDamageable
{
    float Health { get; set; }
    int Defense { get; set; }

    void TakeDamage();
    void RestoreHealth();
    void Die();
}

public interface IUnitStats
{
    int Strength { get; set; }
    int Dexterity { get; set; }
    int Endurance { get; set; }
}
public interface IExplodable
{
    float Mass { get; set; }

    float ExplosiveForce { get; set; }

    float FuseDelay { get; set; }

    void Explode();
}

// 只需要繼承所需行為的介面即可
public class ExplodingBarrel : IDamageable, IExplodable { }

public class EnemyUnit : IDamageable, IMovable, IUnitStats { }

```

## 其他判斷

- 將其拆分成幾個較小的界面,而非製作一個包含太多方法的界面來管理那些可拆分的屬性。如此一來,實現這些功能的類就能夠只使用自己所需要的功能了。