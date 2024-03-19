

# GF

## 源码

StartForce只有dll

![image-20240318093946260](assets/image-20240318093946260.png)

## Procedure

流程不应该划分过多，理想情况应该不是互相依赖的，那样就能像Animator一样任意配置



### 选中Procedure会产生报错

you must initialize procedure

![image-20240318142830747](assets/image-20240318142830747.png)

## 实体模块

### 为何要额外添加一个EntityLogic

```
我在这简单说一下我的理解哈，先说Entity这部分，Entity类才是被框架直接管理的类，因为他实现了IEntity接口，这个接口被GF部分管理，而我们业务是继承EnityLogic不直接继承Entity，Entity是个sealed类，最主要的作用是防止业务继承Entity后override了父类方法，改写了原本的逻辑，会破坏了框架的管理逻辑，那怎么办呢，那就是添加一个EnityLogic，业务只能继承EnityLogic，那就不怕Entity的逻辑被覆盖了。至于EnityLogic里持有了Entity，只是比较方便通过EnityLogic获取他对应的Entity而已。
```



## 资源管理

### 三种更新模式

![image-20240313151346429](assets/image-20240313151346429.png)



### 打包工具

![image-20240313153522593](assets/image-20240313153522593.png)



#### ForceBuild

是否强制打包，如果不勾如果已经打包过就不打了



#### ResourceEditorController

默认会从GameFramework/Configs/ResourceEditor.xml里加载配置文件

```css
m_ConfigurationPath = Type.GetConfigurationPath<ResourceEditorConfigPathAttribute>() ?? Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "GameFramework/Configs/ResourceEditor.xml"));
```

这里也是

```cs
m_ConfigurationPath = Type.GetConfigurationPath<ResourceBuilderConfigPathAttribute>() ?? Utility.Path.GetRegularPath(Path.Combine(Application.dataPath, "GameFramework/Configs/ResourceBuilder.xml"));

```



### 打包出来的文件

![image-20240313153916837](assets/image-20240313153916837.png)

#### BuildReport 报告

#### Full

单独的包

#### Packege

打成一个包



## 热更新流程

![GF热更新流程整理（初版）](assets/GF热更新流程整理（初版）.png)

## 引用池

![ReferencePool](assets/ReferencePool.png)



## UI

![UI](assets/UI.jpg)

### StartForceUI

从数据表中加载

![image-20240318083631484](assets/image-20240318083631484.png)

![image-20240318083614453](assets/image-20240318083614453.png)





### 打开UI界面

![image-20240318094555523](assets/image-20240318094555523-1710726357563-1.png)

## EventPool

![EventPool](assets/EventPool.png)

# 安卓打包

https://blog.csdn.net/qq_29848853/article/details/132788896 



## 包体资源优化

https://egostudio.blog.csdn.net/article/details/51459749?spm=1001.2101.3001.6650.1&utm_medium=distribute.pc_relevant.none-task-blog-2%7Edefault%7ECTRLIST%7ERate-1-51459749-blog-106270636.235%5Ev43%5Epc_blog_bottom_relevance_base3&depth_1-utm_source=distribute.pc_relevant.none-task-blog-2%7Edefault%7ECTRLIST%7ERate-1-51459749-blog-106270636.235%5Ev43%5Epc_blog_bottom_relevance_base3&utm_relevant_index=2



https://wanghan.blog.csdn.net/article/details/135987067?spm=1001.2101.3001.6650.2&utm_medium=distribute.pc_relevant.none-task-blog-2%7Edefault%7EYuanLiJiHua%7EPosition-2-135987067-blog-51459749.235%5Ev43%5Epc_blog_bottom_relevance_base3&depth_1-utm_source=distribute.pc_relevant.none-task-blog-2%7Edefault%7EYuanLiJiHua%7EPosition-2-135987067-blog-51459749.235%5Ev43%5Epc_blog_bottom_relevance_base3&utm_relevant_index=5

# 问题

## JsonUtility

这玩意不支持属性

![image-20240313201717960](assets/image-20240313201717960.png)

C:\Program Files\Unity\Hub\Editor\2022.3.17f1c1\Editor\Data\PlaybackEngines\AndroidPlayer\SDK\platform-tools

## unity使用adb

https://blog.csdn.net/qq_30144243/article/details/136318038

## 真机调试

https://zhuanlan.zhihu.com/p/616063381

## 分支合并

https://juejin.cn/post/7028965736022278175

合并完删了就好了

工作区 暂存区

## 哪些是必要的Procedure？



## 背包怎么搞？

一个UI下面多个Item怎么搞？

## 重启Game窗口竟然能解决scale变化问题

