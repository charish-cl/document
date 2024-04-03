## 原理

https://www.lfzxb.top/hybridclr-source-analyze/ 

https://www.zhihu.com/column/c_1489549396035870720 官方

https://hybridclr.doc.code-philosophy.com/docs/basic/install#hybridclrinstaller%E5%B7%A5%E4%BD%9C%E5%8E%9F%E7%90%86



## 安装原理



## C#什么是AOT JIT

在C#中，AOT（Ahead-of-Time）编译和JIT（Just-In-Time）编译是两种不同的代码编译策略，它们在.NET应用程序的执行性能和部署方式上有着显著的差异。

### JIT（Just-In-Time）编译

JIT编译是.NET运行时环境的一个特性，它在程序执行时动态地将IL（中间语言）代码编译成机器代码。这种编译方式允许应用程序在运行时根据当前执行环境的具体情况进行优化，比如针对特定的CPU架构进行优化。JIT编译的主要优点是灵活性和跨平台兼容性，但缺点是每次应用程序启动时都需要进行编译，这可能会导致一定的启动延迟。

### AOT（Ahead-of-Time）编译

AOT编译是另一种编译策略，它在应用程序部署之前就将IL代码编译成机器代码。这意味着编译过程发生在开发阶段而不是运行时，因此可以消除JIT编译带来的启动延迟，提高应用程序的启动速度和运行性能。AOT编译的主要优点是性能提升，特别是在启动时间和执行效率方面。然而，AOT编译的缺点是缺乏JIT编译时的运行时优化能力，且可能增加应用程序的体积，因为它需要为每个目标平台单独编译。

### 总结

**JIT编译**：在程序运行时进行，提供运行时优化，但可能导致启动延迟。

**AOT编译**：在程序部署前进行，提高启动速度和运行性能，但可能增加应用体积，缺乏运行时优化。

在.NET应用程序开发中，开发者可以根据应用程序的具体需求和部署环境选择最合适的编译策略。



## 加载

挂载热更新脚本的资源（场景或prefab）必须打包成ab，在实例化资源前先加载热更新dll即可

## 通过初始化从打包成assetbundle的prefab或者scene还原挂载的热更新脚本[](https://hybridclr.doc.code-philosophy.com/docs/basic/runhotupdatecodes#通过初始化从打包成assetbundle的prefab或者scene还原挂载的热更新脚本)

假设热更新中有这样的入口脚本，这个脚本被挂到`HotUpdatePrefab.prefab`上。

```csharp
public class HotUpdateMain : MonoBehaviour
{
    void Start()
    {
        Debug.Log("hello, HybridCLR");
    }
}
```



你通过实例化这个prefab，即可运行热更新逻辑。

```csharp
        AssetBundle prefabAb = xxxxx; // 获得HotUpdatePrefab.prefab所在的AssetBundle
        GameObject testPrefab = Instantiate(prefabAb.LoadAsset<GameObject>("HotUpdatePrefab.prefab"));
```



这种方法不需要借助任何反射，而且跟原生的启动流程相同，推荐使用这种方式初始化热更新入口代码！



## 代码裁剪

https://docs.unity3d.com/Manual/ManagedCodeStripping.html

`HybridCLR/Generate/LinkXml`， 能一键生成热更新工程里的所有AOT类型及函数引用。

### 预留你将来可能用到的类型

com.code-philosophy.hybridclr的`HybridCLR/Generate/LinkXml`命令虽然可以智能地扫描出你当前引用的AOT类型，却不能预知你未来将来使用的 类型。因此你仍然需要有规划地提前在 `Assets/link.xml`(注意！不是自动生成的那个link.xml)预留你将来 可能用到的类型。切记不要疏漏，免得出现上线后某次更新使用的类型被裁剪的尴尬状况！







## AOT元数据

https://hybridclr.doc.code-philosophy.com/docs/basic/aotgeneric

由于编译成热更dll后会丢失值类型泛型的数据类型，所以需要把Hotfix中用到的值类型泛型数据提前补充到AOT里，在元数据共享机制下，Hotfix程序就能正确识别那些值类型泛型数据。



```csharp
   public static unsafe void LoadMetadataForAOTAssembly()
    {
        List<string> aotDllList = new List<string>
        {
            "mscorlib.dll",
            "System.dll",
            "System.Core.dll", // 如果使用了Linq，需要这个
            // "Newtonsoft.Json.dll",
            // "protobuf-net.dll",
        };

        AssetBundle dllAB = LoadDll.AssemblyAssetBundle;
        foreach (var aotDllName in aotDllList)
        {
            byte[] dllBytes = dllAB.LoadAsset<TextAsset>(aotDllName).bytes;
              int err = HybridCLR.RuntimeApi.LoadMetadataForAOTAssembly(dllBytes, HomologousImageMode.SuperSet);
              Debug.Log($"LoadMetadataForAOTAssembly:{aotDllName}. ret:{err}");
        }
    }
```

# 使用



## 必须设置为Disable才能不开启huatuo

IPreprocessBuildWithReport

Description

Implement this interface to receive a callback before the Player build is started.

不然如果Enable状态会自动设置为IL2cpp

![image-20240403100753303](assets/image-20240403100753303.png)
## 程序集划分方案

https://www.jianshu.com/p/51a9a757d238
