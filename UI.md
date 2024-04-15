# 设计模式

## MVC

https://blog.csdn.net/qq_46273241/article/details/126805939

https://blog.csdn.net/qq_36709127/article/details/122153113

https://blog.csdn.net/qq_39574690/article/details/80757261

## 关于游戏UI的设计模式

[放牛的星星 ----Unity手游实战：从0开始SLG——UI框架篇（一）各种UI框架模型简介（试读篇）](https://zhuanlan.zhihu.com/p/157273459)



## UI代码开发规范

逻辑以及Log都在Model里面写，UI层只处理自身显示相关的逻辑

## MVVM

在Unity中实现一个简易的MVVM框架，我们需要关注几个关键点：数据绑定（双向和单向）、属性和控件的映射。以下是一个简化的实现方案：



1. 数据绑定基础
   首先，我们需要一个基础的数据绑定类，它能够监听属性的变化并通知视图更新。

```cs
using System;

public class BaseBindableProperty<T>
{
    private T _value;
    public T Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value))
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }

    public event Action<T> OnValueChanged;

    public BaseBindableProperty(T defaultValue = default)
    {
        _value = defaultValue;
    }
}
```

1. 视图模型基类
   接下来，我们需要一个视图模型的基类，它包含了数据绑定属性，并能够通知视图更新。

```cs
public class BaseViewModel
{
    // 在这里定义ViewModel的属性，使用BaseBindableProperty包装
}
```

1. 视图绑定
   视图绑定需要能够将UI控件与ViewModel中的属性绑定起来。这里我们实现一个简单的绑定工具类。

```cs
using UnityEngine;
using UnityEngine.UI;
using System.Reflection;

public static class BindingUtility
{
    public static void BindProperty<T>(BaseBindableProperty<T> property, MonoBehaviour viewComponent, string propertyName)
    {
        property.OnValueChanged += (newValue) =>
        {
            var prop = viewComponent.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (prop != null && prop.CanWrite)
            {
                prop.SetValue(viewComponent, newValue, null);
            }
        };
    }

    public static void BindInputField(BaseBindableProperty<string> property, InputField inputField)
    {
        // 单向绑定：ViewModel -> View
        property.OnValueChanged += (newValue) => { inputField.text = newValue; };

        // 双向绑定：View -> ViewModel
        inputField.onValueChanged.AddListener((newValue) => { property.Value = newValue; });
    }
}
```

1. 使用示例
   最后，我们来看一个如何使用这个简易MVVM框架的例子。

```cs
public class ExampleViewModel : BaseViewModel
{
    public BaseBindableProperty<string> ExampleTextProperty = new BaseBindableProperty<string>();
}
```

在Unity的某个UI控制器中绑定ViewModel到视图：



```cs
using UnityEngine;
using UnityEngine.UI;

public class ExampleView : MonoBehaviour
{
    public InputField exampleInputField;
    private ExampleViewModel viewModel = new ExampleViewModel();

    void Start()
    {
        BindingUtility.BindInputField(viewModel.ExampleTextProperty, exampleInputField);
    }
}
```

这个简易的MVVM框架实现了基本的数据绑定功能，包括双向绑定和单向绑定。通过扩展BaseBindableProperty和BindingUtility，可以支持更多类型的数据绑定和控件。这只是一个起点，根据项目的具体需求，这个框架可以进一步扩展和优化。



# 参考

https://github.com/vovgou/loxodon-framework/blob/master/README_CN.md

# 课程

https://edu.uwa4d.com/course-intro/0/138



## https://github.com/XINCGer/Unity3DTraining/tree/master/UGUITraining

# 工具

对齐

[GitHub - baba-s/UniUGUIToolbar: 【Unity】uGUI ](https://github.com/baba-s/UniUGUIToolbar)

https://github.com/liuhaopen/UGUI-Editor

# 原理

## [Event System Manager 事件与触发](https://blog.csdn.net/qq_32821435/article/details/80157388?spm=1001.2014.3001.5502)



## [RectTransform详解](https://www.jianshu.com/p/dbefa746e50d)



## [Unity——RectTransform详解](https://www.jianshu.com/p/4592bf809c8b)



# 屏幕适配



https://www.jianshu.com/p/9f652a00d873



## 刘海屏

[Unity UI适配总结](https://zhuanlan.zhihu.com/p/482944918)

[【游戏开发进阶】Unity Android刘海屏适配，帮你封装了jar，你不用写java了，直接用c#调用即可_林新发的博客-CSDN博客_unity3d 刘海屏](https://linxinfa.blog.csdn.net/article/details/115346335)

# 相对布局





就是出现锚框的情况



# 绝对布局



瞄框与锚点重合



在`绝对布局`的情况下，`PosX`和`PosY`的值就是Pivot到锚点的值



# sizeDelta



### 瞄点情况



**sizeDelta的值就是OffsetMax-OffsetMin的值**



### 瞄框



在锚框的情况下，offstMax减去Min，得到的将不再是UI元素的大小，而是一个新的奇怪的向量，这个向量代表的物理意义是，**sizeDelta.x值就是锚框的宽度与UI元素的宽度的差值，sizeDelta.y的值就是锚框的的高度与UI元素的高度的差值**



在锚框的情况下，offstMax减去Min，得到的将不再是UI元素的大小，而是一个新的奇怪的向量，这个向量代表的物理意义是，**sizeDelta.x值就是锚框的宽度与UI元素的宽度的差值，sizeDelta.y的值就是锚框的的高度与UI元素的高度的差值**



Pivot中心点，就是该UI元素旋转缩放的中心点，左下角为(0,0)右上角为(1,1)



**这个属性之所以叫做sizeDelta，是因为在锚点情况下其表征的是size（大小），在锚框的情况下其表征的是Delta（差值）**



作者：巨龙饿了
链接：https://www.jianshu.com/p/4592bf809c8b



## [UGUI锚点(Anchors)，轴点(Pivot)及RectTransform组件详解](https://www.jianshu.com/p/5aa5299b491f)



锚点:



轴点:小圆环







# Canvas



https://www.arkaistudio.com/blog/2016/03/28/unity-ugui-原理篇二：canvas-scaler-縮放核心/



Scale Factor直接縮放所有UI元素



# anchoredPosition



通过直接设置anchoredPosition的值可以改变UI元素的位置，但也是要分`锚点`和`锚框`的情况



在使用`锚点`的情况下，anchoredPosition表征的是元素Pivot到Anchor的距离



作者：巨龙饿了
链接：https://www.jianshu.com/p/4592bf809c8b
来源：简书
著作权归作者所有。商业转载请联系作者获得授权，非商业转载请注明出处。



# ScreenPointToLocalPointInRectangle



public static bool ScreenPointToLocalPointInRectangle(RectTransform rect, Vector2 screenPoint, Camera cam, out Vector2 localPoint)
将一个屏幕坐标转换为给定矩形里的坐标
需要注意的是，屏幕坐标是以像素为单位，左下角为(0,0),而返回的坐标是相对于给定矩形的pivot的坐标。
即使点在给定矩形外，依然返回true？此处待研究 .



作者：迷途小路
链接：https://www.jianshu.com/p/185cbe4ee981



# 移动UI



## transform.position



transform.position对应UI锚点的位置,锚点决定了UI坐标轴的圆点



1920*1080屏幕,左下角都是(0,0)右上角(1920,1080)



屏幕左下角为（0,0）点【原点】，右上角为（Screen.宽度，Screen高度）



UI坐标的起点位置是屏幕中心点，如果想实现一个鼠标点击到哪，图片就出现在哪，需要将屏幕坐标`Input.mousePosition`转化为UI坐标。比如屏幕坐标值是（Screen.width/2，Screen.height/2），在UI坐标里正好是0



Canvas以原点中心





# Scroll View

Content与Viewport大小一致，就无法移动，大于Viewport才可以移动。

# 文档

https://docs.unity3d.com/Packages/com.unity.ugui@1.0/manual/UIAutoLayout.html



# 坑

[Unity 3D之UI设置父子关系setParent坑 - CodeAntenna](https://codeantenna.com/a/ql6KctqvMy)

# UGUI源码

[UGUI源码分析](https://blog.csdn.net/qq_28820675/category_9923575.html)

[UGUI源码解读、性能优化、图文混排等](https://www.zhihu.com/column/c_1440746540318650368)
