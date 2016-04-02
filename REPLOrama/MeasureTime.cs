using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;

namespace LINQPadFoo
{
    // TODO: get rid of static? 
    public static class MeasureTime
    {
        public static IEnumerable<double> MeasureTimeInMilliseconds(Action experiment, int repeat, string experimentName = "") =>
            Enumerable.Range(0, repeat)
            .Aggregate(new { Watch = Stopwatch.StartNew(), Results = ImmutableList.Create<double>() }, 
                (x, y) => new { Watch = x.Watch, Results = AddResult(x.Results, experiment, x.Watch) })
            .Results;

        static ImmutableList<double> AddResult(ImmutableList<double> results, Action experiment, Stopwatch watch) =>
            results.Add(MeasureMilliseconds(experiment, watch));

        static double Total(Stopwatch sw) => sw.Elapsed.TotalMilliseconds;

        static double MeasureMilliseconds(Action experiment, Stopwatch sw)
        {
            var t0 = Total(sw);
            experiment();
            var t1 = Total(sw);
            return t1 - t0;
        }
    }
}
