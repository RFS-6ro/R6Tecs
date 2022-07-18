// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

namespace R6ThreadECS
{
    /// <summary>
    /// base for all components
    /// </summary>
    public interface IR6EcsComponent<T> 
        where T : struct, IR6EcsComponent<T>
    {
        void Write(T component);
        
        T Read();
    }
}
