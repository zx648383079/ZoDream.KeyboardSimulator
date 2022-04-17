
---获取窗口句柄
---@param className string
---@param windowName string
---@return number
function FindWindow(className, windowName)

end

---激活窗口
---@param hwnd number
function FocusWindow(hwnd)

end

---获取窗口的坐标，包括标题栏及外边框
---@param hwnd number
---@return number[] #[left,top,width,height]
function GetWindowRect(hwnd)

end

---获取窗口的坐标,不包括标题栏及外边框
---@param hwnd number
---@return number[] #[left,top,width,height]
function GetClientRect(hwnd)

end
---设置全局坐标,影响其他方法的x, y
---@param x number
---@param y number
function SetBasePosition(x, y)

end
---移动鼠标到
---@param x number
---@param y number
function MoveTo(x, y)

end
---移动鼠标到
---@param x number
---@param y number
function Move(x, y)

end
---移动鼠标到
---@param x number
---@param y number
---@param time number #使用多少毫秒从当前位置移动到xy
function MoveTween(x, y, time)

end
---点击鼠标左键
function Click()

end
---左击几次鼠标
---@param count number
function Click(count)

end
---点击按键多少次
---@param button string #可选值Left,Right,Middle,XButton1,XButton2
---@param count number
function Click(button, count)

end

---延迟多少毫秒
---@param milliseconds number
function Delay(milliseconds)

end
---按下鼠标
---@param button string
function MouseDown(button)

end
---释放鼠标
---@param button string
function MouseUp(button)

end
---滚动滑轮
---@param offset number # 正数向上，负数向下
function Scroll(offset)

end
---输入组合键,支持0x或数字或按键名
---@param keys string[]
function HotKey(keys)

end
---输入按键
---@param key string
function Input(key)

end
---输入按键
---@param key string
---@param count number
function Input(key, count)

end
---输入按键
---@param key string
function KeyPress(key)

end

---输入按键
---@param key string
---@param count number
function KeyPress(key, count)

end

---按下键
---@param key string
function KeyDown(key)

end
---释放键
---@param key string
function KeyUp(key)

end
---获取某一点的颜色值
---@param x number
---@param y number
---@return string #大写，示例 FF00CC
function GetPixelColor(x, y)

end
---判断某一点的颜色值是否是
---@param x number
---@param y number
---@param color string #支持大小写及带# 示例 FFFFFF/#FFFFFF/ffffff
---@return boolean
function IsPixelColor(x, y, color)

end

---判断某个区域是否是这个颜色值，
---@param x number
---@param y number
---@param endX number
---@param endY number
---@param color string # 请使用自带拾取工具获取
---@return boolean
function IsRectColor(x, y, endX, endY, color)

end

---比较颜色深度范围
---@param color string
---@param min string? 颜色深例如 #000000
---@param max string? 颜色浅例如 #ffffff
---@return boolean
function InColor(color, min, max)

end

---调试输出,输出到日志中
---@param msg any
function Log(msg)

end