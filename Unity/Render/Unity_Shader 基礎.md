# Unity_Shader 基礎

`Shader` 著色器 專門用來渲染圖像的一種技術 分類有以下：
- `Vertex Shader` 頂點著色器 計算 3D 物件每個頂點 作為像素渲染資訊
- `Pixel Shader` 像素著色器 以像素為單位 計算光照 顏色

不同的圖形API 名稱不同:

|   API   |     頂點      |      像素       |
| :-----: | :-----------: | :-------------: |
| DirectX | Vertex Shader |  Pixel Shader   |
| OpenGL  | Vertex Shader | Fragment Shader |
- `Vertex Shader` 頂點著色器
- `Fragment Shaders` 片段著色器
- `Pixel Shader` 像素著色器

## Shader Graph 可視化工具

目前有二套視覺化工具
- [Shader Graph](https://unity.com/features/shader-graph)
- [Amplify Shader Editor](http://amplify.pt/unity/amplify-shader-editor/)

## Shader 程式語言

主流的語言以下：
1. `GLSL` OpenGL Shading Language 基於OpenGL
2. `HLSL` High Level Shading Language 基於DirectX
3. `Cg` C for Graphic 基於NVIDIA

建議使用 `Cg` `HLSL` 有較好的跨平台性

## Shader 基礎

`Cg` / `HLSL` 語言編寫

```C#
Shader "Unlit/NewUnlitShader" //名稱路徑 顯示在編輯器中 Shader 選項欄
{
  Properties {} //屬性 面板中顯示參數 ex: textures, colours, parameters
  SubShader //子著色 可以多個 用於不同硬體性能配置
  {
    pass {} 
  }
  FallBack "DIFFUSE" //預備 當此著色器不支持時 則使用此
}

```

`Properties` 屬性定義
```C#
// 格式
[Attribute]_Name ("Display",Type) = Value
[Attribute] // 屬性標記 用於內置特殊的處理 可以多個
_Name // 屬性名稱 用於程式內呼叫
("Display",Type) // 面板名稱 用於編輯器顯示
/* Type 有以下
    Color 顏色 (r,g,b,a)
    Int 整型
    Float 浮點數
    Vector 向量 xyz 三維
    2D 2D紋理 (默認值: white, black, gray, bump)
    3D 3D紋理
    Cube 立方體紋理
*/
// 範例
Properties
{
  // 紋理貼圖
  _Texture ("texture", 2D) = "white" {} 
  // 法線貼圖
  _NormalMap ("normal map", 2D) = "bump" {}  //Grey
  // 3D 紋理
  _3dTexture("3d texture", 3d) = "" {}
  // Cube 纹理
  _CubeTexture("cube texture", CUBE) = "" {}
  
    // [NoScaleOffset]_MainTex("我是2D纹理", 2D) = "white" {}
    // [Normal]_MainTex("我是2D纹理", 2D) = "white" {}

  // 數字
  _Int ("integer", Int) = 2
  // 浮點數
  _Float ("float", Float) = 1.5
  // 範圍數字
  _Range ("range", Range(0.0, 1.0)) = 0.5
  
    // [PowerSlider(3)]_Float("I am another Float", Range( 0 , 1)) = 0.5
    // [IntRange]_Float("i am Float", Range( 0 , 1)) = 1
    // [Toggle]_Float("I am Toggle", Range( 0 , 1)) = 1
    // [Enum(UnityEngine.Rendering.CullMode)]_Float("我是Float", Float) = 1

  // 顏色
  _Color ("colour", Color) = (1, 0, 0, 1)    // (R, G, B, A)
    // [HDR]_Color("HDR Color", Color) = (1,1,0,1)
  // 向量
  _Vector ("Vector4", Vector) = (0, 0, 0, 0)    // (x, y, z, w)
}
```

`SubShader` 語法
```C#
SubShader
{
    sampler2D _MyTexture;
    sampler2D _MyNormalMap;

    int _MyInt;
    float _MyFloat;
    float _MyRange;
    half4 _MyColor;
    float4 _MyVector;
    
    Tags
    {
        "Queue" = "Geometry"
        "RenderType" = "Opaque"
    }
    CGPROGRAM
    // Cg / HLSL 語法
    ENDCG
}
```

`Shader` 順序越小越先輩渲染
```C#
Background (1000): 用於背景和天空盒， 
Geometry (2000): 用於大多數實體物件的預設標籤， 
Transparent (3000): 用於玻璃、火、顆粒、水等具有透明特性的材料； 
Overlay (4000): 用於鏡頭光暈、GUI 元素和文字等效果。 
```

## Shader 模板

`Standard Surface Shader` 標準表面著色器 基於物理( PBR: Physically Based Rendering) 的著色系統 模擬材質與燈光 各種金屬反光效果

`Vertex/Fragment Shader` 頂點片段著色器 不受光照影響 多用於特效與介面的效果 最簡單最基礎的

`Image Effect Shader` 圖片特效著色器 針對後處理而定製的頂點片段著色器 後處理指 Bloom(Glow/泛光/輝光)、調色、景深、模糊等

`Compute Shader` 計算著色器 運行再圖形顯卡上的程式 獨立於常規渲染管線之外 直接將GPU作為並行處理器

## Shader 範例

著色器資源
<https://www.shadertoy.com/>
著色器編寫
<https://docs.unity3d.com/Manual/Shaders.html>

Shader 知識與原理
<https://medium.com/ericzhan-publication/shader%E7%AD%86%E8%A8%98-shader-development-using-unity-01-b1cde1f23adf>

官網範例
<https://docs.unity3d.com/Manual/SL-SurfaceShaderExamples.html>
Shader 撰寫教學
<https://www.alanzucconi.com/2015/06/10/a-gentle-introduction-to-shaders-in-unity3d/>

發光效果
[Glow Shader](https://ithelp.ithome.com.tw/articles/10222347)
不規則消失
[Dissolve Shader](https://ithelp.ithome.com.tw/articles/10223309)
波動水面
[Cartoon Water Shader](https://ithelp.ithome.com.tw/articles/10223698)