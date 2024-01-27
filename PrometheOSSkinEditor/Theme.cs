using System;
using System.Globalization;
using System.Text;
using System.Text.Json;

namespace PrometheOSSkinEditor
{
    public enum Alignment
    {
        Left = 0,
        Center = 1,
        Right = 2
    }

    public struct Theme
    {
        const string THEME_SKIN_AUTHOR = "PrometheOS Skin Editor";

        const uint THEME_BACKGROUND_FRAME_DELAY = 100;
        const uint THEME_BACKGROUND_COLOR = 0xff11191f;
        const uint THEME_BACKGROUND_IMAGE_TINT = 0xffffffff;
        const uint THEME_BACKGROUND_OVERLAY_IMAGE_TINT = 0xffffffff;
        const Alignment THEME_PROMETHEOS_ALIGN = Alignment.Left;
        const uint THEME_PROMETHEOS_Y = 32;
        const uint THEME_PROMETHEOS_COLOR = 0xffffcd00;
        const uint THEME_INSTALLER_TINT = 0xffffffff;
        const uint THEME_TEXT_COLOR = 0xffffffff;
        const uint THEME_TEXT_DISABLED_COLOR = 0xff404040;
        const uint THEME_HEADER_TEXT_COLOR = 0xffffffff;
        const uint THEME_FOOTER_TEXT_COLOR = 0xffffffff;

        const Alignment THEME_HEADER_ALIGN = Alignment.Left;
        const uint THEME_HEADER_Y = 32;
        const int THEME_CENTER_OFFSET = 0;
        const uint THEME_FOOTER_Y = 420;

        const uint THEME_SNAKE_WALL_COLOR = 0xff808080;
        const uint THEME_SNAKE_FOOD_COLOR = 0xffff0000;
        const uint THEME_SNAKE_HEAD_COLOR = 0xff00ff00;
        const uint THEME_SNAKE_TAIL_COLOR = 0xff008000;

        const uint THEME_JOY_BUTTON_A_COLOR = 0xff00ff00;
        const uint THEME_JOY_BUTTON_B_COLOR = 0xffff0000;
        const uint THEME_JOY_BUTTON_X_COLOR = 0xff0000ff;
        const uint THEME_JOY_BUTTON_Y_COLOR = 0xffffff00;

        const uint THEME_BUTTON_ACTIVE_FILL_COLOR = 0xff1095c1;
        const uint THEME_BUTTON_ACTIVE_STROKE_COLOR = 0xffffffff;
        const uint THEME_BUTTON_ACTIVE_TEXT_COLOR = 0xffffffff;

        const uint THEME_BUTTON_ACTIVE_HOVER_FILL_COLOR = 0xff19b3e6;
        const uint THEME_BUTTON_ACTIVE_HOVER_STROKE_COLOR = 0xffffffff;
        const uint THEME_BUTTON_ACTIVE_HOVER_TEXT_COLOR = 0xffffffff;

        const uint THEME_BUTTON_INACTIVE_FILL_COLOR = 0xff1095c1;
        const uint THEME_BUTTON_INACTIVE_STROKE_COLOR = 0xff000000;
        const uint THEME_BUTTON_INACTIVE_TEXT_COLOR = 0xffffffff;

        const uint THEME_BUTTON_INACTIVE_HOVER_FILL_COLOR = 0xff19b3e6;
        const uint THEME_BUTTON_INACTIVE_HOVER_STROKE_COLOR = 0xff000000;
        const uint THEME_BUTTON_INACTIVE_HOVER_TEXT_COLOR = 0xffffffff;

        const uint THEME_PANEL_FILL_COLOR = 0xff141e26;
        const uint THEME_PANEL_STROKE_COLOR = 0xff141e26;

        const uint THEME_TEXT_PANEL_FILL_COLOR = 0xff11191f;
        const uint THEME_TEXT_PANEL_STROKE_COLOR = 0xff374956;
        const uint THEME_TEXT_PANEL_TEXT_COLOR = 0xffffffff;
        const uint THEME_TEXT_PANEL_HOVER_FILL_COLOR = 0xff11191f;
        const uint THEME_TEXT_PANEL_HOVER_STROKE_COLOR = 0xffffffff;
        const uint THEME_TEXT_PANEL_HOVER_TEXT_COLOR = 0xffffffff;

        const uint THEME_BUTTON_LED_OFF_FILL_COLOR = 0xff404040;
        const uint THEME_BUTTON_LED_OFF_STROKE_COLOR = 0xff404040;
        const uint THEME_BUTTON_LED_OFF_TEXT_COLOR = 0xffffffff;
        const uint THEME_BUTTON_LED_OFF_HOVER_FILL_COLOR = 0xff202020;
        const uint THEME_BUTTON_LED_OFF_HOVER_STROKE_COLOR = 0xffffffff;
        const uint THEME_BUTTON_LED_OFF_HOVER_TEXT_COLOR = 0xffffffff;

        const uint THEME_BUTTON_LED_RED_FILL_COLOR = 0xffd40c00;
        const uint THEME_BUTTON_LED_RED_STROKE_COLOR = 0xffd40c00;
        const uint THEME_BUTTON_LED_RED_TEXT_COLOR = 0xff000000;
        const uint THEME_BUTTON_LED_RED_HOVER_FILL_COLOR = 0xffaa0a00;
        const uint THEME_BUTTON_LED_RED_HOVER_STROKE_COLOR = 0xffffffff;
        const uint THEME_BUTTON_LED_RED_HOVER_TEXT_COLOR = 0xff000000;

        const uint THEME_BUTTON_LED_GREEN_FILL_COLOR = 0xff32c12c;
        const uint THEME_BUTTON_LED_GREEN_STROKE_COLOR = 0xff32c12c;
        const uint THEME_BUTTON_LED_GREEN_TEXT_COLOR = 0xff000000;
        const uint THEME_BUTTON_LED_GREEN_HOVER_FILL_COLOR = 0xff289a23;
        const uint THEME_BUTTON_LED_GREEN_HOVER_STROKE_COLOR = 0xffffffff;
        const uint THEME_BUTTON_LED_GREEN_HOVER_TEXT_COLOR = 0xff000000;

