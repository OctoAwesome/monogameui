using System;
using System.Collections.Generic;
using engenious;
using engenious.Content;
using engenious.Graphics;

namespace MonoGameUi
{
    public class Skin
    {
        #region Singleton

        /// <summary>
        /// Basis-Pixel
        /// </summary>
        public static Texture2D Pix { get; internal set; }

        /// <summary>
        /// Aktueller Skin
        /// </summary>
        public static Skin Current { get; set; }

        #endregion

        public Skin(ContentManager content)
        {
            ControlSkins = new Dictionary<Type, Action<Control>>();
            StyleSkins = new Dictionary<string, Action<Control>>();

            SplitterWidth = 4;
            ScrollbarWidth = 10;
            ScrollerMinSize = 10;

            BackgroundColor = new Color(20, 147, 73);
            LogoColorOrange = new Color(241, 145, 11);
            LogoColorRed = new Color(225, 7, 22);

            TextColorBlack = new Color(0, 0, 0);
            TextColorGray = new Color(86, 86, 86);
            TextColorWhite = new Color(255, 255, 255);

            HeadlineFont = content.Load<SpriteFont>("HeadlineFont");
            TextFont = content.Load<SpriteFont>("GameFont");
            BoldFont = content.Load<SpriteFont>("BoldFont");

            FocusFrameBrush = new BorderBrush(LineType.Dotted, Color.Black, 1);

            ButtonBrush = new BorderBrush(Color.White, LineType.Solid, Color.LightGray, 1);
            ButtonHoverBrush = new BorderBrush(Color.LightGray, LineType.Solid, Color.LightGray, 1);
            ButtonPressedBrush = new BorderBrush(Color.LightGray, LineType.Solid, Color.Black, 1);
            PanelBrush = new BorderBrush(Color.White, LineType.Solid, Color.Black, 1);

            VerticalScrollBackgroundBrush = new BorderBrush(Color.White, LineType.Solid, Color.Black, 1);
            HorizontalScrollBackgroundBrush = new BorderBrush(Color.White, LineType.Solid, Color.Black, 1);
            VerticalScrollForegroundBrush = new BorderBrush(Color.LightGray, LineType.Solid, Color.Black, 1);
            HorizontalScrollForegroundBrush = new BorderBrush(Color.LightGray, LineType.Solid, Color.Black, 1);
            HorizontalSplitterBrush = new BorderBrush(Color.White, LineType.Solid, Color.LightGray, 1);
            VerticalSplitterBrush = new BorderBrush(Color.White, LineType.Solid, Color.LightGray, 1);

            ProgressBarBrush = new BorderBrush(Color.Blue, LineType.Solid, Color.Black, 1);

            SelectedItemBrush = new BorderBrush(Color.Red);

            // =============
            // Skin-Methoden
            // =============

            // Control
            ControlSkins.Add(typeof(Control), (c) =>
            {
                Control control = c as Control;
                control.Margin = Border.All(0);
                control.Padding = Border.All(0);
                control.HorizontalAlignment = HorizontalAlignment.Center;
                control.VerticalAlignment = VerticalAlignment.Center;
            });

            // Label
            ControlSkins.Add(typeof(Label), (c) =>
            {
                Label label = c as Label;
                label.VerticalTextAlignment = VerticalAlignment.Center;
                label.HorizontalTextAlignment = HorizontalAlignment.Left;
                label.Font = Current.TextFont;
                label.TextColor = Current.TextColorBlack;
                label.Padding = Border.All(5);
            });

            // Button
            ControlSkins.Add(typeof(Button), (c) =>
            {
                Button button = c as Button;
                button.Margin = Border.All(2);
                button.Padding = Border.All(5);
                button.Background = Current.ButtonBrush;
                button.HoveredBackground = Current.ButtonHoverBrush;
                button.PressedBackground = Current.ButtonPressedBrush;
            });

            // Splitter
            ControlSkins.Add(typeof(Splitter), (c) =>
            {
                Splitter splitter = c as Splitter;
                splitter.HorizontalAlignment = HorizontalAlignment.Stretch;
                splitter.VerticalAlignment = VerticalAlignment.Stretch;
                splitter.Orientation = Orientation.Horizontal;
                splitter.SplitterSize = Current.SplitterWidth;
                splitter.SplitterPosition = 200;
                splitter.SplitterBrushHorizontal = Current.HorizontalSplitterBrush;
                splitter.SplitterBrushVertical = Current.VerticalSplitterBrush;
            });

            // Scrollcontainer
            ControlSkins.Add(typeof(ScrollContainer), (c) =>
            {
                ScrollContainer scrollContainer = c as ScrollContainer;
                scrollContainer.HorizontalScrollbarBackground = Current.HorizontalScrollBackgroundBrush;
                scrollContainer.HorizontalScrollbarForeground = Current.HorizontalScrollForegroundBrush;
                scrollContainer.VerticalScrollbarBackground = Current.VerticalScrollBackgroundBrush;
                scrollContainer.VerticalScrollbarForeground = Current.VerticalScrollForegroundBrush;
                scrollContainer.ScrollbarWidth = Current.ScrollbarWidth;
                scrollContainer.ScrollerMinSize = Current.ScrollerMinSize;
            });

            // StackPanel
            ControlSkins.Add(typeof(StackPanel), (c) =>
            {
                StackPanel stackPanel = c as StackPanel;
            });

            // ListControl
            //ControlSkins.Add(typeof(ListControl<>), (c) =>
            //{
            //    dynamic listControl = c;
            //    listControl.SelectedItemBrush = Current.SelectedItemBrush;
            //});

            // Listbox
            ControlSkins.Add(typeof(Listbox<>), (c) =>
            {
                dynamic listBox = c;
            });

            // Combobox
            //ControlSkins.Add(typeof(Combobox<>), (c) =>
            //{
            //    dynamic comboBox = c;
            //    comboBox.Background = new BorderBrush(Color.White);

            //});

            // Progressbar
            ControlSkins.Add(typeof(ProgressBar), (c) =>
            {
                ProgressBar progressBar = c as ProgressBar;
                progressBar.BarBrush = Current.ProgressBarBrush;
            });

            //Slider
            ControlSkins.Add(typeof(Slider), (c) =>
            {
                Slider s = c as Slider;
                s.Orientation = Orientation.Horizontal;
                s.SliderBackgroundBrush = new BorderBrush(Color.LightGray);
                s.SliderForegroundBrush = new BorderBrush(Color.SlateGray);
                s.SliderWidth = 20;
            });

            ControlSkins.Add(typeof(Checkbox), (c) =>
            {
                Checkbox checkbox = c as Checkbox;
                checkbox.BoxBrush = new BorderBrush(Color.Black);
                checkbox.InnerBoxBrush = new BorderBrush(Color.LightGray);
                checkbox.HookBrush = new BorderBrush(Color.LawnGreen);
                checkbox.Width = 20;
                checkbox.Height = 20;
            });

            ControlSkins.Add(typeof(TabControl), (c) =>
            {
                TabControl tabControl = c as TabControl;
                tabControl.TabBrush = new BorderBrush(Color.LightGray);
                tabControl.TabActiveBrush = new BorderBrush(Color.Gray);
                tabControl.TabPageBackground = new BorderBrush(Color.Gray);
                tabControl.TabSpacing = 1;
            });
        }

