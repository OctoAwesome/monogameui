using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;

namespace MonoGameUi
{
    /// <summary>
    /// Basisklasse für alle MonoGame-Komponenten
    /// </summary>
    public class BaseScreenComponent : DrawableGameComponent
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
        /// Gibt an, ob gerade ein Drag-Vorgang im Gange ist.
        /// </summary>
        public bool Dragging { get { return DraggingArgs != null && DraggingArgs.Handled; } }

        /// <summary>
        /// Legt fest, ob es GamePad Support geben soll (nicht unterstützt bisher)
        /// </summary>
        public bool GamePadEnabled { get; set; }

        /// <summary>
        /// Legt fest, ob es Maus Support geben soll
        /// </summary>
        public bool MouseEnabled { get; set; }

        /// <summary>
        /// Legt fest, ob es Touch Support geben soll.
        /// </summary>
        public bool TouchEnabled { get; set; }

        /// <summary>
        /// Legt fest, ob es Keyboard Support geben soll.
        /// </summary>
        public bool KeyboardEnabled { get; set; }

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

        /// <summary>
        /// Erzeugt eine neue Instanz der Klasse BaseScreenComponent.
        /// </summary>
        /// <param name="game">Die aktuelle Game-Instanz.</param>
        public BaseScreenComponent(Game game)
            : base(game)
        {
            Content = game.Content;

            KeyboardEnabled = true;
            MouseEnabled = true;
            GamePadEnabled = true;
            TouchEnabled = true;

#if !ANDROID

            Game.Window.TextInput += (s, e) =>
            {
                if (Game.IsActive)
                {
                    KeyTextEventArgs args = new KeyTextEventArgs() { Character = e.Character };

                    root.InternalKeyTextPress(args);
                }
            };

#endif

            Game.Window.ClientSizeChanged += (s, e) =>
            {
                if (ClientSizeChanged != null)
                    ClientSizeChanged(s, e);
            };
        }

        /// <summary>
        /// Lädt die für MonoGameUI notwendigen Content-Dateien.
        /// </summary>
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

        internal DragEventArgs DraggingArgs { get; private set; }

        private Dictionary<Keys, double> pressedKeys = new Dictionary<Keys, double>();

