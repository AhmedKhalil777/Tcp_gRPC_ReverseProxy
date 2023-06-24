    using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCPgRPCReverseProxy.Scheduling
{
    public class BlockingTaskScheduler : TaskScheduler
    {
        private readonly int threadCount;
        private readonly ConcurrentQueue<Task> tasks = new ConcurrentQueue<Task>();
        private readonly List<Thread> threads = new List<Thread>();
        private bool isRunning;

        public BlockingTaskScheduler(int threadCount)
        {
            if (threadCount <= 0)
                throw new ArgumentOutOfRangeException(nameof(threadCount), "The thread count must be greater than zero.");

            this.threadCount = threadCount;
        }

        protected override IEnumerable<Task> GetScheduledTasks()
        {
            return tasks.ToArray();
        }

        protected override void QueueTask(Task task)
        {
            if (!isRunning)
            {
                isRunning = true;
                StartThreads();
            }

            tasks.Enqueue(task);
        }

        protected override bool TryExecuteTaskInline(Task task, bool taskWasPreviouslyQueued)
        {
            if (!isRunning)
                return false;

            if (taskWasPreviouslyQueued)
                TryDequeue(task);

            return TryExecuteTask(task);
        }

        protected override bool TryDequeue(Task task)
        {
            return tasks.TryDequeue(out task);
        }

        private void StartThreads()
        {
            for (int i = 0; i < threadCount; i++)
            {
                var thread = new Thread(ExecuteTasks);
                thread.Start();
                threads.Add(thread);
            }
        }

        private void ExecuteTasks()
        {
            while (true)
            {
                if (tasks.TryDequeue(out var task))
                {
                    TryExecuteTask(task);
                }
                else if (!isRunning)
                {
                    // If there are no tasks and the scheduler is no longer running,
                    // exit the thread.
                    break;
                }
                else
                {
                    // No tasks are available, yield the thread.
                    Thread.Yield();
                }
            }
        }

        public override int MaximumConcurrencyLevel => threadCount;
    }
}