        const uint THEME_BUTTON_LED_YELLOW_FILL_COLOR = 0xffffcd00;
        const uint THEME_BUTTON_LED_YELLOW_STROKE_COLOR = 0xffffcd00;
        const uint THEME_BUTTON_LED_YELLOW_TEXT_COLOR = 0xff000000;
        const uint THEME_BUTTON_LED_YELLOW_HOVER_FILL_COLOR = 0xffcca400;
        const uint THEME_BUTTON_LED_YELLOW_HOVER_STROKE_COLOR = 0xffffffff;
        const uint THEME_BUTTON_LED_YELLOW_HOVER_TEXT_COLOR = 0xff000000;

        const uint THEME_BUTTON_LED_BLUE_FILL_COLOR = 0xff526eff;
        const uint THEME_BUTTON_LED_BLUE_STROKE_COLOR = 0xff526eff;
        const uint THEME_BUTTON_LED_BLUE_TEXT_COLOR = 0xff000000;
        const uint THEME_BUTTON_LED_BLUE_HOVER_FILL_COLOR = 0xff4358cc;
        const uint THEME_BUTTON_LED_BLUE_HOVER_STROKE_COLOR = 0xffffffff;
        const uint THEME_BUTTON_LED_BLUE_HOVER_TEXT_COLOR = 0xff000000;

        const uint THEME_BUTTON_LED_PURPLE_FILL_COLOR = 0xff7f4fc9;
        const uint THEME_BUTTON_LED_PURPLE_STROKE_COLOR = 0xff7f4fc9;
        const uint THEME_BUTTON_LED_PURPLE_TEXT_COLOR = 0xff000000;
        const uint THEME_BUTTON_LED_PURPLE_HOVER_FILL_COLOR = 0xff663fa1;
        const uint THEME_BUTTON_LED_PURPLE_HOVER_STROKE_COLOR = 0xffffffff;
        const uint THEME_BUTTON_LED_PURPLE_HOVER_TEXT_COLOR = 0xff000000;

        const uint THEME_BUTTON_LED_TURQUOISE_FILL_COLOR = 0xff00bcd9;
        const uint THEME_BUTTON_LED_TURQUOISE_STROKE_COLOR = 0xff00bcd9;
        const uint THEME_BUTTON_LED_TURQUOISE_TEXT_COLOR = 0xff000000;
        const uint THEME_BUTTON_LED_TURQUOISE_HOVER_FILL_COLOR = 0xff0096ae;
        const uint THEME_BUTTON_LED_TURQUOISE_HOVER_STROKE_COLOR = 0xffffffff;
        const uint THEME_BUTTON_LED_TURQUOISE_HOVER_TEXT_COLOR = 0xff000000;

        const uint THEME_BUTTON_LED_WHITE_FILL_COLOR = 0xfff0f0f0;
        const uint THEME_BUTTON_LED_WHITE_STROKE_COLOR = 0xfff0f0f0;
        const uint THEME_BUTTON_LED_WHITE_TEXT_COLOR = 0xff000000;
        const uint THEME_BUTTON_LED_WHITE_HOVER_FILL_COLOR = 0xffc0c0c0;
        const uint THEME_BUTTON_LED_WHITE_HOVER_STROKE_COLOR = 0xffffffff;
        const uint THEME_BUTTON_LED_WHITE_HOVER_TEXT_COLOR = 0xff000000;

        public string SKIN_AUTHOR;
        public uint BACKGROUND_FRAME_DELAY;
        public uint BACKGROUND_COLOR;
        public uint BACKGROUND_IMAGE_TINT;
        public uint BACKGROUND_OVERLAY_IMAGE_TINT;
        public Alignment PROMETHEOS_ALIGN;
        public uint PROMETHEOS_Y;
        public uint PROMETHEOS_COLOR;
        public uint INSTALLER_TINT;
        public uint TEXT_COLOR;
        public uint TEXT_DISABLED_COLOR;
        public uint HEADER_TEXT_COLOR;
        public uint FOOTER_TEXT_COLOR;

        public Alignment HEADER_ALIGN;
        public uint HEADER_Y;
        public int CENTER_OFFSET;
        public uint FOOTER_Y;

        public uint SNAKE_WALL_COLOR;
        public uint SNAKE_FOOD_COLOR;
        public uint SNAKE_HEAD_COLOR;
        public uint SNAKE_TAIL_COLOR;

        public uint JOY_BUTTON_A_COLOR;
        public uint JOY_BUTTON_B_COLOR;
        public uint JOY_BUTTON_X_COLOR;
        public uint JOY_BUTTON_Y_COLOR;

        public uint BUTTON_TOGGLE_FILL_COLOR;
        public uint BUTTON_TOGGLE_STROKE_COLOR;

        public uint BUTTON_ACTIVE_FILL_COLOR;
        public uint BUTTON_ACTIVE_STROKE_COLOR;
        public uint BUTTON_ACTIVE_TEXT_COLOR;

        public uint BUTTON_ACTIVE_HOVER_FILL_COLOR;
        public uint BUTTON_ACTIVE_HOVER_STROKE_COLOR;
        public uint BUTTON_ACTIVE_HOVER_TEXT_COLOR;

        public uint BUTTON_INACTIVE_FILL_COLOR;
        public uint BUTTON_INACTIVE_STROKE_COLOR;
        public uint BUTTON_INACTIVE_TEXT_COLOR;

        public uint BUTTON_INACTIVE_HOVER_FILL_COLOR;
        public uint BUTTON_INACTIVE_HOVER_STROKE_COLOR;
        public uint BUTTON_INACTIVE_HOVER_TEXT_COLOR;

