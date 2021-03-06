﻿// <copyright file="CumulativeMutableViewData.cs" company="OpenCensus Authors">
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

namespace OpenCensus.Stats
{
    using System.Collections.Generic;
    using OpenCensus.Common;
    using OpenCensus.Tags;

    internal class CumulativeMutableViewData : MutableViewData
    {
        private readonly IDictionary<TagValues, MutableAggregation> tagValueAggregationMap = new Dictionary<TagValues, MutableAggregation>();
        private ITimestamp start;

        internal CumulativeMutableViewData(IView view, ITimestamp start)
            : base(view)
        {
            this.start = start;
        }

        internal override void Record(ITagContext context, double value, ITimestamp timestamp)
        {
            IList<ITagValue> values = GetTagValues(GetTagMap(context), this.View.Columns);
            var tagValues = TagValues.Create(values);
            if (!this.tagValueAggregationMap.ContainsKey(tagValues))
            {
                this.tagValueAggregationMap.Add(tagValues, CreateMutableAggregation(this.View.Aggregation));
            }

            this.tagValueAggregationMap[tagValues].Add(value);
        }

        internal override IViewData ToViewData(ITimestamp now, StatsCollectionState state)
        {
            if (state == StatsCollectionState.ENABLED)
            {
                return ViewData.Create(
                    this.View,
                    CreateAggregationMap(this.tagValueAggregationMap, this.View.Measure),
                    this.start,
                    now);
            }
            else
            {
                // If Stats state is DISABLED, return an empty ViewData.
                return ViewData.Create(
                    this.View,
                    new Dictionary<TagValues, IAggregationData>(),
                    ZeroTimestamp,
                    ZeroTimestamp);
            }
        }

        internal override void ClearStats()
        {
            this.tagValueAggregationMap.Clear();
        }

        internal override void ResumeStatsCollection(ITimestamp now)
        {
            this.start = now;
        }
    }
}
