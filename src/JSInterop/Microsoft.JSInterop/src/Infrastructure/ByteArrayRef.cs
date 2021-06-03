// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Microsoft.JSInterop.Infrastructure
{
    internal struct ByteArrayRef
    {
        [JsonPropertyName("__byte[]")]
        public int? Id { get; set; }
    }
}
