using System;
using engenious;

namespace MonoGameUi
{
    /// <summary>
    /// Basisklasse für Animationstransitions von Controls.
    /// </summary>
    public abstract class Transition
    {
        /// <summary>
        /// Aktuell abgelaufene Zeit der Animation.
        /// </summary>
        public TimeSpan Current { get; private set; }

        /// <summary>
        /// Wartezeit bis zum Start der Animation.
        /// </summary>
        public TimeSpan Delay { get; private set; }

        /// <summary>
        /// Länge der Animation.
        /// </summary>
        public TimeSpan Duration { get; private set; }

        /// <summary>
        /// Control, das animiert wird.
        /// </summary>
        public Control Control { get; private set; }

        /// <summary>
        /// Bewegungskurve.
        /// </summary>
        public Func<float, float> Curve { get; private set; }

        /// <summary>
        /// Erstellt eine neue Transition für das angegebene Control.
        /// </summary>
        /// <param name="control">Zielcontrol.</param>
        /// <param name="curve">Bewegungskurve.</param>
        /// <param name="duration">Animationslänge.</param>
        public Transition(Control control, Func<float, float> curve, TimeSpan duration) :
            this(control, curve, duration, TimeSpan.Zero)
        {
        }

        /// <summary>
        /// Erstellt eine neue Transition für das angegebene Control.
        /// </summary>
        /// <param name="control">Zielcontrol.</param>
        /// <param name="curve">Bewegungskurve.</param>
        /// <param name="duration">Animationslänge.</param>
        /// <param name="delay">Wartezeit bis zum Start der Animation.</param>
        public Transition(Control control, Func<float, float> curve, TimeSpan duration, TimeSpan delay)
        {
            Current = new TimeSpan();
            Control = control;
            Curve = curve;
            Duration = duration;
            Delay = delay;
        }

        /// <summary>
        /// Update-Methode für die Animation.
        /// </summary>
        /// <param name="gameTime"></param>
        /// <returns></returns>
        public bool Update(GameTime gameTime)
        {
            // Neue Zeit berechnen
            Current += gameTime.ElapsedGameTime;

            // Vorlaufzeit (Delay)
            if (Current < Delay) return true;

            // Funktionseingang ermitteln
            float position = Math.Max(0, Math.Min(1, (float)(
                (Current.TotalMilliseconds - Delay.TotalMilliseconds) / Duration.TotalMilliseconds)));
            float value = Math.Max(0, Math.Min(1, Curve(position)));

            // Auf Control anwenden
            ApplyValue(Control, value);

            // Abbruchbedingung
            if (position >= 1f)
            {
                if (Finished != null)
                    Finished(this, Control);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Wendet die Transition auf das Steuerelement an.
        /// </summary>
        /// <param name="control">Zielcontrol der Transition.</param>
        /// <param name="value">Wert im zeitlichen Ablauf der Transition.</param>
        protected abstract void ApplyValue(Control control, float value);

        /// <summary>
        /// Fertigt eine Kopie dieser Transition an, ersetzt aber das Zielcontrol.
        /// </summary>
        /// <param name="control">Das neue Zielcontrol.</param>
        /// <returns></returns>
        public abstract Transition Clone(Control control);

        /// <summary>
        /// Signalisiert das Ende der Animation.
        /// </summary>
        public event TransitionDelegate Finished;

        #region Transition Curves

        /// <summary>
        /// Lineare Bewegung
        /// </summary>
        /// <param name="position">Verlaufsposition im Wertebereich 0 bis 1</param>
        /// <returns>Wert-Multiplikator</returns>
        public static float Linear(float position)
        {
            // Clamp
            return position;
        }

        /// <summary>
        /// Quadratische Bewegung
        /// </summary>
        /// <param name="position">Verlaufsposition im Wertebereich 0 bis 1</param>
        /// <returns>Wert-Multiplikator</returns>
        public static float Qubic(float position)
        {
            return (float)Math.Pow(position, 2);
        }

        #endregion
    }

    /// <summary>
    /// Delegat für Events bei Transistions.
    /// </summary>
    /// <param name="sender">Die auslösende Transition.</param>
    /// <param name="control">Das animierte Control.</param>
    public delegate void TransitionDelegate(Transition sender, Control control);
}
