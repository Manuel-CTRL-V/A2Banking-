using System;
using System.Drawing;

namespace BackOffice.Helpers
{
    public enum ThemeType { Light, Dark }

    public static class BackOfficeColors
    {
        private static ThemeType _currentTheme = ThemeType.Light;
        public static event Action<ThemeType> ThemeChanged;

        public static ThemeType CurrentTheme => _currentTheme;

        public static void SetTheme(ThemeType theme)
        {
            _currentTheme = theme;
            ThemeChanged?.Invoke(theme);
        }

        public static void ToggleTheme()
        {
            SetTheme(_currentTheme == ThemeType.Light ? ThemeType.Dark : ThemeType.Light);
        }

        public static Color Background => _currentTheme == ThemeType.Light ? BackgroundLight : BackgroundDark;
        public static Color BackgroundSecondary => _currentTheme == ThemeType.Light ? BackgroundSecondaryLight : BackgroundSecondaryDark;
        public static Color CardBackground => _currentTheme == ThemeType.Light ? CardBackgroundLight : CardBackgroundDark;
        public static Color TextPrimary => _currentTheme == ThemeType.Light ? TextPrimaryLight : TextPrimaryDark;
        public static Color TextSecondary => _currentTheme == ThemeType.Light ? TextSecondaryLight : TextSecondaryDark;
        public static Color TextError => _currentTheme == ThemeType.Light ? TextErrorLight : TextErrorDark;
        public static Color TextSuccess => _currentTheme == ThemeType.Light ? TextSuccessLight : TextSuccessDark;
        public static Color Border => _currentTheme == ThemeType.Light ? BorderLight : BorderDark;
        public static Color Accent => _currentTheme == ThemeType.Light ? AccentLight : AccentDark;
        public static Color MenuBackground => _currentTheme == ThemeType.Light ? MenuBackgroundLight : MenuBackgroundDark;
        public static Color MenuText => _currentTheme == ThemeType.Light ? MenuTextLight : MenuTextDark;
        public static Color ButtonPrimary => _currentTheme == ThemeType.Light ? ButtonPrimaryLight : ButtonPrimaryDark;
        public static Color ButtonPrimaryText => _currentTheme == ThemeType.Light ? ButtonPrimaryTextLight : ButtonPrimaryTextDark;
        public static Color ButtonDanger => _currentTheme == ThemeType.Light ? ButtonDangerLight : ButtonDangerDark;
        public static Color InputBackground => _currentTheme == ThemeType.Light ? InputBackgroundLight : InputBackgroundDark;
        public static Color InputText => _currentTheme == ThemeType.Light ? InputTextLight : InputTextDark;

        // Light Theme
        private static readonly Color BackgroundLight = Color.FromArgb(234, 239, 236);
        private static readonly Color BackgroundSecondaryLight = Color.FromArgb(200, 220, 200);
        private static readonly Color CardBackgroundLight = Color.FromArgb(255, 255, 255);
        private static readonly Color TextPrimaryLight = Color.FromArgb(26, 35, 126);
        private static readonly Color TextSecondaryLight = Color.FromArgb(100, 100, 100);
        private static readonly Color TextErrorLight = Color.FromArgb(229, 57, 53);
        private static readonly Color TextSuccessLight = Color.FromArgb(27, 94, 32);
        private static readonly Color BorderLight = Color.FromArgb(200, 200, 200);
        private static readonly Color AccentLight = Color.FromArgb(54, 92, 57);
        private static readonly Color MenuBackgroundLight = Color.FromArgb(234, 239, 236);
        private static readonly Color MenuTextLight = Color.FromArgb(0, 0, 0);
        private static readonly Color ButtonPrimaryLight = Color.FromArgb(54, 92, 57);
        private static readonly Color ButtonPrimaryTextLight = Color.FromArgb(255, 255, 255);
        private static readonly Color ButtonDangerLight = Color.FromArgb(229, 57, 53);
        private static readonly Color InputBackgroundLight = Color.FromArgb(241, 244, 242);
        private static readonly Color InputTextLight = Color.FromArgb(60, 60, 60);

        // Dark Theme
        private static readonly Color BackgroundDark = Color.FromArgb(30, 30, 30);
        private static readonly Color BackgroundSecondaryDark = Color.FromArgb(45, 45, 45);
        private static readonly Color CardBackgroundDark = Color.FromArgb(40, 40, 40);
        private static readonly Color TextPrimaryDark = Color.FromArgb(255, 255, 255);
        private static readonly Color TextSecondaryDark = Color.FromArgb(180, 180, 180);
        private static readonly Color TextErrorDark = Color.FromArgb(255, 82, 82);
        private static readonly Color TextSuccessDark = Color.FromArgb(105, 240, 174);
        private static readonly Color BorderDark = Color.FromArgb(60, 60, 60);
        private static readonly Color AccentDark = Color.FromArgb(0, 200, 83);
        private static readonly Color MenuBackgroundDark = Color.FromArgb(35, 35, 35);
        private static readonly Color MenuTextDark = Color.FromArgb(255, 255, 255);
        private static readonly Color ButtonPrimaryDark = Color.FromArgb(0, 200, 83);
        private static readonly Color ButtonPrimaryTextDark = Color.FromArgb(20, 20, 20);
        private static readonly Color ButtonDangerDark = Color.FromArgb(255, 23, 68);
        private static readonly Color InputBackgroundDark = Color.FromArgb(50, 50, 50);
        private static readonly Color InputTextDark = Color.FromArgb(220, 220, 220);
    }
}
