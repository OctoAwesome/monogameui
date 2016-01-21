using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace MonoGameUi
{
    public class BaseScreenComponent : DrawableGameComponent, IScreenManager
    {
        private ContainerControl root;

        private FlyoutControl flyout;

        private SpriteBatch batch;

        private MouseMode mouseMode;

        /// <summary>
        /// Prefix für die Titel-Leiste
        /// </summary>
        public string TitlePrefix { get; set; }

        /// <summary>
        /// Gibt das Root-Control zurück oder legt dieses fest.
        /// </summary>
        public ContentControl Frame { get; private set; }

        /// <summary>
        /// Gibt das Control an, das zum navigieren der Screens verwendet wird.
        /// </summary>
        public ContainerControl ScreenTarget { get; set; }

        /// <summary>
        /// Legt das Standard-Template für eine Navigation zu einem Screen fest.
        /// </summary>
        public Transition NavigateToTransition { get; set; }

        /// <summary>
        /// Legt das Standard-Template für eine Navigation von einem Screen weg fest.
        /// </summary>
        public Transition NavigateFromTransition { get; set; }

        /// <summary>
        /// Referenz zum MonoGame Content Manager.
        /// </summary>
        public ContentManager Content { get; private set; }

        /// <summary>
        /// Gibt den aktuellen Modus der Maus zurück.
        /// </summary>
        public MouseMode MouseMode
        {
            get
            {
                return mouseMode;
            }
            private set
            {
                if (mouseMode != value)
                {
                    mouseMode = value;
                    Game.IsMouseVisible = (mouseMode != MouseMode.Captured);
                }
            }
        }

        public BaseScreenComponent(Game game)
            : base(game)
        {
            Content = game.Content;

            Game.Window.TextInput += (s, e) =>
            {
                if (Game.IsActive)
                {
                    KeyTextEventArgs args = new KeyTextEventArgs() { Character = e.Character };
                   
                    root.InternalKeyTextPress(args);
                }
            };
        }

        protected override void LoadContent()
        {
            Skin.Pix = new Texture2D(GraphicsDevice, 1, 1);
            Skin.Pix.SetData(new[] { Color.White });

            Skin.Current = new Skin(Game.Content);
            batch = new SpriteBatch(GraphicsDevice);

            root = new ContainerControl(this)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            Frame = new ContentControl(this)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            root.Controls.Add(Frame);

            ContainerControl screenContainer = new ContainerControl(this)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            Frame.Content = screenContainer;
            ScreenTarget = screenContainer;

            flyout = new FlyoutControl(this)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            root.Controls.Add(flyout);
        }

        private bool lastLeftMouseButtonPressed = false;

        private bool lastRightMouseButtonPressed = false;

        private int lastMouseWheelValue = 0;

        private Point lastMousePosition = Point.Zero;

        Dictionary<Keys, double> pressedKeys = new Dictionary<Keys, double>();

        public override void Update(GameTime gameTime)
        {
            #region Mouse Interaction

            if (Game.IsActive)
            {
                MouseState mouse = Mouse.GetState();

                // Mausposition anhand des Mouse Modes ermitteln
                Point mousePosition = mouse.Position;
                if (MouseMode == MouseMode.Captured)
                    mousePosition = new Point(
                        mousePosition.X - (GraphicsDevice.Viewport.Width / 2), 
                        mousePosition.Y - (GraphicsDevice.Viewport.Height / 2));

                MouseEventArgs moveArgs = new MouseEventArgs()
                {
                    MouseMode = MouseMode,
                    GlobalPosition = mousePosition,
                    LocalPosition = mousePosition,
                };

                root.InternalMouseMove(moveArgs);

                // Linke Maustaste
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (!lastLeftMouseButtonPressed)
                    {
                        // Linke Maustaste wurde neu gedrückt
                        root.InternalLeftMouseDown(new MouseEventArgs
                        {
                            GlobalPosition = mousePosition,
                            LocalPosition = mousePosition
                        });
                    }
                    lastLeftMouseButtonPressed = true;
                }
                else
                {
                    if (lastLeftMouseButtonPressed)
                    {
                        // Linke Maustaste wurde losgelassen
                        root.InternalLeftMouseClick(new MouseEventArgs
                        {
                            MouseMode = MouseMode,
                            GlobalPosition = mousePosition,
                            LocalPosition = mousePosition
                        });

                        root.InternalLeftMouseUp(new MouseEventArgs
                        {
                            MouseMode = MouseMode,
                            GlobalPosition = mousePosition,
                            LocalPosition = mousePosition
                        });
                    }
                    lastLeftMouseButtonPressed = false;
                }

                // Rechte Maustaste
                if (mouse.RightButton == ButtonState.Pressed)
                {
                    if (!lastRightMouseButtonPressed)
                    {
                        // Rechte Maustaste neu gedrückt
                        root.InternalRightMouseDown(new MouseEventArgs
                        {
                            MouseMode = MouseMode,
                            GlobalPosition = mousePosition,
                            LocalPosition = mousePosition
                        });
                    }
                    lastRightMouseButtonPressed = true;
                }
                else
                {
                    if (lastRightMouseButtonPressed)
                    {
                        // Rechte Maustaste losgelassen
                        root.InternalRightMouseUp(new MouseEventArgs
                        {
                            MouseMode = MouseMode,
                            GlobalPosition = mousePosition,
                            LocalPosition = mousePosition
                        });

                        root.InternalRightMouseClick(new MouseEventArgs
                        {
                            MouseMode = MouseMode,
                            GlobalPosition = mousePosition,
                            LocalPosition = mousePosition
                        });
                    }
                    lastRightMouseButtonPressed = false;
                }

                // Mousewheel
                if (lastMouseWheelValue != mouse.ScrollWheelValue)
                {
                    int diff = (mouse.ScrollWheelValue - lastMouseWheelValue);
                    root.InternalMouseScroll(new MouseScrollEventArgs
                    {
                        MouseMode = MouseMode,
                        GlobalPosition = mousePosition,
                        LocalPosition = mousePosition,
                        Steps = diff
                    });
                    lastMouseWheelValue = mouse.ScrollWheelValue;
                }

                // Potentieller Positionsreset
                if (MouseMode == MouseMode.Free)
                {
                    lastMousePosition = mouse.Position;
                }
                else if (mousePosition.X != 0 || mousePosition.Y != 0)
                {
                    Mouse.SetPosition(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2);
                }             
            }

            #endregion

            #region Keyboard Interaction

            if (Game.IsActive)
            {
                KeyboardState keyboard = Keyboard.GetState();

                bool shift = keyboard.IsKeyDown(Keys.LeftShift) | keyboard.IsKeyDown(Keys.RightShift);
                bool ctrl = keyboard.IsKeyDown(Keys.LeftControl) | keyboard.IsKeyDown(Keys.RightControl);
                bool alt = keyboard.IsKeyDown(Keys.LeftAlt) | keyboard.IsKeyDown(Keys.RightAlt);

                KeyEventArgs args;
                foreach (Keys key in Enum.GetValues(typeof(Keys)))
                {
                    if (keyboard.IsKeyDown(key))
                    {
                        if (!pressedKeys.ContainsKey(key))
                        {
                            // Taste ist neu

                            args = new KeyEventArgs()
                            {
                                Key = key,
                                Shift = shift,
                                Ctrl = ctrl,
                                Alt = alt
                            };
                            root.InternalKeyDown(args);

                            if (!args.Handled)
                                KeyDown?.Invoke(args);

                            args = new KeyEventArgs()
                            {
                                Key = key,
                                Shift = shift,
                                Ctrl = ctrl,
                                Alt = alt
                            };
                            root.InternalKeyPress(args);
                            pressedKeys.Add(key, gameTime.TotalGameTime.TotalMilliseconds + 500);



                            // Spezialfall Tab-Taste (falls nicht verarbeitet wurde)
                            if (key == Keys.Tab && !args.Handled)
                            {
                                if (shift) root.InternalTabbedBackward();
                                else root.InternalTabbedForward();
                            }
                        }
                        else
                        {
                            // Taste ist immernoch gedrückt
                            if (pressedKeys[key] <= gameTime.TotalGameTime.TotalMilliseconds)
                            {
                                args = new KeyEventArgs()
                                {
                                    Key = key,
                                    Shift = shift,
                                    Ctrl = ctrl,
                                    Alt = alt
                                };
                                root.InternalKeyPress(args);
                                if (!args.Handled)
                                    KeyPress?.Invoke(args);
                                pressedKeys[key] = gameTime.TotalGameTime.TotalMilliseconds + 50;
                            }
                        }
                    }
                    else
                    {
                        if (pressedKeys.ContainsKey(key))
                        {
                            // Taste losgelassen
                            args = new KeyEventArgs()
                            {
                                Key = key,
                                Shift = shift,
                                Ctrl = ctrl,
                                Alt = alt
                            };
                            root.InternalKeyUp(args);
                            pressedKeys.Remove(key);

                            if (!args.Handled)
                                KeyUp?.Invoke(args);

                        }
                    }
                }
            }

            #endregion

            #region Recalculate Sizes

            if (root.HasInvalidDimensions())
            {
                Point available = new Point(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
                Point required = root.GetExpectedSize(available);
                root.SetActualSize(available);
            }

            root.Update(gameTime);

            #endregion

            #region Form anpassen

            string screentitle = ActiveScreen != null ? ActiveScreen.Title : string.Empty;
            string windowtitle = TitlePrefix + (string.IsNullOrEmpty(screentitle) ? "" : " - " + screentitle);

            if (Game.Window != null && Game.Window.Title != windowtitle)
                Game.Window.Title = windowtitle;

            #endregion
        }

        public override void Draw(GameTime gameTime)
        {
            root.PreDraw(gameTime);
            root.Draw(batch, GraphicsDevice.Viewport.Bounds, gameTime);
        }

        private List<Screen> historyStack = new List<Screen>();

        /// <summary>
        /// Gibt an ob der aktuelle History Stack eine Navigation Back-Navigation erlaubt.
        /// </summary>
        public bool CanGoBack { get { return historyStack.Count > 1; } }

        private Screen activeScreen = null;

        /// <summary>
        /// Referenz auf den aktuellen Screen.
        /// </summary>
        public Screen ActiveScreen
        {
            get
            {
                return activeScreen;
            }
            private set
            {
                if (activeScreen != value)
                {
                    activeScreen = value;
                }
            }
        }

        /// <summary>
        /// Liste der History.
        /// </summary>
        public IEnumerable<Screen> History { get { return historyStack; } }

        /// <summary>
        /// Navigiert den Screen Manager zum angegebenen Screen.
        /// </summary>
        /// <param name="screen"></param>
        /// <returns>Gibt an ob die Navigation durchgeführt wurde.</returns>
        public bool NavigateToScreen(Screen screen, object parameter = null)
        {
            return Navigate(screen, false, parameter);
        }

        /// <summary>
        /// Navigiert zurück, sofern möglich.
        /// </summary>
        /// <returns>Gibt an ob die Navigation durchgeführt wurde.</returns>
        public bool NavigateBack()
        {
            if (CanGoBack)
            {
                historyStack.Remove(ActiveScreen);
                Screen screen = historyStack[0];
                Navigate(screen, true, null);
            }

            return false;
        }

        private bool Navigate(Screen screen, bool isBackNavigation, object parameter = null)
        {
            bool overlayed = false;

            // Navigation ankündigen und prüfen, ob das ok geht.
            NavigationEventArgs args = new NavigationEventArgs()
            {
                IsBackNavigation = isBackNavigation,
                Parameter = parameter,
                Screen = screen,
            };

            // Schritt 1: Vorherigen Screen "abmelden"
            if (ActiveScreen != null)
            {
                overlayed = ActiveScreen.IsOverlay;

                ActiveScreen.InternalNavigateFrom(args);

                // Abbruch durch Screen eingeleitet
                if (args.Cancel) { return false; }

                // Screen deaktivieren
                ActiveScreen.IsActiveScreen = false;

                // Spezialfall (aktueller Screen nicht in History, neuer Screen Overlay)
                if (!ActiveScreen.InHistory && screen != null && screen.IsOverlay)
                    historyStack.Insert(0, ActiveScreen);

                // Ausblenden, wenn der neue Screen nicht gerade ein Overlay ist
                if (screen == null || !screen.IsOverlay)
                {
                    // Überblenden, falls erforderlich
                    if (NavigateFromTransition != null)
                    {
                        ActiveScreen.Alpha = 1f;
                        var trans = NavigateFromTransition.Clone(ActiveScreen);
                        trans.Finished += (s, e) =>
                        {
                            ScreenTarget.Controls.Remove(e);
                            ((Screen)e).IsVisibleScreen = false;
                        };
                        activeScreen.StartTransition(trans);
                    }
                    else
                    {
                        ScreenTarget.Controls.Remove(ActiveScreen);
                        ActiveScreen.IsVisibleScreen = false;
                    }
                }

                // NavigatedFrom-Event aufrufen
                args.Cancel = false;
                args.Handled = false;
                args.IsBackNavigation = isBackNavigation;
                args.Screen = screen;
                ActiveScreen.InternalNavigatedFrom(args);

                // entfernen
                args.Screen = ActiveScreen;
                ActiveScreen = null;
            }
            else args.Screen = null;

            // Schritt 2: zum neuen Screen navigieren
            if (screen != null)
            {
                // NavigateTo-Event aufrufen
                args.Cancel = false;
                args.Handled = false;
                args.IsBackNavigation = isBackNavigation;
                screen.InternalNavigateTo(args);

                // Neuen Screen einhängen
                if (historyStack.Contains(screen))
                    historyStack.Remove(screen);
                if (screen.InHistory)
                    historyStack.Insert(0, screen);

                if (!overlayed)
                {
                    if (NavigateToTransition != null)
                    {
                        screen.Alpha = 0f;
                        var trans = NavigateToTransition.Clone(screen);
                        screen.StartTransition(trans);
                    }
                    screen.IsVisibleScreen = true;
                    ScreenTarget.Controls.Add(screen);
                }

                ActiveScreen = screen;
                ActiveScreen.IsActiveScreen = true;

                // Default Mouse Mode anwenden
                MouseMode = ActiveScreen.DefaultMouseMode;

                // Navigate-Events aufrufen
                args.Cancel = false;
                args.Handled = false;
                args.IsBackNavigation = isBackNavigation;
                ActiveScreen.InternalNavigatedTo(args);
            }

            return true;
        }

        /// <summary>
        /// Navigiert bis zum ersten Screen zurück.
        /// </summary>
        public void NavigateHome()
        {
            while (CanGoBack)
                NavigateBack();
        }

        /// <summary>
        /// Öffnet ein Flyout.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="position"></param>
        public void Flyout(Control control, Point position)
        {
            flyout.AddControl(control, position);
        }

        /// <summary>
        /// Schließt ein Flyout wieder.
        /// </summary>
        /// <param name="control"></param>
        public void Flyback(Control control)
        {
            flyout.RemoveControl(control);
        }

        /// <summary>
        /// Wechselt in den Catured Mouse Mode.
        /// </summary>
        public void CaptureMouse()
        {
            MouseMode = MouseMode.Captured;
        }

        /// <summary>
        /// Wechselt in den Free Mouse Mode.
        /// </summary>
        public void FreeMouse()
        {
            MouseMode = MouseMode.Free;
        }

        public event KeyEventBaseDelegate KeyDown;
        public event KeyEventBaseDelegate KeyPress;
        public event KeyEventBaseDelegate KeyUp;
    }
}