        public uint PANEL_FILL_COLOR;
        public uint PANEL_STROKE_COLOR;

        public uint TEXT_PANEL_FILL_COLOR;
        public uint TEXT_PANEL_STROKE_COLOR;
        public uint TEXT_PANEL_TEXT_COLOR;
        public uint TEXT_PANEL_HOVER_FILL_COLOR;
        public uint TEXT_PANEL_HOVER_STROKE_COLOR;
        public uint TEXT_PANEL_HOVER_TEXT_COLOR;

        public uint BUTTON_LED_OFF_FILL_COLOR;
        public uint BUTTON_LED_OFF_STROKE_COLOR;
        public uint BUTTON_LED_OFF_TEXT_COLOR;
        public uint BUTTON_LED_OFF_HOVER_FILL_COLOR;
        public uint BUTTON_LED_OFF_HOVER_STROKE_COLOR;
        public uint BUTTON_LED_OFF_HOVER_TEXT_COLOR;

        public uint BUTTON_LED_RED_FILL_COLOR;
        public uint BUTTON_LED_RED_STROKE_COLOR;
        public uint BUTTON_LED_RED_TEXT_COLOR;
        public uint BUTTON_LED_RED_HOVER_FILL_COLOR;
        public uint BUTTON_LED_RED_HOVER_STROKE_COLOR;
        public uint BUTTON_LED_RED_HOVER_TEXT_COLOR;

        public uint BUTTON_LED_GREEN_FILL_COLOR;
        public uint BUTTON_LED_GREEN_STROKE_COLOR;
        public uint BUTTON_LED_GREEN_TEXT_COLOR;
        public uint BUTTON_LED_GREEN_HOVER_FILL_COLOR;
        public uint BUTTON_LED_GREEN_HOVER_STROKE_COLOR;
        public uint BUTTON_LED_GREEN_HOVER_TEXT_COLOR;

        public uint BUTTON_LED_YELLOW_FILL_COLOR;
        public uint BUTTON_LED_YELLOW_STROKE_COLOR;
        public uint BUTTON_LED_YELLOW_TEXT_COLOR;
        public uint BUTTON_LED_YELLOW_HOVER_FILL_COLOR;
        public uint BUTTON_LED_YELLOW_HOVER_STROKE_COLOR;
        public uint BUTTON_LED_YELLOW_HOVER_TEXT_COLOR;

        public uint BUTTON_LED_BLUE_FILL_COLOR;
        public uint BUTTON_LED_BLUE_STROKE_COLOR;
        public uint BUTTON_LED_BLUE_TEXT_COLOR;
        public uint BUTTON_LED_BLUE_HOVER_FILL_COLOR;
        public uint BUTTON_LED_BLUE_HOVER_STROKE_COLOR;
        public uint BUTTON_LED_BLUE_HOVER_TEXT_COLOR;

        public uint BUTTON_LED_PURPLE_FILL_COLOR;
        public uint BUTTON_LED_PURPLE_STROKE_COLOR;
        public uint BUTTON_LED_PURPLE_TEXT_COLOR;
        public uint BUTTON_LED_PURPLE_HOVER_FILL_COLOR;
        public uint BUTTON_LED_PURPLE_HOVER_STROKE_COLOR;
        public uint BUTTON_LED_PURPLE_HOVER_TEXT_COLOR;

        public uint BUTTON_LED_TURQUOISE_FILL_COLOR;
        public uint BUTTON_LED_TURQUOISE_STROKE_COLOR;
        public uint BUTTON_LED_TURQUOISE_TEXT_COLOR;
        public uint BUTTON_LED_TURQUOISE_HOVER_FILL_COLOR;
        public uint BUTTON_LED_TURQUOISE_HOVER_STROKE_COLOR;
        public uint BUTTON_LED_TURQUOISE_HOVER_TEXT_COLOR;

        public uint BUTTON_LED_WHITE_FILL_COLOR;
        public uint BUTTON_LED_WHITE_STROKE_COLOR;
        public uint BUTTON_LED_WHITE_TEXT_COLOR;
        public uint BUTTON_LED_WHITE_HOVER_FILL_COLOR;
        public uint BUTTON_LED_WHITE_HOVER_STROKE_COLOR;
        public uint BUTTON_LED_WHITE_HOVER_TEXT_COLOR;

