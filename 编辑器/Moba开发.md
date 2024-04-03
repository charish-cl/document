

[NKGMobaBasedOnET: 基于ET框架致敬LOL的Moba游戏，包含完整的客户端与服务端交互，热更新，基于状态帧同步的战斗系统（包含完整的预测回滚功能），基于双端行为树的技能系统（提供通用的可视化节点编辑器），更多精彩等你发现！ (gitee.com)](https://gitee.com/NKG_admin/NKGMobaBasedOnET/#https://gitee.com/link?target=https%3A%2F%2Fgithub.com%2Fmeniku%2FNPBehave)

# 插件

### Luban

[https://focus-creative-games.github.io/luban/excel/#%E5%9F%BA%E7%A1%80](https://focus-creative-games.github.io/luban/excel/#基础)



写完这个去看看

[Unity热更新LuaFramework - 知乎 (zhihu.com)](https://www.zhihu.com/column/c_1388220390854983680)



# 服务端寻路思路

1. 通过客户端（Unity）导出地图信息
2. 将地图信息放到服务端解析
3. 服务端通过A*寻路算法计算路径点，然后服务端将这些路径点返回给客户端

[服务端寻路实现总结 - leo66666 - 博客园 (cnblogs.com)](https://www.cnblogs.com/leothzhang/p/svr_nav.html)



## A*

[unity A*寻路 （一）导出NavMesh数据_weixin_30488313的博客-CSDN博客](https://blog.csdn.net/weixin_30488313/article/details/96913628?utm_medium=distribute.pc_feed_404.none-task-blog-2~default~BlogCommendFromBaidu~Rate-11-96913628-blog-null.pc_404_mixedpudn&depth_1-utm_source=distribute.pc_feed_404.none-task-blog-2~default~BlogCommendFromBaidu~Rate-11-96913628-blog-null.pc_404_mixedpud)



## RaycastNavMesh

[recastnavigation/recastnavigation: Navigation-mesh Toolset for Games (github.com)](https://github.com/recastnavigation/recastnavigation)

[3D寻路系统NavMesh-服务端篇_流子的博客-CSDN博客_recast4j](https://jiangguilong2000.blog.csdn.net/article/details/125592067)

加载地图

```csharp
ptr = RecastInterface.RecastLoad(name.GetHashCode(), navdata, navdata.Length);
```

先导出obj文件,再导出bin文件



先安装库再重新生成解决了Oh!!!!!!

见证历史

![image-20230114180522525](C:\Users\CodeElk\AppData\Roaming\Typora\typora-user-images\image-20230114180522525.png)

![image-20230114182052332](C:\Users\CodeElk\AppData\Roaming\Typora\typora-user-images\image-20230114182052332.png)

[Visual Studio开源库集成器Vcpkg全教程--利用Vcpkg轻松集成开源第三方库_Achilles的博客-CSDN博客_vcpkg导出](https://blog.csdn.net/cjmqas/article/details/79282847)



调成Win32竟然成功了艹

![image-20230114192030583](C:\Users\CodeElk\AppData\Roaming\Typora\typora-user-images\image-20230114192030583.png)

[vs工程生成dll文件及其调用方法_第55号小白鸭的博客-CSDN博客_vs生成.dll文件但是生成了exe跟.pdb](https://blog.csdn.net/weixin_44536482/article/details/91519413)

![image-20230114193402257](C:\Users\CodeElk\AppData\Roaming\Typora\typora-user-images\image-20230114193402257.png)

------

艹,读取的是obj文件直接闪退,浪费了一些时间,好好看烟雨教程没那么多事了,真是的

艹,y轴必须是地图高度?,不然直接unity寻路直接崩溃 不知道哪里出问题了



# 数值

接收伤害要先计算暴击，然后格挡伤害，护甲减伤，buff减伤，buff增伤等，

+400 生命值
+1% 生命恢复

这种可以分成两类DataModifier,常量值修饰,百分比修饰

再把计算来源分开就好了

EgamePlay的哪个数值组件我觉得更好些,烟雨的顺序计算伤害用DataMofifer也挺好,

想了下没必要监听数值,监听行动事件就好了,间接一点好



## 如何计算暴击?

烟雨里项目没有

# 碰撞

许多英雄技能形状不规则,需要ke'y

## Box2D

fixture(固定装置)必须定义,否则刚体无法物理模拟

刚体可以拥有多个fixture,如锤子



刚体创建好后,无法通过BodyDef修改属性,要通过Body来修改

### 碰撞事件

world有一个注册监听者的方法,参数IContactListener,继承这个接口作为碰撞的监听者

```csharp
World.SetContactListener(self)
```

Fixture有object类型的UserData,用于传自定义数据

```
Unit unitA = (Unit) contact.FixtureA.UserData;
Unit unitB = (Unit) contact.FixtureB.UserData;
```

Body设置位置角度,用于同步物理世界的坐标

```c
Body.SetTransform(pos, self.Body.GetAngle());
```

## 基本使用

```csharp
private World world;
private void Start()
{
    world= new World();
    var body = CreateCircleBody(Vector2.Zero);
    Debug.Log(body.GetPosition());
    body.ApplyForceToCenter(new Vector2(1,0),true);
    
    // world.Step(Time.deltaTime,10,10);
    // Debug.Log(body.GetPosition());
}
[Button]
Body CreateCircleBody(Vector2 pos,float radius  =1 )
{
    var bodyDef = new BodyDef();
    bodyDef.Position = pos;
    var body = world.CreateBody(bodyDef);
    var fixtureDef= new FixtureDef();
    fixtureDef.Shape = new CircleShape(){Radius = radius};
    body.CreateFixture(fixtureDef);
    return body;
}
private void Update()
{
    world.Step(Time.deltaTime,10,10);
    world.DrawDebugData();
}
```

## NKG

他是用bson作为碰撞数据传输的,我用json,luban好了

```
 public class Darius_E_CollisionHandler : AB2S_CollisionHandler
```

他的每个技能都继承了这个,接收碰撞事件,根据name去做每个技能碰撞处理

这个感觉没必要呀....

一个Entity包含

- B2S_ColliderComponent,具有生命周期,根据配置生成碰撞体

  

# Slate时间轴工具

这个用来右键添加行为

```
[Attachable(typeof(ActorGroup))]
```

不要直接改动代码,先删除节点再改,不然闪退了



```
[Description("All tracks of an Actor Group affect a specific actor GameObject or one of it's Components. Specifying a name manually comes in handy if you want to set the target actor of this group via scripting. The ReferenceMode along with InitialCoordinates are essential when you are working with prefab actors.")]
```

CutScene过场动画

```
CutScene驱动->

CutsceneGroup->包含

CutsceneTrack->包含

ActionClip
```

## 更改默认的绘制内容,替换为Odin

![image-20230131152609932](C:\Users\CodeElk\AppData\Roaming\Typora\typora-user-images\image-20230131152609932.png)

注释掉即可

集合里的元素必须要可以序列化,才能绘制出来

# 状态帧同步基础框架

预测的对象只是玩家自己，不会预测别的玩家，否则会极大增加预测回滚成本，比如玩家A操控英雄A，那么就会预测英雄A的行为，对于玩家B,C,D一律只根据服务器回包来进行状态更新

## 核心代码



```csharp
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Box2DSharp.Dynamics;
using ET.EventType;
using UnityEngine;

#if !SERVER
using UnityEngine.Profiling;
#endif


namespace ET
{
    public static class LSF_ComponentUtilities
    {
        /// <summary>
        /// 正常Tick（由FixedUpdate发起调用）
        /// 对于客户端来说，自带一致性检查和预测回滚操作
        /// </summary>
        private static void LSF_TickNormally(this LSF_Component self)
        {
#if !SERVER
            Profiler.BeginSample("LockStepStateFrameSyncComponentUpdateSystem");
#else
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif


#if !SERVER
            if (!self.ShouldTickInternal)
            {
                return;
            }
#endif
            self.CurrentFrame++;

#if !SERVER
            self.CurrentArrivedFrame = self.CurrentFrame;
#endif

#if SERVER
            //Log.Info($"------------帧同步Tick Time Point： {TimeHelper.ClientNow()} Frame : {self.CurrentFrame}");
            if (self.FrameCmdsToHandle.TryGetValue(self.CurrentFrame, out var currentFrameCmdToHandle))
            {
                foreach (var cmd in currentFrameCmdToHandle)
                {
                    //Log.Info($"------------处理第{self.CurrentFrame}帧指令");
                    LSF_CmdDispatcherComponent.Instance.Handle(self.GetParent<Room>(), cmd);
                }
            }

            self.FrameCmdsToHandle.Remove(self.CurrentFrame);

#else
            Unit playerUnit = self.GetParent<Room>().GetComponent<UnitComponent>().MyUnit;

            //Log.Error($"current frame: {self.CurrentFrame}, CmdCountToHandle: {self.FrameCmdsToHandle.Count}");

            if (self.FrameCmdsToHandle.Count > 0)
            {
                var frameCmdsQueuePair = self.FrameCmdsToHandle.First();

                // 现根据服务端发回的指令进行一致性检测，如果需要的话就进行回滚
                bool shouldRollback = false;
                Queue<ALSF_Cmd> frameCmdsQueue = frameCmdsQueuePair.Value;
                uint targetFrame = frameCmdsQueuePair.Key;

                foreach (var frameCmd in frameCmdsQueue)
                {
                    // 非本地玩家的指令直接执行
                    if (frameCmd.UnitId != playerUnit.Id)
                    {
                        // 远程玩家指令直接执行
                        LSF_CmdDispatcherComponent.Instance.Handle(self.GetParent<Room>(), frameCmd);
                    }

                    //只有本地玩家的指令才有回滚的可能性
                    if (frameCmd.UnitId == playerUnit.Id)
                    {
                        // 在一致性检查过程中需要手动将指令的HasHandled设置为true，因为我们无法得知究竟那些指令被哪些一致性检查组件所使用了
                        if (!self.CheckConsistencyCompareSpecialFrame(targetFrame, frameCmd))
                        {
                            shouldRollback = true;
                            Log.Error($"由于{MongoHelper.ToJson(frameCmd)}的不一致，准备进入回滚流程");
                        }
                        // 如果指令已经经历过一致性检查，但frameCmd.PassingConsistencyCheck标记依旧为false，说明一致性检查未通过，则直接进入回滚流程进行处理（说明是类似RPC调用或者在本地无记录的本地玩家命令）
                        else if (!frameCmd.PassingConsistencyCheck)
                        {
                            shouldRollback = true;
                            Log.Error($"由于{MongoHelper.ToJson(frameCmd)}未被处理，准备进入回滚流程");
                        }
                    }
                }

                if (shouldRollback)
                {
                    self.IsInChaseFrameState = true;
                    self.CurrentFrame = targetFrame;

                    foreach (var frameCmd in frameCmdsQueue)
                    {
                        // 本地玩家的的指令才会回滚
                        if (frameCmd.UnitId == playerUnit.Id)
                        {
                            //回滚处理
                            self.RollBack(self.CurrentFrame, frameCmd);

                            if (!frameCmd.PassingConsistencyCheck)
                            {
                                LSF_CmdDispatcherComponent.Instance.Handle(self.GetParent<Room>(), frameCmd);
                            }

                            frameCmd.PassingConsistencyCheck = true;
                        }
                    }

                    //因为这一帧已经重置过数据，所以从下一帧开始追帧
                    self.CurrentFrame++;

                    //Log.Error("收到服务器回包后发现模拟的结果与服务器不一致，即需要强行回滚，则回滚，然后开始追帧");
                    // 注意这里追帧到当前已抵达帧的前一帧，因为最后有一步self.LSF_TickManually();用于当前帧Tick，不属于追帧的范围
                    int count = (int) self.CurrentArrivedFrame - 1 - (int) self.CurrentFrame;

                    while (count-- >= 0)
                    {
                        Log.Error($"开始追帧Tick，：{self.CurrentFrame}");
                        self.LSF_TickManually();
                        self.CurrentFrame++;
                    }

                    self.IsInChaseFrameState = false;
                }

                self.FrameCmdsToHandle.Remove(frameCmdsQueuePair.Key);
            }


#endif
            // 执行本帧本应该执行的的Tick
            self.LSF_TickManually();

            // 发送本帧收集的指令
            self.SendCurrentFrameMessage();

#if !SERVER
            Profiler.EndSample();
#else
            stopwatch.Stop();
            //Log.Info($"LockStepStateFrameSyncComponentUpdateSystem Cost: {stopwatch.ElapsedMilliseconds}");
#endif
        }

        /// <summary>
        /// 正式的帧同步Tick，所有的战斗逻辑都从这里出发，会自增CurrentFrame
        /// </summary>
        /// <param name="chaseFrame">是否处于追帧状态</param>
        private static void LSF_TickManually(this LSF_Component self)
        {
#if !SERVER
            Queue<ALSF_Cmd> validCmds = null;

            self.PlayerInputCmdsBuffer.TryGetValue(self.CurrentFrame, out validCmds);

            if (validCmds != null)
            {
                foreach (var cmd in validCmds)
                {
                    //处理用户输入缓冲区中的指令，用于预测
                    //Log.Info($"------------第{self.CurrentFrame}帧处理用户输入缓冲区指令");
                    LSF_CmdDispatcherComponent.Instance.Handle(self.GetParent<Room>(), cmd);
                }
            }
#endif

            // LSFTick Room，tick room的相关组件, 然后由Room去Tick其子组件，即此处是战斗的Tick起点
            self.GetParent<Room>().GetComponent<LSF_TickComponent>()
                ?.TickStart(self.CurrentFrame, GlobalDefine.FixedUpdateTargetDTTime_Long);

            // LSFTick Room，tick room的相关组件, 然后由Room去Tick其子组件，即此处是战斗的Tick起点
            self.GetParent<Room>().GetComponent<LSF_TickComponent>()
                ?.Tick(self.CurrentFrame, GlobalDefine.FixedUpdateTargetDTTime_Long);

            // 所有Tick结束后，一些数据收集工作，比如收集快照信息（对于服务端来说，每个玩家都要记录，而对于客户端来说，只需要记录本地玩家即可，因为只有本地玩家进行了预测）
            self.GetParent<Room>().GetComponent<LSF_TickComponent>()
                ?.TickEnd(self.CurrentFrame, GlobalDefine.FixedUpdateTargetDTTime_Long);
        }

        /// <summary>
        /// 发送本帧收集的指令，所有的帧同步消息都通过这个接口发送
        /// </summary>
        /// <param name="self"></param>
        /// <param name="messageToSend"></param>
        /// <typeparam name="T"></typeparam>
        private static void SendCurrentFrameMessage(this LSF_Component self)
        {
            if (self.FrameCmdsToSend.TryGetValue(self.CurrentFrame, out var cmdQueueToSend))
            {
                foreach (var cmdToSend in cmdQueueToSend)
                {
#if SERVER
                    M2C_FrameCmd m2CFrameCmd = new M2C_FrameCmd() {CmdContent = cmdToSend, ServerTimeSnap =
 TimeHelper.ClientNow()};
                    MessageHelper.BroadcastToRoom(self.GetParent<Room>(), m2CFrameCmd);
#else
                    C2M_FrameCmd c2MFrameCmd = new C2M_FrameCmd() {CmdContent = cmdToSend};
                    Game.Scene.GetComponent<PlayerComponent>().GateSession.Send(c2MFrameCmd);
#endif
                }
            }

            //因为我们KCP确保消息可靠性，所以可以直接移除
            self.FrameCmdsToSend.Remove(self.CurrentFrame);
        }

        /// <summary>
        /// 注意这里的帧数是消息中的帧数
        /// 特殊的，对于服务器来说，哪一帧收到客户端指令就会当成客户端在哪一帧的输入(累加一个缓冲帧时长)
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cmdToHandle"></param>
        public static void AddCmdToHandleQueue(this LSF_Component self, ALSF_Cmd cmdToHandle)
        {
            uint correntFrame = cmdToHandle.Frame;

            if (self.FrameCmdsToHandle.TryGetValue(correntFrame, out var queue))
            {
                queue.Enqueue(cmdToHandle);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToHandle);

                self.FrameCmdsToHandle[correntFrame] = newQueue;
            }
        }

        /// <summary>
        /// 将指令加入待发送列表，将在本帧末尾进行发送
        /// </summary>
        /// <param name="self"></param>
        /// <param name="cmdToSend"></param>
        /// <param name="shouldAddToPlayerInputBuffer">如果为false代表这个cmd不在预测的考虑范围里，通常用于一些重要Unit的创建，因为这些UnitId需要有服务端裁定</param>
        public static void AddCmdToSendQueue<T>(this LSF_Component self, T cmdToSend,
            bool shouldAddToPlayerInputBuffer = true) where T : ALSF_Cmd
        {
#if SERVER
            cmdToSend.Frame = self.CurrentFrame;
            self.AddCmdsToWholeCmdsBuffer(ref cmdToSend);
            
            M2C_FrameCmd m2CFrameCmd = new M2C_FrameCmd() {CmdContent = cmdToSend};

            //将消息放入待发送列表，本帧末尾进行发送
            if (self.FrameCmdsToSend.TryGetValue(self.CurrentFrame, out var queue2))
            {
                queue2.Enqueue(m2CFrameCmd.CmdContent);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToSend);
                self.FrameCmdsToSend[self.CurrentFrame] = newQueue;
            }
#else

            //客户端用户输入有他的特殊性，往往会在Update里收集输入，在FixedUpdate里进行指令发送，所以要放到下一帧
            uint correctFrame = self.CurrentFrame + 1;

            cmdToSend.Frame = correctFrame;
            C2M_FrameCmd c2MFrameCmd = new C2M_FrameCmd() {CmdContent = cmdToSend};

            if (shouldAddToPlayerInputBuffer)
            {
                //将消息放入玩家输入缓冲区，用于预测回滚
                if (self.PlayerInputCmdsBuffer.TryGetValue(correctFrame, out var queue1))
                {
                    queue1.Enqueue(c2MFrameCmd.CmdContent);
                }
                else
                {
                    Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                    newQueue.Enqueue(cmdToSend);
                    self.PlayerInputCmdsBuffer[correctFrame] = newQueue;
                }
            }

            //将消息放入待发送列表，本帧末尾进行发送
            if (self.FrameCmdsToSend.TryGetValue(correctFrame, out var queue2))
            {
                queue2.Enqueue(c2MFrameCmd.CmdContent);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToSend);
                self.FrameCmdsToSend[correctFrame] = newQueue;
            }
#endif
        }


        public static void AddCmdsToWholeCmdsBuffer<T>(this LSF_Component self, ref T cmdToSend) where T : ALSF_Cmd
        {
            cmdToSend.Frame = self.CurrentFrame;

            //将指令放入整局游戏的缓冲区，用于录像和观战系统
            if (self.WholeCmds.TryGetValue(self.CurrentFrame, out var queue))
            {
                queue.Enqueue(cmdToSend);
            }
            else
            {
                Queue<ALSF_Cmd> newQueue = new Queue<ALSF_Cmd>();
                newQueue.Enqueue(cmdToSend);
                self.WholeCmds[self.CurrentFrame] = newQueue;
            }
        }

        public static void StartFrameSync(this LSF_Component self)
        {
            self.StartSync = true;
            self.FixedUpdate = new FixedUpdate() {UpdateCallback = self.LSF_TickNormally};
        }

#if !SERVER
        public static void LSF_TickBattleView(this LSF_Component self, long deltaTime)
        {
            // LSFTick Room，tick room的相关组件, 然后由Room去Tick其子组件，即此处是战斗的Tick起点
            self.GetParent<Room>().GetComponent<LSF_TickComponent>()
                ?.TickView(deltaTime);
        }

        /// <summary>
        /// 根据消息包中服务端帧数 和 服务端的TimeSnap来计算出服务端当前帧数并且对一些字段和数据进行处理
        /// </summary>
        public static void RefreshNetInfo(this LSF_Component self, long serverTimeSnap,
            uint messageFrame)
        {
            self.ServerCurrentFrame = messageFrame + TimeAndFrameConverter.Frame_Long2Frame(
                TimeHelper.ClientNow() - serverTimeSnap);
            self.CurrentAheadOfFrame = (int) (self.CurrentFrame - self.ServerCurrentFrame);

            //Log.Info($"刷新服务端CurrentFrame成功：{self.ServerCurrentFrame} ---- {TimeHelper.ClientNow()}");
        }

        /// <summary>
        /// 检测指定帧的数据一致性，并得出结果
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static bool CheckConsistencyCompareSpecialFrame(this LSF_Component self, uint frame, ALSF_Cmd alsfCmd)
        {
            return self.GetParent<Room>().GetComponent<LSF_TickComponent>()
                .CheckConsistency(frame, alsfCmd);
        }

        /// <summary>
        /// 回滚
        /// </summary>
        /// <param name="frame"></param>
        /// <returns></returns>
        public static bool RollBack(this LSF_Component self, uint frame, ALSF_Cmd alsfCmd)
        {
            return self.GetParent<Room>().GetComponent<LSF_TickComponent>()
                .RollBack(frame, alsfCmd);
        }

        /// <summary>
        /// 在本地玩家的输入缓冲区寻找某个指令
        /// </summary>
        /// <returns></returns>
        public static bool FindCmdInPlayInputCmd(this LSF_Component self, ALSF_Cmd cmd)
        {
            if (self.PlayerInputCmdsBuffer.TryGetValue(cmd.Frame, out var queue))
            {
                return queue.Contains(cmd);
            }

            return false;
        }

        /// <summary>
        /// 客户端处理异常的网络状况
        /// </summary>
        /// <returns></returns>
        public static async ETVoid ClientHandleExceptionNet(this LSF_Component self)
        {
            // 直到上一次异常状态处理完成之前都不会处理这一次异常
            if (!self.ShouldTickInternal)
            {
                return;
            }

            // 当前客户端帧数大于服务端帧数，两种情况，
            // 1.正常情况，客户端为了保证自己的消息在合适的时间点抵达服务端需要领先于服务器
            // 2.非正常情况，客户端由于网络延迟或者断开导致没有收到服务端的帧指令，导致ServerCurrentFrame长时间没有更新，再次收到服务端回包的时候发现是很久之前包了，也就会导致CurrentAheadOfFrame变大，当达到一个阈值的时候将会进行断线重连
            if (self.CurrentFrame > self.ServerCurrentFrame)
            {
                self.CurrentAheadOfFrame = (int) (self.CurrentFrame - self.ServerCurrentFrame);

                if (self.CurrentAheadOfFrame > LSF_Component.AheadOfFrameMax)
                {
                    self.ShouldTickInternal = false;

                    Log.Error("长时间未收到服务端回包，开始断线重连，停止模拟");
                    //TODO 开始断线重连，这里假设3s后重连完成
                    await TimerComponent.Instance.WaitAsync(3000);

                    self.ShouldTickInternal = true;

                    return;
                }
            }
            else // 当前客户端帧数小于服务端帧数，是因为开局的时候由于网络延迟问题导致服务端先行于客户端，直接多次tick
            {
                self.CurrentAheadOfFrame = -(int) (self.ServerCurrentFrame - self.CurrentFrame);

                // 落后，追帧，追到目标帧
                int count = self.TargetAheadOfFrame - self.CurrentAheadOfFrame;

                while (--count >= 0)
                {
                    self.CurrentFrame++;
                    self.LSF_TickManually();
                }

                self.CurrentAheadOfFrame = self.TargetAheadOfFrame;
            }

            // Log.Info(
            //     $"-------------------CurrentAheadOfFrame: {self.CurrentAheadOfFrame} TargetAheadOfFrame: {self.TargetAheadOfFrame} ServerCurrentFrame: {self.ServerCurrentFrame}");

            if (self.CurrentAheadOfFrame != self.TargetAheadOfFrame)
            {
                //Log.Info("------------------进入变速状态");
                self.HasInSpeedChangeState = true;
                self.FixedUpdate.TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond /
                                                                        (GlobalDefine.FixedUpdateTargetFPS +
                                                                         self.TargetAheadOfFrame -
                                                                         self.CurrentAheadOfFrame
                                                                        ));
            }
            else if (self.HasInSpeedChangeState)
            {
                //Log.Info("------------------已经对齐");
                self.HasInSpeedChangeState = false;
                self.FixedUpdate.TargetElapsedTime =
                    TimeSpan.FromTicks(TimeSpan.TicksPerSecond / (GlobalDefine.FixedUpdateTargetFPS));
            }
        }
#endif
    }
}
```

好像自己以前理解错了,比较逻辑一直不一致不用比较属性,只需要比较帧信息就行了



## 客户端服务端Tick

对于服务端来说，每个玩家都要记录，而对于客户端来说，只需要记录本地玩家即可，因为只有本地玩家进行了预测

```csharp
// LSFTick Room，tick room的相关组件, 然后由Room去Tick其子组件，即此处是战斗的Tick起点
self.GetParent<Room>().GetComponent<LSF_TickComponent>()
    ?.TickStart(self.CurrentFrame, GlobalDefine.FixedUpdateTargetDTTime_Long);

// LSFTick Room，tick room的相关组件, 然后由Room去Tick其子组件，即此处是战斗的Tick起点
self.GetParent<Room>().GetComponent<LSF_TickComponent>()
    ?.Tick(self.CurrentFrame, GlobalDefine.FixedUpdateTargetDTTime_Long);

// 所有Tick结束后，一些数据收集工作，比如收集快照信息（对于服务端来说，每个玩家都要记录，而对于客户端来说，只需要记录本地玩家即可，因为只有本地玩家进行了预测）
self.GetParent<Room>().GetComponent<LSF_TickComponent>()
    ?.TickEnd(self.CurrentFrame, GlobalDefine.FixedUpdateTargetDTTime_Long);
```

这里收集脏数据也是只收集帧信息的差异

# Why状态帧

[基于行为树的MOBA技能系统：基于状态帧的战斗，技能编辑器与录像回放系统设计 | 烟雨迷离半世殇的成长之路 (lfzxb.top)](https://www.lfzxb.top/nkgmoba-framestepstate-architecture-battle-design/)

## 录像回放

帧同步的一大特点就是只需要转发玩家输入的指令就能得到最终的结果

状态帧则更复杂一些，因为我们本地没有确定性结果（没有使用定点数），所以需要将服务端计算后的结果回传给客户端，客户端才能根据这个正确的结果进行正确的表现

我们的流程是客户端发送指令给服务端，然后服务端计算后将结果传回客户端，然后客户端根据结果做表现，也就是说客户端的表现完全取决于服务端每帧回包的指令

所以对于录像，回放来说只需要不断将服务端每帧传给客户端的指令持久化下来，然后进行分发即可

------

客户端发送帧(玩家操作),服务端发送脏数据

客户端先行,操作输入都添加到localInputBuffer,本地预先表现,等到服务端下发数据比较一致性,若是不同,

再回滚,快速执行之前的逻辑,重新开始正常Tick



# 技能系统





MonkeyCommand->Setting 设置快捷键打开窗口

![image-20230129185551206](C:\Users\CodeElk\AppData\Roaming\Typora\typora-user-images\image-20230129185551206.png)

## 思路

[《守望先锋》网络脚本化的武器和技能系统 | 烟雨迷离半世殇的成长之路 (lfzxb.top)](https://www.lfzxb.top/ow-gdc-weapon-and-skillsystem/#statescript实例)



事件驱动两种方式

- 写脚本
- 行为树

或者直接在面板里ifelse,NodeCanvas的行为树用odin重绘费时,不如重新学下NodeGraph

把事件和技能的一些变量存到黑板里



## 问题

思路有了,实操还是有很多问题呀,不同技能不同的加成方式,如何优雅的实现?

不同技能有独属于自己的字段,需要再加一层抽象吗,还是都改为方法?像卡特R还有重伤属性,如何优雅的把数据与逻辑分离呢,改属性把unity与Excel分开感觉好些

赶紧看看烟雨代码,看看他怎么处理的



## 技能保存路径

![image-20230129194003818](C:\Users\CodeElk\AppData\Roaming\Typora\typora-user-images\image-20230129194003818.png)

## Why行为树

Excel缺点:逻辑跳转不好控制难以维护

Editor编辑器缺点:难以批量更改数据

观点:动态的数据用Editor,其它用Excel,逻辑用行为树

## UIElement学习

[UIElements Tutorial for Unity: Getting Started | Kodeco, the new raywenderlich.com](https://www.kodeco.com/6452218-uielements-tutorial-for-unity-getting-started#toc-anchor-009)

## NodeGraph学习

API

https://alelievr.github.io/NodeGraphProcessor/api/GraphProcessor.BaseNode.html

https://github.com/alelievr/NodeGraphProcessor/wiki/Graph-Window-API#customize-graph-view

[NodeGraphProcessor整合Odin | 烟雨迷离半世殇的成长之路 (lfzxb.top)](https://www.lfzxb.top/nodegraphprocesssor-and-odin/#graphview介绍)

[feat：重新接入Odin序列化方案 · wqaetly/NodeGraphProcessor@2a68396 (github.com)](https://github.com/wqaetly/NodeGraphProcessor/commit/2a68396239d92773e827a63c0c44efb5383cccfe)

虽然改了,但是无法直接显示在Panel上难受了,未完待续.....

### 作者视频

[Mert Kirimgeri - YouTube](https://www.youtube.com/channel/UCDK3uzTu962H75UFVWlkrJw)



## 行为树绘制

[Creating your first animated AI Character! [AI #01\] - YouTube](https://www.youtube.com/watch?v=TpQbqRNCgM0&list=PLyBYG1JGBcd009lc1ZfX9ZN5oVUW7AFVy)

Cool!!

## 重新理解序列化

[Unity - Scripting API: ISerializationCallbackReceiver (unity3d.com)](https://docs.unity3d.com/ScriptReference/ISerializationCallbackReceiver.html)

[深入Unity序列化 - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/76247383)

# 卡特技能

Katarina

## 抛物线



https://www.jianshu.com/p/ad4e00efbd45



| 技能名称                                                     | 触发     | 技能属性                                                     | 技能效果                                                     |
| ------------------------------------------------------------ | -------- | :----------------------------------------------------------- | ------------------------------------------------------------ |
| [![img](https://bkimg.cdn.bcebos.com/pic/21a4462309f790520c32d73608f3d7ca7bcbd58d?x-bce-process=image/resize,m_lfit,w_128,limit_1)](https://baike.baidu.com/pic/不祥之刃/8904820/0/21a4462309f790520c32d73608f3d7ca7bcbd58d?fr=lemma&fromModule=lemma_content-image&ct=single)[![img](https://bkimg.cdn.bcebos.com/pic/aec379310a55b31934b7a44d4aa98226cffc17bf?x-bce-process=image/resize,m_lfit,w_128,limit_1)](https://baike.baidu.com/pic/不祥之刃/8904820/0/aec379310a55b31934b7a44d4aa98226cffc17bf?fr=lemma&fromModule=lemma_content-image&ct=single)**贪婪** | **被动** | 每当一名在过去3秒被卡特琳娜所伤害的敌方英雄阵亡时，卡特琳娜的技能的冷却时间就会减少15秒。如果卡特琳娜拾起一把匕首，她会用它来斩击附近的所有敌人来造成68/72/77/82/89/96/103/112/121/131/142/ 154/166/180/194/208/224/240(基于英雄等级)(+0.75额外AD) (+0.55/0.66/0.77/0.88AP)魔法伤害并减少瞬步的冷却时间78/84/90/96%(于1/6/11/16级)。对命中的敌人施加攻击特效。匕首的斩击会对英雄们施加攻击特效。 |                                                              |
| [![img](https://bkimg.cdn.bcebos.com/pic/a2cc7cd98d1001e9eb1d8407bc0e7bec54e79791?x-bce-process=image/resize,m_lfit,w_128,limit_1)](https://baike.baidu.com/pic/不祥之刃/8904820/0/a2cc7cd98d1001e9eb1d8407bc0e7bec54e79791?fr=lemma&fromModule=lemma_content-image&ct=single)**[弹射之刃](https://baike.baidu.com/item/弹射之刃/16972767?fromModule=lemma_inlink)** | **Q**    | 冷却：11/10/9/8/7射程：625                                   | 卡特琳娜投掷一把匕首，造成75/105/135/165/195(+0.3AP)魔法伤害给目标及附近的2个敌人。匕首随后会弹落到主要目标身后的地面上。匕首总会落在首个目标的远侧350码处，并且无论弹射多少次都会在相同的时间内落地。 |
| [![img](https://bkimg.cdn.bcebos.com/pic/95eef01f3a292df5b74ea1f8b4315c6035a87349?x-bce-process=image/resize,m_lfit,w_128,limit_1)](https://baike.baidu.com/pic/不祥之刃/8904820/0/95eef01f3a292df5b74ea1f8b4315c6035a87349?fr=lemma&fromModule=lemma_content-image&ct=single)**伺机待发** | **W**    | 冷却：15/14/13/12/11范围：400                                | 卡特琳娜扔出一把匕首至空中并获得50/60/70/80/90%移动速度，该移速会在1.25秒里持续衰减。 |
| [![img](https://bkimg.cdn.bcebos.com/pic/4afbfbedab64034fcd6dcfeba7c379310b551dbe?x-bce-process=image/resize,m_lfit,w_128,limit_1)](https://baike.baidu.com/pic/不祥之刃/8904820/0/4afbfbedab64034fcd6dcfeba7c379310b551dbe?fr=lemma&fromModule=lemma_content-image&ct=single)**瞬步** | **E**    | 冷却：14/12.5/11/9.5/8距离：725                              | 卡特琳娜霎那间闪烁到目标友军、敌人、或匕首处。如果目标是敌人，那么卡特琳娜会造成15/30/45/60/75(+0.5AD)(+0.25AP)魔法伤害——如果是其它情况，那么她会对范围内距她最近的敌人造成伤害。施加攻击特效。拾取一把匕首将使【E瞬步】的冷却时间减少%。这次袭击会施加攻击特效。卡特琳娜可以闪烁到目标附近的任何位置。 |
| [![img](https://bkimg.cdn.bcebos.com/pic/63d9f2d3572c11dfd62c2f2b672762d0f703c23d?x-bce-process=image/resize,m_lfit,w_128,limit_1)](https://baike.baidu.com/pic/不祥之刃/8904820/0/63d9f2d3572c11dfd62c2f2b672762d0f703c23d?fr=lemma&fromModule=lemma_content-image&ct=single)**[死亡莲华](https://baike.baidu.com/item/死亡莲华/16823622?fromModule=lemma_inlink)** | **R**    | 冷却：90/60/45范围：550                                      | 卡特琳娜化身为一道剑刃飓风，快速朝最近的3名敌方英雄投掷匕首，每把匕首造成25/37.5/50(+0.19AP)魔法伤害和15%(+0.099攻击速度)额外攻击力物理伤害，施加25%伤害的攻击特效。总共在2.5秒里持续对每个敌方英雄造成：375/562.5/750(+2.85AP)魔法伤害和2.25(+1.485攻击速度)额外攻击力物理伤害，并施加25%伤害的攻击特效。对被伤害的所有敌方英雄造成重伤效果，使其受到的治疗效果和生命回复降低60%。 |





# GraphView

```
UnityEditor.Experimental.GraphView
```

## 参考文档

https://zhuanlan.zhihu.com/p/371171403

[Unity Editor扩展 GraphView_unityeditor.experimental.graphview_漫漫无期的博客-CSDN博客](https://blog.csdn.net/dmk17771552304/article/details/121499476)

[Unity之GraphView - 知乎 (zhihu.com)](https://zhuanlan.zhihu.com/p/442839336)



最基础的Window 绑定 GridView

绑定SearchWindow,他继承了提供的serch接口

## 主页面代码

```csharp
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace Scenes
{
    public class MyGraphWindow:EditorWindow
    {
        public static MyGraphWindow Window;
        [MenuItem("Tool/打开")]
        public static void Open()
        {
            if (Window==null)
            {
                Window = GetWindow<MyGraphWindow>();
            }
            else
            {
                Window.rootVisualElement.Clear();
            }
            Window.Show();
        }

        private void OnEnable()
        {
            //绑定视图
            rootVisualElement.Add(new MyGraphView(this)
                                  {
                                      style = { flexGrow = 1}
                                  });

        }
    }

    public class MyGraphView : GraphView
    {
        public MyGraphView(EditorWindow window)
        {
            SetupZoom(ContentZoomer.DefaultMinScale,ContentZoomer.DefaultMaxScale);
            Insert(0,new GridBackground());
            //拖拽背景
            this.AddManipulator(new ContentDragger());
            //拖拽节点
            this.AddManipulator(new SelectionDragger());
            Add(new RootNode());
            AddSearchWindow(window);
        }
        private void AddSearchWindow(EditorWindow window)
        {
            //初始搜索栏
            var searchWindow = ScriptableObject.CreateInstance<MySearchWindow>();
            searchWindow.Init(window, this);
            nodeCreationRequest += (e) =>
            {
                SearchWindow.Open(new SearchWindowContext(e.screenMousePosition), searchWindow);
            };
        }

        /// <summary>
        /// 返回连接的端口
        /// </summary>
        /// <param name="startPort"></param>
        /// <param name="nodeAdapter"></param>
        /// <returns></returns>
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
        {
            var compatiblePorts=new List<Port>();
            foreach (var port in ports.ToList())
            {
                if (startPort.node ==port.node||
                    startPort.direction == port.direction||
                    startPort.portType!= port.portType)
                {
                    continue;
                }
                compatiblePorts.Add(port);
            }

            return compatiblePorts;
        }

    }
}
```

## 兼容Odin



参考这位的代码

https://github.com/taco970123/TreeDesigner



核心代码

默认构造方法是一个OnGUI的绘制方法

 IMGUIContainer container = new IMGUIContainer(() =>
            {
                if (editor.target != null)
                    editor.OnInspectorGUI();
            });

```csharp

  public class ShowScriptObjectNode : MyNode
    {
        private UnityEditor.Editor editor;
        public override void OnCreated(MyGraphView view)
        {
            UnityEngine.Object.DestroyImmediate(editor);
            var tree=AssetDatabase.LoadAssetAtPath<BehaviourTree>("Assets/TheKiwiCoder/BehaviourTree/Example/BehaviourTree_RandomWalk.asset");
            editor = UnityEditor.Editor.CreateEditor(tree);
            IMGUIContainer container = new IMGUIContainer(() =>
            {
                if (editor.target != null)
                    editor.OnInspectorGUI();
            });
            Add(container);
        }
    }
```

这样就可以把ScriptObject绘制上去了,Perfect!

![image-20230202165337435](C:\Users\CodeElk\AppData\Roaming\Typora\typora-user-images\image-20230202165337435.png)



## 打开资源的回调

```csharp
[OnOpenAsset]
public static bool OnOpenAsset(int instanceId, int line) {
    if (Selection.activeObject is BehaviourTree) {
        OpenWindow(Selection.activeObject as BehaviourTree);
        return true;
    }
    return false;
}
```

终于搞好了,明天就加事件



# ScriptObject的坑

一个类单独放一个文件

如果变量中有List这样的集合也是SO类型的,ScriptObject一运行就丢失,不能直接ScriptableObject.CreateInstance<T>();,还要保存文件的形式

这个以前一直不明白

![image-20230203204814970](C:\Users\CodeElk\AppData\Roaming\Typora\typora-user-images\image-20230203204814970.png)

