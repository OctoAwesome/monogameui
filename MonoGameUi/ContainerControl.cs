using System.Collections.Generic;

namespace MonoGameUi
{
    /// <summary>
    /// Basisklasse für alle Controls mit mehreren Inhalten
    /// </summary>
    public class ContainerControl : Control
    {
        /// <summary>
        /// Liste aller Controls
        /// </summary>
        public IList<Control> Controls { get { return Children; } }

        public ContainerControl(IScreenManager manager, string style = "") :
            base(manager, style)
        {
            ApplySkin(typeof(ContainerControl));
        }
    }
}
