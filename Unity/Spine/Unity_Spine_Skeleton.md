# Unity Spine Skeleton

Spine 用來處存 Bones Data ...等，資料的類別

## 修改 Bone

+ FindBone(String boneName) - 搜尋對應名稱的 Bone
+ bone.SetPositionSkeletonSpace(Vector3 position) - 設置 Bone 在 Spine 的位置
+ bone.GetWorldPosition(Vector3 parentTransform) - 取得世界座標，必須有父物件做為參考

```C#
Bone bone = skeletonAnimation.Skeleton.FindBone("boneName");
Vector3 worldPosition = bone.GetWorldPosition(skeletonAnimation.transform);
// note: when using SkeletonGraphic, all values need to be scaled by the parent Canvas.referencePixelsPerUnit.

Vector3 position = ...;
bone.SetPositionSkeletonSpace(position);

Quaternion worldRotationQuaternion = bone.GetQuaternion();
```

## 反轉圖片

+ ScaleX - 等同於 Spine ScaleX
+ ScaleY - 等同於 Spine ScaleY

設置為 -1 值，達翻轉效果
```C#
skeleton.ScaleX = -1;
skeleton.ScaleY = -1;
```

## 重新設置

+ SetToSetupPose() - 依序執行 SetBonesToSetupPose() SetSlotsToSetupPose()
+ SetBonesToSetupPose() - 如果有修改到 Bone 則執行此
+ SetSlotsToSetupPose() - 如果有修改到 Skin Slot Attachment 則執行此

設置外觀
```C#
bool success = skeletonAnimation.Skeleton.SetSkin("skinName");
skeletonAnimation.Skeleton.SetSlotsToSetupPose(); // see note below
```
外觀自訂外觀
```C#
var skeleton = skeletonAnimation.Skeleton;
var skeletonData = skeleton.Data;
var mixAndMatchSkin = new Skin("custom-girl");
mixAndMatchSkin.AddSkin(skeletonData.FindSkin("skin-base"));
mixAndMatchSkin.AddSkin(skeletonData.FindSkin("nose/short"));
...
skeleton.SetSkin(mixAndMatchSkin);
skeleton.SetSlotsToSetupPose();
skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton); // skeletonMecanim.Update() for SkeletonMecanim
```

如果有自訂外觀且多重 material 時，使用 GetRepackedSkin() 重新打包
```C#
using Spine.Unity.AttachmentTools;

// Create a repacked skin.
Skin repackedSkin = collectedSkin.GetRepackedSkin("Repacked skin", skeletonAnimation.SkeletonDataAsset.atlasAssets[0].PrimaryMaterial, out runtimeMaterial, out runtimeAtlas);
collectedSkin.Clear();

// Use the repacked skin.
skeletonAnimation.Skeleton.Skin = repackedSkin;
skeletonAnimation.Skeleton.SetSlotsToSetupPose();
skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton); // skeletonMecanim.Update() for SkeletonMecanim

// You can optionally clear the cache after multiple repack operations.
AtlasUtilities.ClearCache();
```

同上，重打包主texture的法线贴图和其他附加texture
```C#
Material runtimeMaterial;
Texture2D runtimeAtlas;
Texture2D[] additionalOutputTextures = null;
int[] additionalTexturePropertyIDsToCopy = new int[] { Shader.PropertyToID("_BumpMap") };
Skin repackedSkin = prevSkin.GetRepackedSkin("Repacked skin", skeletonAnimation.SkeletonDataAsset.atlasAssets[0].PrimaryMaterial, out runtimeMaterial, out runtimeAtlas,
 additionalTexturePropertyIDsToCopy : additionalTexturePropertyIDsToCopy, additionalOutputTextures : additionalOutputTextures);

// Use the repacked skin.
skeletonAnimation.Skeleton.Skin = repackedSkin;
skeletonAnimation.Skeleton.SetSlotsToSetupPose();
skeletonAnimation.AnimationState.Apply(skeletonAnimation.Skeleton); // skeletonMecanim.Update() for SkeletonMecanim
```

