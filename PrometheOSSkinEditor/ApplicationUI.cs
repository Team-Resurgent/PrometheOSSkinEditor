using ImGuiNET;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Diagnostics;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;

namespace PrometheOSSkinEditor
{
    public unsafe class ApplicationUI
    {
        enum PreviewModeEnum
        {
            General_1 = 0,
            General_2 = 1,
            WideMenu = 2,
            Led_1 = 3,
            Led_2 = 4,
            Led_3 = 5,
            Led_4 = 6,
            Snake = 7
        }

        private struct Background
        {
            public int BackgroundTextureId;
            public byte[] BackgroundData;
        }

        private Window m_window;
        private PathPicker? m_backgroundFileOpenPicker;
        private PathPicker? m_overlayFileOpenPicker;
        private PathPicker? m_backgroundFolderOpenPicker;
        private PathPicker? m_themeFileOpenPicker;
        private PathPicker? m_themeFileSavePicker;
        private SplashDialog m_splashDialog = new();
        private Settings m_settings = new();
        private Theme m_theme = new();
        private bool m_showSplash = true;
        private string m_version;
        private int m_frameIndex = 0;
        private float m_totalTime = 0;
        private PreviewModeEnum m_previewMode = PreviewModeEnum.General_1;

        private List<Background> m_Backgrounds = new List<Background>();
        private ulong m_backgroundMemUsed = 0;
        private int m_OverlayTextureId;
        private byte[] m_OverlayData = Array.Empty<byte>();
        private bool m_OverlayLoaded;

        static List<Vector2> CreateRoundedEdgeRectangle(Vector2 pos, Vector2 size, float radius)
        {
            var vertices = new List<Vector2>();

            // Top-left corner
            AddArc(vertices, pos.X + radius, pos.Y + radius, radius, 180, 90);

            // Top edge
            AddLine(vertices, pos.X + radius, pos.Y, pos.X + size.X - radius, pos.Y);

            // Top-right corner
            AddArc(vertices, pos.X + size.X - radius, pos.Y + radius, radius, 270, 90);

            // Right edge
            AddLine(vertices, pos.X + size.X, pos.Y + radius, pos.X + size.X, pos.Y + size.Y - radius);

            // Bottom-right corner
            AddArc(vertices, pos.X + size.X - radius, pos.Y + size.Y - radius, radius, 0, 90);

            // Bottom edge
            AddLine(vertices, pos.X + size.X - radius, pos.Y + size.Y, pos.X + radius, pos.Y + size.Y);

            // Bottom-left corner
            AddArc(vertices, pos.X + radius, pos.Y + size.Y - radius, radius, 90, 90);

            // Left edge
            AddLine(vertices, pos.X, pos.Y + size.Y - radius, pos.X, pos.Y + radius);

            vertices.Add(vertices[0]);

            return vertices;
        }

        static void AddLine(List<Vector2> vertices, float startX, float startY, float endX, float endY)
        {
            vertices.Add(new Vector2(startX, startY));
            vertices.Add(new Vector2(endX, endY));
        }

        static void AddArc(List<Vector2> vertices, float centerX, float centerY, float radius, float startAngle, float sweepAngle)
        {
            int numSegments = 20; // Adjust the number of segments for smoother curves
            for (int i = 0; i <= numSegments; i++)
            {
                float angle = ToRadians(startAngle + (sweepAngle / numSegments) * i);
                float x = centerX + radius * (float)Math.Cos(angle);
                float y = centerY + radius * (float)Math.Sin(angle);
                vertices.Add(new Vector2(x, y));
            }
        }

        public static float ToRadians(float degrees)
        {
            return degrees * (float)Math.PI / 180.0f;
        }

        private static void DrawPanel(Vector2 pos, Vector2 size, uint fillColor, uint strokeColor)
        {
            //new Vector2(9, 35)
            var xPos = pos.X + 9;
            var yPos = pos.Y + 35;

            var rectangle = CreateRoundedEdgeRectangle(new Vector2(xPos, yPos), size, 2).ToArray();
            var drawList = ImGui.GetWindowDrawList();
            drawList.AddConvexPolyFilled(ref rectangle[0], rectangle.Length, Theme.ConvertARGBtoABGR(fillColor));
            drawList.AddPolyline(ref rectangle[0], rectangle.Length, Theme.ConvertARGBtoABGR(strokeColor), ImDrawFlags.RoundCornersDefault, 2);
        }

        // 0 = app font
        // 1 = theme font small
        // 2 = theme font medium
        // 3 = theme font large
        // 4 = theme font controller
        // 5 = theme font crossover

        private static void DrawButton(Vector2 pos, Vector2 size, string label, uint fillColor, uint strokeColor, uint textColor)
        {
            DrawPanel(pos, size, fillColor, strokeColor);

            var xPos = pos.X + 9;
            var yPos = pos.Y + 35 + 2;

            ImGui.PushFont(ImGui.GetIO().Fonts.Fonts[1]);
            var originalPos = ImGui.GetCursorPos();
            var textSize = ImGui.CalcTextSize(label);
            ImGui.SetCursorPos(new Vector2(xPos + ((size.X - textSize.X) / 2), yPos - (size.Y - (textSize.Y / 2))));
            ImGui.PushStyleColor(ImGuiCol.Text, Theme.ConvertARGBtoABGR(textColor));
            ImGui.Text(label);
            ImGui.PopStyleColor();
            ImGui.SetCursorPos(originalPos);
            ImGui.PopFont();
        }

        private static void DrawAllignedText(Vector2 pos, uint width, Alignment alignment, string label, uint fontIndex, uint textColor)
        {
            ImGui.PushFont(ImGui.GetIO().Fonts.Fonts[(int)fontIndex]);

            var originalPos = ImGui.GetCursorPos();
            var textSize = ImGui.CalcTextSize(label);

            var yPos = pos.Y + (textSize.Y / 2);
            var xPos = pos.X + 16;

            if (fontIndex == 4)
            {
                yPos += 7;
            }

            if (alignment == Alignment.Center)
            {
                xPos = pos.X + ((width - textSize.X) / 2);
            }
            else if (alignment == Alignment.Right)
            {
                xPos = (pos.X + width) - textSize.X;
            }
            ImGui.SetCursorPos(new Vector2(xPos, yPos));
            ImGui.PushStyleColor(ImGuiCol.Text, Theme.ConvertARGBtoABGR(textColor));
            ImGui.Text(label);
            ImGui.PopStyleColor();
            ImGui.SetCursorPos(originalPos);
            ImGui.PopFont();
        }

        private static void DrawImage(int textureId, Vector2 pos, Vector2 size, uint tint)
        {
            var origCursor = ImGui.GetCursorPos();
            ImGui.SetCursorPos(pos + new Vector2(9, 11));
            ImGui.Image(textureId, size, Vector2.Zero, Vector2.One, ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(tint)));
            ImGui.SetCursorPos(origCursor);
        }

        private void DisposeBackgroundImages()
        {
            foreach (var b in m_Backgrounds)
            {
                m_window.Controller.DisposeOwnedTexture(b.BackgroundTextureId);
            }
            m_Backgrounds.Clear();
            m_backgroundMemUsed = 0;
        }

        private void DisposeOverlayImage()
        {
            m_window.Controller.DisposeOwnedTexture(m_OverlayTextureId);
            m_OverlayData = Array.Empty<byte>();
            m_OverlayLoaded = false;
        }

        private void LoadBackgroundImages(string imagePath)
        {
            DisposeBackgroundImages();

            if (File.Exists(imagePath))
            {
                try
                {
                    Background background;
                    background.BackgroundTextureId = m_window.Controller.CreateTextureFromFile(imagePath, out var width, out var height);
                    background.BackgroundData = File.ReadAllBytes(imagePath);
                    m_Backgrounds.Add(background);
                    m_backgroundMemUsed += (Utility.RoundUpToNextPowerOf2((uint)width) * Utility.RoundUpToNextPowerOf2((uint)height) * 4);
                }
                catch
                {
                    // Ignore invalid file
                }
            }
            else if (Directory.Exists(imagePath))
            {
                var files = Directory.GetFiles(imagePath);
                foreach (var f in files)
                {
                    try
                    {
                        Background background;
                        background.BackgroundTextureId = m_window.Controller.CreateTextureFromFile(f, out var width, out var height);
                        background.BackgroundData = File.ReadAllBytes(f);
                        m_Backgrounds.Add(background);
                        m_backgroundMemUsed += (Utility.RoundUpToNextPowerOf2((uint)width) * Utility.RoundUpToNextPowerOf2((uint)height) * 4);
                    }
                    catch
                    {
                        // Ignore invalid file
                    }
                }
            }
        }

