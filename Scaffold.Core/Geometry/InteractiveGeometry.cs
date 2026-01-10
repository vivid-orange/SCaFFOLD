// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scaffold.Core.Geometry.Abstract;

namespace Scaffold.Core.Geometry
{
    public class InteractiveGeometry : IInteractiveGeometry
    {
        public InteractiveGeometry(List<IInteractiveGeometryItem> interactiveItemss, List<GeometryBase> geometryItems)
        {
            InteractiveGeometryItems = interactiveItemss;
            Geometry = geometryItems;
        }

        public List<IInteractiveGeometryItem> InteractiveGeometryItems { get; } 

        public List<GeometryBase> Geometry { get; }
    }
}
