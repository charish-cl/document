# 设计模式

## MVC

https://blog.csdn.net/qq_46273241/article/details/126805939

https://blog.csdn.net/qq_36709127/article/details/122153113



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



