using System;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using Unity.Burst;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[GenerateAuthoringComponent]
public struct MovableCubeComponent : IComponentData
{
    [GhostField]
    public int Value;
}