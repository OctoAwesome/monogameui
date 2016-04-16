namespace MonoGameUi
{
    /// <summary>
    /// Liste der möglichen Mouse-Modi für den Screen.
    /// </summary>
    public enum MouseMode
    {
        /// <summary>
        /// Der Mauszeiger wird im Zentrum gefangen und nach jeder Bewegung wieder zurück 
        /// gesetzt. Er wird ausgeblendet und die Bewegungswerte innerhalb der Mouse-Events 
        /// entsprechen dem Bewegungsdelta seit dem letzten Aufruf.
        /// </summary>
        Captured,

        /// <summary>
        /// Der Mauszeiger kann sich frei bewegen und wird ganz normal angezeigt.
        /// </summary>
        Free
    }
}
