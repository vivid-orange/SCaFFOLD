// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scaffold.Core.Geometry
{
    public interface IInteractiveGeometry
    {
        List<IInteractiveGeometryItem> InteractiveGeometryItems { get; }

        List<Abstract.GeometryBase> Geometry { get; }

    }
}
