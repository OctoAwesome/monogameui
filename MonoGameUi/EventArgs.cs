using engenious;
using engenious.Graphics;
using engenious.Input;


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
    /// Basisklasse für alle DragDrop Events
    /// </summary>
    public class DragEventArgs : PointerEventArgs
    {
        /// <summary>
        /// Optionales Feld um das sendende Control einzufügen.
        /// </summary>
        public Control Sender { get; set; }

        /// <summary>
        /// Optionales Icon, das während des Drag-Vorgangs angezeigt werden soll.
        /// </summary>
        public Texture2D Icon { get; set; }

        /// <summary>
        /// Angabe der Größe des Icons, das beim Drag-Vorgang angezeigt wird.
        /// </summary>
        public Point IconSize { get; set; }

        /// <summary>
        /// Content, der gedraggt wird.
        /// </summary>
        public object Content { get; set; }
    }

    /// <summary>
    /// Basisklasse für alle Positionsbasierten Events (Maus, Touch)
    /// </summary>
    public class PointerEventArgs : EventArgs
    {
        /// <summary>
        /// Gibt an, ob das Event 
        /// </summary>
        public bool Bubbled { get; set; }

        /// <summary>
        /// Position des Mauspointers bezogen auf den Ursprung des aktuellen Controls
        /// </summary>
        public Point LocalPosition { get; set; }

        /// <summary>
        /// Position des Mauspointers in globaler Screen-Koordinate
        /// </summary>
        public Point GlobalPosition { get; set; }
    }

    /// <summary>
    /// Standard Event Args bei Property Changed Events.
    /// </summary>
    /// <typeparam name="T">Typ des Properties</typeparam>
    public class PropertyEventArgs<T> : EventArgs
    {
        /// <summary>
        /// Der alte Wert der Property
        /// </summary>
        public T OldValue { get; set; }

        /// <summary>
        /// Der neue Wert der Property
        /// </summary>
        public T NewValue { get; set; }

        /// <summary>
        /// Erzeugt eine neue Instaz der PropertyEventArgs-Klasse
        /// </summary>
        public PropertyEventArgs() { }

        /// <summary>
        /// Erzeugt eine neue Instaz der PropertyEventArgs-Klasse
        /// </summary>
        /// <param name="oldValue">Der alte Wert</param>
        /// <param name="newValue">Der neue Wert</param>
        public PropertyEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }

    /// <summary>
    /// Event Arguments für alle Mouse Events.
    /// </summary>
    public class MouseEventArgs : PointerEventArgs
    {
        /// <summary>
        /// Gibt den aktuellen Modus der Maus an.
        /// </summary>
        public MouseMode MouseMode { get; set; }

        /// <summary>
        /// Erzeugt eine neue Instanz der MouseEventArgs-Klasse
        /// </summary>
        public MouseEventArgs() { }

        /// <summary>
        /// Erzeugt eine neue Instanz der MouseEventArgs-Klasse
        /// </summary>
        /// <param name="mouseMode">Der aktuelle Modus der Maus</param>
        /// <param name="localPosition">Position des Mauspointers bezogen auf den Ursprung des aktuellen Controls</param>
        /// <param name="globalPosition">Position des Mauspointers in globaler Screen-Koordinate</param>
        public MouseEventArgs(MouseMode mouseMode, Point localPosition, Point globalPosition)
        {
            MouseMode = mouseMode;
            LocalPosition = localPosition;
            GlobalPosition = globalPosition;
        }
    }

    /// <summary>
    /// Event Arguments für Maus Scroll Events
    /// </summary>
    public class MouseScrollEventArgs : MouseEventArgs
    {
        /// <summary>
        /// Gibt die  Anzahl der gescrollte Einheiten an.
        /// </summary>
        public int Steps { get; set; }

        /// <summary>
        /// Erzeugt eine neue Instanz der MouseScrollEventArgs-Klasse.
        /// </summary>
        public MouseScrollEventArgs() { }

        /// <summary>
        /// Erzeugt eine neue Instanz der MouseScrollEventArgs-Klasse.
        /// </summary>
        /// <param name="steps">Anzahl der gescrollten Einheiten</param>
        public MouseScrollEventArgs(int steps)
        {
            Steps = steps;
        }
    }

    /// <summary>
    /// Event Args für alle Touch-basierten Events.
    /// </summary>
    public class TouchEventArgs : PointerEventArgs
    {
        /// <summary>
        /// ID des Touch Points.
        /// </summary>
        public int TouchId { get; set; }
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

    public delegate void DragEventDelegate(DragEventArgs args);

    /// <summary>
    /// Event Delegat für Maus-Events.
    /// </summary>
    /// <param name="sender">Aufrufendes Control</param>
    /// <param name="args">Eventargumente</param>
    public delegate void MouseEventDelegate(Control sender, MouseEventArgs args);

    public delegate void MouseEventBaseDelegate(MouseEventArgs args);

    /// <summary>
    /// Event-Delegat für Maus-Scroll-Events.
    /// </summary>
    /// <param name="sender">Aufrufendes Control</param>
    /// <param name="args">Eventargumente</param>
    public delegate void MouseScrollEventDelegate(Control sender, MouseScrollEventArgs args);

    public delegate void MouseScrollEventBaseDelegate(MouseScrollEventArgs args);

    public delegate void TouchEventDelegate(Control control, TouchEventArgs args);

    public delegate void TouchEventBaseDelegate(TouchEventArgs args);

    /// <summary>
    /// Event Delegat für Keyboard-Events.
    /// </summary>
    /// <param name="sender">Aufrufendes Control</param>
    /// <param name="args">Eventargumente</param>
    public delegate void KeyEventDelegate(Control sender, KeyEventArgs args);

    /// <summary>
    /// Event Delegat für KeyDown im ScreenManager
    /// </summary>
    /// <param name="args">Eventargumente</param>
    public delegate void KeyEventBaseDelegate(KeyEventArgs args);

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
    /// <param name="args">Eventargumente</param>
    public delegate void SelectionDelegate<T>(Control sender, SelectionEventArgs<T> args);

    public delegate void CollectionDelegate(Control sender, CollectionEventArgs args);

    /// <summary>
    /// Event Delegat für PropertyChanged-Events
    /// </summary>
    /// <typeparam name="T">Typ der Property</typeparam>
    /// <param name="sender">Aufrufendes Control</param>
    /// <param name="args">Eventargumente</param>
    public delegate void PropertyChangedDelegate<T>(Control sender, PropertyEventArgs<T> args);
}
