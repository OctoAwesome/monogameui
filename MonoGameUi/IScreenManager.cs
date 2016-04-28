using System;
using System.Collections.Generic;
using engenious;
using engenious.Graphics;
using engenious.Content;

namespace MonoGameUi
{
    /// <summary>
    /// Standard Interface für Implementierungen von Screen Managern.
    /// </summary>
    public interface IScreenManager
    {
        /// <summary>
        /// Referenz auf das aktuelle Graphics Device.
        /// </summary>
        GraphicsDevice GraphicsDevice { get; }

        /// <summary>
        /// Referenz auf das Root-Element des Visual Trees.
        /// </summary>
        ContentControl Frame { get; }

        /// <summary>
        /// Referenz auf den Container der für die Screens verwendet werden soll.
        /// </summary>
        ContainerControl ScreenTarget { get; set; }

        /// <summary>
        /// Referenz auf den aktuellen Content Manager.
        /// </summary>
        ContentManager Content { get; }

        /// <summary>
        /// Gibt den aktuellen Modus der Maus zurück.
        /// </summary>
        MouseMode MouseMode { get; }

        /// <summary>
        /// Gibt an ob der aktuelle History Stack eine Navigation Back-Navigation erlaubt.
        /// </summary>
        bool CanGoBack { get; }

        /// <summary>
        /// Referenz auf den aktuellen Screen.
        /// </summary>
        Screen ActiveScreen { get; }

        /// <summary>
        /// Liste der History.
        /// </summary>
        IEnumerable<Screen> History { get; }

        /// <summary>
        /// Navigiert den Screen Manager zum angegebenen Screen.
        /// </summary>
        /// <param name="screen"></param>
        /// <param name="parameter">Parameter für den neuen Screen.</param>
        /// <returns>Gibt an ob die Navigation durchgeführt wurde.</returns>
        bool NavigateToScreen(Screen screen, object parameter = null);

        /// <summary>
        /// Navigiert zurück, sofern möglich.
        /// </summary>
        /// <returns>Gibt an ob die Navigation durchgeführt wurde.</returns>
        bool NavigateBack();

        /// <summary>
        /// Navigiert bis zum ersten Screen zurück.
        /// </summary>
        void NavigateHome();

        /// <summary>
        /// Öffnet ein Flyout.
        /// </summary>
        /// <param name="control"></param>
        /// <param name="position"></param>
        void Flyout(Control control, Point position);

        /// <summary>
        /// Schließt ein Flyout wieder.
        /// </summary>
        /// <param name="control"></param>
        void Flyback(Control control);

        /// <summary>
        /// Wechselt in den Catured Mouse Mode.
        /// </summary>
        void CaptureMouse();

        /// <summary>
        /// Wechselt in den Free Mouse Mode.
        /// </summary>
        void FreeMouse();

        /// <summary>
        /// Wird aufgerufen, wenn eine Taste gedrückt wird.
        /// </summary>
        event KeyEventBaseDelegate KeyDown;

        /// <summary>
        /// Wird aufgerufen, wenn eine Taste gedrückt ist.
        /// </summary>
        event KeyEventBaseDelegate KeyPress;

        /// <summary>
        /// Wird aufgerufen, wenn eine Taste losgelassen wird.
        /// </summary>
        event KeyEventBaseDelegate KeyUp;

        /// <summary>
        /// Wird aufgerufen, wenn die Auflösung geändert wird, z.B. bei einem Window-Resize.
        /// </summary>
        event EventHandler ClientSizeChanged;
    }
}
