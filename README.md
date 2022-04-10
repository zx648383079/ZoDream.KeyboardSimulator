# ZoDream.KeyboardSimulator
 按键录制与回放

 ## 说明

 整合 [globalmousekeyhook](https://github.com/gmamaladze/globalmousekeyhook) 和 [InputSimulator](https://github.com/michaelnoonan/inputsimulator)  代码，基于`netstandard2.1`，并统一使用按键枚举，方便 `Net Core WPF` 调用

 ## 预览

 ![ZoDream.KeyboardSimulator](screen/1.jpg)

 ## 已完成功能

1. 拾取
2. 录制
3. 模拟执行
4. `ctrl + O` 或拖拽文件进行打开脚本文件
5. `ctrl + s` 进行脚本保存
6. 基于`Lua`实现脚本功能


## 示例

```lua

local hwn = FindWindow("UnityWndClass", "Game")
FocusWindow(hwn)
Delay(100)
local rect = GetClientRect(hwn)
SetBasePosition(rect[0], rect[1])
MoveTo(1111, 801)
Delay(1000)
if (!IsPixelColor(0, 0, "ffffff"))
then
    Click()
end
```

## 语法规则

请参考 [`lua`](http://www.lua.org/) 文档

|添加方法|说明|示例|
|:--|:--|:--|
|FindWindow(string className, string windowName): IntPtr|获取窗口句柄||
|FocusWindow(IntPtr hwnd)|激活窗口|
|GetWindowRect(IntPtr hwnd): int[left,top,width,height]|获取窗口的坐标，包括标题栏及外边框|
|GetClientRect(IntPtr hwnd): int[left,top,width,height]|获取窗口的坐标,不包括标题栏及外边框|
|SetBasePosition(int x, int y)|设置全局坐标，影响其他方法的 `x、y`|
|MoveTo(int x, int y)|移动鼠标到，受全局坐标影响|
|Move(int x, int y)|同上|
|MoveTween(int x, int y, int step)|慢慢移动到|
|Click(int count)|左击几次鼠标||
|Delay(int milliseconds)|延迟多少毫秒|
|MouseDown(string button)|按下鼠标, 默认`Left`，可选值`Left,Right,Middle,XButton1,XButton2`||
|MouseUp(string button)|按下鼠标||
|Scroll(int offset)|滚动滑轮|`Scroll(10)`|
|HotKey(...string[] keys)|输入组合键，支持0x或数字或按键名，数字为`VK`|`HotKey(0xA2,0x41)` 等于`HotKey(LeftCtrl,A)` |
|Input(string key)|输入按键，数字为直接作为 `scancode` 输入|`Input(A)` 等于 `Input(0x30)`|
|KeyDown(string key)|按下键，数字为直接作为 `scancode` 输入||
|KeyUp(string key)|释放键，数字为直接作为 `scancode` 输入||
|GetPixelColor(int x, int y)|获取某一点的颜色值|`GetPixelColor(0,0) => ff00cc`
|IsPixelColor(int x, int y, string color)|判断某一点的颜色值是否是|`IsPixelColor(0,0, "ff00cc"`|