        private void LoadOverlayImage(string imagePath)
        {
            DisposeOverlayImage();

            try
            {
                m_OverlayTextureId = m_window.Controller.CreateTextureFromFile(imagePath, out var width, out var height);
                m_OverlayData = File.ReadAllBytes(imagePath);
                m_OverlayLoaded = true;
            }
            catch
            {
                // Ignore invalid file
            }
        }

        [DllImport("dwmapi.dll", PreserveSig = true)]
        public static extern int DwmSetWindowAttribute(IntPtr hwnd, uint attr, ref int attrValue, int attrSize);

        public ApplicationUI(string version)
        {
            m_window = new Window();
            m_version = version;
        }

        private Vector2 GetScaledWindowSize()
        {
            return new Vector2(m_window.Size.X, m_window.Size.Y) / m_window.Controller.GetScaleFactor();
        }

        public void OpenUrl(string url)
        {
            try
            {
                if (OperatingSystem.IsWindows())
                {
                    Process.Start("cmd", "/C start" + " " + url);
                }
                else if (OperatingSystem.IsLinux())
                {
                    Process.Start("xdg-open", url);
                }
                else if (OperatingSystem.IsMacOS())
                {
                    Process.Start("open", url);
                }
            }
            catch
            {
                // do nothing
            }
        }

        public void Run()
        {
            m_window.Title = $"PrometheOS Skin Editor - {m_version} (Team Resurgent)";
            m_window.Size = new OpenTK.Mathematics.Vector2i(1018, 564);
            m_window.WindowBorder = OpenTK.Windowing.Common.WindowBorder.Fixed;
            m_window.VSync = OpenTK.Windowing.Common.VSyncMode.On;

            var resourceBytes = ResourceLoader.GetEmbeddedResourceBytes("PrometheOSSkinEditor.Resources.icon.png");
            using var resourceImage = SixLabors.ImageSharp.Image.Load<Rgba32>(resourceBytes);
            var pixelSpan = new Span<Rgba32>(new Rgba32[resourceImage.Width * resourceImage.Height]);
            resourceImage.CopyPixelDataTo(pixelSpan);
            var byteSpan = MemoryMarshal.AsBytes(pixelSpan);
            var iconImage = new OpenTK.Windowing.Common.Input.Image(resourceImage.Width, resourceImage.Height, byteSpan.ToArray());
            m_window.Icon = new OpenTK.Windowing.Common.Input.WindowIcon(iconImage);

            if (OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000, 0))
            {
                int value = -1;
                uint DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
                _ = DwmSetWindowAttribute(GLFW.GetWin32Window(m_window.WindowPtr), DWMWA_USE_IMMERSIVE_DARK_MODE, ref value, sizeof(int));
            }

            m_settings = Settings.LoadSettings();

            m_backgroundFileOpenPicker = new PathPicker
            {
                Title = "Load Background File",
                Mode = PathPicker.PickerMode.FileOpen,
                AllowedFiles = new[] { "*.jpg", "*.png" },
                ButtonName = "Load"
            };

            m_overlayFileOpenPicker = new PathPicker
            {
                Title = "Load Background File",
                Mode = PathPicker.PickerMode.FileOpen,
                AllowedFiles = new[] { "*.png" },
                ButtonName = "Load"
            };

            m_backgroundFolderOpenPicker = new PathPicker
            {
                Title = "Load Backgrounds From Folder",
                Mode = PathPicker.PickerMode.Folder,
                ButtonName = "Load"
            };

            m_themeFileOpenPicker = new PathPicker
            {
                Title = "Load Theme",
                Mode = PathPicker.PickerMode.FileOpen,
                AllowedFiles = new[] { "*.ini" },
                ButtonName = "Load"
            };

            m_themeFileSavePicker = new PathPicker
            {
                Title = "Save Theme",
                Mode = PathPicker.PickerMode.Folder,
                ButtonName = "Save"
            };

