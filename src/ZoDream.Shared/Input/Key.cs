using System;
using System.Collections.Generic;
using System.Text;

namespace ZoDream.Shared.Input
{
    public enum Key
    {

        //
        // 摘要:
        //     No key pressed.
        None = 0x0,
        KeyCode = 0xFFFF,
        Modifiers = -65536,
        Button = 0x1,
        RButton = 0x2,
        MButton = 0x4,
        XButton1 = 0x5,
        XButton2 = 0x6,
        //
        // 摘要:
        //     The Cancel key.
        Cancel = 0x3,
        //
        // 摘要:
        //     The Backspace key.
        Back = 0x8,
        //
        // 摘要:
        //     The Tab key.
        Tab = 0x9,
        //
        // 摘要:
        //     The Linefeed key.
        LineFeed = 0xA,
        //
        // 摘要:
        //     The Clear key.
        Clear = 0xC,
        //
        // 摘要:
        //     The Enter key.
        Enter = 0xD,
        //
        // 摘要:
        //     The Return key.
        Return = 0xD,
        //
        // 摘要:
        //     The Pause key.
        Pause = 0x13,
        //
        // 摘要:
        //     The Caps Lock key.
        Capital = 0x14,
        //
        // 摘要:
        //     The Caps Lock key.
        CapsLock = 0x14,
        //
        // 摘要:
        //     The IME Hangul mode key.
        HangulMode = 0x15,
        //
        // 摘要:
        //     The IME Kana mode key.
        KanaMode = 0x15,
        //
        // 摘要:
        //     The IME Junja mode key.
        JunjaMode = 0x17,
        //
        // 摘要:
        //     The IME Final mode key.
        FinalMode = 0x18,
        //
        // 摘要:
        //     The IME Hanja mode key.
        HanjaMode = 0x19,
        //
        // 摘要:
        //     The IME Kanji mode key.
        KanjiMode = 0x19,
        //
        // 摘要:
        //     The ESC key.
        Escape = 0x1B,
        //
        // 摘要:
        //     The IME Convert key.
        ImeConvert = 0x1C,
        //
        // 摘要:
        //     The IME NonConvert key.
        ImeNonConvert = 0x1D,
        //
        // 摘要:
        //     The IME Accept key.
        ImeAccept = 0x1E,
        //
        // 摘要:
        //     The IME Mode change request.
        ImeModeChange = 0x1F,
        //
        // 摘要:
        //     The Spacebar key.
        Space = 0x20,
        //
        // 摘要:
        //     The Page Up key.
        PageUp = 0x21,
        //
        // 摘要:
        //     The Page Up key.
        Prior = 0x21,
        //
        // 摘要:
        //     The Page Down key.
        Next = 0x22,
        //
        // 摘要:
        //     The Page Down key.
        PageDown = 0x22,
        //
        // 摘要:
        //     The End key.
        End = 0x23,
        //
        // 摘要:
        //     The Home key.
        Home = 0x24,
        //
        // 摘要:
        //     The Left Arrow key.
        Left = 0x25,
        //
        // 摘要:
        //     The Up Arrow key.
        Up = 0x26,
        //
        // 摘要:
        //     The Right Arrow key.
        Right = 0x27,
        //
        // 摘要:
        //     The Down Arrow key.
        Down = 0x28,
        //
        // 摘要:
        //     The Select key.
        Select = 0x29,
        //
        // 摘要:
        //     The Print key.
        Print = 0x2A,
        //
        // 摘要:
        //     The Execute key.
        Execute = 0x2B,
        //
        // 摘要:
        //     The Print Screen key.
        PrintScreen = 0x2C,
        //
        // 摘要:
        //     The Print Screen key.
        Snapshot = 0x2C,
        //
        // 摘要:
        //     The Insert key.
        Insert = 0x2D,
        //
        // 摘要:
        //     The Delete key.
        Delete = 0x2E,
        //
        // 摘要:
        //     The Help key.
        Help = 0x2F,
        //
        // 摘要:
        //     The 0 (zero) key.
        D0 = 0x30,
        //
        // 摘要:
        //     The 1 (one) key.
        D1 = 0x31,
        //
        // 摘要:
        //     The 2 key.
        D2 = 0x32,
        //
        // 摘要:
        //     The 3 key.
        D3 = 0x33,
        //
        // 摘要:
        //     The 4 key.
        D4 = 0x34,
        //
        // 摘要:
        //     The 5 key.
        D5 = 0x35,
        //
        // 摘要:
        //     The 6 key.
        D6 = 0x36,
        //
        // 摘要:
        //     The 7 key.
        D7 = 0x37,
        //
        // 摘要:
        //     The 8 key.
        D8 = 0x38,
        //
        // 摘要:
        //     The 9 key.
        D9 = 0x39,
        //
        // 摘要:
        //     The A key.
        A = 0x41,
        B = 0x42,
        C = 0x43,
        D = 0x44,
        E = 0x45,
        F = 0x46,
        G = 0x47,
        H = 0x48,
        I = 0x49,
        J = 0x4A,
        K = 0x4B,
        L = 0x4C,
        M = 0x4D,
        N = 0x4E,
        O = 0x4F,
        P = 0x50,
        Q = 0x51,
        R = 0x52,
        S = 0x53,
        T = 0x54,
        U = 0x55,
        V = 0x56,
        W = 0x57,
        X = 0x58,
        Y = 0x59,
        Z = 0x5A,
        //
        // 摘要:
        //     The left Windows logo key (Microsoft Natural Keyboard).
        LWin = 0x5B,
        //
        // 摘要:
        //     The right Windows logo key (Microsoft Natural Keyboard).
        RWin = 0x5C,
        //
        // 摘要:
        //     The Application key (Microsoft Natural Keyboard). Also known as the Menu key,
        //     as it displays an application-specific context menu.
        Apps = 0x5D,
        //
        // 摘要:
        //     The Computer Sleep key.
        Sleep = 0x5F,
        //
        // 摘要:
        //     The 0 key on the numeric keypad.
        NumPad0 = 0x60,
        NumPad1 = 0x61,
        NumPad2 = 0x62,
        NumPad3 = 0x63,
        NumPad4 = 0x64,
        NumPad5 = 0x65,
        NumPad6 = 0x66,
        NumPad7 = 0x67,
        NumPad8 = 0x68,
        NumPad9 = 0x69,
        //
        // 摘要:
        //     The Multiply key.
        Multiply = 0x6A,
        //
        // 摘要:
        //     The Add key.
        Add = 0x6B,
        //
        // 摘要:
        //     The Separator key.
        Separator = 0x6C,
        //
        // 摘要:
        //     The Subtract key.
        Subtract = 0x6D,
        //
        // 摘要:
        //     The Decimal key.
        Decimal = 0x6E,
        //
        // 摘要:
        //     The Divide key.
        Divide = 0x6F,
        //
        // 摘要:
        //     The F1 key.
        F1 = 0x70,
        F2 = 0x71,
        F3 = 0x72,
        F4 = 0x73,
        F5 = 0x74,
        F6 = 0x75,
        F7 = 0x76,
        F8 = 0x77,
        F9 = 0x78,
        F10 = 0x79,
        F11 = 0x7A,
        F12 = 0x7B,
        F13 = 0x7C,
        F14 = 0x7D,
        F15 = 0x7E,
        F16 = 0x7F,
        F17 = 0x80,
        F18 = 0x81,
        F19 = 0x82,
        F20 = 0x83,
        F21 = 0x84,
        F22 = 0x85,
        F23 = 0x86,
        F24 = 0x87,
        //
        // 摘要:
        //     The Num Lock key.
        NumLock = 0x90,
        //
        // 摘要:
        //     The Scroll Lock key.
        Scroll = 0x91,
        //
        // 摘要:
        //     The left Shift key.
        LeftShift = 0xA0,
        //
        // 摘要:
        //     The right Shift key.
        RightShift = 0xA1,
        //
        // 摘要:
        //     The left CTRL key.
        LeftCtrl = 0xA2,
        //
        // 摘要:
        //     The right CTRL key.
        RightCtrl = 0xA3,
        //
        // 摘要:
        //     The left ALT key.
        LeftAlt = 0xA4,
        //
        // 摘要:
        //     The right ALT key.
        RightAlt = 0xA5,
        //
        // 摘要:
        //     The Browser Back key.
        BrowserBack = 0xA6,
        //
        // 摘要:
        //     The Browser Forward key.
        BrowserForward = 0xA7,
        //
        // 摘要:
        //     The Browser Refresh key.
        BrowserRefresh = 0xA8,
        //
        // 摘要:
        //     The Browser Stop key.
        BrowserStop = 0xA9,
        //
        // 摘要:
        //     The Browser Search key.
        BrowserSearch = 0xAA,
        //
        // 摘要:
        //     The Browser Favorites key.
        BrowserFavorites = 0xAB,
        //
        // 摘要:
        //     The Browser Home key.
        BrowserHome = 0xAC,
        //
        // 摘要:
        //     The Volume Mute key.
        VolumeMute = 0xAD,
        //
        // 摘要:
        //     The Volume Down key.
        VolumeDown = 0xAE,
        //
        // 摘要:
        //     The Volume Up key.
        VolumeUp = 0xAF,
        MediaNextTrack = 0xB0,
        MediaPreviousTrack = 0xB1,
        MediaStop = 0xB2,
        MediaPlayPause = 0xB3,
        LaunchMail = 0xB4,
        SelectMedia = 0xB5,
        LaunchApplication1 = 0xB6,
        LaunchApplication2 = 0xB7,
        OemSemicolon = 0xBA,
        //
        // 摘要:
        //     The OEM 1 key.
        Oem1 = 0xBA,
        //
        // 摘要:
        //     The OEM Addition key.
        OemPlus = 0xBB,
        //
        // 摘要:
        //     The OEM Comma key.
        OemComma = 0xBC,
        //
        // 摘要:
        //     The OEM Minus key.
        OemMinus = 0xBD,
        //
        // 摘要:
        //     The OEM Period key.
        OemPeriod = 0xBE,
        //
        // 摘要:
        //     The OEM 2 key.
        Oem2 = 0xBF,
        //
        // 摘要:
        //     The OEM Question key.
        OemQuestion = 0xBF,
        //
        // 摘要:
        //     The OEM 3 key.
        Oem3 = 0xC0,
        //
        // 摘要:
        //     The OEM Tilde key.
        OemTilde = 0xC0,
        //
        // 摘要:
        //     The ABNT_C1 (Brazilian) key.
        AbntC1 = 10001,
        //
        // 摘要:
        //     The ABNT_C2 (Brazilian) key.
        AbntC2 = 10002,
        //
        // 摘要:
        //     The OEM 4 key.
        Oem4 = 0xDB,
        //
        // 摘要:
        //     The OEM Open Brackets key.
        OemOpenBrackets = 0xDB,
        //
        // 摘要:
        //     The OEM 5 key.
        Oem5 = 0xDC,
        //
        // 摘要:
        //     The OEM Pipe key.
        OemPipe = 0xDC,
        //
        // 摘要:
        //     The OEM 6 key.
        Oem6 = 0xDD,
        //
        // 摘要:
        //     The OEM Close Brackets key.
        OemCloseBrackets = 0xDD,
        //
        // 摘要:
        //     The OEM 7 key.
        Oem7 = 0xDE,
        //
        // 摘要:
        //     The OEM Quotes key.
        OemQuotes = 0xDE,
        //
        // 摘要:
        //     The OEM 8 key.
        Oem8 = 0xDF,
        //
        // 摘要:
        //     The OEM 102 key.
        Oem102 = 0xE2,
        //
        // 摘要:
        //     The OEM Backslash key.
        OemBackslash = 0xE2,
        //
        // 摘要:
        //     A special key masking the real key being processed by an IME.
        ImeProcessed = 155,
        //
        // 摘要:
        //     A special key masking the real key being processed as a system key.
        System = 10003,
        //
        // 摘要:
        //     The DBE_ALPHANUMERIC key.
        DbeAlphanumeric = 10004,
        //
        // 摘要:
        //     The OEM ATTN key.
        OemAttn = 10005,
        //
        // 摘要:
        //     The DBE_KATAKANA key.
        DbeKatakana = 10006,
        //
        // 摘要:
        //     The OEM FINISH key.
        OemFinish = 10006,
        //
        // 摘要:
        //     The DBE_HIRAGANA key.
        DbeHiragana = 10007,
        //
        // 摘要:
        //     The OEM COPY key.
        OemCopy = 10007,
        //
        // 摘要:
        //     The DBE_SBCSCHAR key.
        DbeSbcsChar = 10008,
        //
        // 摘要:
        //     The OEM AUTO key.
        OemAuto = 10008,
        //
        // 摘要:
        //     The DBE_DBCSCHAR key.
        DbeDbcsChar = 10009,
        //
        // 摘要:
        //     The OEM ENLW key.
        OemEnlw = 10009,
        //
        // 摘要:
        //     The DBE_ROMAN key.
        DbeRoman = 10010,
        //
        // 摘要:
        //     The OEM BACKTAB key.
        OemBackTab = 10011,
        //
        // 摘要:
        //     The ATTN key.
        Attn = 0xF6,
        //
        // 摘要:
        //     The DBE_NOROMAN key.
        DbeNoRoman = 10012,
        //
        // 摘要:
        //     The CRSEL key.
        CrSel = 0xF7,
        //
        // 摘要:
        //     The DBE_ENTERWORDREGISTERMODE key.
        DbeEnterWordRegisterMode = 10013,
        //
        // 摘要:
        //     The DBE_ENTERIMECONFIGMODE key.
        DbeEnterImeConfigureMode = 10014,
        //
        // 摘要:
        //     The EXSEL key.
        ExSel = 0xF8,
        //
        // 摘要:
        //     The DBE_FLUSHSTRING key.
        DbeFlushString = 10015,
        //
        // 摘要:
        //     The ERASE EOF key.
        EraseEof = 0xF9,
        //
        // 摘要:
        //     The DBE_CODEINPUT key.
        DbeCodeInput = 10016,
        //
        // 摘要:
        //     The PLAY key.
        Play = 0xFA,
        //
        // 摘要:
        //     The DBE_NOCODEINPUT key.
        DbeNoCodeInput = 10017,
        //
        // 摘要:
        //     The ZOOM key.
        Zoom = 0xFB,
        //
        // 摘要:
        //     The DBE_DETERMINESTRING key.
        DbeDetermineString = 10018,
        //
        // 摘要:
        //     A constant reserved for future use.
        NoName = 0xFC,
        //
        // 摘要:
        //     The DBE_ENTERDLGCONVERSIONMODE key.
        DbeEnterDialogConversionMode = 10019,
        //
        // 摘要:
        //     The PA1 key.
        Pa1 = 0xFD,
        //
        // 摘要:
        //     The OEM Clear key.
        OemClear = 0xFE,
        //
        // 摘要:
        //     The key is used with another key to create a single combined character.
        DeadCharProcessed = 10020
    }
}
