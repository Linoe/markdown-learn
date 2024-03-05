# Unity_Collider 碰撞器

Unity Collider 用來處理物理碰撞檢測元件 相關名詞如下

- `Collider` 碰撞檢測元件
  - `isTrigger` 設定為觸發器
  - `static collider` 指沒有 Rigidbody 的 collider
  - `dynamic collider` 指擁有 Rigidbody 的 collider
- `Rigidbody` 物理數值元件
  - `IsKinematic` 讓 `Rigidbody` 不受物理引擎碰撞 仍會對其他造成影響

- `OnCollision` 是 `Collider` 碰撞都會觸發 除了 `static collider`
- `OnTrigger` 只有在 `isTrigger` 啟用時才會觸發 除了 `static collider`

- `character controller` 人物用的控制器 自帶 `Collider`

## Colliders

compound colliders 复杂的物体 组合碰撞体应该只有一个rigidbody

static collider 可用来设置墙，地面等

## Rigidbody

rigidbody的物体才会受到unity物理系统影响，应该通过force来改变物体的tranform

## OnCollision


unity物理引擎只会影响非kinematic rigidbody，如果unity检测到了两个物体的碰撞，并且需要物理引擎处理碰撞结果，只有这时才会有OnCollisionEnter

## Collision action matrix

當兩個對象發生碰撞時，可能會根據碰撞對象的剛性鍵的配置發生許多不同的腳本事件。
下面的圖表詳細介紹了哪些事件函數是根據附加到對象的組件調用的。
其中一些組合僅導致兩個對象之一受碰撞影響，但一般規則是物理不會應用於沒有附加剛體組件的對象。

發生碰撞檢測，並在碰撞時發送消息
 |                                      | Static Collider | Rigidbody Collider | Kinematic Rigidbody Collider | Static Trigger Collider | Rigidbody Trigger Collider | Kinematic Rigidbody Trigger Collider |
 | :----------------------------------: | :-------------: | :----------------: | :--------------------------: | :---------------------: | :------------------------: | :----------------------------------: |
 |           Static Collider            |                 |         Y          |                              |                         |                            |                                      |
 |          Rigidbody Collider          |        Y        |         Y          |              Y               |                         |                            |                                      |
 |     Kinematic Rigidbody Collider     |                 |         Y          |                              |                         |                            |                                      |
 |       Static Trigger Collider        |                 |                    |                              |                         |                            |                                      |
 |      Rigidbody Trigger Collider      |                 |                    |                              |                         |                            |                                      |
 | Kinematic Rigidbody Trigger Collider |                 |                    |                              |                         |                            |                                      |


碰撞時發送觸發消息
 |                                      | Static Collider | Rigidbody Collider | Kinematic Rigidbody Collider | Static Trigger Collider | Rigidbody Trigger Collider | Kinematic Rigidbody Trigger Collider |
 | :----------------------------------: | :-------------: | :----------------: | :--------------------------: | :---------------------: | :------------------------: | :----------------------------------: |
 |           Static Collider            |                 |                    |                              |                         |             Y              |                  Y                   |
 |          Rigidbody Collider          |                 |                    |                              |            Y            |             Y              |                  Y                   |
 |     Kinematic Rigidbody Collider     |                 |                    |                              |            Y            |             Y              |                  Y                   |
 |       Static Trigger Collider        |                 |         Y          |              Y               |                         |             Y              |                  Y                   |
 |      Rigidbody Trigger Collider      |        Y        |         Y          |              Y               |            Y            |             Y              |                  Y                   |
 | Kinematic Rigidbody Trigger Collider |        Y        |         Y          |              Y               |            Y            |             Y              |                  Y                   |