        public void DefaultTheme()
        {
            SKIN_AUTHOR = THEME_SKIN_AUTHOR;

            BACKGROUND_FRAME_DELAY = THEME_BACKGROUND_FRAME_DELAY;
            BACKGROUND_COLOR = THEME_BACKGROUND_COLOR;
            BACKGROUND_IMAGE_TINT = THEME_BACKGROUND_IMAGE_TINT;
            BACKGROUND_OVERLAY_IMAGE_TINT = THEME_BACKGROUND_OVERLAY_IMAGE_TINT;

            PROMETHEOS_ALIGN = THEME_PROMETHEOS_ALIGN;
            PROMETHEOS_Y = THEME_PROMETHEOS_Y;
            HEADER_ALIGN = THEME_HEADER_ALIGN;
            HEADER_Y = THEME_HEADER_Y;
            CENTER_OFFSET = THEME_CENTER_OFFSET;
            FOOTER_Y = THEME_FOOTER_Y;

            PROMETHEOS_COLOR = THEME_PROMETHEOS_COLOR;
            INSTALLER_TINT = THEME_INSTALLER_TINT;

            TEXT_COLOR = THEME_TEXT_COLOR;
            TEXT_DISABLED_COLOR = THEME_TEXT_DISABLED_COLOR;
            HEADER_TEXT_COLOR = THEME_HEADER_TEXT_COLOR;
            FOOTER_TEXT_COLOR = THEME_FOOTER_TEXT_COLOR;

            SNAKE_WALL_COLOR = THEME_SNAKE_WALL_COLOR;
            SNAKE_FOOD_COLOR = THEME_SNAKE_FOOD_COLOR;
            SNAKE_HEAD_COLOR = THEME_SNAKE_HEAD_COLOR;
            SNAKE_TAIL_COLOR = THEME_SNAKE_TAIL_COLOR;

            JOY_BUTTON_A_COLOR = THEME_JOY_BUTTON_A_COLOR;
            JOY_BUTTON_B_COLOR = THEME_JOY_BUTTON_B_COLOR;
            JOY_BUTTON_X_COLOR = THEME_JOY_BUTTON_X_COLOR;
            JOY_BUTTON_Y_COLOR = THEME_JOY_BUTTON_Y_COLOR;

            BUTTON_ACTIVE_FILL_COLOR = THEME_BUTTON_ACTIVE_FILL_COLOR;
            BUTTON_ACTIVE_STROKE_COLOR = THEME_BUTTON_ACTIVE_STROKE_COLOR;
            BUTTON_ACTIVE_TEXT_COLOR = THEME_BUTTON_ACTIVE_TEXT_COLOR;

            BUTTON_ACTIVE_HOVER_FILL_COLOR = THEME_BUTTON_ACTIVE_HOVER_FILL_COLOR;
            BUTTON_ACTIVE_HOVER_STROKE_COLOR = THEME_BUTTON_ACTIVE_HOVER_STROKE_COLOR;
            BUTTON_ACTIVE_HOVER_TEXT_COLOR = THEME_BUTTON_ACTIVE_HOVER_TEXT_COLOR;

            BUTTON_INACTIVE_FILL_COLOR = THEME_BUTTON_INACTIVE_FILL_COLOR;
            BUTTON_INACTIVE_STROKE_COLOR = THEME_BUTTON_INACTIVE_STROKE_COLOR;
            BUTTON_INACTIVE_TEXT_COLOR = THEME_BUTTON_INACTIVE_TEXT_COLOR;

            BUTTON_INACTIVE_HOVER_FILL_COLOR = THEME_BUTTON_INACTIVE_HOVER_FILL_COLOR;
            BUTTON_INACTIVE_HOVER_STROKE_COLOR = THEME_BUTTON_INACTIVE_HOVER_STROKE_COLOR;
            BUTTON_INACTIVE_HOVER_TEXT_COLOR = THEME_BUTTON_INACTIVE_HOVER_TEXT_COLOR;

            PANEL_FILL_COLOR = THEME_PANEL_FILL_COLOR;
            PANEL_STROKE_COLOR = THEME_PANEL_STROKE_COLOR;

            TEXT_PANEL_FILL_COLOR = THEME_TEXT_PANEL_FILL_COLOR;
            TEXT_PANEL_STROKE_COLOR = THEME_TEXT_PANEL_STROKE_COLOR;
            TEXT_PANEL_TEXT_COLOR = THEME_TEXT_PANEL_TEXT_COLOR;
            TEXT_PANEL_HOVER_FILL_COLOR = THEME_TEXT_PANEL_HOVER_FILL_COLOR;
            TEXT_PANEL_HOVER_STROKE_COLOR = THEME_TEXT_PANEL_HOVER_STROKE_COLOR;
            TEXT_PANEL_HOVER_TEXT_COLOR = THEME_TEXT_PANEL_HOVER_TEXT_COLOR;

            BUTTON_LED_OFF_FILL_COLOR = THEME_BUTTON_LED_OFF_FILL_COLOR;
            BUTTON_LED_OFF_STROKE_COLOR = THEME_BUTTON_LED_OFF_STROKE_COLOR;
            BUTTON_LED_OFF_TEXT_COLOR = THEME_BUTTON_LED_OFF_TEXT_COLOR;
            BUTTON_LED_OFF_HOVER_FILL_COLOR = THEME_BUTTON_LED_OFF_HOVER_FILL_COLOR;
            BUTTON_LED_OFF_HOVER_STROKE_COLOR = THEME_BUTTON_LED_OFF_HOVER_STROKE_COLOR;
            BUTTON_LED_OFF_HOVER_TEXT_COLOR = THEME_BUTTON_LED_OFF_HOVER_TEXT_COLOR;

            BUTTON_LED_RED_FILL_COLOR = THEME_BUTTON_LED_RED_FILL_COLOR;
            BUTTON_LED_RED_STROKE_COLOR = THEME_BUTTON_LED_RED_STROKE_COLOR;
            BUTTON_LED_RED_TEXT_COLOR = THEME_BUTTON_LED_RED_TEXT_COLOR;
            BUTTON_LED_RED_HOVER_FILL_COLOR = THEME_BUTTON_LED_RED_HOVER_FILL_COLOR;
            BUTTON_LED_RED_HOVER_STROKE_COLOR = THEME_BUTTON_LED_RED_HOVER_STROKE_COLOR;
            BUTTON_LED_RED_HOVER_TEXT_COLOR = THEME_BUTTON_LED_RED_HOVER_TEXT_COLOR;

            BUTTON_LED_GREEN_FILL_COLOR = THEME_BUTTON_LED_GREEN_FILL_COLOR;
            BUTTON_LED_GREEN_STROKE_COLOR = THEME_BUTTON_LED_GREEN_STROKE_COLOR;
            BUTTON_LED_GREEN_TEXT_COLOR = THEME_BUTTON_LED_GREEN_TEXT_COLOR;
            BUTTON_LED_GREEN_HOVER_FILL_COLOR = THEME_BUTTON_LED_GREEN_HOVER_FILL_COLOR;
            BUTTON_LED_GREEN_HOVER_STROKE_COLOR = THEME_BUTTON_LED_GREEN_HOVER_STROKE_COLOR;
            BUTTON_LED_GREEN_HOVER_TEXT_COLOR = THEME_BUTTON_LED_GREEN_HOVER_TEXT_COLOR;

            BUTTON_LED_YELLOW_FILL_COLOR = THEME_BUTTON_LED_YELLOW_FILL_COLOR;
            BUTTON_LED_YELLOW_STROKE_COLOR = THEME_BUTTON_LED_YELLOW_STROKE_COLOR;
            BUTTON_LED_YELLOW_TEXT_COLOR = THEME_BUTTON_LED_YELLOW_TEXT_COLOR;
            BUTTON_LED_YELLOW_HOVER_FILL_COLOR = THEME_BUTTON_LED_YELLOW_HOVER_FILL_COLOR;
            BUTTON_LED_YELLOW_HOVER_STROKE_COLOR = THEME_BUTTON_LED_YELLOW_HOVER_STROKE_COLOR;
            BUTTON_LED_YELLOW_HOVER_TEXT_COLOR = THEME_BUTTON_LED_YELLOW_HOVER_TEXT_COLOR;

            BUTTON_LED_BLUE_FILL_COLOR = THEME_BUTTON_LED_BLUE_FILL_COLOR;
            BUTTON_LED_BLUE_STROKE_COLOR = THEME_BUTTON_LED_BLUE_STROKE_COLOR;
            BUTTON_LED_BLUE_TEXT_COLOR = THEME_BUTTON_LED_BLUE_TEXT_COLOR;
            BUTTON_LED_BLUE_HOVER_FILL_COLOR = THEME_BUTTON_LED_BLUE_HOVER_FILL_COLOR;
            BUTTON_LED_BLUE_HOVER_STROKE_COLOR = THEME_BUTTON_LED_BLUE_HOVER_STROKE_COLOR;
            BUTTON_LED_BLUE_HOVER_TEXT_COLOR = THEME_BUTTON_LED_BLUE_HOVER_TEXT_COLOR;

            BUTTON_LED_PURPLE_FILL_COLOR = THEME_BUTTON_LED_PURPLE_FILL_COLOR;
            BUTTON_LED_PURPLE_STROKE_COLOR = THEME_BUTTON_LED_PURPLE_STROKE_COLOR;
            BUTTON_LED_PURPLE_TEXT_COLOR = THEME_BUTTON_LED_PURPLE_TEXT_COLOR;
            BUTTON_LED_PURPLE_HOVER_FILL_COLOR = THEME_BUTTON_LED_PURPLE_HOVER_FILL_COLOR;
            BUTTON_LED_PURPLE_HOVER_STROKE_COLOR = THEME_BUTTON_LED_PURPLE_HOVER_STROKE_COLOR;
            BUTTON_LED_PURPLE_HOVER_TEXT_COLOR = THEME_BUTTON_LED_PURPLE_HOVER_TEXT_COLOR;

            BUTTON_LED_TURQUOISE_FILL_COLOR = THEME_BUTTON_LED_TURQUOISE_FILL_COLOR;
            BUTTON_LED_TURQUOISE_STROKE_COLOR = THEME_BUTTON_LED_TURQUOISE_STROKE_COLOR;
            BUTTON_LED_TURQUOISE_TEXT_COLOR = THEME_BUTTON_LED_TURQUOISE_TEXT_COLOR;
            BUTTON_LED_TURQUOISE_HOVER_FILL_COLOR = THEME_BUTTON_LED_TURQUOISE_HOVER_FILL_COLOR;
            BUTTON_LED_TURQUOISE_HOVER_STROKE_COLOR = THEME_BUTTON_LED_TURQUOISE_HOVER_STROKE_COLOR;
            BUTTON_LED_TURQUOISE_HOVER_TEXT_COLOR = THEME_BUTTON_LED_TURQUOISE_HOVER_TEXT_COLOR;

            BUTTON_LED_WHITE_FILL_COLOR = THEME_BUTTON_LED_WHITE_FILL_COLOR;
            BUTTON_LED_WHITE_STROKE_COLOR = THEME_BUTTON_LED_WHITE_STROKE_COLOR;
            BUTTON_LED_WHITE_TEXT_COLOR = THEME_BUTTON_LED_WHITE_TEXT_COLOR;
            BUTTON_LED_WHITE_HOVER_FILL_COLOR = THEME_BUTTON_LED_WHITE_HOVER_FILL_COLOR;
            BUTTON_LED_WHITE_HOVER_STROKE_COLOR = THEME_BUTTON_LED_WHITE_HOVER_STROKE_COLOR;
            BUTTON_LED_WHITE_HOVER_TEXT_COLOR = THEME_BUTTON_LED_WHITE_HOVER_TEXT_COLOR;
        }

