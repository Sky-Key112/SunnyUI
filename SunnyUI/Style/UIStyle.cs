﻿/******************************************************************************
 * SunnyUI 开源控件库、工具类库、扩展类库、多页面开发框架。
 * CopyRight (C) 2012-2022 ShenYongHua(沈永华).
 * QQ群：56829229 QQ：17612584 EMail：SunnyUI@QQ.Com
 *
 * Blog:   https://www.cnblogs.com/yhuse
 * Gitee:  https://gitee.com/yhuse/SunnyUI
 * GitHub: https://github.com/yhuse/SunnyUI
 *
 * SunnyUI.dll can be used for free under the GPL-3.0 license.
 * If you use this code, please keep this note.
 * 如果您使用此代码，请保留此说明。
 ******************************************************************************
 * 文件名称: UIStyle.cs
 * 文件说明: 控件样式定义类
 * 当前版本: V3.1
 * 创建日期: 2020-01-01
 *
 * 2020-01-01: V2.2.0 增加文件说明
 * 2021-07-12: V3.0.5 增加紫色主题
 * 2021-07-18: V3.0.5 增加多彩主题，以颜色深色，文字白色为主
 * 2021-09-24: V3.0.7 修改默认字体的GdiCharSet
 * 2021-10-16: V3.0.8 增加系统DPI缩放自适应
******************************************************************************/

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace Sunny.UI
{
    public interface IStyleInterface
    {
        UIStyle Style
        {
            get; set;
        }

        bool StyleCustomMode
        {
            get; set;
        }

        string Version
        {
            get;
        }

        string TagString
        {
            get; set;
        }

        void SetStyleColor(UIBaseStyle uiColor);

        void SetStyle(UIStyle style);

        bool IsScaled { get; }

        void SetDPIScale();
    }

    /// <summary>
    /// 主题样式
    /// </summary>
    public enum UIStyle
    {
        /// <summary>
        /// 自定义
        /// </summary>
        [DisplayText("Custom")]
        Custom = 0,

        /// <summary>
        /// 蓝
        /// </summary>
        [DisplayText("Blue")]
        Blue = 1,

        /// <summary>
        /// 绿
        /// </summary>
        [DisplayText("Green")]
        Green = 2,

        /// <summary>
        /// 橙
        /// </summary>
        [DisplayText("Orange")]
        Orange = 3,

        /// <summary>
        /// 红
        /// </summary>
        [DisplayText("Red")]
        Red = 4,

        /// <summary>
        /// 灰
        /// </summary>
        [DisplayText("Gray")]
        Gray = 5,

        /// <summary>
        /// 紫
        /// </summary>
        [DisplayText("Purple")]
        Purple = 6,

        /// <summary>
        /// LayuiGreen
        /// </summary>
        LayuiGreen = 7,

        /// <summary>
        /// LayuiRed
        /// </summary>
        LayuiRed = 8,

        /// <summary>
        /// LayuiOrange
        /// </summary>
        LayuiOrange = 9,

        /// <summary>
        /// 深蓝
        /// </summary>
        [DisplayText("DarkBlue")]
        DarkBlue = 101,

        /// <summary>
        /// 黑
        /// </summary>
        [DisplayText("Black")]
        Black = 102,

        /// <summary>
        /// 多彩的
        /// </summary>
        [DisplayText("Colorful")]
        Colorful = 999
    }

    /// <summary>
    /// 主题样式管理类
    /// </summary>
    public static class UIStyles
    {
        public static bool DPIScale { get; set; }

        public static List<UIStyle> PopularStyles()
        {
            List<UIStyle> styles = new List<UIStyle>();
            foreach (UIStyle style in Enum.GetValues(typeof(UIStyle)))
            {
                if (style.Value() >= UIStyle.Blue.Value() && style.Value() < UIStyle.Colorful.Value())
                {
                    styles.Add(style);
                }
            }

            return styles;
        }

        /// <summary>
        /// 自定义
        /// </summary>
        private static readonly UIBaseStyle Custom = new UICustomStyle();

        /// <summary>
        /// 蓝
        /// </summary>
        public static readonly UIBaseStyle Blue = new UIBlueStyle();

        /// <summary>
        /// 橙
        /// </summary>
        public static readonly UIBaseStyle Orange = new UIOrangeStyle();

        /// <summary>
        /// 灰
        /// </summary>
        private static readonly UIBaseStyle Gray = new UIGrayStyle();

        /// <summary>
        /// 绿
        /// </summary>
        public static readonly UIBaseStyle Green = new UIGreenStyle();

        /// <summary>
        /// 红
        /// </summary>
        public static readonly UIBaseStyle Red = new UIRedStyle();

        /// <summary>
        /// 深蓝
        /// </summary>
        private static readonly UIBaseStyle DarkBlue = new UIDarkBlueStyle();

        /// <summary>
        /// 黑
        /// </summary>
        private static readonly UIBaseStyle Black = new UIBlackStyle();

        /// <summary>
        /// 紫
        /// </summary>
        private static readonly UIBaseStyle Purple = new UIPurpleStyle();

        /// <summary>
        /// 多彩
        /// </summary>
        private static readonly UIColorfulStyle Colorful = new UIColorfulStyle();

        public static void InitColorful(Color styleColor, Color foreColor)
        {
            Colorful.Init(styleColor, foreColor);
            Style = UIStyle.Colorful;
            SetStyle(Style);
        }

        private static readonly ConcurrentDictionary<UIStyle, UIBaseStyle> Styles = new ConcurrentDictionary<UIStyle, UIBaseStyle>();
        private static readonly ConcurrentDictionary<Guid, UIForm> Forms = new ConcurrentDictionary<Guid, UIForm>();
        private static readonly ConcurrentDictionary<Guid, UIPage> Pages = new ConcurrentDictionary<Guid, UIPage>();

        /// <summary>
        /// 菜单颜色集合
        /// </summary>
        public static readonly ConcurrentDictionary<UIMenuStyle, UIMenuColor> MenuColors = new ConcurrentDictionary<UIMenuStyle, UIMenuColor>();

        static UIStyles()
        {
            AddStyle(Custom);
            AddStyle(Blue);
            AddStyle(Orange);
            AddStyle(Gray);
            AddStyle(Green);
            AddStyle(Red);
            AddStyle(DarkBlue);

            AddStyle(new UIBaseStyle().Init(UIColor.LayuiGreen, UIStyle.LayuiGreen, Color.White, UIFontColor.Primary));
            AddStyle(new UIBaseStyle().Init(UIColor.LayuiRed, UIStyle.LayuiRed, Color.White, UIFontColor.Primary));
            AddStyle(new UIBaseStyle().Init(UIColor.LayuiOrange, UIStyle.LayuiOrange, Color.White, UIFontColor.Primary));

            AddStyle(Black);
            AddStyle(Purple);

            AddStyle(Colorful);

            MenuColors.TryAdd(UIMenuStyle.Custom, new UIMenuCustomColor());
            MenuColors.TryAdd(UIMenuStyle.Black, new UIMenuBlackColor());
            MenuColors.TryAdd(UIMenuStyle.White, new UIMenuWhiteColor());
        }

        /// <summary>
        /// 主题样式整数值
        /// </summary>
        /// <param name="style">主题样式</param>
        /// <returns>整数值</returns>
        public static int Value(this UIStyle style)
        {
            return (int)style;
        }

        /// <summary>
        /// 注册窗体
        /// </summary>
        /// <param name="guid">GUID</param>
        /// <param name="form">窗体</param>
        public static bool Register(Guid guid, UIForm form)
        {
            if (!Forms.ContainsKey(guid))
            {
                Forms.TryAddOrUpdate(guid, form);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <param name="guid">GUID</param>
        /// <param name="page">页面</param>
        public static bool Register(Guid guid, UIPage page)
        {
            if (!Pages.ContainsKey(guid))
            {
                Pages.TryAddOrUpdate(guid, page);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 注册窗体
        /// </summary>
        /// <param name="form">窗体</param>
        public static bool Register(this UIForm form)
        {
            if (!Forms.ContainsKey(form.Guid))
            {
                Forms.TryAddOrUpdate(form.Guid, form);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 注册页面
        /// </summary>
        /// <param name="page">页面</param>
        public static bool Register(this UIPage page)
        {
            if (!Pages.ContainsKey(page.Guid))
            {
                Pages.TryAddOrUpdate(page.Guid, page);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 反注册窗体
        /// </summary>
        /// <param name="form">窗体</param>
        public static void UnRegister(this UIForm form)
        {
            Forms.TryRemove(form.Guid, out _);
        }

        /// <summary>
        /// 反注册页面
        /// </summary>
        /// <param name="page">页面</param>
        public static void UnRegister(this UIPage page)
        {
            Pages.TryRemove(page.Guid, out _);
        }

        /// <summary>
        /// 反注册窗体、页面
        /// </summary>
        /// <param name="guid">GUID</param>
        public static void UnRegister(Guid guid)
        {
            if (Forms.ContainsKey(guid))
                Forms.TryRemove(guid, out _);

            if (Pages.ContainsKey(guid))
                Pages.TryRemove(guid, out _);

        }

        /// <summary>
        /// 获取主题样式
        /// </summary>
        /// <param name="style">主题样式名称</param>
        /// <returns>主题样式</returns>
        public static UIBaseStyle GetStyleColor(UIStyle style)
        {
            if (Styles.ContainsKey(style))
            {
                return Styles[style];
            }

            Style = UIStyle.Blue;
            return Styles[Style];
        }

        public static UIBaseStyle ActiveStyleColor => GetStyleColor(Style);

        private static void AddStyle(UIBaseStyle uiColor)
        {
            if (Styles.ContainsKey(uiColor.Name))
            {
                MessageBox.Show(uiColor.Name + " is already exist.");
            }

            Styles.TryAdd(uiColor.Name, uiColor);
        }

        /// <summary>
        /// 主题样式
        /// </summary>
        public static UIStyle Style { get; private set; } = UIStyle.Blue;

        /// <summary>
        /// 设置主题样式
        /// </summary>
        /// <param name="style">主题样式</param>
        public static void SetStyle(UIStyle style)
        {
            Style = style;

            foreach (var form in Forms.Values)
            {
                form.Style = style;
            }

            foreach (var page in Pages.Values)
            {
                page.Style = style;
            }
        }

        public static void SetDPIScale()
        {
            foreach (var form in Forms.Values)
            {
                if (!form.DPIScale().EqualsFloat(1))
                    form.SetDPIScale();
            }

            foreach (var page in Pages.Values)
            {
                if (!page.DPIScale().EqualsFloat(1))
                    page.SetDPIScale();
            }
        }

        public static void Translate()
        {
            foreach (var form in Forms.Values)
            {
                form.Translate();
            }
        }
    }

    /// <summary>
    /// 主题颜色
    /// </summary>
    public static class UIColor
    {
        /// <summary>
        /// 蓝
        /// </summary>
        public static readonly Color Blue = Color.FromArgb(80, 160, 255);

        /// <summary>
        /// 绿
        /// </summary>
        public static readonly Color Green = Color.FromArgb(110, 190, 40);

        /// <summary>
        /// 红
        /// </summary>
        public static readonly Color Red = Color.FromArgb(230, 80, 80);

        /// <summary>
        /// 灰
        /// </summary>
        public static readonly Color Gray = Color.FromArgb(140, 140, 140);

        /// <summary>
        /// 橙
        /// </summary>
        public static readonly Color Orange = Color.FromArgb(220, 155, 40);

        /// <summary>
        /// LayuiGreen
        /// </summary>
        public static readonly Color LayuiGreen = Color.FromArgb(0, 150, 136);

        /// <summary>
        /// LayuiRed
        /// </summary>
        public static readonly Color LayuiRed = Color.FromArgb(255, 87, 34);

        /// <summary>
        /// LayuiOrange
        /// </summary>
        public static readonly Color LayuiOrange = Color.FromArgb(255, 184, 0);

        /// <summary>
        /// LayuiCyan
        /// </summary>
        public static readonly Color LayuiCyan = Color.FromArgb(46, 57, 79);

        /// <summary>
        /// LayuiCyan
        /// </summary>
        public static readonly Color LayuiBlue = Color.FromArgb(69, 149, 255);

        /// <summary>
        /// LayuiCyan
        /// </summary>
        public static readonly Color LayuiBlack = Color.FromArgb(52, 55, 66);

        /// <summary>
        /// 深蓝
        /// </summary>
        public static readonly Color DarkBlue = Color.FromArgb(14, 30, 63);

        /// <summary>
        /// 白
        /// </summary>
        public static readonly Color White = Color.White;

        /// <summary>
        /// 黑
        /// </summary>
        public static readonly Color Black = Color.Black;

        /// <summary>
        /// 紫
        /// </summary>
        public static readonly Color Purple = Color.FromArgb(102, 58, 183);

        /// <summary>
        /// 浅紫
        /// </summary>
        public static readonly Color LightPurple = Color.FromArgb(250, 238, 255);

        /// <summary>
        /// 透明
        /// </summary>
        public static readonly Color Transparent = Color.Transparent;

        /// <summary>
        /// 浅蓝
        /// </summary>
        public static readonly Color LightBlue = Color.FromArgb(235, 243, 255);

        /// <summary>
        /// 浅绿
        /// </summary>
        public static readonly Color LightGreen = Color.FromArgb(239, 248, 232);

        /// <summary>
        /// 浅红
        /// </summary>
        public static readonly Color LightRed = Color.FromArgb(251, 238, 238);

        /// <summary>
        /// 浅灰
        /// </summary>
        public static readonly Color LightGray = Color.FromArgb(242, 242, 244);

        /// <summary>
        /// 浅橙
        /// </summary>
        public static readonly Color LightOrange = Color.FromArgb(251, 245, 233);

        /// <summary>
        /// 中蓝
        /// </summary>
        public static readonly Color RegularBlue = Color.FromArgb(216, 233, 255);

        /// <summary>
        /// 中绿
        /// </summary>
        public static readonly Color RegularGreen = Color.FromArgb(224, 242, 210);

        /// <summary>
        /// 中红
        /// </summary>
        public static readonly Color RegularRed = Color.FromArgb(248, 222, 222);

        /// <summary>
        /// 中灰
        /// </summary>
        public static readonly Color RegularGray = Color.FromArgb(230, 230, 232);

        /// <summary>
        /// 中橙
        /// </summary>
        public static readonly Color RegularOrange = Color.FromArgb(247, 234, 210);
    }

    /// <summary>
    /// 不可用颜色
    /// </summary>
    public static class UIDisableColor
    {
        /// <summary>
        /// 填充色
        /// </summary>
        public static readonly Color Fill = UIFontColor.Plain;

        /// <summary>
        /// 字体色
        /// </summary>
        public static readonly Color Fore = UIFontColor.Regular;
    }

    /// <summary>
    /// 字体颜色
    /// </summary>
    public static class UIFontColor
    {
        public static byte GdiCharSet
        {
            get
            {
                byte value = 1;
                // 注解
                // 除非在构造函数中指定了不同的字符集，否则此属性将返回 1 
                // Font(String, Single, FontStyle, GraphicsUnit, Byte) 。 
                // 此属性采用 Windows SDK 头文件 WinGDI 中定义的列表的值。 下表列出了字符集和字节值。
                // 字符集 “值”
                // ANSI    0
                // DEFAULT 1
                // 代号  2
                // SHIFTJIS    128
                // HANGEUL 129
                // 文字  129
                // GB2312  134
                // CHINESEBIG5 136
                // OEM 255
                // JOHAB   130
                // 希伯来语    177
                // 阿拉伯语    178
                // 希腊语 161
                // 土耳其语    162
                // 越南语 163
                // 泰语  222
                // EASTEUROPE  238
                // 俄语  204
                // MAC 77
                // 波罗  186

                if (System.Text.Encoding.Default.BodyName.ToUpper() == "GB2312") value = 134;
                return value;
            }
        }

        /// <summary>
        /// 默认字体
        /// </summary>
        public static Font Font()
        {
            return new Font("微软雅黑", FontSize, FontStyle.Regular, GraphicsUnit.Point, GdiCharSet);
        }

        /// <summary>
        /// 默认字体
        /// </summary>
        public static Font Font(float fontSize)
        {
            return new Font("微软雅黑", fontSize, FontStyle.Regular, GraphicsUnit.Point, GdiCharSet);
        }

        public static float FontSize = 12;

        /// <summary>
        /// 默认二级字体
        /// </summary>
        public static Font SubFont()
        {
            return new Font("微软雅黑", SubFontSize, FontStyle.Regular, GraphicsUnit.Point, GdiCharSet);
        }

        public static float SubFontSize = 9;

        /// <summary>
        /// 主要颜色
        /// </summary>
        public static readonly Color Primary = Color.FromArgb(48, 48, 48);

        /// <summary>
        /// 正常颜色
        /// </summary>
        public static readonly Color Regular = Color.FromArgb(96, 96, 96);

        /// <summary>
        /// 次要颜色
        /// </summary>
        public static readonly Color Secondary = Color.FromArgb(144, 144, 144);

        /// <summary>
        /// 其他颜色
        /// </summary>
        public static readonly Color Plain = Color.Silver;

        /// <summary>
        /// 白色
        /// </summary>
        public static readonly Color White = Color.FromArgb(248, 248, 248);
    }

    /// <summary>
    /// 边框颜色
    /// </summary>
    public static class UIRectColorColor
    {
        /// <summary>
        /// 主要颜色
        /// </summary>
        public static readonly Color Primary = Color.FromArgb(0xDC, 0xDF, 0xE6);

        /// <summary>
        /// 正常颜色
        /// </summary>
        public static readonly Color Regular = Color.FromArgb(0xE4, 0xE7, 0xED);

        /// <summary>
        /// 次要颜色
        /// </summary>
        public static readonly Color Secondary = Color.FromArgb(0xEB, 0xEE, 0xF5);

        /// <summary>
        /// 其他颜色
        /// </summary>
        public static readonly Color Plain = Color.FromArgb(0xF2, 0xF6, 0xFC);
    }

    /// <summary>
    /// 背景色
    /// </summary>
    public static class UIBackgroundColor
    {
        /// <summary>
        /// 白
        /// </summary>
        public static readonly Color White = UIColor.White;

        /// <summary>
        /// 黑
        /// </summary>
        public static readonly Color Black = UIColor.Black;

        /// <summary>
        /// 透明色
        /// </summary>
        public static readonly Color Transparent = Color.Transparent;
    }

    public static class UIStyleHelper
    {
        /// <summary>
        /// 主题的调色板
        /// </summary>
        /// <param name="style"></param>
        /// <returns></returns>
        public static UIBaseStyle Colors(this UIStyle style)
        {
            return UIStyles.GetStyleColor(style);
        }

        public static bool IsCustom(this UIStyle style)
        {
            return style.Equals(UIStyle.Custom);
        }

        public static bool IsValid(this UIStyle style)
        {
            return !style.IsCustom();
        }

        public static bool IsCustom(this UIBaseStyle style)
        {
            return style.Name.IsCustom();
        }

        public static bool IsValid(this UIBaseStyle style)
        {
            return !style.IsCustom();
        }

        public static void SetChildUIStyle(Control ctrl, UIStyle style)
        {
            List<Control> controls = ctrl.GetUIStyleControls("IStyleInterface");
            foreach (var control in controls)
            {
                if (control is IStyleInterface item)
                {
                    if (!item.StyleCustomMode)
                    {
                        item.Style = style;
                    }
                }
            }

            FieldInfo[] fieldInfo = ctrl.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var info in fieldInfo)
            {
                if (info.FieldType.Name == "UIContextMenuStrip")
                {
                    UIContextMenuStrip context = (UIContextMenuStrip)info.GetValue(ctrl);
                    if (context != null && !context.StyleCustomMode)
                    {
                        context.SetStyle(style);
                    }
                }
            }
        }

        /// <summary>
        /// 查找包含接口名称的控件列表
        /// </summary>
        /// <param name="ctrl">容器</param>
        /// <param name="interfaceName">接口名称</param>
        /// <returns>控件列表</returns>
        public static List<Control> GetUIStyleControls(this Control ctrl, string interfaceName)
        {
            List<Control> values = new List<Control>();

            foreach (Control obj in ctrl.Controls)
            {
                if (obj.GetType().GetInterface(interfaceName) != null)
                {
                    values.Add(obj);
                }

                if (obj is UIPage) continue;
                if (obj is UITableLayoutPanel) continue;
                if (obj is UIFlowLayoutPanel) continue;
                if (obj is UIPanel) continue;

                if (obj is TableLayoutPanel) continue;

                if (obj.Controls.Count > 0)
                {
                    values.AddRange(obj.GetUIStyleControls(interfaceName));
                }
            }

            return values;
        }

        public static List<Control> GetTranslateControls(this Control ctrl, string interfaceName)
        {
            List<Control> values = new List<Control>();

            foreach (Control obj in ctrl.Controls)
            {
                if (obj.GetType().GetInterface(interfaceName) != null)
                {
                    values.Add(obj);
                }

                if (obj.Controls.Count > 0)
                {
                    values.AddRange(obj.GetTranslateControls(interfaceName));
                }
            }

            return values;
        }

        public static void SetRawControlStyle(ControlEventArgs e, UIStyle style)
        {
            if (e.Control is TableLayoutPanel)
            {
                List<Control> controls = e.Control.GetUIStyleControls("IStyleInterface");
                foreach (var control in controls)
                {
                    if (control is IStyleInterface item)
                    {
                        if (!item.StyleCustomMode)
                            item.Style = style;
                    }
                }

                return;
            }

            if (e.Control is FlowLayoutPanel)
            {
                List<Control> controls = e.Control.GetUIStyleControls("IStyleInterface");
                foreach (var control in controls)
                {
                    if (control is IStyleInterface item)
                    {
                        if (!item.StyleCustomMode)
                            item.Style = style;
                    }
                }

                return;
            }

            if (e.Control is Panel)
            {
                List<Control> controls = e.Control.GetUIStyleControls("IStyleInterface");
                foreach (var control in controls)
                {
                    if (control is IStyleInterface item)
                    {
                        if (!item.StyleCustomMode)
                            item.Style = style;
                    }
                }
            }
        }
    }
}