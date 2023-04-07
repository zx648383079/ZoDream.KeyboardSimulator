-- 建议在 1960*1080分辨率下设置 1600*900 非全屏模式，并关闭自动开始
local hwn = FindWindow("Engine", "Bubbles (DEBUG)")
FocusWindow(hwn)
Delay(100)
local rect = GetClientRect(hwn)
SetBasePosition(rect[0], rect[1])

local MapOffsetX = 353
local MapOffsetY = 261
local luckyMode = 0 -- 抽取哪个奖品 0 随机，1-4 固定

---判断是否是有奖励的关卡，
---@param x number
---@param y number
---@return boolean
function IsLuckyLevel(x, y)
	-- 有奖励的边框的颜色
	local color = GetPixelColor(x, y)
	-- 602E10  502111 5D2C10
	return string.find(color, "6") == 1 or string.find(color, "5") == 1
end

---判断是否进入抽奖页面
---@return boolean
function IsLuckyScene()
	return IsPixelColor(686,47,"BC310A")
end

---难度选取
---@param diffIndex number #0-2
---@param diffNum number #0-2
function ChangeDiff(diffIndex, diffNum)
	local DiffOffsetX = 277
	Delay(1000)
    MoveTo(diffIndex * DiffOffsetX + 519, 464)
    Delay(500)
    Click(2)
    Delay(500)
    if diffNum < 1
    then
        -- 选取标准模式
        MoveTo(530, 488)
	elseif diffNum == 1
    then
        -- 选取极难模式
        MoveTo(1070, 616)
	elseif diffNum > 1
    then
        -- 选取点击模式
        MoveTo(1337, 616)
    end
    Delay(1000)
    Click(2)
    Delay(1000)
	while IsPixelColor(56,15, "DA2A00") == false
	do
		Delay(1000)
	end
end

function DiffEasy()
    ChangeDiff(0,0)
end

function DiffDeflation()
    ChangeDiff(1,0)
end

function DiffHard()
    ChangeDiff(2,0)
end


-- 判断当前是否是失败页面
function GameIsLose()
    return IsPixelColor(727, 312, "001FFF")
end

-- 判断游戏是否结束
function GameIsFinish()
    if IsPixelColor(773, 353, "FFEB00")
    then
        return true
    end
    if IsPixelColor(773, 353, "FFEB00")
    then
        return true
    end
    return false
end

-- 当前是否是游戏结束界面
function GameIsEnd()
    if GameIsLose()
    then
        return true
    end
    if GameIsFinish()
    then
        return true
    end
    return false
end

-- 游戏结束，进入首页重新开始
function GameOver()
	Delay(1000)
    if GameIsLose()
    then
        MoveTo(476, 680)
        Delay(100)
        Click()
        Delay(3000)
        Log("游戏失败了！")
        Main();
    end
    -- 胜利的统计数据
    if IsPixelColor(775, 124, "FFEB00")
    then
        MoveTo(793, 757)
        Delay(100)
        Click()
        Delay(1000)
    end
    -- 胜利的返回首页
    if IsPixelColor(773, 353, "FFEB00")
    then
        MoveTo(582, 704)
        Delay(100)
        Click()
        Delay(1000)
        Log("游戏完成！")
        IsLucky()
        Main()
    end
end