        public Theme()
        {
            SKIN_AUTHOR = string.Empty;
            DefaultTheme();
        }

        public static uint ConvertARGBtoABGR(uint argbValue)
        {
            byte alpha = (byte)((argbValue & 0xFF000000) >> 24);
            byte red = (byte)((argbValue & 0x00FF0000) >> 16);
            byte green = (byte)((argbValue & 0x0000FF00) >> 8);
            byte blue = (byte)(argbValue & 0x000000FF);
            return (uint)((alpha << 24) | (blue << 16) | (green << 8) | red);
        }

        public static uint ConvertABGRtoARGB(uint abgrValue)
        {
            byte alpha = (byte)((abgrValue & 0xFF000000) >> 24);
            byte blue = (byte)((abgrValue & 0x00FF0000) >> 16);
            byte green = (byte)((abgrValue & 0x0000FF00) >> 8);
            byte red = (byte)(abgrValue & 0x000000FF);
            return (uint)((alpha << 24) | (red << 16) | (green << 8) | blue);
        }

        private static void ParseUnsignedNumber(string value, ref uint result)
        {
            uint tempResult;

            result = 0;
            if (value.StartsWith("0x") && value.Length == 10)
            {
                if (uint.TryParse(value[2..], NumberStyles.HexNumber, CultureInfo.InvariantCulture, out tempResult))
                {
                    result = tempResult;
                }
                return;
            }
            
            if (uint.TryParse(value, out tempResult))
            {
                result = tempResult;
            }
        }
        private static void ParseSignedNumber(string value, ref int result)
        {
            int tempResult;

            result = 0;
            if (int.TryParse(value, out tempResult))
            {
                result = tempResult;
            }
        }

