# TortoiseGit_衝突合併

TortoiseGit 紀錄要如何解決版本衝突與合併

## 衝突合併設定

TortoiseGit 可以對指定的副檔名執行特定的方法
1. 開啟設定特定檔案合併工具
   > Settings> DiffViewer> Merge Tool> Advanced...
2. 加入指定的檔案
   > .unity
   > C:\Program Files\Unity\Hub\Editor\2022.3.25f1\Editor\Data\Tools\UnityYAMLMerge.exe merge -p %base %theirs %mine %merged
   > .prefab
   > C:\Program Files\Unity\Hub\Editor\2022.3.25f1\Editor\Data\Tools\UnityYAMLMerge.exe merge -p %base %theirs %mine %merged
3. 當發生衝突時，點擊編輯衝突會優先執行對應的檔名的指令

<https://www.andreasjakl.com/resoving-unity-scene-merge-conflicts-unityyamlmerge-tortoisegit/>