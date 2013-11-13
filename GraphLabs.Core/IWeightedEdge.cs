﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GraphLabs.Core
{
    /// <summary> Взвешенное ребро </summary>
    public interface IWeightedEdge : IEdge, IEquatable<IWeightedEdge>
    {
        /// <summary> Вес ребра </summary>
        int Weight { get; }
    }
}