        /// <summary>
        /// Handling aller Eingaben, Mausbewegungen und Updaten aller Screens und Controls.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            if (Game.IsActive)
            {

                #region Mouse Interaction

                if (MouseEnabled)
                {
                    MouseState mouse = Mouse.GetState();

                    // Mausposition anhand des Mouse Modes ermitteln
                    Point mousePosition = mouse.Position;
                    if (MouseMode == MouseMode.Captured)
                        mousePosition = new Point(
                            mousePosition.X - (GraphicsDevice.Viewport.Width / 2),
                            mousePosition.Y - (GraphicsDevice.Viewport.Height / 2));

                    // Mouse Move
                    if (mousePosition != lastMousePosition)
                    {
                        MouseEventArgs moveArgs = new MouseEventArgs()
                        {
                            MouseMode = MouseMode,
                            GlobalPosition = mousePosition,
                            LocalPosition = mousePosition,
                        };

                        root.InternalMouseMove(moveArgs);
                        if (!moveArgs.Handled && MouseMove != null)
                            MouseMove(moveArgs);

                        // Start Drag Handling
                        if (mouse.LeftButton == ButtonState.Pressed &&
                            DraggingArgs == null)
                        {
                            DraggingArgs = new DragEventArgs()
                            {
                                MouseMode = MouseMode,
                                GlobalPosition = mousePosition,
                                LocalPosition = mousePosition,
                            };

                            root.InternalStartDrag(DraggingArgs);
                            if (!DraggingArgs.Handled && StartDrag != null)
                                StartDrag(DraggingArgs);
                        }

                        // Drop hovered
                        if (mouse.LeftButton == ButtonState.Pressed &&
                            DraggingArgs != null &&
                            DraggingArgs.Handled)
                        {
                            DragEventArgs args = new DragEventArgs()
                            {
                                MouseMode = MouseMode,
                                GlobalPosition = mousePosition,
                                LocalPosition = mousePosition,
                                Content = DraggingArgs.Content,
                                Icon = DraggingArgs.Icon,
                                Sender = DraggingArgs.Sender
                            };

                            root.InternalDropHover(args);
                            if (!args.Handled && DropHover != null)
                                DropHover(args);
                        }
                    }

                    // Linke Maustaste
                    if (mouse.LeftButton == ButtonState.Pressed)
                    {
                        if (!lastLeftMouseButtonPressed)
                        {
                            MouseEventArgs leftDownArgs = new MouseEventArgs
                            {
                                MouseMode = MouseMode,
                                GlobalPosition = mousePosition,
                                LocalPosition = mousePosition
                            };

                            // Linke Maustaste wurde neu gedrückt
                            root.InternalLeftMouseDown(leftDownArgs);
                            if (!leftDownArgs.Handled && LeftMouseDown != null)
                                LeftMouseDown(leftDownArgs);
                        }
                        lastLeftMouseButtonPressed = true;
                    }
                    else
                    {
                        if (lastLeftMouseButtonPressed)
                        {
                            // Handle Drop
                            if (DraggingArgs != null && DraggingArgs.Handled)
                            {
                                DragEventArgs args = new DragEventArgs()
                                {
                                    MouseMode = MouseMode,
                                    GlobalPosition = mousePosition,
                                    LocalPosition = mousePosition,
                                    Content = DraggingArgs.Content,
                                    Icon = DraggingArgs.Icon,
                                    Sender = DraggingArgs.Sender
                                };

                                root.InternalEndDrop(args);
                                if (!args.Handled && EndDrop != null)
                                    EndDrop(args);
                            }

                            // Discard Dragging Infos
                            DraggingArgs = null;

                            // Linke Maustaste wurde losgelassen
                            MouseEventArgs leftClickArgs = new MouseEventArgs
                            {
                                MouseMode = MouseMode,
                                GlobalPosition = mousePosition,
                                LocalPosition = mousePosition
                            };

                            root.InternalLeftMouseClick(leftClickArgs);
                            if (!leftClickArgs.Handled && LeftMouseClick != null)
                                LeftMouseClick(leftClickArgs);

                            MouseEventArgs leftUpArgs = new MouseEventArgs
                            {
                                MouseMode = MouseMode,
                                GlobalPosition = mousePosition,
                                LocalPosition = mousePosition
                            };

                            root.InternalLeftMouseUp(leftUpArgs);
                            if (!leftUpArgs.Handled && LeftMouseUp != null)
                                LeftMouseUp(leftUpArgs);
                        }
                        lastLeftMouseButtonPressed = false;
                    }

                    // Rechte Maustaste
                    if (mouse.RightButton == ButtonState.Pressed)
                    {
                        if (!lastRightMouseButtonPressed)
                        {
                            // Rechte Maustaste neu gedrückt
                            MouseEventArgs rightDownArgs = new MouseEventArgs
                            {
                                MouseMode = MouseMode,
                                GlobalPosition = mousePosition,
                                LocalPosition = mousePosition
                            };

                            root.InternalRightMouseDown(rightDownArgs);
                            if (!rightDownArgs.Handled && RightMouseDown != null)
                                RightMouseDown(rightDownArgs);
                        }
                        lastRightMouseButtonPressed = true;
                    }
                    else
                    {
                        if (lastRightMouseButtonPressed)
                        {
                            // Rechte Maustaste losgelassen
                            MouseEventArgs rightClickArgs = new MouseEventArgs
                            {
                                MouseMode = MouseMode,
                                GlobalPosition = mousePosition,
                                LocalPosition = mousePosition
                            };
                            root.InternalRightMouseClick(rightClickArgs);
                            if (!rightClickArgs.Handled && RightMouseClick != null)
                                RightMouseClick(rightClickArgs);

                            MouseEventArgs rightUpArgs = new MouseEventArgs
                            {
                                MouseMode = MouseMode,
                                GlobalPosition = mousePosition,
                                LocalPosition = mousePosition
                            };
                            root.InternalRightMouseUp(rightUpArgs);
                            if (!rightUpArgs.Handled && RightMouseUp != null)
                                RightMouseUp(rightUpArgs);
                        }
                        lastRightMouseButtonPressed = false;
                    }

                    // Mousewheel
                    if (lastMouseWheelValue != mouse.ScrollWheelValue)
                    {
                        int diff = (mouse.ScrollWheelValue - lastMouseWheelValue);

                        MouseScrollEventArgs scrollArgs = new MouseScrollEventArgs
                        {
                            MouseMode = MouseMode,
                            GlobalPosition = mousePosition,
                            LocalPosition = mousePosition,
                            Steps = diff
                        };
                        root.InternalMouseScroll(scrollArgs);
                        if (!scrollArgs.Handled && MouseScroll != null)
                            MouseScroll(scrollArgs);

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

                if (KeyboardEnabled)
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
                                {
                                    if (KeyDown != null)
                                        KeyDown(args);
                                }

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
                                    {
                                        if (KeyPress != null)
                                            KeyPress(args);
                                    }
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
                                {
                                    if (KeyUp != null)
                                        KeyUp(args);
                                }

                            }
                        }
                    }
                }

                #endregion

                #region Touchpanel Interaction

                if (TouchEnabled)
                {
                    TouchCollection touchPoints = TouchPanel.GetState();
                    foreach (var touchPoint in touchPoints)
                    {
                        Point point = touchPoint.Position.ToPoint();
                        TouchEventArgs args = new TouchEventArgs()
                        {
                            TouchId = touchPoint.Id,
                            GlobalPosition = point,
                            LocalPosition = point
                        };

                        switch (touchPoint.State)
                        {
                            case TouchLocationState.Pressed:
                                root.InternalTouchDown(args);
                                if (!args.Handled && TouchDown != null)
                                    TouchDown(args);
                                break;
                            case TouchLocationState.Moved:
                                root.InternalTouchMove(args);
                                if (!args.Handled && TouchMove != null)
                                    TouchMove(args);
                                break;
                            case TouchLocationState.Released:

                                // Linke Maustaste wurde losgelassen
                                TouchEventArgs tapArgs = new TouchEventArgs
                                {
                                    TouchId = touchPoint.Id,
                                    GlobalPosition = point,
                                    LocalPosition = point
                                };

                                root.InternalTouchTap(tapArgs);
                                if (!tapArgs.Handled && TouchTap != null)
                                    TouchTap(tapArgs);

                                root.InternalTouchUp(args);
                                if (!args.Handled && TouchUp != null)
                                    TouchUp(args);
                                break;
                        }
                    }
                }

                #endregion
            }

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

        /// <summary>
        /// Zeichnet Screens und Controls.
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Draw(GameTime gameTime)
        {
            root.PreDraw(gameTime);
            root.Draw(batch, GraphicsDevice.Viewport.Bounds, gameTime);

            // Drag Overlay
            if (DraggingArgs != null && DraggingArgs.Handled && DraggingArgs.Icon != null)
            {
                batch.Begin();
                if (DraggingArgs.IconSize != Point.Zero)
                    batch.Draw(DraggingArgs.Icon, new Rectangle(lastMousePosition, DraggingArgs.IconSize), Color.White);
                else
                    batch.Draw(DraggingArgs.Icon, new Vector2(lastMousePosition.X, lastMousePosition.Y), Color.White);
                batch.End();
            }
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
        /// <param name="parameter">Ein Parameter für den neuen Screen.</param>
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

        public event MouseEventBaseDelegate MouseMove;

        public event DragEventDelegate StartDrag;

        public event DragEventDelegate DropHover;

        public event DragEventDelegate EndDrop;

        public event MouseEventBaseDelegate LeftMouseUp;

        public event MouseEventBaseDelegate LeftMouseDown;

        public event MouseEventBaseDelegate LeftMouseClick;

        public event MouseEventBaseDelegate RightMouseUp;

        public event MouseEventBaseDelegate RightMouseDown;

        public event MouseEventBaseDelegate RightMouseClick;

        public event MouseScrollEventBaseDelegate MouseScroll;

        public event TouchEventBaseDelegate TouchDown;

        public event TouchEventBaseDelegate TouchMove;

        public event TouchEventBaseDelegate TouchUp;

        public event TouchEventBaseDelegate TouchTap;

        /// <summary>
        /// Event, das aufgerufen wird, wenn eine Taste gedrückt wird.
        /// </summary>
        public event KeyEventBaseDelegate KeyDown;

        /// <summary>
        /// Event, das aufgerufen wird, wenn eine Taste gedrückt ist.
        /// </summary>
        public event KeyEventBaseDelegate KeyPress;

        /// <summary>
        /// Event, das aufgerufen wird, wenn eine taste losgelassen wird.
        /// </summary>
        public event KeyEventBaseDelegate KeyUp;

        /// <summary>
        /// Event, das aufgerufen wird, wenn die Fenstergröße geändert wurde.
        /// </summary>
        public event EventHandler ClientSizeChanged;
    }
}
