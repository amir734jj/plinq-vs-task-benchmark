# plinq-vs-task-benchmark
PLINQ vs Task array benchmarking

### Result:
- `50000` objects
- Running on i7 10th gen


```
PLINQ: : 29 seconds
Tasks without batch: : 29 seconds
Tasks with batch: : 32 seconds
```

### Conclusion
PLINQ is very much faster than creating a Task array and awaiting it.
