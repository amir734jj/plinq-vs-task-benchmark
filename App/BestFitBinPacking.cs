using System.Collections.Generic;

namespace plinq_vs_task_benchmark
{
    public static class BestFitBinPacking
    {
        // Returns number of bins required using best fit 
        public static int Resolve(IReadOnlyList<int> weight, int n, int c) 
        { 
            // Initialize result (Count of bins) 
            var res = 0; 
  
            // Create an array to store remaining space in bins 
            // there can be at most n bins 
            var binRem = new int[n]; 
  
            // Place items one by one 
            for (var i = 0; i < n; i++) { 
                // Find the best bin that ca\n accomodate 
                // weight[i] 
                int j; 
  
                // Initialize minimum space left and index 
                // of best bin 
                int min = c + 1, bi = 0; 
  
                for (j = 0; j < res; j++) {
                    if (binRem[j] < weight[i] || binRem[j] - weight[i] >= min) continue;
                    bi = j; 
                    min = binRem[j] - weight[i];
                } 
  
                // If no bin could accommodate weight[i], 
                // create a new bin 
                if (min == c + 1) { 
                    binRem[res] = c - weight[i]; 
                    res++; 
                } 
                else // Assign the item to best bin 
                    binRem[bi] -= weight[i]; 
            } 
            return res; 
        }
    }
}