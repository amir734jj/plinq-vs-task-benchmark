using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.Kernel;
using plinq_vs_task_benchmark.Extensions;

namespace plinq_vs_task_benchmark
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var fixture = new Fixture();

            var n = 500;
            
            Benchmark("PLINQ: ", () => PlinqBinPacking(fixture, n));
            
            Benchmark("Tasks without batch: ", () => TaskArrayBinPacking(fixture, n));
            
            Benchmark("Tasks with batch: ", () => TaskArrayBinPackingWithBatch(fixture, n));
        }
        
        private static int PrepareBinPacking(ISpecimenBuilder fixture) 
        {
            var bins = fixture.CreateMany<int>(10000).ToList();
            var c = fixture.Create<int>();
            const int n = 100;
            return BestFitBinPacking.Resolve(bins, n, c);
        }

        private static void Benchmark(string description, Func<Task> action)
        {
            var sw = new Stopwatch();
            
            sw.Start();

            action().Wait();
            
            sw.Stop();
            
            Console.WriteLine($"{description}: {sw.Elapsed.Seconds} seconds");
        }

        private static async Task PlinqBinPacking(ISpecimenBuilder fixture, int count)
        {
            Enumerable.Range(0, count)
                .AsParallel()
                .ForAll(action: x => PrepareBinPacking(fixture));
        }
        
        private static async Task TaskArrayBinPacking(ISpecimenBuilder fixture, int count)
        {
            var tasks = Enumerable.Range(0, count)
                .Select(_ => Task.Factory.StartNew(() => PrepareBinPacking(fixture)));

            await Task.WhenAll(tasks);
        }
        
        private static async Task TaskArrayBinPackingWithBatch(Fixture fixture, int count)
        {
            var tasks = Enumerable.Range(0, count)
                .Batch(10)
                .Select(x => Task.Factory.StartNew(() =>
                {
                    foreach (var _ in x)
                    {
                        PrepareBinPacking(fixture);
                    }
                }));

            await Task.WhenAll(tasks);
        }
    }
}