using System;
using Unity.Entities;
using Unity.NetCode;
using Unity.Networking.Transport;
using Unity.Burst;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Transforms;

[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public struct CubeInput : ICommandData
{

    public uint Tick { get; set;}
    public int horizontal;
    public int vertical;
    
}

[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class SampleCubeInput : ComponentSystem
{
    protected override void OnCreate()
    {
        RequireSingletonForUpdate<NetworkIdComponent>();
    }

    protected override void OnUpdate()
    {
        var localInput = GetSingleton<CommandTargetComponent>().targetEntity;
        if (localInput == Entity.Null)
        {
            var localPlayerId = GetSingleton<NetworkIdComponent>().Value;
            Entities.WithAll<MovableCubeComponent>().WithNone<CubeInput>().ForEach((Entity ent, ref GhostOwnerComponent ghostOwner) =>
                {

                    if (ghostOwner.NetworkId == localPlayerId)
                    {
                        PostUpdateCommands.AddBuffer<CubeInput>(ent);
                        PostUpdateCommands.SetComponent(GetSingletonEntity<CommandTargetComponent>(), new CommandTargetComponent { targetEntity = ent });
                    }
            });
            return;
        }
        var input = default(CubeInput);
        input.Tick = World.GetExistingSystem<ClientSimulationSystemGroup>().ServerTick;

        if (Input.GetKey("a"))
            input.horizontal -= 1;
        if (Input.GetKey("d"))
            input.horizontal += 1;
        if (Input.GetKey("s"))
            input.vertical -= 1;
        if (Input.GetKey("w"))
            input.vertical += 1;
        var inputBuffer = EntityManager.GetBuffer<CubeInput>(localInput);
        inputBuffer.AddCommandData(input);
    }
}

[UpdateInGroup(typeof(GhostPredictionSystemGroup))]
public class MoveCubeSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        var group = World.GetExistingSystem<GhostPredictionSystemGroup>();
        var tick = group.PredictingTick;
        var deltaTime = Time.DeltaTime;
        Entities.ForEach((DynamicBuffer<CubeInput> inputBuffer, ref Translation trans, ref PredictedGhostComponent prediction) =>
        {
            if (!GhostPredictionSystemGroup.ShouldPredict(tick, prediction))
                return;
            CubeInput input;
            inputBuffer.GetDataAtTick(tick, out input);
            if (input.horizontal > 0)
                trans.Value.x += deltaTime;
            if (input.horizontal < 0)
                trans.Value.x -= deltaTime;
            if (input.vertical > 0)
                trans.Value.z += deltaTime;
            if (input.vertical < 0)
                trans.Value.z -= deltaTime;
        });
    }
}

[UpdateInGroup(typeof(ClientSimulationSystemGroup))]
public class GoInGameClientSystem : ComponentSystem
{
    protected override void OnCreate()
    {
    }

    protected override void OnUpdate()
    {
        Entities.WithNone<NetworkStreamInGame>().ForEach((Entity ent, ref NetworkIdComponent id) =>
        {
            PostUpdateCommands.AddComponent<NetworkStreamInGame>(ent);
            var req = PostUpdateCommands.CreateEntity();
            PostUpdateCommands.AddComponent<Game.GoInGameRequest>(req);
            PostUpdateCommands.AddComponent(req, new SendRpcCommandRequestComponent { TargetConnection = ent });
        });
    }
}