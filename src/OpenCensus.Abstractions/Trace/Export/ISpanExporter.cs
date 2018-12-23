﻿// <copyright file="ISpanExporter.cs" company="OpenCensus Authors">
// Copyright 2018, OpenCensus Authors
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

namespace OpenCensus.Trace.Export
{
    using System;

    /// <summary>
    /// Span exporter.
    /// </summary>
    public interface ISpanExporter : IDisposable
    {
        /// <summary>
        /// Adds a single span to the exporter.
        /// </summary>
        /// <param name="span">Span to export.</param>
        void AddSpan(ISpan span);

        /// <summary>
        /// Registers the exporter handler.
        /// </summary>
        /// <param name="name">Name of the handler.</param>
        /// <param name="handler">Handler instance.</param>
        void RegisterHandler(string name, IHandler handler);

        /// <summary>
        /// Unregister handler by it's name.
        /// </summary>
        /// <param name="name">Name of the handler to unregister.</param>
        void UnregisterHandler(string name);
    }
}
