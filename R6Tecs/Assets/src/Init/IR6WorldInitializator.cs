// ----------------------------------------------------------------------------
// The Proprietary or MIT License
// Copyright (c) 2022-2022 RFS_6ro <rfs6ro@gmail.com>
// ----------------------------------------------------------------------------

using R6ThreadECS.World;

namespace R6ThreadECS.Init
{
    public interface IR6WorldInitializator
    {
        R6World GetWorld();
    }
}
