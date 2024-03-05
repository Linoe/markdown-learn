# StateMachineBehaviour

Unity 動畫機使用的基類，繼承後可以自訂動畫機觸發的狀態

## 方法

+ OnStateEnter(...) - 當進入新狀態時

+ layerIndex - 指 Layers 的 index，從 0 開始往下增加

## 程式

第一次初始化保存
```C#
override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	if (!initialized) {
		this.component = animator.GetComponent<Component>();
		this.animator = animator;
		this.initialized = true;
	}
}
```