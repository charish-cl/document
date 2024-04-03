https://blog.csdn.net/final5788/article/details/130533255

```cs
public string ButtonName = "Dynamic button name";

public bool Toggle;

[Button("$ButtonName")]
private void DefaultSizedButton()
{
    this.Toggle = !this.Toggle;
}

[Button("@\"Expression label: \" + DateTime.Now.ToString(\"HH:mm:ss\")")]
public void ExpressionLabel()
{
    this.Toggle = !this.Toggle;
}

[Button("Name of button")]
private void NamedButton()
{
    this.Toggle = !this.Toggle;
}

[Button(ButtonSizes.Small)]
private void SmallButton()
{
    this.Toggle = !this.Toggle;
}

[Button(ButtonSizes.Medium)]
private void MediumSizedButton()
{
    this.Toggle = !this.Toggle;
}

[DisableIf("Toggle")]
[HorizontalGroup("Split", 0.5f)]
[Button(ButtonSizes.Large), GUIColor(0.4f, 0.8f, 1)]
private void FanzyButton1()
{
    this.Toggle = !this.Toggle;
}

[HideIf("Toggle")]
[VerticalGroup("Split/right")]
[Button(ButtonSizes.Large), GUIColor(0, 1, 0)]
private void FanzyButton2()
{
    this.Toggle = !this.Toggle;
}

[ShowIf("Toggle")]
[VerticalGroup("Split/right")]
[Button(ButtonSizes.Large), GUIColor(1, 0.2f, 0)]
private void FanzyButton3()
{
    this.Toggle = !this.Toggle;
}

[Button(ButtonSizes.Gigantic)]
private void GiganticButton()
{
    this.Toggle = !this.Toggle;
}

[Button(90)]
private void CustomSizedButton()
{
    this.Toggle = !this.Toggle;
}

[Button(Icon = SdfIconType.Dice1Fill, IconAlignment = IconAlignment.LeftOfText)]
private void IconButton01()
{
    this.Toggle = !this.Toggle;
}

[Button(Icon = SdfIconType.Dice2Fill, IconAlignment = IconAlignment.LeftOfText)]
private void IconButton02()
{
    this.Toggle = !this.Toggle;
}

```

```
curl -X POST -H "Content-Type: application/json" -d '{"msg_type":"post","content": {"post": {"zh_cn": {"title": "打包结果通知","content": [[{"tag": "text","text": "项目名称：***  \n构建编号：第1次构建 \n构建日期：2024.2.24 19:23:34 "}]]} } }}' https://open.feishu.cn/open-apis/bot/v2/hook/92eee745-a0a7-455f-9a84-60d69481fc4f
```

```
curl -X POST -H "Content-Type: application/json" -d Requstjson.json https://open.feishu.cn/open-apis/bot/v2/hook/92eee745-a0a7-455f-9a84-60d69481fc4f 
```





```csharp
import os
import sys
import time
 
# 设置你本地的Unity安装目录
unity_exe = '/Applications/Unity/Hub/Editor/2022.3.17f1c1/Unity.app'
# unity工程目录，当前脚本放在unity工程根目录中
project_path = '/Users/nightq.mini/Documents/GitHub/MonopolyRush'
# 日志
log_file = os.getcwd() + '/unity_log.log'
 
static_func = 'BuildToolChain.BuildTool.OneKeyBuild'
 
# 杀掉unity进程
def kill_unity():
    os.system('taskkill /IM Unity.exe /F')
 
def clear_log():
    if os.path.exists(log_file):
        os.remove(log_file)
 
# 调用unity中我们封装的静态函数
def call_unity_static_func(func):
    kill_unity()
    time.sleep(1)
    clear_log()
    time.sleep(1)
    cmd = 'start %s -quit -batchmode -projectPath %s -logFile %s -executeMethod %s --productName:%s --version:%s'%(unity_exe,project_path,log_file,func, sys.argv[1], sys.argv[2])
    print('run cmd:  ' + cmd)
    os.system(cmd)
 
    
 
# 实时监测unity的log, 参数target_log是我们要监测的目标log, 如果检测到了, 则跳出while循环    
def monitor_unity_log(target_log):
    pos = 0
    while True:
        if os.path.exists(log_file):
            break
        else:
            time.sleep(0.1) 
    while True:
        fd = open(log_file, 'r', encoding='utf-8')
        if 0 != pos:
            fd.seek(pos, 0)
        while True:
            line = fd.readline()
            pos = pos + len(line)
            if target_log in line:
                print(u'监测到unity输出了目标log: ' + target_log)
                fd.close()
                return
            if line.strip():
                print(line)
            else:
                break
        fd.close()
 
if __name__ == '__main__':
    call_unity_static_func(static_func)
    monitor_unity_log('Build App Done!')
    print('done')

```

## 修改工作空间

https://www.cnblogs.com/li150dan/p/16141522.html
