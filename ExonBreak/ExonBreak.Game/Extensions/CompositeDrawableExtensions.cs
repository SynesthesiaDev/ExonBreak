using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Transforms;

namespace ExonBreak.Game.Extensions;

public static class CompositeDrawableExtensions
{
    extension<T>(T drawable) where T : CompositeDrawable
    {
        public TransformSequence<T> FadeBorderTo(ColourInfo newColor, double duration, in Easing easing) => drawable.TransformTo(nameof(drawable.BorderColour), newColor, duration, easing);

        public TransformSequence<T> FlashBorderColor(ColourInfo newColor, double duration, in Easing easing)
        {
            var oldColor = drawable.BorderColour;
            return drawable.FadeBorderTo(newColor, duration / 2, easing).Then().FadeColour(oldColor, duration / 2, easing);
        }
        // => drawable.FadeBorderTo(newColor, duration, in easing)
        // .Then(drawable.FadeColour());
    }
}
