# ZoDream.KeyboardSimulator
 按键录制与回放

 ## 说明

 整合 [globalmousekeyhook](https://github.com/gmamaladze/globalmousekeyhook) 和 [InputSimulator](https://github.com/michaelnoonan/inputsimulator)  代码，基于`netstandard2.1`，并统一使用按键枚举，方便 `Net Core WPF` 调用

 ## 预览

 ![ZoDream.KeyboardSimulator](screen/1.jpg)

 ## 已完成功能

1. if 拾取
2. 录制
3. 模拟执行

## 示例

```c#
fn test // 定义方法
DoubleClick(Right) //双击鼠标右键
2000  // 延时2s
Click() //点击左键
 // 方法结束
1000 //延迟1s
Delay(1000) //延迟1s
Move(20,20) // 鼠标移动到点(20,20)
Click()   //点击左键
Move(0,0,2000)   // 鼠标从当前坐标用2s匀速移动到点(0,0)
if 0,20,40,60=md5  // 判断点(0,20)到(40,60)的直线路径上的颜色值是否为，请通过拾取按钮自动框选获取
    test()        // 为True则执行 定义的方法test
endif

```
