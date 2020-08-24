﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT license.
//
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement.AspNetCore;

namespace Tests.FeatureManagement
{
    [Route("")]
    public class TestController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }

        [Route("/gateAll")]
        [HttpGet]
        [FeatureGate(Features.ConditionalFeature, Features.ConditionalFeature2)]
        public IActionResult GateAll()
        {
            return Ok();
        }

        [Route("/gateAny")]
        [HttpGet]
        [FeatureGate(RequirementType.Any, Features.ConditionalFeature, Features.ConditionalFeature2)]
        public IActionResult GateAny()
        {
            return Ok();
        }
    }
}
