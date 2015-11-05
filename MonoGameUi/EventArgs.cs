using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace MonoGameUi
{
    /// <summary>
    /// Basisklasse für alle Arten von Event Args innerhalb des UI Frameworks
    /// </summary>
    public class EventArgs
    {
        /// <summary>
        /// Gibt an ob das Event bereits verarbeitet wurde oder legt dies fest.
        /// </summary>
        public bool Handled { get; set; }
    }

    /// <summary>
    /// Standard Event Args bei Property Changed Events.
    /// </summary>
    /// <typeparam name="T">Typ des Properties</typeparam>
    public class PropertyEventArgs<T> : EventArgs
    {
        public T OldValue { get; set; }

        public T NewValue { get; set; }

        public PropertyEventArgs() { }

        public PropertyEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    /// <summary>
    /// Event Arguments für alle Mouse Events.
    /// </summary>
    public class MouseEventArgs : EventArgs
    {
        /// <summary>
        /// Gibt den aktuellen Modus der Maus an.
        /// </summary>
        public MouseMode MouseMode { get; set; }

        /// <summary>
        /// Position des Mauspointers bezogen auf den Ursprung des aktuellen Controls
        /// </summary>
        public Point LocalPosition { get; set; }

        /// <summary>
        /// Position des Mauspointers in globaler Screen-Koordinate
        /// </summary>
        public Point GlobalPosition { get; set; }

        public MouseEventArgs() { }

        public MouseEventArgs(MouseMode mouseMode, Point localPosition, Point globalPosition)
        {
            MouseMode = mouseMode;
            LocalPosition = localPosition;
            GlobalPosition = globalPosition;
        }
    }

    public class MouseScrollEventArgs : MouseEventArgs
    {
        public int Steps { get; set; }

        public MouseScrollEventArgs() { }

        public MouseScrollEventArgs(int steps)
        {
            Steps = steps;
        }
    }

    /// <summary>
    /// Event Arguemnts für Tastatur-Events.
    /// </summary>
    public class KeyEventArgs : EventArgs
    {
        /// <summary>
        /// Gibt an ob eine der Shift-Tasten gedrückt wurde.
        /// </summary>
        public bool Shift { get; set; }

        /// <summary>
        /// Gibt an ob die Steuerungstaste gedrückt wurde.
        /// </summary>
        public bool Ctrl { get; set; }

        /// <summary>
        /// Gibts an ob eine der Alt-Tasten gedrückt wurde.
        /// </summary>
        public bool Alt { get; set; }

        /// <summary>
        /// Gibt die Taste an, für die das Event gefeuert wurde.
        /// </summary>
        public Keys Key { get; set; }
    }

    /// <summary>
    /// Event Arguments für Text-Eingabe
    /// </summary>
    public class KeyTextEventArgs : EventArgs
    {
        /// <summary>
        /// Der eingegebene Buchstabe.
        /// </summary>
        public char Character { get; set; }
    }

    /// <summary>
    /// Event Argumgents für Selektionsänderungen
    /// </summary>
    public class SelectionEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Das bisher selektiertes Item
        /// </summary>
        public T OldItem { get; set; }

        /// <summary>
        /// Das neu selektierte Item
        /// </summary>
        public T NewItem { get; set; }
    }

    public class CollectionEventArgs : EventArgs
    {
        public Control Control { get; set; }

        public int Index { get; set; }
    }

    /// <summary>
    /// Parameter für Screen-Navigationsevents
    /// </summary>
    public class NavigationEventArgs : EventArgs
    {
        /// <summary>
        /// Soll die Navigation an dieser Stelle abgebrochen werden?
        /// </summary>
        public bool Cancel { get; set; }

        /// <summary>
        /// Übergabe-Parameter aus dem anderen Screen.
        /// </summary>
        public object Parameter { get; set; }

        /// <summary>
        /// Gibt den zweiten an der Navigation beteiligten Screen hat.
        /// </summary>
        public Screen Screen { get; set; }

        /// <summary>
        /// Gibt an ob es sich dabei um eine Back-Navigation handelt oder nicht.
        /// </summary>
        public bool IsBackNavigation { get; set; }
    }

    /// <summary>
    /// Standard Event-Delegate ohne größeren Parameter.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void EventDelegate(Control sender, EventArgs args);

    /// <summary>
    /// Event Delegat für Maus-Events.
    /// </summary>
    /// <param name="sender">Aufrufendes Control</param>
    /// <param name="args">Eventargumente</param>
    public delegate void MouseEventDelegate(Control sender, MouseEventArgs args);

    public delegate void MouseScrollEventDelegate(Control sender, MouseScrollEventArgs args);

    /// <summary>
    /// Event Delegat für Keyboard-Events.
    /// </summary>
    /// <param name="sender">Aufrufendes Control</param>
    /// <param name="args">Eventargumente</param>
    public delegate void KeyEventDelegate(Control sender, KeyEventArgs args);

    /// <summary>
    /// Event Delegat für Texteingabe-Events
    /// </summary>
    /// <param name="sender">Aufrufendes Control</param>
    /// <param name="args">Eventargumente</param>
    public delegate void KeyTextEventDelegate(Control sender, KeyTextEventArgs args);

    /// <summary>
    /// Delegat für Events rum um Selektionsänderungen
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    public delegate void SelectionDelegate<T>(Control sender, SelectionEventArgs<T> args);

    public delegate void CollectionDelegate(Control sender, CollectionEventArgs args);

    public delegate void PropertyChangedDelegate<T>(Control sender, PropertyEventArgs<T> args);
}
