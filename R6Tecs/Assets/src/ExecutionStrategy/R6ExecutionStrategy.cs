// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

namespace R6ThreadECS.ExecutionStrategy
{
    public abstract class R6ExecutionStrategy
    {
        public static R6ExecutionStrategy MainThread = new R6MainThreadExecutionStrategy();
        public static R6ExecutionStrategy Manual = new R6ManualExecutionStrategy();

        public abstract void Run();
    }
}