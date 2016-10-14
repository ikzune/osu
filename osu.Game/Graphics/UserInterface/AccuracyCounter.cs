﻿//Copyright (c) 2007-2016 ppy Pty Ltd <contact@ppy.sh>.
//Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Transformations;
using osu.Framework.Timing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace osu.Game.Graphics.UserInterface
{
    /// <summary>
    /// Used as an accuracy counter. Represented visually as a percentage, internally as a fraction.
    /// </summary>
    public class AccuracyCounter : RollingCounter<float>
    {
        protected override Type transformType => typeof(TransformAccuracy);

        private long numerator = 0;
        public long Numerator
        {
            get
            {
                return numerator;
            }
            set
            {
                numerator = value;
                updateCount();
            }
        }

        private ulong denominator = 0;
        public ulong Denominator
        {
            get
            {
                return denominator;
            }
            set
            {
                denominator = value;
                updateCount();
            }
        }

        public AccuracyCounter()
        {
            RollingDuration = 500;
            RollingEasing = EasingTypes.Out;
        }

        public override void Load(BaseGame game)
        {
            base.Load(game);

            updateCount();
            StopRolling();
        }

        public void SetCount(long num, ulong den)
        {
            numerator = num;
            denominator = den;
            updateCount();
        }

        private void updateCount()
        {
            Count = Denominator == 0 ? 100.0f : (Numerator * 100.0f) / Denominator;
        }

        public override void ResetCount()
        {
            numerator = 0;
            denominator = 0;
            updateCount();
            StopRolling();
        }

        protected override string formatCount(float count)
        {
            return count.ToString("0.00") + "%";
        }

        protected override double getProportionalDuration(float currentValue, float newValue)
        {
            return Math.Abs(currentValue - newValue) * RollingDuration;
        }

        protected class TransformAccuracy : TransformFloat
        {
            public override void Apply(Drawable d)
            {
                base.Apply(d);
                (d as AccuracyCounter).VisibleCount = CurrentValue;
            }

            public TransformAccuracy(IClock clock)
                : base(clock)
            {
            }
        }
    }
}
