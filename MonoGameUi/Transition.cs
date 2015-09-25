using Microsoft.Xna.Framework;
using System;

namespace MonoGameUi
{
    /// <summary>
    /// Basisklasse für Animationstransitions von Controls.
    /// </summary>
    public abstract class Transition
    {
        public TimeSpan Current { get; private set; }
        public TimeSpan Delay { get; private set; }
        public TimeSpan Time { get; private set; }
        public Control Control { get; private set; }
        public Func<float, float> Curve { get; private set; }

        public Transition(Control control, Func<float, float> curve, TimeSpan time) :
            this(control, curve, time, TimeSpan.Zero)
        { }

        /// <summary>
        /// Erstellt eine neue Transition für das angegebene Control.
        /// </summary>
        /// <param name="control">Basis-Control</param>
        /// <param name="curve">Bewegungskurve</param>
        /// <param name="time">Animationslänge</param>
        /// <param name="delay">Wartezeit bis zum Start der Animation</param>
        public Transition(Control control, Func<float, float> curve, TimeSpan time, TimeSpan delay)
        {
            Current = new TimeSpan();
            Control = control;
            Curve = curve;
            Time = time;
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
                (Current.TotalMilliseconds - Delay.TotalMilliseconds) / Time.TotalMilliseconds)));
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

        protected abstract void ApplyValue(Control control, float value);

        /// <summary>
        /// Fertigt eine Kopie dieser Transition an, ersetzt aber das Zielcontrol.
        /// </summary>
        /// <param name="control"></param>
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

    public delegate void TransitionDelegate(Transition sender, Control control);
}