            m_window.RenderUI = RenderUI;
            m_window.Run();
        }

        private void RenderUI(float dt)
        {
            if (m_backgroundFileOpenPicker == null ||
                m_overlayFileOpenPicker == null ||
                m_backgroundFolderOpenPicker == null ||
                m_themeFileOpenPicker == null ||
                m_themeFileSavePicker == null)
            {
                return;
            }

            if (m_Backgrounds.Count() > 0)
            {
                m_totalTime += dt;
                if (m_totalTime > m_theme.BACKGROUND_FRAME_DELAY)
                {
                    m_frameIndex = (m_frameIndex + 1) % m_Backgrounds.Count();
                    m_totalTime = 0;
                }
            }
            else
            {
                m_frameIndex = 0;
                m_totalTime = 0;
            }

            if (m_backgroundFileOpenPicker.Render() && !m_backgroundFileOpenPicker.Cancelled)
            {
                m_settings.LastPath = m_backgroundFileOpenPicker.SelectedFolder;
                Settings.SaveSattings(m_settings);

                var loadPath = Path.Combine(m_backgroundFileOpenPicker.SelectedFolder, m_backgroundFileOpenPicker.SelectedFile);
                LoadBackgroundImages(loadPath);
            }

            if (m_backgroundFolderOpenPicker.Render() && !m_backgroundFolderOpenPicker.Cancelled)
            {
                m_settings.LastPath = m_backgroundFileOpenPicker.SelectedFolder;
                Settings.SaveSattings(m_settings);

                LoadBackgroundImages(m_backgroundFolderOpenPicker.SelectedFolder);
            }

            if (m_overlayFileOpenPicker.Render() && !m_overlayFileOpenPicker.Cancelled)
            {
                m_settings.LastPath = m_overlayFileOpenPicker.SelectedFolder;
                Settings.SaveSattings(m_settings);

                var loadPath = Path.Combine(m_overlayFileOpenPicker.SelectedFolder, m_overlayFileOpenPicker.SelectedFile);
                LoadOverlayImage(loadPath);
            }

            if (m_themeFileOpenPicker.Render() && !m_themeFileOpenPicker.Cancelled)
            {
                m_settings.LastPath = m_themeFileOpenPicker.SelectedFolder;
                Settings.SaveSattings(m_settings);

                m_theme = Theme.LoadTheme(Path.Combine(m_themeFileOpenPicker.SelectedFolder, m_themeFileOpenPicker.SelectedFile));
                var themeBackgroundPath = Path.Combine(m_themeFileOpenPicker.SelectedFolder, "backgrounds");
                LoadBackgroundImages(themeBackgroundPath);
            }

            if (m_themeFileSavePicker.Render() && !m_themeFileSavePicker.Cancelled && string.IsNullOrEmpty(m_settings.LastPath) == false)
            {
                var themeFolderPath = Path.Combine(m_themeFileSavePicker.SelectedFolder, m_themeFileSavePicker.SaveName);
                m_theme.SaveTheme(themeFolderPath);

                if (m_Backgrounds.Count > 0)
                {
                    var backgroundsPath = Path.Combine(themeFolderPath, "backgrounds");
                    if (Directory.Exists(backgroundsPath) == false)
                    {
                        Directory.CreateDirectory(backgroundsPath);
                    }
                    else
                    {
                        string[] backgroundFiles = Directory.GetFiles(backgroundsPath);
                        foreach (var backgroundFile in backgroundFiles)
                        {
                            File.Delete(backgroundFile);
                        }
                    }

                    for (int i = 0; i < m_Backgrounds.Count; i++)
                    {
                        Background b = m_Backgrounds[i];
                        using var backgroundImage = SixLabors.ImageSharp.Image.Load<Rgba32>(b.BackgroundData);
                        var backgroundSavePath = Path.Combine(backgroundsPath, m_Backgrounds.Count == 1 ? "background.png" : $"background{i + 1}.png");
                        backgroundImage.Save(backgroundSavePath);
                    }
                }

                if (m_OverlayLoaded == true)
                {
                    using var overlayImage = SixLabors.ImageSharp.Image.Load<Rgba32>(m_OverlayData);
                    var backgroundOverlaySavePath = Path.Combine(themeFolderPath, "background-overlay.png");
                    overlayImage.Save(backgroundOverlaySavePath);
                }
            }

            m_splashDialog.Render();

            if (m_showSplash)
            {
                m_showSplash = false;
                m_splashDialog.ShowdDialog(m_window.Controller.SplashTexture);
            }

            ImGui.Begin("Main", ImGuiWindowFlags.NoMove | ImGuiWindowFlags.NoTitleBar | ImGuiWindowFlags.NoResize);
            ImGui.SetWindowSize(GetScaledWindowSize());
            ImGui.SetWindowPos(new Vector2(0, 24), ImGuiCond.Always);

            if (ImGui.BeginMainMenuBar())
            {
                if (ImGui.BeginMenu("File"))
                {
                    if (ImGui.MenuItem("Load Theme", "CTRL+L"))
                    {
                        var path = Directory.Exists(m_settings.LastPath) ? m_settings.LastPath : Directory.GetCurrentDirectory();
                        m_themeFileOpenPicker.ShowModal(path);
                    }

                    if (ImGui.MenuItem("Save Theme", "CTRL+S"))
                    {
                        var path = Directory.Exists(m_settings.LastPath) ? m_settings.LastPath : Directory.GetCurrentDirectory();
                        m_themeFileSavePicker.ShowModal(path);
                    }

                    ImGui.EndMenu();
                }


                ImGui.EndMainMenuBar();
            }

            ImGui.Spacing();

            var drawList = ImGui.GetWindowDrawList();

            drawList.AddRect(new Vector2(8, 34), new Vector2(8 + 722, 34 + 482), Theme.ConvertARGBtoABGR(0x80ffffff));
            drawList.AddRectFilled(new Vector2(9, 35), new Vector2(9 + 720, 35 + 480), Theme.ConvertARGBtoABGR(m_theme.BACKGROUND_COLOR));

            if (m_Backgrounds.Count() > 0 && m_frameIndex < m_Backgrounds.Count())
            {
                DrawImage(m_Backgrounds[m_frameIndex].BackgroundTextureId, new Vector2(0, 0), new Vector2(720, 480), Theme.ConvertARGBtoABGR(m_theme.BACKGROUND_IMAGE_TINT));
            }

            if (m_OverlayLoaded)
            {
                DrawImage(m_OverlayTextureId, new Vector2(0, 0), new Vector2(720, 480), Theme.ConvertARGBtoABGR(m_theme.BACKGROUND_OVERLAY_IMAGE_TINT));
            }

            DrawPanel(new Vector2(16, 16), new Vector2(688, 448), m_theme.PANEL_FILL_COLOR, m_theme.PANEL_STROKE_COLOR);

            if (m_previewMode == PreviewModeEnum.General_1)
            {
                DrawAllignedText(new Vector2(40, m_theme.PROMETHEOS_Y), 640, m_theme.PROMETHEOS_ALIGN, "PROMETHEOS", 5, m_theme.PROMETHEOS_COLOR);

                DrawButton(new Vector2(260, 125 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "Button Active", m_theme.BUTTON_ACTIVE_FILL_COLOR, m_theme.BUTTON_ACTIVE_STROKE_COLOR, m_theme.BUTTON_ACTIVE_TEXT_COLOR);
                DrawButton(new Vector2(260, 205 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "Button Active Hover", m_theme.BUTTON_ACTIVE_HOVER_FILL_COLOR, m_theme.BUTTON_ACTIVE_HOVER_STROKE_COLOR, m_theme.BUTTON_ACTIVE_HOVER_TEXT_COLOR);
                DrawButton(new Vector2(260, 165 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "Button Inactive", m_theme.BUTTON_INACTIVE_FILL_COLOR, m_theme.BUTTON_INACTIVE_STROKE_COLOR, m_theme.BUTTON_INACTIVE_TEXT_COLOR);
                DrawButton(new Vector2(260, 245 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "Button Inactive Hover", m_theme.BUTTON_INACTIVE_HOVER_FILL_COLOR, m_theme.BUTTON_INACTIVE_HOVER_STROKE_COLOR, m_theme.BUTTON_INACTIVE_HOVER_TEXT_COLOR);
                DrawButton(new Vector2(260, 285 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "Text Button", m_theme.TEXT_PANEL_FILL_COLOR, m_theme.TEXT_PANEL_STROKE_COLOR, m_theme.TEXT_PANEL_TEXT_COLOR);
                DrawButton(new Vector2(260, 325 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "Text Button Hover", m_theme.TEXT_PANEL_HOVER_FILL_COLOR, m_theme.TEXT_PANEL_HOVER_STROKE_COLOR, m_theme.TEXT_PANEL_HOVER_TEXT_COLOR);
            }
            else if (m_previewMode == PreviewModeEnum.General_2)
            {
                DrawAllignedText(new Vector2(40, m_theme.HEADER_Y), 640, m_theme.HEADER_ALIGN, "Header Text", 3, m_theme.HEADER_TEXT_COLOR);

                DrawImage(m_window.Controller.InstallerTexture, new Vector2(269, 175), new Vector2(178, 46), m_theme.INSTALLER_TINT);

                DrawAllignedText(new Vector2(260, 235 + m_theme.CENTER_OFFSET), 200, Alignment.Center, "Text", 2, m_theme.TEXT_COLOR);
                DrawAllignedText(new Vector2(260, 275 + m_theme.CENTER_OFFSET), 200, Alignment.Center, "Text Disabled", 2, m_theme.TEXT_DISABLED_COLOR);
            }
            else if (m_previewMode == PreviewModeEnum.WideMenu)
            {
                DrawAllignedText(new Vector2(40, m_theme.HEADER_Y), 640, m_theme.HEADER_ALIGN, "Header Text", 3, m_theme.HEADER_TEXT_COLOR);

                DrawButton(new Vector2(40, 105 + m_theme.CENTER_OFFSET), new Vector2(640, 30), "Button Inactive", m_theme.BUTTON_INACTIVE_FILL_COLOR, m_theme.BUTTON_INACTIVE_STROKE_COLOR, m_theme.BUTTON_INACTIVE_TEXT_COLOR);
                DrawButton(new Vector2(40, 145 + m_theme.CENTER_OFFSET), new Vector2(640, 30), "Button Inactive", m_theme.BUTTON_INACTIVE_FILL_COLOR, m_theme.BUTTON_INACTIVE_STROKE_COLOR, m_theme.BUTTON_INACTIVE_TEXT_COLOR);
                DrawButton(new Vector2(40, 185 + m_theme.CENTER_OFFSET), new Vector2(640, 30), "Button Inactive", m_theme.BUTTON_INACTIVE_FILL_COLOR, m_theme.BUTTON_INACTIVE_STROKE_COLOR, m_theme.BUTTON_INACTIVE_TEXT_COLOR);
                DrawButton(new Vector2(40, 225 + m_theme.CENTER_OFFSET), new Vector2(640, 30), "Button Inactive Hover", m_theme.BUTTON_INACTIVE_HOVER_FILL_COLOR, m_theme.BUTTON_INACTIVE_HOVER_STROKE_COLOR, m_theme.BUTTON_INACTIVE_HOVER_TEXT_COLOR);
                DrawButton(new Vector2(40, 265 + m_theme.CENTER_OFFSET), new Vector2(640, 30), "Button Inactive", m_theme.BUTTON_INACTIVE_FILL_COLOR, m_theme.BUTTON_INACTIVE_STROKE_COLOR, m_theme.BUTTON_INACTIVE_TEXT_COLOR);
                DrawButton(new Vector2(40, 305 + m_theme.CENTER_OFFSET), new Vector2(640, 30), "Button Inactive", m_theme.BUTTON_INACTIVE_FILL_COLOR, m_theme.BUTTON_INACTIVE_STROKE_COLOR, m_theme.BUTTON_INACTIVE_TEXT_COLOR);
                DrawButton(new Vector2(40, 345 + m_theme.CENTER_OFFSET), new Vector2(640, 30), "Button Inactive", m_theme.BUTTON_INACTIVE_FILL_COLOR, m_theme.BUTTON_INACTIVE_STROKE_COLOR, m_theme.BUTTON_INACTIVE_TEXT_COLOR);
            }
            else if (m_previewMode == PreviewModeEnum.Led_1)
            {
                DrawAllignedText(new Vector2(40, m_theme.HEADER_Y), 640, m_theme.HEADER_ALIGN, "Header Text", 3, m_theme.HEADER_TEXT_COLOR);

                DrawButton(new Vector2(260, 165 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED Off", m_theme.BUTTON_LED_OFF_FILL_COLOR, m_theme.BUTTON_LED_OFF_STROKE_COLOR, m_theme.BUTTON_LED_OFF_TEXT_COLOR);
                DrawButton(new Vector2(260, 205 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED Off Hover", m_theme.BUTTON_LED_OFF_HOVER_FILL_COLOR, m_theme.BUTTON_LED_OFF_HOVER_STROKE_COLOR, m_theme.BUTTON_LED_OFF_HOVER_TEXT_COLOR);
                DrawButton(new Vector2(260, 245 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED Red", m_theme.BUTTON_LED_RED_FILL_COLOR, m_theme.BUTTON_LED_RED_STROKE_COLOR, m_theme.BUTTON_LED_RED_TEXT_COLOR);
                DrawButton(new Vector2(260, 285 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED Red Hover", m_theme.BUTTON_LED_RED_HOVER_FILL_COLOR, m_theme.BUTTON_LED_RED_HOVER_STROKE_COLOR, m_theme.BUTTON_LED_RED_HOVER_TEXT_COLOR);
            }
            else if (m_previewMode == PreviewModeEnum.Led_2)
            {
                DrawAllignedText(new Vector2(40, m_theme.HEADER_Y), 640, m_theme.HEADER_ALIGN, "Header Text", 3, m_theme.HEADER_TEXT_COLOR);

                DrawButton(new Vector2(260, 165 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED Green", m_theme.BUTTON_LED_GREEN_FILL_COLOR, m_theme.BUTTON_LED_GREEN_STROKE_COLOR, m_theme.BUTTON_LED_GREEN_TEXT_COLOR);
                DrawButton(new Vector2(260, 205 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED Green Hover", m_theme.BUTTON_LED_GREEN_HOVER_FILL_COLOR, m_theme.BUTTON_LED_GREEN_HOVER_STROKE_COLOR, m_theme.BUTTON_LED_GREEN_HOVER_TEXT_COLOR);
                DrawButton(new Vector2(260, 245 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED Yellow", m_theme.BUTTON_LED_YELLOW_FILL_COLOR, m_theme.BUTTON_LED_YELLOW_STROKE_COLOR, m_theme.BUTTON_LED_YELLOW_TEXT_COLOR);
                DrawButton(new Vector2(260, 285 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED Yellow Hover", m_theme.BUTTON_LED_YELLOW_HOVER_FILL_COLOR, m_theme.BUTTON_LED_YELLOW_HOVER_STROKE_COLOR, m_theme.BUTTON_LED_YELLOW_HOVER_TEXT_COLOR);
            }
            else if (m_previewMode == PreviewModeEnum.Led_3)
            {
                DrawAllignedText(new Vector2(40, m_theme.HEADER_Y), 640, m_theme.HEADER_ALIGN, "Header Text", 3, m_theme.HEADER_TEXT_COLOR);

                DrawButton(new Vector2(260, 165 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED Blue", m_theme.BUTTON_LED_BLUE_FILL_COLOR, m_theme.BUTTON_LED_BLUE_STROKE_COLOR, m_theme.BUTTON_LED_BLUE_TEXT_COLOR);
                DrawButton(new Vector2(260, 205 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED Blue Hover", m_theme.BUTTON_LED_BLUE_HOVER_FILL_COLOR, m_theme.BUTTON_LED_BLUE_HOVER_STROKE_COLOR, m_theme.BUTTON_LED_BLUE_HOVER_TEXT_COLOR);
                DrawButton(new Vector2(260, 245 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED Purple", m_theme.BUTTON_LED_PURPLE_FILL_COLOR, m_theme.BUTTON_LED_PURPLE_STROKE_COLOR, m_theme.BUTTON_LED_PURPLE_TEXT_COLOR);
                DrawButton(new Vector2(260, 285 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED Purple Hover", m_theme.BUTTON_LED_PURPLE_HOVER_FILL_COLOR, m_theme.BUTTON_LED_PURPLE_HOVER_STROKE_COLOR, m_theme.BUTTON_LED_PURPLE_HOVER_TEXT_COLOR);
            }
            else if (m_previewMode == PreviewModeEnum.Led_4)
            {
                DrawAllignedText(new Vector2(40, m_theme.HEADER_Y), 640, m_theme.HEADER_ALIGN, "Header Text", 3, m_theme.HEADER_TEXT_COLOR);

                DrawButton(new Vector2(260, 165 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED Turquoise", m_theme.BUTTON_LED_TURQUOISE_FILL_COLOR, m_theme.BUTTON_LED_TURQUOISE_STROKE_COLOR, m_theme.BUTTON_LED_TURQUOISE_TEXT_COLOR);
                DrawButton(new Vector2(260, 205 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED Turquoise Hover", m_theme.BUTTON_LED_TURQUOISE_HOVER_FILL_COLOR, m_theme.BUTTON_LED_TURQUOISE_HOVER_STROKE_COLOR, m_theme.BUTTON_LED_TURQUOISE_HOVER_TEXT_COLOR);
                DrawButton(new Vector2(260, 245 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED White", m_theme.BUTTON_LED_WHITE_FILL_COLOR, m_theme.BUTTON_LED_WHITE_STROKE_COLOR, m_theme.BUTTON_LED_WHITE_TEXT_COLOR);
                DrawButton(new Vector2(260, 285 + m_theme.CENTER_OFFSET), new Vector2(200, 30), "LED White Hover", m_theme.BUTTON_LED_WHITE_HOVER_FILL_COLOR, m_theme.BUTTON_LED_WHITE_HOVER_STROKE_COLOR, m_theme.BUTTON_LED_WHITE_HOVER_TEXT_COLOR);
            }
            else if (m_previewMode == PreviewModeEnum.Snake)
            {
                DrawAllignedText(new Vector2(40, m_theme.HEADER_Y), 640, m_theme.HEADER_ALIGN, "Header Text", 3, m_theme.HEADER_TEXT_COLOR);

                int width = 58;
                int height = 16;

                int xPos = (720 - ((width + 2) * 10)) / 2;
                int yPos = (480 - ((height + 2) * 10)) / 2;
                yPos += m_theme.CENTER_OFFSET;

                for (int i = 0; i < width + 2; i++)
                {
                    DrawPanel(new Vector2(xPos + (i * 10), yPos), new Vector2(10, 10), m_theme.SNAKE_WALL_COLOR, m_theme.SNAKE_WALL_COLOR);
                    DrawPanel(new Vector2(xPos + (i * 10), yPos + (height + 1) * 10), new Vector2(10, 10), m_theme.SNAKE_WALL_COLOR, m_theme.SNAKE_WALL_COLOR);
                }

                for (int i = 0; i < height; i++)
                {
                    DrawPanel(new Vector2(xPos, yPos + ((i + 1) * 10)), new Vector2(10, 10), m_theme.SNAKE_WALL_COLOR, m_theme.SNAKE_WALL_COLOR);
                    DrawPanel(new Vector2(xPos + ((width + 1) * 10), yPos + ((i + 1) * 10)), new Vector2(10, 10), m_theme.SNAKE_WALL_COLOR, m_theme.SNAKE_WALL_COLOR);
                }

                DrawPanel(new Vector2(335, 235 + m_theme.CENTER_OFFSET), new Vector2(10, 10), m_theme.SNAKE_FOOD_COLOR, 0);
                DrawPanel(new Vector2(355, 235 + m_theme.CENTER_OFFSET), new Vector2(10, 10), m_theme.SNAKE_HEAD_COLOR, 0);
                for (int s = 0; s < 6; s++)
                {
                    DrawPanel(new Vector2(365 + (s * 10), 235 + m_theme.CENTER_OFFSET), new Vector2(10, 10), m_theme.SNAKE_TAIL_COLOR, 0);
                }
                for (int s = 0; s < 3; s++)
                {
                    DrawPanel(new Vector2(415, 245 + (s * 10) + m_theme.CENTER_OFFSET), new Vector2(10, 10), m_theme.SNAKE_TAIL_COLOR, 0);
                }
                DrawPanel(new Vector2(375, 235 + m_theme.CENTER_OFFSET), new Vector2(10, 10), m_theme.SNAKE_TAIL_COLOR, 0);
            }

            DrawAllignedText(new Vector2(40, m_theme.FOOTER_Y), 640, Alignment.Left, "a", 4, m_theme.JOY_BUTTON_A_COLOR);
            DrawAllignedText(new Vector2(80, m_theme.FOOTER_Y), 640, Alignment.Left, "b", 4, m_theme.JOY_BUTTON_B_COLOR);
            DrawAllignedText(new Vector2(120, m_theme.FOOTER_Y), 640, Alignment.Left, "x", 4, m_theme.JOY_BUTTON_X_COLOR);
            DrawAllignedText(new Vector2(160, m_theme.FOOTER_Y), 640, Alignment.Left, "y", 4, m_theme.JOY_BUTTON_Y_COLOR);
            DrawAllignedText(new Vector2(200, m_theme.FOOTER_Y), 640, Alignment.Left, "Joy Buttons", 2, m_theme.FOOTER_TEXT_COLOR);

            DrawAllignedText(new Vector2(40, m_theme.FOOTER_Y), 640, Alignment.Right, "Footer Text", 2, m_theme.FOOTER_TEXT_COLOR);

            ImGui.SetCursorPos(new Vector2(730, 10));
            ImGui.BeginChild(2, new Vector2(280, 484), true, ImGuiWindowFlags.AlwaysUseWindowPadding);

            string[] previewModeValues = Enum.GetNames(typeof(PreviewModeEnum)).Select(s => s.Replace("_", " ")).ToArray();

            var previewMode = (int)m_previewMode;
            ImGui.Text("Preview Mode:");
            ImGui.PushItemWidth(250);
            ImGui.Combo("##previewMode", ref previewMode, previewModeValues, previewModeValues.Length);
            ImGui.PopItemWidth();
            m_previewMode = (PreviewModeEnum)previewMode;

            ImGui.Spacing();
            ImGui.Separator();

            var skinAuthor = m_theme.SKIN_AUTHOR;
            ImGui.Text("Skin Author:");
            ImGui.PushItemWidth(250);
            ImGui.InputText("##skinAuthor", ref skinAuthor, 49, ImGuiInputTextFlags.None);
            ImGui.PopItemWidth();
            m_theme.SKIN_AUTHOR = skinAuthor;

            ImGui.Spacing();
            ImGui.Separator();

            ImGui.Text("Background Loading:");
            if (ImGui.Button("Load File##loadFile1", new Vector2(120, 0)))
            {
                var path = Directory.Exists(m_settings.LastPath) ? m_settings.LastPath : Directory.GetCurrentDirectory();
                m_backgroundFileOpenPicker.ShowModal(path);
            }
            ImGui.SameLine();
            if (ImGui.Button("Load Folder", new Vector2(120, 0)))
            {
                var path = Directory.Exists(m_settings.LastPath) ? m_settings.LastPath : Directory.GetCurrentDirectory();
                m_backgroundFolderOpenPicker.ShowModal(path);
            }

            if (m_Backgrounds.Count == 0)
            {
                ImGui.BeginDisabled();
            }

            if (ImGui.Button("Close Background(s)", new Vector2(250, 0)))
            {
                DisposeBackgroundImages();
            }

            uint maxMem = 20 * 1024 * 1024;
            if (m_backgroundMemUsed > maxMem)
            {
                ImGui.PushStyleColor(ImGuiCol.Text, ImGui.ColorConvertFloat4ToU32(new Vector4(1.0f, 0.25f, 0.5f, 1.0f)));
                ImGui.Text($"Mem used {m_backgroundMemUsed} of Max {maxMem}");
                ImGui.PopStyleColor();
            }
            else
            {
                ImGui.Text($"Mem used {m_backgroundMemUsed} of Max {maxMem}");
            }

            if (m_Backgrounds.Count == 0)
            {
                ImGui.EndDisabled();
            }

            ImGui.Text("Background Overlay Loading:");

            if (ImGui.Button("Load File##loadFile2", new Vector2(120, 0)))
            {
                var path = Directory.Exists(m_settings.LastPath) ? m_settings.LastPath : Directory.GetCurrentDirectory();
                m_overlayFileOpenPicker.ShowModal(path);
            }

            if (m_OverlayLoaded == false)
            {
                ImGui.BeginDisabled();
            }

            ImGui.SameLine();

            if (ImGui.Button("Close Overlay", new Vector2(120, 0)))
            {
                m_window.Controller.DisposeOwnedTexture(m_OverlayTextureId);
            }

            if (m_OverlayLoaded == false)
            {
                ImGui.EndDisabled();
            }

            ImGui.Spacing();
            ImGui.Separator();

            var backgroundFrameDelay = (int)m_theme.BACKGROUND_FRAME_DELAY;
            ImGui.Text("Background Fram Delay (ms):");
            ImGui.PushItemWidth(250);
            ImGui.InputInt("##backgroundFrameDelay", ref backgroundFrameDelay, 1, 5, ImGuiInputTextFlags.CharsDecimal);
            ImGui.PopItemWidth();
            m_theme.BACKGROUND_FRAME_DELAY = (uint)Math.Min(Math.Max(backgroundFrameDelay, 0), 65536);

            var backgroundColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BACKGROUND_COLOR));
            ImGui.Text("Background Color:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit4("##backgroundColor", ref backgroundColor, ImGuiColorEditFlags.AlphaBar);
            ImGui.PopItemWidth();
            m_theme.BACKGROUND_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(backgroundColor));

            var backgroundImageTint = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BACKGROUND_IMAGE_TINT));
            ImGui.Text("Background Image Tint:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit4("##backgroundImageTint", ref backgroundImageTint, ImGuiColorEditFlags.AlphaBar);
            ImGui.PopItemWidth();
            m_theme.BACKGROUND_IMAGE_TINT = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(backgroundImageTint));

            var backgroundOverlayImageTint = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BACKGROUND_OVERLAY_IMAGE_TINT));
            ImGui.Text("Background Overlay Image Tint:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit4("##backgroundOverlayImageTint", ref backgroundOverlayImageTint, ImGuiColorEditFlags.AlphaBar);
            ImGui.PopItemWidth();
            m_theme.BACKGROUND_OVERLAY_IMAGE_TINT = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(backgroundOverlayImageTint));

            ImGui.Spacing();
            ImGui.Separator();

            string[] alignmentValues = new string[] { "Left", "Center", "Right" };

            if (m_previewMode == PreviewModeEnum.General_1)
            {
                var prometheosAlign = (int)m_theme.PROMETHEOS_ALIGN;
                ImGui.Text("PrometheOS Align:");
                ImGui.PushItemWidth(250);
                ImGui.Combo("##prometheosAlign", ref prometheosAlign, alignmentValues, alignmentValues.Length);
                ImGui.PopItemWidth();
                m_theme.PROMETHEOS_ALIGN = (Alignment)prometheosAlign;

                var prometheosY = (int)m_theme.PROMETHEOS_Y;
                ImGui.Text("PrometheOS Y:");
                ImGui.PushItemWidth(250);
                ImGui.InputInt("##prometheosY", ref prometheosY, 1, 5, ImGuiInputTextFlags.CharsDecimal);
                ImGui.PopItemWidth();
                m_theme.PROMETHEOS_Y = (uint)Math.Min(Math.Max(prometheosY, 0), 480);
            }

            if (m_previewMode != PreviewModeEnum.General_1)
            {
                var headerAlign = (int)m_theme.HEADER_ALIGN;
                ImGui.Text("Header Align:");
                ImGui.PushItemWidth(250);
                ImGui.Combo("##headerAlign", ref headerAlign, alignmentValues, alignmentValues.Length);
                ImGui.PopItemWidth();
                m_theme.HEADER_ALIGN = (Alignment)headerAlign;

                var headerY = (int)m_theme.HEADER_Y;
                ImGui.Text("Header Y:");
                ImGui.PushItemWidth(250);
                ImGui.InputInt("##headerY", ref headerY, 1, 5, ImGuiInputTextFlags.CharsDecimal);
                ImGui.PopItemWidth();
                m_theme.HEADER_Y = (uint)Math.Min(Math.Max(headerY, 0), 480);
            }

            var centerOffset = m_theme.CENTER_OFFSET;
            ImGui.Text("Center Offset:");
            ImGui.PushItemWidth(250);
            ImGui.InputInt("##centerOffset", ref centerOffset, 1, 5, ImGuiInputTextFlags.CharsDecimal);
            ImGui.PopItemWidth();
            m_theme.CENTER_OFFSET = Math.Min(Math.Max(centerOffset, -240), 240);

            var footerY = (int)m_theme.FOOTER_Y;
            ImGui.Text("Footer Y:");
            ImGui.PushItemWidth(250);
            ImGui.InputInt("##footerY", ref footerY, 1, 5, ImGuiInputTextFlags.CharsDecimal);
            ImGui.PopItemWidth();
            m_theme.FOOTER_Y = (uint)Math.Min(Math.Max(footerY, 0), 480);

            ImGui.Spacing();
            ImGui.Separator();

            if (m_previewMode == PreviewModeEnum.General_1 || m_previewMode == PreviewModeEnum.General_2)
            {
                if (m_previewMode == PreviewModeEnum.General_1)
                {
                    var prometheosColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.PROMETHEOS_COLOR));
                    ImGui.Text("PrometheOS Color:");
                    ImGui.PushItemWidth(250);
                    ImGui.ColorEdit4("##prometheosColor", ref prometheosColor, ImGuiColorEditFlags.AlphaBar);
                    ImGui.PopItemWidth();
                    m_theme.PROMETHEOS_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(prometheosColor));
                }
                if (m_previewMode == PreviewModeEnum.General_2)
                {
                    var installerTint = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.INSTALLER_TINT));
                    ImGui.Text("Installer Tint:");
                    ImGui.PushItemWidth(250);
                    ImGui.ColorEdit4("##installerTint", ref installerTint, ImGuiColorEditFlags.AlphaBar);
                    ImGui.PopItemWidth();
                    m_theme.INSTALLER_TINT = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(installerTint));
                }
                ImGui.Spacing();
                ImGui.Separator();
            }

            if (m_previewMode == PreviewModeEnum.General_2)
            {
                var textColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.TEXT_COLOR));
                ImGui.Text("Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##textColor", ref textColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(textColor));

                var textDisabledColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.TEXT_DISABLED_COLOR));
                ImGui.Text("Text Disabled Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##textDisabledColor", ref textDisabledColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.TEXT_DISABLED_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(textDisabledColor));
            }

            if (m_previewMode != PreviewModeEnum.General_1)
            {
                var headerTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.HEADER_TEXT_COLOR));
                ImGui.Text("Title Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##headerTextColor", ref headerTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.HEADER_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(headerTextColor));
            }

            var footerTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.FOOTER_TEXT_COLOR));
            ImGui.Text("Footer Text Color:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit4("##footerTextColor", ref footerTextColor, ImGuiColorEditFlags.AlphaBar);
            ImGui.PopItemWidth();
            m_theme.FOOTER_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(footerTextColor));

            ImGui.Spacing();
            ImGui.Separator();

            if (m_previewMode == PreviewModeEnum.General_1)
            {
                var butonActiveFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_ACTIVE_FILL_COLOR));
                ImGui.Text("Button Active Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##butonActiveFillColor", ref butonActiveFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_ACTIVE_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(butonActiveFillColor));

                var butonActiveStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_ACTIVE_STROKE_COLOR));
                ImGui.Text("Button Active Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##butonActiveStrokeColor", ref butonActiveStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_ACTIVE_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(butonActiveStrokeColor));

                var butonActiveTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_ACTIVE_TEXT_COLOR));
                ImGui.Text("Button Active Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##butonActiveTextColor", ref butonActiveTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_ACTIVE_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(butonActiveTextColor));

                ImGui.Spacing();
                ImGui.Separator();

                var butonActiveHoverFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_ACTIVE_HOVER_FILL_COLOR));
                ImGui.Text("Button Active Hover Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##butonActiveHoverFillColor", ref butonActiveHoverFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_ACTIVE_HOVER_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(butonActiveHoverFillColor));

                var butonActiveHoverStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_ACTIVE_HOVER_STROKE_COLOR));
                ImGui.Text("Button Active Hover Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##butonActiveHoverStrokeColor", ref butonActiveHoverStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_ACTIVE_HOVER_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(butonActiveHoverStrokeColor));

                var butonActiveHoverTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_ACTIVE_HOVER_TEXT_COLOR));
                ImGui.Text("Button Active Hover Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##butonActiveHoverTextColor", ref butonActiveHoverTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_ACTIVE_HOVER_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(butonActiveHoverTextColor));

                ImGui.Spacing();
                ImGui.Separator();
            }

            if (m_previewMode == PreviewModeEnum.General_1 || m_previewMode == PreviewModeEnum.WideMenu)
            {
                var butonInactiveFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_INACTIVE_FILL_COLOR));
                ImGui.Text("Button Inactive Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##butonInactiveFillColor", ref butonInactiveFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_INACTIVE_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(butonInactiveFillColor));

                var butonInactiveStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_INACTIVE_STROKE_COLOR));
                ImGui.Text("Button Inactive Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##butonInactiveStrokeColor", ref butonInactiveStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_INACTIVE_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(butonInactiveStrokeColor));

                var butonInactiveTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_INACTIVE_TEXT_COLOR));
                ImGui.Text("Button Inactive Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##butonInactiveTextColor", ref butonInactiveTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_INACTIVE_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(butonInactiveTextColor));

                ImGui.Spacing();
                ImGui.Separator();

                var butonInactiveHoverFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_INACTIVE_HOVER_FILL_COLOR));
                ImGui.Text("Button Inactive Hover Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##butonInactiveHoverFillColor", ref butonInactiveHoverFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_INACTIVE_HOVER_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(butonInactiveHoverFillColor));

                var butonInactiveHoverStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_INACTIVE_HOVER_STROKE_COLOR));
                ImGui.Text("Button Inactive Hover Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##butonInactiveHoverStrokeColor", ref butonInactiveHoverStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_INACTIVE_HOVER_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(butonInactiveHoverStrokeColor));

                var butonInactiveHoverTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_INACTIVE_HOVER_TEXT_COLOR));
                ImGui.Text("Button Inactive Hover Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##butonInactiveHoverTextColor", ref butonInactiveHoverTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_INACTIVE_HOVER_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(butonInactiveHoverTextColor));

                ImGui.Spacing();
                ImGui.Separator();
            }

            var panelFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.PANEL_FILL_COLOR));
            ImGui.Text("Panel Fill Color:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit4("##panelFillColor", ref panelFillColor, ImGuiColorEditFlags.AlphaBar);
            ImGui.PopItemWidth();
            m_theme.PANEL_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(panelFillColor));

            var panelStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.PANEL_STROKE_COLOR));
            ImGui.Text("Panel Stroke Color:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit4("##panelStrokeColor", ref panelStrokeColor, ImGuiColorEditFlags.AlphaBar);
            ImGui.PopItemWidth();
            m_theme.PANEL_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(panelStrokeColor));

            ImGui.Spacing();
            ImGui.Separator();

            if (m_previewMode == PreviewModeEnum.General_1)
            {
                var textPanelFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.TEXT_PANEL_FILL_COLOR));
                ImGui.Text("Text Panel Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##textPanelFillColor", ref textPanelFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.TEXT_PANEL_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(textPanelFillColor));

                var textPanelStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.TEXT_PANEL_STROKE_COLOR));
                ImGui.Text("Text Panel Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##textPanelStrokeColor", ref textPanelStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.TEXT_PANEL_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(textPanelStrokeColor));

                var textPanelTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.TEXT_PANEL_TEXT_COLOR));
                ImGui.Text("Text Panel Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##textPanelTextColor", ref textPanelTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.TEXT_PANEL_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(textPanelTextColor));

                ImGui.Spacing();
                ImGui.Separator();

                var textPanelHoverFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.TEXT_PANEL_HOVER_FILL_COLOR));
                ImGui.Text("Text Panel Hover Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##textPanelHoverFillColor", ref textPanelHoverFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.TEXT_PANEL_HOVER_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(textPanelHoverFillColor));

                var textPanelHoverStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.TEXT_PANEL_HOVER_STROKE_COLOR));
                ImGui.Text("Text Panel Hover Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##textPanelHoverStrokeColor", ref textPanelHoverStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.TEXT_PANEL_HOVER_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(textPanelHoverStrokeColor));

                var textPanelHoverTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.TEXT_PANEL_HOVER_TEXT_COLOR));
                ImGui.Text("Text Panel Hover Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##textPanelHoverTextColor", ref textPanelHoverTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.TEXT_PANEL_HOVER_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(textPanelHoverTextColor));

                ImGui.Spacing();
                ImGui.Separator();
            }

            if (m_previewMode == PreviewModeEnum.Led_1)
            {
                var buttonLedOffFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_OFF_FILL_COLOR));
                ImGui.Text("Button Led Off Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedOffFillColor", ref buttonLedOffFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_OFF_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedOffFillColor));

                var buttonLedOffStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_OFF_STROKE_COLOR));
                ImGui.Text("Button Led Off Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedOffStrokeColor", ref buttonLedOffStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_OFF_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedOffStrokeColor));

                var buttonLedOffTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_OFF_TEXT_COLOR));
                ImGui.Text("Button Led Off Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedOffTextColor", ref buttonLedOffTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_OFF_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedOffTextColor));

                ImGui.Spacing();
                ImGui.Separator();

                var buttonLedOffHoverFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_OFF_HOVER_FILL_COLOR));
                ImGui.Text("Button Led Off Hover Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedOffHoverFillColor", ref buttonLedOffHoverFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_OFF_HOVER_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedOffHoverFillColor));

                var buttonLedOffHoverStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_OFF_HOVER_STROKE_COLOR));
                ImGui.Text("Button Led Off Hover Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedOffHoverStrokeColor", ref buttonLedOffHoverStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_OFF_HOVER_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedOffHoverStrokeColor));

                var buttonLedOffHoverTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_OFF_HOVER_TEXT_COLOR));
                ImGui.Text("Button Led Off Hover Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedOffHoverTextColor", ref buttonLedOffHoverTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_OFF_HOVER_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedOffHoverTextColor));

                ImGui.Spacing();
                ImGui.Separator();


                var buttonLedRedFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_RED_FILL_COLOR));
                ImGui.Text("Button Led Red Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedRedFillColor", ref buttonLedRedFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_RED_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedRedFillColor));

                var buttonLedRedStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_RED_STROKE_COLOR));
                ImGui.Text("Button Led Red Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedRedStrokeColor", ref buttonLedRedStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_RED_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedRedStrokeColor));

                var buttonLedRedTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_RED_TEXT_COLOR));
                ImGui.Text("Button Led Red Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedRedTextColor", ref buttonLedRedTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_RED_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedRedTextColor));

                ImGui.Spacing();
                ImGui.Separator();

                var buttonLedRedHoverFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_RED_HOVER_FILL_COLOR));
                ImGui.Text("Button Led Red Hover Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedRedHoverFillColor", ref buttonLedRedHoverFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_RED_HOVER_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedRedHoverFillColor));

                var buttonLedRedHoverStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_RED_HOVER_STROKE_COLOR));
                ImGui.Text("Button Led Red Hover Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedRedHoverStrokeColor", ref buttonLedRedHoverStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_RED_HOVER_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedRedHoverStrokeColor));

                var buttonLedRedHoverTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_RED_HOVER_TEXT_COLOR));
                ImGui.Text("Button Led Red Hover Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedRedHoverTextColor", ref buttonLedRedHoverTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_RED_HOVER_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedRedHoverTextColor));

                ImGui.Spacing();
                ImGui.Separator();
            }

            if (m_previewMode == PreviewModeEnum.Led_2)
            {
                var buttonLedGreenFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_GREEN_FILL_COLOR));
                ImGui.Text("Button Led Green Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedGreenFillColor", ref buttonLedGreenFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_GREEN_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedGreenFillColor));

                var buttonLedGreenStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_GREEN_STROKE_COLOR));
                ImGui.Text("Button Led Green Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedGreenStrokeColor", ref buttonLedGreenStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_GREEN_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedGreenStrokeColor));

                var buttonLedGreenTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_GREEN_TEXT_COLOR));
                ImGui.Text("Button Led Green Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedGreenTextColor", ref buttonLedGreenTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_GREEN_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedGreenTextColor));

                ImGui.Spacing();
                ImGui.Separator();

                var buttonLedGreenHoverFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_GREEN_HOVER_FILL_COLOR));
                ImGui.Text("Button Led Green Hover Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedGreenHoverFillColor", ref buttonLedGreenHoverFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_GREEN_HOVER_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedGreenHoverFillColor));

                var buttonLedGreenHoverStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_GREEN_HOVER_STROKE_COLOR));
                ImGui.Text("Button Led Green Hover Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedGreenHoverStrokeColor", ref buttonLedGreenHoverStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_GREEN_HOVER_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedGreenHoverStrokeColor));

                var buttonLedGreenHoverTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_GREEN_HOVER_TEXT_COLOR));
                ImGui.Text("Button Led Green Hover Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedGreenHoverTextColor", ref buttonLedGreenHoverTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_GREEN_HOVER_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedGreenHoverTextColor));

                ImGui.Spacing();
                ImGui.Separator();

                var buttonLedYellowFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_YELLOW_FILL_COLOR));
                ImGui.Text("Button Led Yellow Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedYellowFillColor", ref buttonLedYellowFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_YELLOW_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedYellowFillColor));

                var buttonLedYellowStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_YELLOW_STROKE_COLOR));
                ImGui.Text("Button Led Yellow Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedYellowStrokeColor", ref buttonLedYellowStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_YELLOW_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedYellowStrokeColor));

                var buttonLedYellowTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_YELLOW_TEXT_COLOR));
                ImGui.Text("Button Led Yellow Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedYellowTextColor", ref buttonLedYellowTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_YELLOW_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedYellowTextColor));

                ImGui.Spacing();
                ImGui.Separator();

                var buttonLedYellowHoverFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_YELLOW_HOVER_FILL_COLOR));
                ImGui.Text("Button Led Yellow Hover Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedYellowHoverFillColor", ref buttonLedYellowHoverFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_YELLOW_HOVER_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedYellowHoverFillColor));

                var buttonLedYellowHoverStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_YELLOW_HOVER_STROKE_COLOR));
                ImGui.Text("Button Led Yellow Hover Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedYellowHoverStrokeColor", ref buttonLedYellowHoverStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_YELLOW_HOVER_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedYellowHoverStrokeColor));

                var buttonLedYellowHoverTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_YELLOW_HOVER_TEXT_COLOR));
                ImGui.Text("Button Led Yellow Hover Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedYellowHoverTextColor", ref buttonLedYellowHoverTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_YELLOW_HOVER_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedYellowHoverTextColor));

                ImGui.Spacing();
                ImGui.Separator();
            }

            if (m_previewMode == PreviewModeEnum.Led_3)
            {
                var buttonLedBlueFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_BLUE_FILL_COLOR));
                ImGui.Text("Button Led Blue Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedBlueFillColor", ref buttonLedBlueFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_BLUE_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedBlueFillColor));

                var buttonLedBlueStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_BLUE_STROKE_COLOR));
                ImGui.Text("Button Led Blue Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedBlueStrokeColor", ref buttonLedBlueStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_BLUE_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedBlueStrokeColor));

                var buttonLedBlueTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_BLUE_TEXT_COLOR));
                ImGui.Text("Button Led Blue Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedBlueTextColor", ref buttonLedBlueTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_BLUE_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedBlueTextColor));

                ImGui.Spacing();
                ImGui.Separator();

                var buttonLedBlueHoverFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_BLUE_HOVER_FILL_COLOR));
                ImGui.Text("Button Led Blue Hover Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedBlueHoverFillColor", ref buttonLedBlueHoverFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_BLUE_HOVER_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedBlueHoverFillColor));

                var buttonLedBlueHoverStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_BLUE_HOVER_STROKE_COLOR));
                ImGui.Text("Button Led Blue Hover Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedBlueHoverStrokeColor", ref buttonLedBlueHoverStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_BLUE_HOVER_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedBlueHoverStrokeColor));

                var buttonLedBlueHoverTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_BLUE_HOVER_TEXT_COLOR));
                ImGui.Text("Button Led Blue Hover Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedBlueHoverTextColor", ref buttonLedBlueHoverTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_BLUE_HOVER_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedBlueHoverTextColor));

                ImGui.Spacing();
                ImGui.Separator();

                var buttonLedPurpleFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_PURPLE_FILL_COLOR));
                ImGui.Text("Button Led Purple Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedPurpleFillColor", ref buttonLedPurpleFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_PURPLE_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedPurpleFillColor));

                var buttonLedPurpleStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_PURPLE_STROKE_COLOR));
                ImGui.Text("Button Led Purple Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedPurpleStrokeColor", ref buttonLedPurpleStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_PURPLE_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedPurpleStrokeColor));

                var buttonLedPurpleTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_PURPLE_TEXT_COLOR));
                ImGui.Text("Button Led Purple Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedPurpleTextColor", ref buttonLedPurpleTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_PURPLE_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedPurpleTextColor));

                ImGui.Spacing();
                ImGui.Separator();

                var buttonLedPurpleHoverFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_PURPLE_HOVER_FILL_COLOR));
                ImGui.Text("Button Led Purple Hover Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedPurpleHoverFillColor", ref buttonLedPurpleHoverFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_PURPLE_HOVER_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedPurpleHoverFillColor));

                var buttonLedPurpleHoverStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_PURPLE_HOVER_STROKE_COLOR));
                ImGui.Text("Button Led Purple Hover Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedPurpleHoverStrokeColor", ref buttonLedPurpleHoverStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_PURPLE_HOVER_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedPurpleHoverStrokeColor));

                var buttonLedPurpleHoverTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_PURPLE_HOVER_TEXT_COLOR));
                ImGui.Text("Button Led Purple Hover Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedPurpleHoverTextColor", ref buttonLedPurpleHoverTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_PURPLE_HOVER_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedPurpleHoverTextColor));

                ImGui.Spacing();
                ImGui.Separator();
            }

            if (m_previewMode == PreviewModeEnum.Led_4)
            {
                var buttonLedTurquoiseFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_TURQUOISE_FILL_COLOR));
                ImGui.Text("Button Led Turquoise Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedTurquoiseFillColor", ref buttonLedTurquoiseFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_TURQUOISE_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedTurquoiseFillColor));

                var buttonLedTurquoiseStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_TURQUOISE_STROKE_COLOR));
                ImGui.Text("Button Led Turquoise Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedTurquoiseStrokeColor", ref buttonLedTurquoiseStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_TURQUOISE_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedTurquoiseStrokeColor));

                var buttonLedTurquoiseTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_TURQUOISE_TEXT_COLOR));
                ImGui.Text("Button Led Turquoise Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedTurquoiseTextColor", ref buttonLedTurquoiseTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_TURQUOISE_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedTurquoiseTextColor));

                ImGui.Spacing();
                ImGui.Separator();

                var buttonLedTurquoiseHoverFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_TURQUOISE_HOVER_FILL_COLOR));
                ImGui.Text("Button Led Turquoise Hover Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedTurquoiseHoverFillColor", ref buttonLedTurquoiseHoverFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_TURQUOISE_HOVER_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedTurquoiseHoverFillColor));

                var buttonLedTurquoiseHoverStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_TURQUOISE_HOVER_STROKE_COLOR));
                ImGui.Text("Button Led Turquoise Hover Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedTurquoiseHoverStrokeColor", ref buttonLedTurquoiseHoverStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_TURQUOISE_HOVER_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedTurquoiseHoverStrokeColor));

                var buttonLedTurquoiseHoverTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_TURQUOISE_HOVER_TEXT_COLOR));
                ImGui.Text("Button Led Turquoise Hover Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedTurquoiseHoverTextColor", ref buttonLedTurquoiseHoverTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_TURQUOISE_HOVER_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedTurquoiseHoverTextColor));

                ImGui.Spacing();
                ImGui.Separator();

                var buttonLedWhiteFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_WHITE_FILL_COLOR));
                ImGui.Text("Button Led White Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedWhiteFillColor", ref buttonLedWhiteFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_WHITE_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedWhiteFillColor));

                var buttonLedWhiteStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_WHITE_STROKE_COLOR));
                ImGui.Text("Button Led White Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedWhiteStrokeColor", ref buttonLedWhiteStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_WHITE_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedWhiteStrokeColor));

                var buttonLedWhiteTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_WHITE_TEXT_COLOR));
                ImGui.Text("Button Led White Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedWhiteTextColor", ref buttonLedWhiteTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_WHITE_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedWhiteTextColor));

                ImGui.Spacing();
                ImGui.Separator();

                var buttonLedWhiteHoverFillColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_WHITE_HOVER_FILL_COLOR));
                ImGui.Text("Button Led White Hover Fill Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedWhiteHoverFillColor", ref buttonLedWhiteHoverFillColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_WHITE_HOVER_FILL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedWhiteHoverFillColor));

                var buttonLedWhiteHoverStrokeColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_WHITE_HOVER_STROKE_COLOR));
                ImGui.Text("Button Led White Hover Stroke Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedWhiteHoverStrokeColor", ref buttonLedWhiteHoverStrokeColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_WHITE_HOVER_STROKE_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedWhiteHoverStrokeColor));

                var buttonLedWhiteHoverTextColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.BUTTON_LED_WHITE_HOVER_TEXT_COLOR));
                ImGui.Text("Button Led White Hover Text Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##buttonLedWhiteHoverTextColor", ref buttonLedWhiteHoverTextColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.BUTTON_LED_WHITE_HOVER_TEXT_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(buttonLedWhiteHoverTextColor));

                ImGui.Spacing();
                ImGui.Separator();
            }

            if (m_previewMode == PreviewModeEnum.Snake)
            {
                var snakeWallColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.SNAKE_WALL_COLOR));
                ImGui.Text("Snake Wall Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##snakeWallColor", ref snakeWallColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.SNAKE_WALL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(snakeWallColor));

                var snakeFoodColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.SNAKE_FOOD_COLOR));
                ImGui.Text("Snake Food Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##snakeFoodColor", ref snakeFoodColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.SNAKE_FOOD_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(snakeFoodColor));

                var snakeHeadColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.SNAKE_HEAD_COLOR));
                ImGui.Text("Snake Head Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##snakeHeadColor", ref snakeHeadColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.SNAKE_HEAD_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(snakeHeadColor));

                var snakeTailColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.SNAKE_TAIL_COLOR));
                ImGui.Text("Snake Tail Color:");
                ImGui.PushItemWidth(250);
                ImGui.ColorEdit4("##snakeTailColor", ref snakeTailColor, ImGuiColorEditFlags.AlphaBar);
                ImGui.PopItemWidth();
                m_theme.SNAKE_TAIL_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(snakeTailColor));

                ImGui.Spacing();
                ImGui.Separator();
            }

            var joyButtonAColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.JOY_BUTTON_A_COLOR));
            ImGui.Text("Joy Button A Color:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit4("##joyButtonAColor", ref joyButtonAColor, ImGuiColorEditFlags.AlphaBar);
            ImGui.PopItemWidth();
            m_theme.JOY_BUTTON_A_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(joyButtonAColor));

            var joyButtonBColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.JOY_BUTTON_B_COLOR));
            ImGui.Text("Joy Button B Color:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit4("##joyButtonBColor", ref joyButtonBColor, ImGuiColorEditFlags.AlphaBar);
            ImGui.PopItemWidth();
            m_theme.JOY_BUTTON_B_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(joyButtonBColor));

            var joyButtonXColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.JOY_BUTTON_X_COLOR));
            ImGui.Text("Joy Button X Color:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit4("##joyButtonXColor", ref joyButtonXColor, ImGuiColorEditFlags.AlphaBar);
            ImGui.PopItemWidth();
            m_theme.JOY_BUTTON_X_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(joyButtonXColor));

            var joyButtonYColor = ImGui.ColorConvertU32ToFloat4(Theme.ConvertARGBtoABGR(m_theme.JOY_BUTTON_Y_COLOR));
            ImGui.Text("Joy Button Y Color:");
            ImGui.PushItemWidth(250);
            ImGui.ColorEdit4("##joyButtonYColor", ref joyButtonYColor, ImGuiColorEditFlags.AlphaBar);
            ImGui.PopItemWidth();
            m_theme.JOY_BUTTON_Y_COLOR = Theme.ConvertABGRtoARGB(ImGui.ColorConvertFloat4ToU32(joyButtonYColor));

            ImGui.EndChild();

            var windowSize = ImGui.GetWindowSize();

            ImGui.SetCursorPosY(windowSize.Y - 64);

            if (ImGui.Button("Set Default Theme", new Vector2(150, 30)))
            {
                DisposeBackgroundImages();
                DisposeOverlayImage();
                m_theme.DefaultTheme();
            }

            ImGui.SameLine();

            ImGui.SetCursorPosX(windowSize.X - 374);

            if (ImGui.Button("Patreon", new Vector2(100, 30)))
            {
                OpenUrl("https://www.patreon.com/teamresurgent");
            }

            ImGui.SameLine();

            if (ImGui.Button("Ko-Fi", new Vector2(100, 30)))
            {
                OpenUrl("https://ko-fi.com/teamresurgent");
            }

            ImGui.SameLine();

            if (ImGui.Button("Coded by EqUiNoX", new Vector2(150, 30)))
            {
                OpenUrl("https://github.com/Team-Resurgent/PrometheOSSkinEditor");
            }

            ImGui.End();
        }
    }




}