--- 判断是否是抽奖页面
function IsLucky()
    -- local GiftOffsetX = 253
    Delay(3000)
    if IsLuckyScene() == false
    then
        return
    end

	local bgColor = "000000"
	if luckyMode > 0
	then
		-- 104, 93
		local luckyIndex = luckyMode - 1
		MoveTo(1195 + luckyIndex % 2 * 104, 340 + luckyIndex // 2 * 93)
		Click()
		Delay(200)
	end
	MoveTo(800,563)
	Click()
	Delay(2000)
	for i = 422, 1179, 126
	do
		if IsPixelColor(i,452, bgColor) == false
		then
			MoveTo(i,452)
			Delay(100)
			Click()
			Delay(2000)
			Click()
			Delay(1000)
		end
		if IsPixelColor(800,837, bgColor) == false
		then
			break
		end
	end
	-- 确认按钮
	if IsPixelColor(800,837, bgColor) == false
	then
		MoveTo(800,837)
		Click(1)
		Delay(1000)
		Input("Esc") --go home
	end
end

---关闭面板
function ClosePanel()
	Delay(200)
    Input("Esc")
	Delay(300)
end

---地图选取
---@param mapIndex number #0-5
function ChangeMap(mapIndex)
    MoveTo(mapIndex % 3 * MapOffsetX + 443, mapIndex // 3 * MapOffsetY + 217)
    Delay(200)
    Click()
    Delay(1000)
end

---英雄选取
---@param index number 0 - 13
function ChangeHero(index)
    local HeroOffsetX = 126
    local HeroOffsetY = 160
    MoveTo(96,821)
    Click()
    Delay(200)

    MoveTo(80 + index % 3 * HeroOffsetX, 184 + index // 3 * HeroOffsetY)
    Delay(200)
    Click(2)
    MoveTo(926,516)
    Delay(500)
    Click()
    Delay(500)
    Input("Esc")
    Delay(1000)
end

--- 默认英雄
function HeroDefault()
    ChangeHero(12)
end


---放置
---@param key string # 按键
---@param x number # x
---@param y number # y
function DrogTower(key, x, y)
    Delay(300)
    Input(key)
    Delay(500)
    MoveTo(x, y)
    Delay(500)
    Click()
    Delay(200)
end

---卖掉
---@param x number
---@param y number
function SellTower(x, y)
	MoveTo(x, y)
    Delay(300)
    Click()
	Delay(500)
	Input("Backspace")
	Delay(200)
end

---升级技能
---@param skill number #技能 1-3
---@param count number #次数
function UpgradeSkill(skill, count)
	if count < 1
	then
		count = 1
	end
    for i = 1, count, 1 do
		if skill < 2
		then
			Input(",")
		elseif skill == 2
		then
			Input(".")
		else
			Input("/")
		end
		Delay(200)
	end
end
---点击并升级
---@param skill number
---@param count number
function OpenUpgradeSkill(skill, count)
	Delay(200)
	Click()
	Delay(400)
	UpgradeSkill(skill, count)
end
---切换攻击目标
---@param count number
function SwitchTarget(count)
	if count < 1
	then
		count = 1
	end
	Delay(100)
	Input("Tab", count)
	Delay(200)
end
---点击并切换攻击目标
---@param count number
function OpenSwitchTarget(count)
	Delay(200)
	Click()
	Delay(400)
	SwitchTarget(count)
end

---升级
---@param skill number #1-3
---@param count number
---@param x number
---@param y number
---@param close boolean
function UpgradeTower(x, y, skill, count, close)
    MoveTo(x, y)
    OpenUpgradeSkill(skill, count)
    if close == true
    then
        ClosePanel()
    end
end

---升级技能
---@param x number
---@param y number
---@param skills number[]
---@param close boolean
function UpgradeTowerSkill(x, y, skills, close)
    MoveTo(x, y)
	Delay(100)
    Click()
    Delay(400)
    for _, skill in pairs(skills)
    do
        UpgradeSkill(skill)
    end
    if close == true
    then
        ClosePanel()
    end
end

---等待一关结束
function WaitGradeEnd()
    while IsPixelColor(1507,845,"FFFFFF")
    do
        Delay(1000)
        if GameIsEnd()
        then
            return
        end
    end
end

---设置移动速度
---@param speed number #0|1
function StartGrade(speed)
    Delay(100)
    if IsPixelColor(1507,845,"FFFFFF") == false
    then
        Input("Space")
        Delay(200)
    end
    if IsPixelColor(1514,832,"62CA01")
    then
        if speed > 0
        then
            Input("Space")
        end
    elseif speed < 1
    then
        Input("Space")
    end
end

--- 跳过多少关
---@param count number
function JumpGrade(count)
    for i = 1, count, 1 
    do
        StartGrade(1)
        Delay(1000)
        WaitGradeEnd()
        if GameIsEnd()
        then
            count = 0
            GameOver()
        end
    end
end

--- 一个关卡脚本
---@see 1.0
function GameLevel(debug)
	if debug ~= true
	then
		HeroDefault()
		ChangeMap(0)
		DiffHard()
	end

    DrogTower("Q", 210,203)
	OpenSwitchTarget()
	DrogTower("Q", 1065,350)
	OpenSwitchTarget()

	UpgradeTower(631,768,2,1,true)
	
	JumpGrade(6)
	
	UpgradeTower(631,768,2)
	SwitchTarget()
	ClosePanel()
	
	JumpGrade(1)
	
	StartGrade(1)
	Delay(1000)
	Input("4")
	WaitGradeEnd()
	GameOver()
end

---选择关卡
function ChooseLevel()
    local DotColor = "409FFF"

    local DotX = 630 + 31 * 10
    local DotX2 = DotX + 31
    local DotY = 631
    local ColumnX1 = 592
    local ColumnX2 = ColumnX1 + MapOffsetX
    local ColumnX3 = ColumnX2 + MapOffsetX
    local RowY1 = 229
    local RowY2 = RowY1 + MapOffsetY

    MoveTo(1111, 801)
    Delay(1000)
    local flag = 0
    local loopCount = 0
    while flag == 0
    do
        loopCount = loopCount + 1
        if loopCount > 4
        then
            Log("未找到含奖励的地图")
            break
        end
        Click()
        Delay(1000)
        if IsPixelColor(DotX, DotY, DotColor)
        then
            if IsLuckyLevel(ColumnX1, RowY1)
            then
                SanctuaryLevel()
                flag = 1
            elseif IsLuckyLevel(ColumnX2,RowY1)
            then
                RavineLevel()
                flag = 1
            elseif IsLuckyLevel(ColumnX3,RowY1)
            then
                FloodyValleyLevel()
                flag = 1
            elseif IsLuckyLevel(ColumnX1,RowY2)
            then
                InfernalLevel()
                flag = 1
            elseif IsLuckyLevel(ColumnX2,RowY2)
            then
                BloodyPuddlesLevel()
                flag = 1
            elseif IsLuckyLevel(ColumnX3,RowY2)
            then
                WorkshopLevel()
                flag = 1
            end
        elseif IsPixelColor(DotX2, DotY, DotColor)
        then
            if IsLuckyLevel(ColumnX1, RowY1)
            then
                QuadLevel()
                flag = 1
            elseif IsLuckyLevel(ColumnX2,RowY1)
            then
                DarkCastleLevel()
                flag = 1
            elseif IsLuckyLevel(ColumnX3,RowY1)
            then
                MuddyPuddlesLevel()
                flag = 1
            elseif IsLuckyLevel(ColumnX1,RowY2)
            then
                OuchLevel()
                flag = 1
            end
        end
    end
	GameOver()
end

---主入口
function Main()
    MoveTo(699, 785)
    Delay(200)
    Click()
    Delay(500)
    ChooseLevel()
end

Main()