﻿using System;
using System.Collections.Generic;
using System.Linq;
using ImperaPlus.Domain.Services;

namespace ImperaPlus.Domain.Utilities
{
    public static class LinqExtensions
    {
        public static T RandomElement<T>(this IEnumerable<T> source, IRandomGen random)
        {
            var count = source.Count();

            var idx = random.GetNext(0, count);

            return source.Skip(idx).Take(1).FirstOrDefault();
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source, IRandomGen rng)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (rng == null) throw new ArgumentNullException("rng");

            return source.ShuffleIterator(rng);
        }

        private static IEnumerable<T> ShuffleIterator<T>(
            this IEnumerable<T> source, IRandomGen rng)
        {
            var buffer = source.ToList();
            for (int i = 0; i < buffer.Count; i++)
            {
                int j = rng.GetNext(i, buffer.Count);
                yield return buffer[j];

                buffer[j] = buffer[i];
            }
        }
    }
}
