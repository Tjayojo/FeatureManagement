﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Microsoft.FeatureManagement.AspNetCore
{
    /// <summary>
    /// A disabled feature handler that wraps an inline handler.
    /// </summary>
    class InlineDisabledFeaturesHandler : IDisabledFeaturesHandler
    {
        private readonly Action<IEnumerable<string>, ActionExecutingContext> _handler;

        public InlineDisabledFeaturesHandler(Action<IEnumerable<string>, ActionExecutingContext> handler)
        {
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public Task HandleDisabledFeatures(IEnumerable<string> features, ActionExecutingContext context)
        {
            _handler(features, context);

            return Task.CompletedTask;
        }
    }
}