        #region Basic Values

        /// <summary>
        /// Gibt die Standard-Breite für den Splitter innerhalb eines Split-Containers an.
        /// </summary>
        public int SplitterWidth { get; set; }

        /// <summary>
        /// Gibt die Standard-Breite einer Scrollbar an.
        /// </summary>
        public int ScrollbarWidth { get; set; }

        /// <summary>
        /// Gibt die mindestgröße des greifbaren Scrollers innerhalb einer Scrollbar an.
        /// </summary>
        public int ScrollerMinSize { get; set; }

        #endregion

        #region Basic Colors

        public Color LogoColorOrange { get; set; }

        public Color LogoColorRed { get; set; }

        public Color BackgroundColor { get; set; }

        public Color TextColorBlack { get; set; }

        public Color TextColorGray { get; set; }

        public Color TextColorWhite { get; set; }

        #endregion

        #region Fonts

        public SpriteFont HeadlineFont { get; set; }

        public SpriteFont TextFont { get; set; }

        public SpriteFont BoldFont { get; set; }

        #endregion

        #region Textures

        #endregion

        #region Brushes

        public Brush PanelBrush { get; set; }

        /// <summary>
        /// Default Hintergrund für Buttons.
        /// </summary>
        public Brush ButtonBrush { get; set; }

        /// <summary>
        /// Hintergrund für Buttons im Hover-State.
        /// </summary>
        public Brush ButtonHoverBrush { get; set; }

        /// <summary>
        /// Hintergrund für Buttons die aktuell gedrückt sind.
        /// </summary>
        public Brush ButtonPressedBrush { get; set; }

        /// <summary>
        /// Definiert den Brush zum Zeichnen eines vertikalen Scrollbar-Hintergrunds.
        /// </summary>
        public Brush VerticalScrollBackgroundBrush { get; set; }

        /// <summary>
        /// Definiert den Brush zum Zeichnen eines horizontalen Scrollbar-Hintergrunds.
        /// </summary>
        public Brush HorizontalScrollBackgroundBrush { get; set; }

        /// <summary>
        /// Definiert den Brush zum Zeichnen des Scrollbar-Blocks innerhalb einer vertikalen Scrollbar.
        /// </summary>
        public Brush VerticalScrollForegroundBrush { get; set; }

        /// <summary>
        /// Definiert den Brush zum Zeichnen des Scrollbar-Blocks innerhalb einer horizontalen Scrollbar.
        /// </summary>
        public Brush HorizontalScrollForegroundBrush { get; set; }

        /// <summary>
        /// Definiert den Brush zum Zeichnen eines horizontalen Splitters.
        /// </summary>
        public Brush HorizontalSplitterBrush { get; set; }

        /// <summary>
        /// Definiert den Brush zum Zeichnen eines vertikalen Splitters.
        /// </summary>
        public Brush VerticalSplitterBrush { get; set; }

        /// <summary>
        /// Definiert den Standard-Brush zur Markierung des fokusierten Controls.
        /// </summary>
        public Brush FocusFrameBrush { get; set; }

        /// <summary>
        /// Standard-Brush für selektierte Elemente innerhalb von Listboxen.
        /// </summary>
        public Brush SelectedItemBrush { get; set; }

        /// <summary>
        /// Standard-Brush für die Progress-Bar
        /// </summary>
        public Brush ProgressBarBrush { get; set; }

        #endregion

        /// <summary>
        /// Dictionary für das Skinning gezielter Control-Types
        /// </summary>
        public Dictionary<Type, Action<Control>> ControlSkins { get; set; }

        /// <summary>
        /// Dictionary für das Skinning gezielter Style-Klassen.
        /// </summary>
        public Dictionary<string, Action<Control>> StyleSkins { get; set; }
    }
}
