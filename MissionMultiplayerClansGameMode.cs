using System;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.Diamond;
using TaleWorlds.MountAndBlade.Multiplayer;
using TaleWorlds.MountAndBlade.Source.Missions;

namespace Clans
{
    public class MissionMultiplayerClansGameMode : MissionBasedMultiplayerGameMode // MultiplayerGameMode
    {
        public MissionMultiplayerClansGameMode(string name) : base(name) { }

        public override void StartMultiplayerGame(string scene)
        {
            //MultiplayerMissions.OpenBattleMission(scene); // Works, game and mission will start
            OpenMission(scene); // Doesn't work, mission doesn't start, even though it contains exactly the same code as the method above
        }

        public void OpenMission(string scene)
        {
            MissionState.OpenNew(
                "MultiplayerBattle",
                new MissionInitializerRecord(scene),
                (Mission missionController) =>
                {
                    if (!GameNetwork.IsServer)
                    {
                        return new MissionBehavior[]
                        {
                        MissionLobbyComponent.CreateBehavior(),
                        new MultiplayerRoundComponent(),
                        new MultiplayerWarmupComponent(),
                        new MissionMultiplayerGameModeFlagDominationClient(),
                        new MultiplayerTimerComponent(),
                        new MultiplayerMissionAgentVisualSpawnComponent(),
                        new ConsoleMatchStartEndHandler(),
                        new MissionLobbyEquipmentNetworkComponent(),
                        new MultiplayerTeamSelectComponent(),
                        new MissionHardBorderPlacer(),
                        new MissionBoundaryPlacer(),
                        new MissionBoundaryCrossingHandler(),
                        new MultiplayerPollComponent(),
                        new MultiplayerAdminComponent(),
                        new MultiplayerGameNotificationsComponent(),
                        new MissionOptionsComponent(),
                        new MissionScoreboardComponent(new BattleScoreboardData()),
                        MissionMatchHistoryComponent.CreateIfConditionsAreMet(),
                        new EquipmentControllerLeaveLogic(),
                        new MultiplayerPreloadHelper()
                        };
                    }
                    else
                    {
                        return new MissionBehavior[]
                        {
                        MissionLobbyComponent.CreateBehavior(),
                        new MultiplayerRoundController(),
                        new MissionMultiplayerFlagDomination(MultiplayerGameType.Battle),
                        new MultiplayerWarmupComponent(),
                        new MissionMultiplayerGameModeFlagDominationClient(),
                        new MultiplayerTimerComponent(),
                        new MultiplayerMissionAgentVisualSpawnComponent(),
                        new ConsoleMatchStartEndHandler(),
                        new SpawnComponent(new FlagDominationSpawnFrameBehavior(), new FlagDominationSpawningBehavior()),
                        new MissionLobbyEquipmentNetworkComponent(),
                        new MultiplayerTeamSelectComponent(),
                        new MissionHardBorderPlacer(),
                        new MissionBoundaryPlacer(),
                        new AgentVictoryLogic(),
                        new AgentHumanAILogic(),
                        new MissionBoundaryCrossingHandler(),
                        new MultiplayerPollComponent(),
                        new MultiplayerAdminComponent(),
                        new MultiplayerGameNotificationsComponent(),
                        new MissionOptionsComponent(),
                        new MissionScoreboardComponent(new BattleScoreboardData()),
                        new EquipmentControllerLeaveLogic(),
                        new MultiplayerPreloadHelper()
                        };
                    }
                }
            );
        }
    }
}
