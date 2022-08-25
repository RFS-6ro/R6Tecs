// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using R6Tasks.Parallelizing;
using R6ThreadECS.Attributes;
using R6ThreadECS.Systems;

namespace R6ThreadECS.ExecutionStrategy
{
    public abstract class R6ExecutionStrategy
    {
        public static R6ExecutionStrategy MainThread = new R6MainThreadExecutionStrategy();
        public static R6ExecutionStrategy Manual = new R6ManualExecutionStrategy();
        public static R6ExecutionStrategy Auto = new R6AutoExecutionStrategy();


        protected List<System.Type> types;
        protected List<R6EcsSystem> systems;
        protected List<R6SystemInfo> systemInfos;
        protected List<R6ParallelGroup> groups;
        protected R6Parallelizer parallelizer = new R6Parallelizer();




        public void BuildGroups()
        {
            R6ParallelGroup buildingGroup = new R6ParallelGroup();

            foreach (var r6EcsSystem in systems)
            {
                if (buildingGroup.TryAdd(r6EcsSystem))
                {
                    continue;
                }
                
                groups.Add(buildingGroup);
                buildingGroup = new R6ParallelGroup(r6EcsSystem);
            }

            groups.Add(buildingGroup);
        }










        public abstract void BuildExecutionOrder();
        // {
        //
        //     int[] dependentTasksIds = null;
        //     int taskId = 0;
        //     
        //     foreach (var parallelGroup in groups)
        //     {
        //         List<int> currentTasksIds = new List<int>(parallelGroup.Count);
        //
        //         foreach (R6EcsSystem r6EcsSystem in parallelGroup)
        //         {
        //             //Queue
        //             parallelizer.AddTask(taskId, r6EcsSystem.Action, dependentTasksIds);
        //             currentTasksIds.Add(taskId);
        //             ++taskId;
        //         }
        //
        //         dependentTasksIds = currentTasksIds.ToArray();
        //     }
        // }

        public abstract void Run();
        // {
        //     parallelizer?.Resolve();
        // }
    }
}