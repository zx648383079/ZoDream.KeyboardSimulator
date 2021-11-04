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
4. `ctrl + O` 或拖拽文件进行打开脚本文件
5. `ctrl + s` 进行脚本保存

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

## 语法规则

|关键字|说明|示例|
|:--:|:--|:--|
|fn|声明方法，以空行结束|`fn name`|
|if|条件判断，目前只支持两点间颜色判断|`if x,y,endX,endY=hash`|
|else||
|endif||
|Delay|延迟,纯数字行也会执行|
|Move(x,y[,duration])|移动鼠标,duration为移动时间||
|MouseDown|按下鼠标, 默认`Left`，可选值`Left,Right,Middle,XButton1,XButton2`||
|MouseUp|按下鼠标||
|Click|单击鼠标||
|DoubleClick|双击鼠标||
|Scroll|滚动滑轮|`Scroll(10)`|
|Input|输入按键||
|HotKey|输入组合键，支持0x或数字或按键名|`HotKey(0x29, A, LeftCtrl)`|
|KeyDown|按下键||
|KeyUp|释放键||
|//|只支持单行注释，忽略||