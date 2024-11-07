using Unity.Netcode;
using UnityEngine;

[ClientRpc]
public void TeleportInsideClientRPC(ulong clientID, Vector3 teleportPos)
{
	NetworkManager networkManager = base.NetworkManager;
	if ((object)networkManager != null && networkManager.IsListening)
	{
		if (__rpc_exec_stage != __RpcExecStage.Client && (networkManager.IsServer || networkManager.IsHost))
		{
			ClientRpcParams clientRpcParams = default(ClientRpcParams);
			FastBufferWriter bufferWriter = __beginSendClientRpc(2679626543u, clientRpcParams, RpcDelivery.Reliable);
			BytePacker.WriteValueBitPacked(bufferWriter, clientID);
			bufferWriter.WriteValueSafe(in teleportPos);
			__endSendClientRpc(ref bufferWriter, 2679626543u, clientRpcParams, RpcDelivery.Reliable);
		}
		if (__rpc_exec_stage == __RpcExecStage.Client && (networkManager.IsClient || networkManager.IsHost))
		{
			TeleportInside.TeleportPlayerInside(clientID, teleportPos);
		}
	}
}