        public void SaveTheme(string themePath)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine($"SKIN_AUTHOR = {SKIN_AUTHOR}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"BACKGROUND_FRAME_DELAY = {BACKGROUND_FRAME_DELAY}");
            stringBuilder.AppendLine($"BACKGROUND_COLOR = 0x{BACKGROUND_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BACKGROUND_IMAGE_TINT = 0x{BACKGROUND_IMAGE_TINT.ToString("x8")}");
            stringBuilder.AppendLine($"BACKGROUND_OVERLAY_IMAGE_TINT = 0x{BACKGROUND_OVERLAY_IMAGE_TINT.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"PROMETHEOS_ALIGN = {(uint)PROMETHEOS_ALIGN}");
            stringBuilder.AppendLine($"PROMETHEOS_Y = {PROMETHEOS_Y}");
            stringBuilder.AppendLine($"HEADER_ALIGN = {(uint)HEADER_ALIGN}");
            stringBuilder.AppendLine($"HEADER_Y = {HEADER_Y}");
            stringBuilder.AppendLine($"CENTER_OFFSET = {CENTER_OFFSET}");
            stringBuilder.AppendLine($"FOOTER_Y = {FOOTER_Y}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"PROMETHEOS_COLOR = 0x{PROMETHEOS_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"INSTALLER_TINT = 0x{INSTALLER_TINT.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"TEXT_COLOR = 0x{TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"TEXT_DISABLED_COLOR = 0x{TEXT_DISABLED_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"HEADER_TEXT_COLOR = 0x{HEADER_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"FOOTER_TEXT_COLOR = 0x{FOOTER_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"SNAKE_WALL_COLOR = 0x{SNAKE_WALL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"SNAKE_FOOD_COLOR = 0x{SNAKE_FOOD_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"SNAKE_HEAD_COLOR = 0x{SNAKE_HEAD_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"SNAKE_TAIL_COLOR = 0x{SNAKE_TAIL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"JOY_BUTTON_A_COLOR = 0x{JOY_BUTTON_A_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"JOY_BUTTON_B_COLOR = 0x{JOY_BUTTON_B_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"JOY_BUTTON_X_COLOR = 0x{JOY_BUTTON_X_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"JOY_BUTTON_Y_COLOR = 0x{JOY_BUTTON_Y_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"BUTTON_ACTIVE_FILL_COLOR = 0x{BUTTON_ACTIVE_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_ACTIVE_STROKE_COLOR = 0x{BUTTON_ACTIVE_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_ACTIVE_TEXT_COLOR = 0x{BUTTON_ACTIVE_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"BUTTON_ACTIVE_HOVER_FILL_COLOR = 0x{BUTTON_ACTIVE_HOVER_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_ACTIVE_HOVER_STROKE_COLOR = 0x{BUTTON_ACTIVE_HOVER_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_ACTIVE_HOVER_TEXT_COLOR = 0x{BUTTON_ACTIVE_HOVER_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"BUTTON_INACTIVE_FILL_COLOR = 0x{BUTTON_INACTIVE_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_INACTIVE_STROKE_COLOR = 0x{BUTTON_INACTIVE_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_INACTIVE_TEXT_COLOR = 0x{BUTTON_INACTIVE_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"BUTTON_INACTIVE_HOVER_FILL_COLOR = 0x{BUTTON_INACTIVE_HOVER_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_INACTIVE_HOVER_STROKE_COLOR = 0x{BUTTON_INACTIVE_HOVER_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_INACTIVE_HOVER_TEXT_COLOR = 0x{BUTTON_INACTIVE_HOVER_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"PANEL_FILL_COLOR = 0x{PANEL_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"PANEL_STROKE_COLOR = 0x{PANEL_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"TEXT_PANEL_FILL_COLOR = 0x{TEXT_PANEL_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"TEXT_PANEL_STROKE_COLOR = 0x{TEXT_PANEL_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"TEXT_PANEL_TEXT_COLOR = 0x{TEXT_PANEL_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"TEXT_PANEL_HOVER_FILL_COLOR = 0x{TEXT_PANEL_HOVER_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"TEXT_PANEL_HOVER_STROKE_COLOR = 0x{TEXT_PANEL_HOVER_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"TEXT_PANEL_HOVER_TEXT_COLOR = 0x{TEXT_PANEL_HOVER_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"BUTTON_LED_OFF_FILL_COLOR = 0x{BUTTON_LED_OFF_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_OFF_STROKE_COLOR = 0x{BUTTON_LED_OFF_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_OFF_TEXT_COLOR = 0x{BUTTON_LED_OFF_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_OFF_HOVER_FILL_COLOR = 0x{BUTTON_LED_OFF_HOVER_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_OFF_HOVER_STROKE_COLOR = 0x{BUTTON_LED_OFF_HOVER_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_OFF_HOVER_TEXT_COLOR = 0x{BUTTON_LED_OFF_HOVER_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"BUTTON_LED_RED_FILL_COLOR = 0x{BUTTON_LED_RED_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_RED_STROKE_COLOR = 0x{BUTTON_LED_RED_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_RED_TEXT_COLOR = 0x{BUTTON_LED_RED_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_RED_HOVER_FILL_COLOR = 0x{BUTTON_LED_RED_HOVER_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_RED_HOVER_STROKE_COLOR = 0x{BUTTON_LED_RED_HOVER_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_RED_HOVER_TEXT_COLOR = 0x{BUTTON_LED_RED_HOVER_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"BUTTON_LED_GREEN_FILL_COLOR = 0x{BUTTON_LED_GREEN_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_GREEN_STROKE_COLOR = 0x{BUTTON_LED_GREEN_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_GREEN_TEXT_COLOR = 0x{BUTTON_LED_GREEN_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_GREEN_HOVER_FILL_COLOR = 0x{BUTTON_LED_GREEN_HOVER_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_GREEN_HOVER_STROKE_COLOR = 0x{BUTTON_LED_GREEN_HOVER_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_GREEN_HOVER_TEXT_COLOR = 0x{BUTTON_LED_GREEN_HOVER_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"BUTTON_LED_YELLOW_FILL_COLOR = 0x{BUTTON_LED_YELLOW_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_YELLOW_STROKE_COLOR = 0x{BUTTON_LED_YELLOW_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_YELLOW_TEXT_COLOR = 0x{BUTTON_LED_YELLOW_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_YELLOW_HOVER_FILL_COLOR = 0x{BUTTON_LED_YELLOW_HOVER_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_YELLOW_HOVER_STROKE_COLOR = 0x{BUTTON_LED_YELLOW_HOVER_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_YELLOW_HOVER_TEXT_COLOR = 0x{BUTTON_LED_YELLOW_HOVER_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"BUTTON_LED_BLUE_FILL_COLOR = 0x{BUTTON_LED_BLUE_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_BLUE_STROKE_COLOR = 0x{BUTTON_LED_BLUE_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_BLUE_TEXT_COLOR = 0x{BUTTON_LED_BLUE_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_BLUE_HOVER_FILL_COLOR = 0x{BUTTON_LED_BLUE_HOVER_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_BLUE_HOVER_STROKE_COLOR = 0x{BUTTON_LED_BLUE_HOVER_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_BLUE_HOVER_TEXT_COLOR = 0x{BUTTON_LED_BLUE_HOVER_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"BUTTON_LED_PURPLE_FILL_COLOR = 0x{BUTTON_LED_PURPLE_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_PURPLE_STROKE_COLOR = 0x{BUTTON_LED_PURPLE_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_PURPLE_TEXT_COLOR = 0x{BUTTON_LED_PURPLE_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_PURPLE_HOVER_FILL_COLOR = 0x{BUTTON_LED_PURPLE_HOVER_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_PURPLE_HOVER_STROKE_COLOR = 0x{BUTTON_LED_PURPLE_HOVER_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_PURPLE_HOVER_TEXT_COLOR = 0x{BUTTON_LED_PURPLE_HOVER_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"BUTTON_LED_TURQUOISE_FILL_COLOR = 0x{BUTTON_LED_TURQUOISE_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_TURQUOISE_STROKE_COLOR = 0x{BUTTON_LED_TURQUOISE_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_TURQUOISE_TEXT_COLOR = 0x{BUTTON_LED_TURQUOISE_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_TURQUOISE_HOVER_FILL_COLOR = 0x{BUTTON_LED_TURQUOISE_HOVER_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_TURQUOISE_HOVER_STROKE_COLOR = 0x{BUTTON_LED_TURQUOISE_HOVER_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_TURQUOISE_HOVER_TEXT_COLOR = 0x{BUTTON_LED_TURQUOISE_HOVER_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine();

            stringBuilder.AppendLine($"BUTTON_LED_WHITE_FILL_COLOR = 0x{BUTTON_LED_WHITE_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_WHITE_STROKE_COLOR = 0x{BUTTON_LED_WHITE_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_WHITE_TEXT_COLOR = 0x{BUTTON_LED_WHITE_TEXT_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_WHITE_HOVER_FILL_COLOR = 0x{BUTTON_LED_WHITE_HOVER_FILL_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_WHITE_HOVER_STROKE_COLOR = 0x{BUTTON_LED_WHITE_HOVER_STROKE_COLOR.ToString("x8")}");
            stringBuilder.AppendLine($"BUTTON_LED_WHITE_HOVER_TEXT_COLOR = 0x{BUTTON_LED_WHITE_HOVER_TEXT_COLOR.ToString("x8")}");

            File.WriteAllText(Path.Combine(themePath, "theme.ini"), stringBuilder.ToString());
        }

        public static Theme LoadTheme(string themePath)
        {
            var theme = new Theme();
            var themeLines = File.ReadAllLines(themePath);
            for (int i = 0; i < themeLines.Length; i++)
            {
                var line = themeLines[i];

                // Remove Comments
                int index = line.IndexOf('#');
                if (index != -1)
                {
                    line = line.Substring(0, index);
                }

                // Clean any spaces
                line = line.Trim();

                var lineParams = line.Split('=').Select(s => s.Trim()).ToArray();
                if (lineParams.Length != 2)
                {
                    continue;
                }

                if (string.Equals(lineParams[0], "SKIN_AUTHOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    theme.SKIN_AUTHOR = lineParams[1];
                }
                else if (string.Equals(lineParams[0], "BACKGROUND_FRAME_DELAY", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BACKGROUND_FRAME_DELAY);
                }
                else if (string.Equals(lineParams[0], "BACKGROUND_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BACKGROUND_COLOR);
                }
                else if (string.Equals(lineParams[0], "BACKGROUND_IMAGE_TINT", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BACKGROUND_IMAGE_TINT);
                }
                else if (string.Equals(lineParams[0], "BACKGROUND_OVERLAY_IAMGE_TINT", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BACKGROUND_OVERLAY_IMAGE_TINT);
                }
                else if (string.Equals(lineParams[0], "PROMETHEOS_ALIGN", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    uint prometheosAlign = 0;
                    ParseUnsignedNumber(lineParams[1], ref prometheosAlign);
                    theme.PROMETHEOS_ALIGN = (Alignment)prometheosAlign;
                }
                else if (string.Equals(lineParams[0], "PROMETHEOS_Y", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.PROMETHEOS_Y);
                }
                else if (string.Equals(lineParams[0], "HEADER_ALIGN", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    uint headerAlign = 0;
                    ParseUnsignedNumber(lineParams[1], ref headerAlign);
                    theme.HEADER_ALIGN = (Alignment)headerAlign;
                }
                else if (string.Equals(lineParams[0], "HEADER_Y", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.HEADER_Y);
                }
                else if (string.Equals(lineParams[0], "CENTER_OFFSET", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseSignedNumber(lineParams[1], ref theme.CENTER_OFFSET);
                }
                else if (string.Equals(lineParams[0], "FOOTER_Y", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.FOOTER_Y);
                }
                else if (string.Equals(lineParams[0], "PROMETHEOS_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.PROMETHEOS_COLOR);
                }
                else if (string.Equals(lineParams[0], "INSTALLER_TINT", StringComparison.CurrentCultureIgnoreCase) == true || string.Equals(lineParams[0], "INSTALLER_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.INSTALLER_TINT);
                }
                else if (string.Equals(lineParams[0], "TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "TEXT_DISABLED_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.TEXT_DISABLED_COLOR);
                }
                else if (string.Equals(lineParams[0], "TITLE_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true || string.Equals(lineParams[0], "HEADER_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.HEADER_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "FOOTER_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.FOOTER_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "SNAKE_WALL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.SNAKE_WALL_COLOR);
                }
                else if (string.Equals(lineParams[0], "SNAKE_FOOD_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.SNAKE_FOOD_COLOR);
                }
                else if (string.Equals(lineParams[0], "SNAKE_HEAD_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.SNAKE_HEAD_COLOR);
                }
                else if (string.Equals(lineParams[0], "SNAKE_TAIL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.SNAKE_TAIL_COLOR);
                }
                else if (string.Equals(lineParams[0], "JOY_BUTTON_A_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.JOY_BUTTON_A_COLOR);
                }
                else if (string.Equals(lineParams[0], "JOY_BUTTON_B_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.JOY_BUTTON_B_COLOR);
                }
                else if (string.Equals(lineParams[0], "JOY_BUTTON_X_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.JOY_BUTTON_X_COLOR);
                }
                else if (string.Equals(lineParams[0], "JOY_BUTTON_Y_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.JOY_BUTTON_Y_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_ACTIVE_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_ACTIVE_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_ACTIVE_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_ACTIVE_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_ACTIVE_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_ACTIVE_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_ACTIVE_HOVER_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_ACTIVE_HOVER_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_ACTIVE_HOVER_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_ACTIVE_HOVER_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_ACTIVE_HOVER_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_ACTIVE_HOVER_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_INACTIVE_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_INACTIVE_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_INACTIVE_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_INACTIVE_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_INACTIVE_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_INACTIVE_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_INACTIVE_HOVER_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_INACTIVE_HOVER_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_INACTIVE_HOVER_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_INACTIVE_HOVER_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_INACTIVE_HOVER_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_INACTIVE_HOVER_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "PANEL_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.PANEL_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "PANEL_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.PANEL_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "TEXT_PANEL_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.TEXT_PANEL_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "TEXT_PANEL_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.TEXT_PANEL_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "TEXT_PANEL_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.TEXT_PANEL_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "TEXT_PANEL_HOVER_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.TEXT_PANEL_HOVER_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "TEXT_PANEL_HOVER_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.TEXT_PANEL_HOVER_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "TEXT_PANEL_HOVER_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.TEXT_PANEL_HOVER_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_OFF_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_OFF_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_OFF_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_OFF_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_OFF_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_OFF_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_OFF_HOVER_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_OFF_HOVER_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_OFF_HOVER_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_OFF_HOVER_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_OFF_HOVER_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_OFF_HOVER_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_RED_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_RED_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_RED_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_RED_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_RED_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_RED_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_RED_HOVER_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_RED_HOVER_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_RED_HOVER_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_RED_HOVER_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_RED_HOVER_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_RED_HOVER_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_GREEN_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_GREEN_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_GREEN_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_GREEN_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_GREEN_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_GREEN_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_GREEN_HOVER_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_GREEN_HOVER_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_GREEN_HOVER_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_GREEN_HOVER_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_GREEN_HOVER_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_GREEN_HOVER_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_YELLOW_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_YELLOW_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_YELLOW_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_YELLOW_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_YELLOW_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_YELLOW_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_YELLOW_HOVER_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_YELLOW_HOVER_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_YELLOW_HOVER_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_YELLOW_HOVER_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_YELLOW_HOVER_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_YELLOW_HOVER_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_BLUE_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_BLUE_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_BLUE_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_BLUE_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_BLUE_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_BLUE_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_BLUE_HOVER_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_BLUE_HOVER_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_BLUE_HOVER_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_BLUE_HOVER_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_BLUE_HOVER_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_BLUE_HOVER_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_PURPLE_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_PURPLE_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_PURPLE_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_PURPLE_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_PURPLE_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_PURPLE_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_PURPLE_HOVER_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_PURPLE_HOVER_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_PURPLE_HOVER_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_PURPLE_HOVER_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_PURPLE_HOVER_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_PURPLE_HOVER_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_TURQUOISE_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_TURQUOISE_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_TURQUOISE_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_TURQUOISE_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_TURQUOISE_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_TURQUOISE_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_TURQUOISE_HOVER_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_TURQUOISE_HOVER_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_TURQUOISE_HOVER_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_TURQUOISE_HOVER_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_TURQUOISE_HOVER_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_TURQUOISE_HOVER_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_WHITE_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_WHITE_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_WHITE_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_WHITE_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_WHITE_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_WHITE_TEXT_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_WHITE_HOVER_FILL_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_WHITE_HOVER_FILL_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_WHITE_HOVER_STROKE_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_WHITE_HOVER_STROKE_COLOR);
                }
                else if (string.Equals(lineParams[0], "BUTTON_LED_WHITE_HOVER_TEXT_COLOR", StringComparison.CurrentCultureIgnoreCase) == true)
                {
                    ParseUnsignedNumber(lineParams[1], ref theme.BUTTON_LED_WHITE_HOVER_TEXT_COLOR);
                }
            }
            return theme;
        }
    }
}
