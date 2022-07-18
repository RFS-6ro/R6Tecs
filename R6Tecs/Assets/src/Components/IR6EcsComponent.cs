// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

namespace R6ThreadECS
{
    public interface IR6EcsComponent
    {
        
    }
    
    /// <summary>
    /// base for all components
    /// </summary>
    public interface IR6EcsComponent<T> : IR6EcsComponent
        where T : struct, IR6EcsComponent<T>
    {
        void Write(T other);
        
        T Read();
    }
}
