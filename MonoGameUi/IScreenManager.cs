using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

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
    }
}
